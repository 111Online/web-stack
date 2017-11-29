Function SetEnironmentVariableValue ($key)
{
    Write-Output "Processing key $($key)"
	
	$matchingEnvVar = [Environment]::GetEnvironmentVariable($key)
    
	Write-Output "Found matching environment variable for key: $($key)"
    $substitueVar = '#{{{0}}}' -f $key
    Write-Output "Replacing value $($substitueVar) with $matchingEnvVar"

    (Get-Content $configPath).replace($substitueVar, $matchingEnvVar) | Set-Content $configPath
}

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

$configPath = "$env:APPLICATION_PATH\ApplicationInsights.config"

SetEnironmentVariableValue -key "InstrumentationKey"

$command = "C:\\ServiceMonitor.exe w3svc"
iex $command