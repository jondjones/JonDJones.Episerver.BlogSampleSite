
Import-Module "$EPiServerPackagingScripts\Get-WebConfig.psm1"
Import-Module "$EPiServerPackagingScripts\Set-WebConfig.psm1"
Import-Module "$EPiServerPackagingScripts\Get-ProtectedModulesPath.psm1"
Import-Module "$EPiServerPackagingScripts\Get-RepositoryPath.psm1"

# moves the procted modules inside modules/_protected folder & writes this info to config files. 
# $projectPath is optional i.e will get from Get-Project cmdlet if not given
Function Move-EPiServerProtectedModules($projectPath)
{	
	$ErrorActionPreference = "Stop"	
	try
	{    
		# fill up projectPath if not given
		if($projectPath -eq $null -or $projectPath -eq "")
		{
			$name = (Get-Project).FullName 
			$projectPath = Split-Path $name
		}

		# quit if no project path
		if($projectPath -eq $null -or $projectPath -eq "")
		{						    
			throw "Project path can't be found. Try to re-run this cmdlet with full path to root directory of the current project."
		}
		
		# get web.config as xml doc
		$elementsToBeLoaded = (,"episerver.framework") # array of elements which we want to expand with web.config
		$config = Get-WebConfig $projectPath $elementsToBeLoaded

		# exit if webConfig is not at root 
		if($config -eq $null -or $config -eq "") 
		{
			throw "WebConfig can't be found. This cmdlet only works when you have a web.config file present at the project's root path."
		}

		# Get the protected modules path
		$protectedPath = Get-ProtectedModulesPath $config $projectPath

		# exit if protectedPath isn't there 
		if($protectedPath -eq $null -or $protectedPath -eq "")
		{
			throw "Current protectedPath can't be found. This cmdlet only works when it can find the current protectedPath location."
		}

		$newProtectedPath = Join-Path $projectPath "modules\_protected"

		# Don't continue if old protected path is pointing to new protected path 
		if(IsParentPath $protectedPath $newProtectedPath)
		{
			throw "The protected modules are already configured to be in /modules/_protected location."
		}

		# set the web.config last change time so it can trigger iis/iis_express to restart app pool
		Set-WebConfigLastWriteTime $projectPath 


		# move the modules folder. This will copy everything from protectedPath to newProtectedPath which is always modules\_protected
		Copy-Folder $protectedPath $newProtectedPath			

		# remove the old modules folder
		if(Test-Path $protectedPath) 
		{
			Remove-Item -Recurse -Force $protectedPath 
		}

		#remove the repository.config from ModuleRepository
		$repositoryPath = Get-RepositoryPath $config $projectPath 
		$repositoryConfig = Join-Path $repositoryPath "repositories.config"

		# it might have been deleted, check it first 
		If(Test-Path $repositoryConfig)
		{
			Remove-Item -Recurse -Force $repositoryConfig
		}

		Update-ConfigEntries $config $projectPath 

		Set-WebConfig $config $projectPath $elementsToBeLoaded
	}
	catch [Exception]
	{
		$ErrorMessage = $_.Exception.Message		
		Show-Message $ErrorMessage

	}	
}

# displays a messagebox with error info
Function Show-Message ($message) 
{
	[System.Windows.Forms.MessageBox]::Show("The cmdlet failed to run due to an error. The error message is: $message") | Out-Null
}

Function Set-WebConfigLastWriteTime($projectPath) 
{
	$webConfigPath = Join-Path $projectPath "web.config"
	if(Test-Path $webConfigPath) 
	{
		(dir $webConfigPath).LastWriteTime = Get-Date
	}
}

# determines if the child path is part of given parent path
Function IsParentPath($parentPath, $childPath) 
{
	$parent = Normalize-Path $parentPath 
	$child = Normalize-Path $childPath 
	return ($parent -eq $child -or $child.Contains($parent)) 
}

