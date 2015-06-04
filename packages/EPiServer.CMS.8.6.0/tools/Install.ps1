param($installPath, $toolsPath, $package, $project)

#import Module
Import-Module (Join-Path (join-path $installPath  "tools")  EPiServer.Cms.psm1)

$projectFilePath = Get-ChildItem $project.Fullname

$sitePath = $projectFilePath.Directory.FullName

$WebConfigFile = GetWebConfigPath $sitePath


$epiConnection = GetEPiServerConnectionString($WebConfigFile)
if ($epiConnection -eq $null)
{
	WRITE-HOST "Starting Deploy into $($project.ProjectName)"

	# Get EPiDeploy.exe Path
	$EpidpeployEXEPath = GetEPiDeployPath $installPath $project 

	$episerverWebConfig = Join-Path (join-path $installPath  "tools")  "Episerver.Cms.config"
	# Deploy Web Config
	&$EpidpeployEXEPath  -a config -s $sitePath  -p $episerverWebConfig

	#Create ModulesBin (We should remove it after Packaging)
	CreateFolder $sitePath "modulesbin"

	# Make unique DB name
	$seed = [GUID]::NewGuid()
	$uniqueDB  = "EPiServerDB_" + $seed.ToString("N").Substring(0,8)

	# Copying DB -> App_Data
	WRITE-HOST "Copying $($uniqueDB) To $($project.ProjectName) -> App_Data"
	CopyDB $toolsPath $sitePath ($uniqueDB+".mdf")
	
	$connectionPath = GetConnectionConfigPath $sitePath
	$connectionContent = Get-Content -path $connectionPath
	
	# Modify ConnectionString according to seed
	$connectionContent = $connectionContent -replace "{EPiServerDB_SEED}", $uniqueDB
	Set-Content $connectionContent -path $connectionPath

	$cmsCoreToolsPath = GetPackagesToolPath $installPath $project "EPiServer.Cms.Core"
	$cmsCoreDbPath = join-path  ($cmsCoreToolsPath) "EPiServer.Cms.Core.sql"

	# Run Script
	WRITE-HOST "Running $($cmsCoreDbPath) into $($uniqueDB)"
	&$EpidpeployEXEPath  -a sql -s $sitePath  -p $cmsCoreDbPath -v false -c "EPiServerDB"
	
	WRITE-HOST "EPiServer.Cms is installed"
}
