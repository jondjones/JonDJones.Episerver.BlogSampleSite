##
## Inserts a new or updates an existing dependentAssembly element for a specified assembly
##
Function Add-EPiBindingRedirect($installPath, $project)
{
	[regex]$regex = '[\w\.]+,\sVersion=[\d\.]+,\sCulture=(?<culture>[\w-]+),\sPublicKeyToken=(?<publicKeyToken>\w+)'
	$ns = "urn:schemas-microsoft-com:asm.v1"
	$libPath = join-path $installPath "lib\net45"
	$projectFile = Get-Item $project.FullName

	#locate the project configuration file
	$webConfigPath = join-path $projectFile.Directory.FullName "web.config"
	$appConfigPath = join-path $projectFile.Directory.FullName "app.config"
	if (Test-Path $webConfigPath) 
	{
		$configPath = $webConfigPath
	}
	elseif (Test-Path $appConfigPath)
	{
		$configPath = $appConfigPath
	}
	else 
	{
		Write-Host "Unable to find a configuration file, binding redirect not configured."
		return
	}
 
	#load the configuration file for the project
	$config = New-Object xml
	$config.Load($configPath)

	# assume that we have the configuration element and make sure we have all the other parents of the AssemblyIdentity element.
	$configElement = $config.configuration
	$runtimeElement = GetOrCreateXmlElement $configElement "runtime" $null $config
	$assemblyBindingElement = GetOrCreateXmlElement $runtimeElement "assemblyBinding" $ns $config

	if ($assemblyBindingElement.length -gt 1)
	{
		for ($i=1; $i -lt $assemblyBindingElement.length; $i++) 
		{
			$assemblyBindingElement[0].InnerXml +=  $assemblyBindingElement[$i].InnerXml
			$runtimeElement.RemoveChild($assemblyBindingElement[$i])
		}
		$config.Save($configPath)
	}

	else 
	{
		$assemblyBindingElement = @($assemblyBindingElement)
	}

	$assemblyConfigs = $assemblyBindingElement[0].ChildNodes | where {$_.GetType().Name -eq "XmlElement"}

	#add/update binding redirects for assemblies in the current package
	get-childItem "$libPath\*.dll" | % { AddOrUpdateBindingRedirect $_  $assemblyConfigs $config }
	$config.Save($configPath)
}

##
## Inserts a new or updates an existing dependentAssembly element for a specified assembly
##
Function AddOrUpdateBindingRedirect([System.IO.FileInfo] $file, [System.Xml.XmlElement[]] $assemblyConfigs, [System.Xml.XmlDocument] $config, $install)
{

    $name = [System.IO.Path]::GetFileNameWithoutExtension($file)
    $assemblyName = [System.Reflection.AssemblyName]::GetAssemblyName($file)
    $assemblyConfig =  $assemblyConfigs | ? { $_.assemblyIdentity.Name -Eq $name } 

    if ($assemblyConfig -Eq $null) 
    { 
        #there is no existing binding configuration for the assembly, we need to create a new config element for it
        Write-Host "Adding binding redirect for $name".

        $matches = $regex.Matches($assemblyName.FullName)
        if ($matches.Count -gt 0)
        {
            $publicKeyToken = $matches[0].Groups["publicKeyToken"].Value
            $culture = $matches[0].Groups["culture"].Value
        }
        else 
        {
            Write-Host "Unable to figure out culture and publicKeyToken for $name"
            $publicKeyToken = "null"
            $culture = "neutral"
        }
    
        $assemblyIdentity = $config.CreateElement("assemblyIdentity", $ns)
        $assemblyIdentity.SetAttribute("name", $name)
        if (![String]::IsNullOrEmpty($publicKeyToken))
        {
            $assemblyIdentity.SetAttribute("publicKeyToken", $publicKeyToken)
        }
        if (![String]::IsNullOrEmpty($culture))
        {
            $assemblyIdentity.SetAttribute("culture", $culture)
        }
        
        $bindingRedirect = $config.CreateElement("bindingRedirect", $ns)
        $bindingRedirect.SetAttribute("oldVersion", "")
        $bindingRedirect.SetAttribute("newVersion", "")
        
        $assemblyConfig = $config.CreateElement("dependentAssembly", $ns)
        $assemblyConfig.AppendChild($assemblyIdentity) | Out-Null
        $assemblyConfig.AppendChild($bindingRedirect) | Out-Null

        #locate the assemblyBinding element and append the newly created dependentAssembly element
        $assemblyBinding = $config.configuration.runtime.ChildNodes | where {$_.Name -eq "assemblyBinding"}
        $assemblyBinding.AppendChild($assemblyConfig) | Out-Null
    } 
    else 
    {
        Write-Host "Updating binding redirect for $name"
    }

    $assemblyConfig.bindingRedirect.oldVersion = "0.0.0.0-" + $assemblyName.Version
    $assemblyConfig.bindingRedirect.newVersion = $assemblyName.Version.ToString()
}