Function Normalize-Path($path)
{
	if(([System.IO.Path]::IsPathRooted($path)))
	{
		$fullPath = [System.IO.Path]::GetFullPath($path);
	}
	else
	{
		$fullPath = [System.IO.Path]::GetFullPath((Join-Path (pwd) $path));
	}
	
	return $fullPath.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar).ToUpperInvariant();     
}


# Update the web.config and episerverframework.config file if found
Function Update-ConfigEntries($config, $projectPath)
{
	# Update the packaging section first 

	$packagingElement = $config.configuration."episerver.packaging"
	if ($packagingElement -ne $null)
	{
		$packagingElement.SetAttribute("protectedPath", "modules\_protected")
	}

	# Update the vpp physical path
	$epiframework = $config.configuration."episerver.framework"

	if($epiframework -ne $null)
	{
		FOREACH($add in $epiframework.virtualPathProviders.ChildNodes)
		{
			if($add.name -eq "ProtectedAddOns")
			{           
				$add.physicalPath = "modules\_protected"
			}
		}
	} 
}

# copies the source path (including subfolders, files etc.) to the destination path
Function Copy-Folder($sourcePath, $destinationPath)
{	

	robocopy $sourcePath $destinationPath /e	  
}

# Warn the user if the protected modules are not configured to standard path (modules\_protected).
Function ExistsProtectedModulePath($projectPath)
{
		# fill up projectPath if not given
		if($projectPath -eq $null -or $projectPath -eq "")
		{
			$name = (Get-Project).FullName 
			$projectPath = Split-Path $name
		}

		# quit if no project path
		if($projectPath -eq $null -or $projectPath -eq "")
		{						    
			return $true
		}
		
		# get web.config as xml doc
		$elementsToBeLoaded = (,"episerver.framework") # array of elements which we want to expand with web.config
		$config = Get-WebConfig $projectPath $elementsToBeLoaded

		# exit if webConfig is not at root 
		if($config -eq $null -or $config -eq "") 
		{
			return $true
		}

		# Get the protected modules path
		$protectedPath = Get-ProtectedModulesPath $config $projectPath
		$newProtectedPath = Join-Path $projectPath "modules\_protected"

		# Checkif old protected path is pointing to new protected path
		if(!(IsParentPath $protectedPath $newProtectedPath))
		{
			return $false
		}

		return $true
}

Function VerifyProtectedModulePath($projectPath)
{
	if(!(ExistsProtectedModulePath $projectPath))
	{
		Write-Warning "The protected modules are not configured to be in /modules/_protected location. You will not be able to install addons from visual studio. You need to run Move-EPiServerProtectedModules cmdlet."
	}
}


Export-ModuleMember -Function Move-EPiServerProtectedModules, ExistsProtectedModulePath, VerifyProtectedModulePath

