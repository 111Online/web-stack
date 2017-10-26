$configPath = "$env:APPLICATION_PATH\web.config"

Write-Output "Loading config file from $configPath"
$xml = [xml](Get-Content $configPath)

ForEach($add in $xml.configuration.appSettings.add)
{
	Write-Output "Processing AppSetting key $($add.key)"
	
	$matchingEnvVar = [Environment]::GetEnvironmentVariable($add.key)

	if($matchingEnvVar)
	{
		Write-Output "Found matching environment variable for key: $($add.key)"
		Write-Output "Replacing value $($add.value)  with $matchingEnvVar"

		$add.value = $matchingEnvVar
	}
}

$xml.Save($configPath)

$command = "C:\\ServiceMonitor.exe w3svc"
iex $command