Function GetOrCreateXmlElement([System.Xml.XmlElement]$parent, $elementName, $ns, $document)
{
    $child = $parent.$($elementName)
    if ($child -eq $null) 
    {
        $child = $document.CreateElement($elementName, $ns)
        $parent.AppendChild($child) | Out-Null
    }
    $child
}

Function GetIIsUrl($project)
{
	if ($project -eq $null)
	{
		$project = Get-Project
	}

	$baseAddress = 'IndexingService/IndexingService.svc'
	try
	{
		$iisUrlProperty = $null
		$useIIS = ReadProjectProperty $project "WebApplication.UseIIS"
		if ($useIIS -ne $null -and $useIIS.value -eq $true)
		{
			$iisUrlProperty = ReadProjectProperty $project "WebApplication.IISUrl"
		}
		
		if ($iisUrlProperty -eq $null)
		{
			$useIIS = ReadProjectProperty $project "WebApplication.IsUsingCustomServer"
			if ($useIIS -ne $null -and $useIIS.value -eq $true)
			{
				$iisUrlProperty = ReadProjectProperty $project "WebApplication.CurrentDebugUrl"
			}
		}
	}
	catch{}

	if ($iisUrlProperty -ne $null -and $iisUrlProperty.Value -ne "")
	{
 		If (!($iisUrlProperty.Value.SubString($iisUrlProperty.Value.Length-1,1) -eq "/")) 
		{
			$baseAddress =  $iisUrlProperty.Value + "/" + $baseAddress
		}
		else
		{
			$baseAddress =  $iisUrlProperty.Value + $baseAddress
		}
	}

	$baseAddress
}

Function ReadProjectProperty($project, $propertyName)
{
	if ($project -eq $null)
	{
		$project = Get-Project
	}

	$propValue = $null
	try
	{
		$propValue = $project.Properties.Item($propertyName)
	}
	catch
	{
		$propValue = $null
	}

	$propValue

}

Function Set-EPiBaseUri($project)
{
	if ($project -eq $null)
	{
		$project = Get-Project
	}
	
	$sitePath = (Get-ChildItem $project.Fullname).Directory.FullName
	$configPath = Join-Path $sitePath "web.config"
	if (Test-Path $configPath) 
	{
		$webConfig = New-Object xml
		$webConfig.Load($configPath)
		if($webConfig.configuration.'episerver.search'-ne $null)
		{
			$defaultServiceName = $webConfig.configuration.'episerver.search'.namedIndexingServices.defaultService
 			$defaultService = $webConfig.configuration.'episerver.search'.namedIndexingServices.services.SelectSingleNode("add[@name='$defaultServiceName']")

 			if ($defaultService -ne $null -and (IsValidURL $defaultService.baseUri) -eq $false) {
				$defaultService.baseUri =  GetIIsUrl($project)
				Write-Output  "Adding EPiServer Search Base Url  '$($defaultService.baseUri)'"
				$webConfig.Save($configPath)
			}

			if ($defaultService -eq $null -and $defaultServiceName -eq "")
			{
				$webConfig.configuration.'episerver.search'.namedIndexingServices.defaultService = "serviceName"
				$baseUri = GetIIsUrl $project
				$serviceElement = $webConfig.CreateElement('add')
				$serviceElement.SetAttribute('name','serviceName')
				$serviceElement.SetAttribute('baseUri',$baseUri)
				$serviceElement.SetAttribute('accessKey','local')
				[void]$webConfig.configuration.'episerver.search'.namedIndexingServices.services.AppendChild($serviceElement)
				$webConfig.Save($configPath)
			} 
		}
	}
}

Function IsValidURL($address) 
{
	$uri = $address -as [System.URI] 
	$uri.AbsoluteURI -ne $null -and $uri.Scheme -match '[http|https]' 
} 