# SIG # Begin signature block
# MIIZBwYJKoZIhvcNAQcCoIIY+DCCGPQCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUJ880fVDUeGhx/QABiJGOVDpp
# EtigghP/MIID7jCCA1egAwIBAgIQfpPr+3zGTlnqS5p31Ab8OzANBgkqhkiG9w0B
# AQUFADCBizELMAkGA1UEBhMCWkExFTATBgNVBAgTDFdlc3Rlcm4gQ2FwZTEUMBIG
# A1UEBxMLRHVyYmFudmlsbGUxDzANBgNVBAoTBlRoYXd0ZTEdMBsGA1UECxMUVGhh
# d3RlIENlcnRpZmljYXRpb24xHzAdBgNVBAMTFlRoYXd0ZSBUaW1lc3RhbXBpbmcg
# Q0EwHhcNMTIxMjIxMDAwMDAwWhcNMjAxMjMwMjM1OTU5WjBeMQswCQYDVQQGEwJV
# UzEdMBsGA1UEChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xMDAuBgNVBAMTJ1N5bWFu
# dGVjIFRpbWUgU3RhbXBpbmcgU2VydmljZXMgQ0EgLSBHMjCCASIwDQYJKoZIhvcN
# AQEBBQADggEPADCCAQoCggEBALGss0lUS5ccEgrYJXmRIlcqb9y4JsRDc2vCvy5Q
# WvsUwnaOQwElQ7Sh4kX06Ld7w3TMIte0lAAC903tv7S3RCRrzV9FO9FEzkMScxeC
# i2m0K8uZHqxyGyZNcR+xMd37UWECU6aq9UksBXhFpS+JzueZ5/6M4lc/PcaS3Er4
# ezPkeQr78HWIQZz/xQNRmarXbJ+TaYdlKYOFwmAUxMjJOxTawIHwHw103pIiq8r3
# +3R8J+b3Sht/p8OeLa6K6qbmqicWfWH3mHERvOJQoUvlXfrlDqcsn6plINPYlujI
# fKVOSET/GeJEB5IL12iEgF1qeGRFzWBGflTBE3zFefHJwXECAwEAAaOB+jCB9zAd
# BgNVHQ4EFgQUX5r1blzMzHSa1N197z/b7EyALt0wMgYIKwYBBQUHAQEEJjAkMCIG
# CCsGAQUFBzABhhZodHRwOi8vb2NzcC50aGF3dGUuY29tMBIGA1UdEwEB/wQIMAYB
# Af8CAQAwPwYDVR0fBDgwNjA0oDKgMIYuaHR0cDovL2NybC50aGF3dGUuY29tL1Ro
# YXd0ZVRpbWVzdGFtcGluZ0NBLmNybDATBgNVHSUEDDAKBggrBgEFBQcDCDAOBgNV
# HQ8BAf8EBAMCAQYwKAYDVR0RBCEwH6QdMBsxGTAXBgNVBAMTEFRpbWVTdGFtcC0y
# MDQ4LTEwDQYJKoZIhvcNAQEFBQADgYEAAwmbj3nvf1kwqu9otfrjCR27T4IGXTdf
# plKfFo3qHJIJRG71betYfDDo+WmNI3MLEm9Hqa45EfgqsZuwGsOO61mWAK3ODE2y
# 0DGmCFwqevzieh1XTKhlGOl5QGIllm7HxzdqgyEIjkHq3dlXPx13SYcqFgZepjhq
# IhKjURmDfrYwggSjMIIDi6ADAgECAhAOz/Q4yP6/NW4E2GqYGxpQMA0GCSqGSIb3
# DQEBBQUAMF4xCzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBDb3Jwb3Jh
# dGlvbjEwMC4GA1UEAxMnU3ltYW50ZWMgVGltZSBTdGFtcGluZyBTZXJ2aWNlcyBD
# QSAtIEcyMB4XDTEyMTAxODAwMDAwMFoXDTIwMTIyOTIzNTk1OVowYjELMAkGA1UE
# BhMCVVMxHTAbBgNVBAoTFFN5bWFudGVjIENvcnBvcmF0aW9uMTQwMgYDVQQDEytT
# eW1hbnRlYyBUaW1lIFN0YW1waW5nIFNlcnZpY2VzIFNpZ25lciAtIEc0MIIBIjAN
# BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAomMLOUS4uyOnREm7Dv+h8GEKU5Ow
# mNutLA9KxW7/hjxTVQ8VzgQ/K/2plpbZvmF5C1vJTIZ25eBDSyKV7sIrQ8Gf2Gi0
# jkBP7oU4uRHFI/JkWPAVMm9OV6GuiKQC1yoezUvh3WPVF4kyW7BemVqonShQDhfu
# ltthO0VRHc8SVguSR/yrrvZmPUescHLnkudfzRC5xINklBm9JYDh6NIipdC6Anqh
# d5NbZcPuF3S8QYYq3AhMjJKMkS2ed0QfaNaodHfbDlsyi1aLM73ZY8hJnTrFxeoz
# C9Lxoxv0i77Zs1eLO94Ep3oisiSuLsdwxb5OgyYI+wu9qU+ZCOEQKHKqzQIDAQAB
# o4IBVzCCAVMwDAYDVR0TAQH/BAIwADAWBgNVHSUBAf8EDDAKBggrBgEFBQcDCDAO
# BgNVHQ8BAf8EBAMCB4AwcwYIKwYBBQUHAQEEZzBlMCoGCCsGAQUFBzABhh5odHRw
# Oi8vdHMtb2NzcC53cy5zeW1hbnRlYy5jb20wNwYIKwYBBQUHMAKGK2h0dHA6Ly90
# cy1haWEud3Muc3ltYW50ZWMuY29tL3Rzcy1jYS1nMi5jZXIwPAYDVR0fBDUwMzAx
# oC+gLYYraHR0cDovL3RzLWNybC53cy5zeW1hbnRlYy5jb20vdHNzLWNhLWcyLmNy
# bDAoBgNVHREEITAfpB0wGzEZMBcGA1UEAxMQVGltZVN0YW1wLTIwNDgtMjAdBgNV
# HQ4EFgQURsZpow5KFB7VTNpSYxc/Xja8DeYwHwYDVR0jBBgwFoAUX5r1blzMzHSa
# 1N197z/b7EyALt0wDQYJKoZIhvcNAQEFBQADggEBAHg7tJEqAEzwj2IwN3ijhCcH
# bxiy3iXcoNSUA6qGTiWfmkADHN3O43nLIWgG2rYytG2/9CwmYzPkSWRtDebDZw73
# BaQ1bHyJFsbpst+y6d0gxnEPzZV03LZc3r03H0N45ni1zSgEIKOq8UvEiCmRDoDR
# EfzdXHZuT14ORUZBbg2w6jiasTraCXEQ/Bx5tIB7rGn0/Zy2DBYr8X9bCT2bW+IW
# yhOBbQAuOA2oKY8s4bL0WqkBrxWcLC9JG9siu8P+eJRRw4axgohd8D20UaF5Mysu
# e7ncIAkTcetqGVvP6KUwVyyJST+5z3/Jvz4iaGNTmr1pdKzFHTx/kuDDvBzYBHUw
# ggVUMIIEPKADAgECAhBqBz1Yk9Ce+JomHWkTBhgAMA0GCSqGSIb3DQEBBQUAMIG0
# MQswCQYDVQQGEwJVUzEXMBUGA1UEChMOVmVyaVNpZ24sIEluYy4xHzAdBgNVBAsT
# FlZlcmlTaWduIFRydXN0IE5ldHdvcmsxOzA5BgNVBAsTMlRlcm1zIG9mIHVzZSBh
# dCBodHRwczovL3d3dy52ZXJpc2lnbi5jb20vcnBhIChjKTEwMS4wLAYDVQQDEyVW
# ZXJpU2lnbiBDbGFzcyAzIENvZGUgU2lnbmluZyAyMDEwIENBMB4XDTEzMDIwNTAw
# MDAwMFoXDTE2MDQwNTIzNTk1OVowgZcxCzAJBgNVBAYTAlNFMQowCAYDVQQIEwEt
# MQ4wDAYDVQQHEwVLSVNUQTEVMBMGA1UEChQMRVBpU2VydmVyIEFCMT4wPAYDVQQL
# EzVEaWdpdGFsIElEIENsYXNzIDMgLSBNaWNyb3NvZnQgU29mdHdhcmUgVmFsaWRh
# dGlvbiB2MjEVMBMGA1UEAxQMRVBpU2VydmVyIEFCMIIBIjANBgkqhkiG9w0BAQEF
# AAOCAQ8AMIIBCgKCAQEAo6coNqVVn2Rk4HBEl0kc/HO+PttBuDrEx/9fKLONe3yT
# SFWk6dg7/Lv1l+uSTwt4GbWkk3HU4tRRd2gPJ3AK14AysycQRE9T0H5mhcJntXnz
# 6i6rOOQjEqmjipcu1iO1BPl8OSEK3h37kjjhtPCei2KpViH2icmVfVgtevF988qh
# n7V/B66QtQGjl44gBAI3JBgUDUhCCFO+d9+tY6gYx9SR9+OwYWNusRpEG4wHlzpo
# mK4xIcrk6CBfktyEjDRs7ZCNOdL3mWvPKVeZjj3+f+XPrARmEOkBsCCDuRPG2bRK
# /3gLrAfVP5L73EHHNgLqS2uzPppChulRvIKjnUyOVwIDAQABo4IBezCCAXcwCQYD
# VR0TBAIwADAOBgNVHQ8BAf8EBAMCB4AwQAYDVR0fBDkwNzA1oDOgMYYvaHR0cDov
# L2NzYzMtMjAxMC1jcmwudmVyaXNpZ24uY29tL0NTQzMtMjAxMC5jcmwwRAYDVR0g
# BD0wOzA5BgtghkgBhvhFAQcXAzAqMCgGCCsGAQUFBwIBFhxodHRwczovL3d3dy52
# ZXJpc2lnbi5jb20vcnBhMBMGA1UdJQQMMAoGCCsGAQUFBwMDMHEGCCsGAQUFBwEB
# BGUwYzAkBggrBgEFBQcwAYYYaHR0cDovL29jc3AudmVyaXNpZ24uY29tMDsGCCsG
# AQUFBzAChi9odHRwOi8vY3NjMy0yMDEwLWFpYS52ZXJpc2lnbi5jb20vQ1NDMy0y
# MDEwLmNlcjAfBgNVHSMEGDAWgBTPmanqeyb0S8mOj9fwBSbv49KnnTARBglghkgB
# hvhCAQEEBAMCBBAwFgYKKwYBBAGCNwIBGwQIMAYBAQABAf8wDQYJKoZIhvcNAQEF
# BQADggEBAIk7DJSHwVYLgVDJzo1GSsMElW0XhAkl167CGHP0q18xgRCEZsb1M93u
# Z6uSnJWbtnGrxHSjbOxvWPUQSChMq7h+aZdw/emdFpZ5g3tbKcTZN/1l8pREvPG7
# vO/UUmXSG20xezxcuzM2bRgIYxmFIHNn6XXVORkWVGujm/zo/dYVDPjH1udFZ5nj
# IenD/YeO2ZjvnZssAoyTZuDhpf3qUtEff2Kc+PXVYoMsk8Q4TO74ps6DbpqddLDg
# k73Xbyr++tvmhZIL8XSzB9j1thEijIwYFn6k2TMls4pVQ8s/37oJcvwZ/KPICLUY
# +A8+Kx6iywx7QN1mfwEekzsKiYA7LHswggYKMIIE8qADAgECAhBSAOWqJVb8Gobt
# lsnUSzPHMA0GCSqGSIb3DQEBBQUAMIHKMQswCQYDVQQGEwJVUzEXMBUGA1UEChMO
# VmVyaVNpZ24sIEluYy4xHzAdBgNVBAsTFlZlcmlTaWduIFRydXN0IE5ldHdvcmsx
# OjA4BgNVBAsTMShjKSAyMDA2IFZlcmlTaWduLCBJbmMuIC0gRm9yIGF1dGhvcml6
# ZWQgdXNlIG9ubHkxRTBDBgNVBAMTPFZlcmlTaWduIENsYXNzIDMgUHVibGljIFBy
# aW1hcnkgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkgLSBHNTAeFw0xMDAyMDgwMDAw
# MDBaFw0yMDAyMDcyMzU5NTlaMIG0MQswCQYDVQQGEwJVUzEXMBUGA1UEChMOVmVy
# aVNpZ24sIEluYy4xHzAdBgNVBAsTFlZlcmlTaWduIFRydXN0IE5ldHdvcmsxOzA5
# BgNVBAsTMlRlcm1zIG9mIHVzZSBhdCBodHRwczovL3d3dy52ZXJpc2lnbi5jb20v
# cnBhIChjKTEwMS4wLAYDVQQDEyVWZXJpU2lnbiBDbGFzcyAzIENvZGUgU2lnbmlu
# ZyAyMDEwIENBMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA9SNLXqXX
# irsy6dRX9+/kxyZ+rRmY/qidfZT2NmsQ13WBMH8EaH/LK3UezR0IjN9plKc3o5x7
# gOCZ4e43TV/OOxTuhtTQ9Sc1vCULOKeMY50Xowilq7D7zWpigkzVIdob2fHjhDuK
# Kk+FW5ABT8mndhB/JwN8vq5+fcHd+QW8G0icaefApDw8QQA+35blxeSUcdZVAccA
# JkpAPLWhJqkMp22AjpAle8+/PxzrL5b65Yd3xrVWsno7VDBTG99iNP8e0fRakyiF
# 5UwXTn5b/aSTmX/fze+kde/vFfZH5/gZctguNBqmtKdMfr27Tww9V/Ew1qY2jtaA
# dtcZLqXNfjQtiQIDAQABo4IB/jCCAfowEgYDVR0TAQH/BAgwBgEB/wIBADBwBgNV
# HSAEaTBnMGUGC2CGSAGG+EUBBxcDMFYwKAYIKwYBBQUHAgEWHGh0dHBzOi8vd3d3
# LnZlcmlzaWduLmNvbS9jcHMwKgYIKwYBBQUHAgIwHhocaHR0cHM6Ly93d3cudmVy
# aXNpZ24uY29tL3JwYTAOBgNVHQ8BAf8EBAMCAQYwbQYIKwYBBQUHAQwEYTBfoV2g
# WzBZMFcwVRYJaW1hZ2UvZ2lmMCEwHzAHBgUrDgMCGgQUj+XTGoasjY5rw8+AatRI
# GCx7GS4wJRYjaHR0cDovL2xvZ28udmVyaXNpZ24uY29tL3ZzbG9nby5naWYwNAYD
# VR0fBC0wKzApoCegJYYjaHR0cDovL2NybC52ZXJpc2lnbi5jb20vcGNhMy1nNS5j
# cmwwNAYIKwYBBQUHAQEEKDAmMCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC52ZXJp
# c2lnbi5jb20wHQYDVR0lBBYwFAYIKwYBBQUHAwIGCCsGAQUFBwMDMCgGA1UdEQQh
# MB+kHTAbMRkwFwYDVQQDExBWZXJpU2lnbk1QS0ktMi04MB0GA1UdDgQWBBTPmanq
# eyb0S8mOj9fwBSbv49KnnTAfBgNVHSMEGDAWgBR/02Wnwt3su/AwCfNDOfoCrzMx
# MzANBgkqhkiG9w0BAQUFAAOCAQEAViLmNKTEYctIuQGtVqhkD9mMkcS7zAzlrXqg
# In/fRzhKLWzRf3EafOxwqbHwT+QPDFP6FV7+dJhJJIWBJhyRFEewTGOMu6E01MZF
# 6A2FJnMD0KmMZG3ccZLmRQVgFVlROfxYFGv+1KTteWsIDEFy5zciBgm+I+k/RJoe
# 6WGdzLGQXPw90o2sQj1lNtS0PUAoj5sQzyMmzEsgy5AfXYxMNMo82OU31m+lIL00
# 6ybZrg3nxZr3obQhkTNvhuhYuyV8dA5Y/nUbYz/OMXybjxuWnsVTdoRbnK2R+qzt
# k7pdyCFTwoJTY68SDVCHERs9VFKWiiycPZIaCJoFLseTpUiR0zGCBHIwggRuAgEB
# MIHJMIG0MQswCQYDVQQGEwJVUzEXMBUGA1UEChMOVmVyaVNpZ24sIEluYy4xHzAd
# BgNVBAsTFlZlcmlTaWduIFRydXN0IE5ldHdvcmsxOzA5BgNVBAsTMlRlcm1zIG9m
# IHVzZSBhdCBodHRwczovL3d3dy52ZXJpc2lnbi5jb20vcnBhIChjKTEwMS4wLAYD
# VQQDEyVWZXJpU2lnbiBDbGFzcyAzIENvZGUgU2lnbmluZyAyMDEwIENBAhBqBz1Y
# k9Ce+JomHWkTBhgAMAkGBSsOAwIaBQCgcDAQBgorBgEEAYI3AgEMMQIwADAZBgkq
# hkiG9w0BCQMxDAYKKwYBBAGCNwIBBDAcBgorBgEEAYI3AgELMQ4wDAYKKwYBBAGC
# NwIBFTAjBgkqhkiG9w0BCQQxFgQUJ7PCA7fN3SuJr3Nw1k4C+LYuSREwDQYJKoZI
# hvcNAQEBBQAEggEAQQGWqRJkIUhN2Tje/eaHdtA8cwJZhxIGQZgmUO34MHAV/vur
# il+nwjXnS1zcedxP+94fM7hvZLdEI+3fpjV3vWR7t9dMR3DX9xyxj+u5mAbgwTxu
# XQjDKC0yHfokV2dS84RfEHs2eWW96alf+dExdzBIcFOZ2ZQk1YgvSbdp8RMGbyrh
# IKRgnA0eJfrqQyLIb3VHxpmLtwCEFIfbD2T+WKxTYbWLJ3gVA9vKqaKr0TSx0Q+j
# 7BI7+wu7pML7Hp9OccZtFl5dqpsMkKced0cew8UXUHcDashZY6aV5ws+MbCUeiKC
# sGe9/FCVDky04iGzEsmKZmZoKp7tVZsAeWELaaGCAgswggIHBgkqhkiG9w0BCQYx
# ggH4MIIB9AIBATByMF4xCzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBD
# b3Jwb3JhdGlvbjEwMC4GA1UEAxMnU3ltYW50ZWMgVGltZSBTdGFtcGluZyBTZXJ2
# aWNlcyBDQSAtIEcyAhAOz/Q4yP6/NW4E2GqYGxpQMAkGBSsOAwIaBQCgXTAYBgkq
# hkiG9w0BCQMxCwYJKoZIhvcNAQcBMBwGCSqGSIb3DQEJBTEPFw0xNTAzMTMxMDAx
# MDVaMCMGCSqGSIb3DQEJBDEWBBSbobgvhq8DrnPiN+NVAor9/GCacjANBgkqhkiG
# 9w0BAQEFAASCAQAHXc5+9hDUUkWXDrvJ3FC9PpA6HTrrUwuqiG+e5U8B6VHZZtoJ
# ADDcqBP2FrhJZy+Ahza5kKX5CMQaeqTSInNr/3E6AQ4fK+V50lFFdnxbVJKaTsM6
# /02fDednYDTDIiRKMa4iASbcheBjheCsxVNwXf50JRR9Q3IUhzUAy/92C2Qfb7nh
# aDQe5ud17iPDx07pqymrcc+/WEvtrhi6DDtdAPqXrhI1Quf5iWW6UYEIwCOWxF+j
# 6KCMJU8eARmlFsgpOG4Gqp4VvHhzBNJxFM2Bxa2YEV+F/i/zBDba10MNToAvT500
# pYtUhT1FKgBS5l+cxaxcpY74KjfMN06i4tIs
# SIG # End signature block
