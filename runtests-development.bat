@echo off

setlocal enabledelayedexpansion

set NUNIT_RUNNER="L:\Tools\NUnit 2.6.4\bin\nunit-console-x86.exe"
set OPENCOVER_BIN="L:\Tools\OpenCover\OpenCover.Console.exe"
set APP_BIN_DIR="C:\Temp\build\nhs111_dev"
set OPENCOVER_OUTPUT_FILE=%APP_BIN_DIR%/opencover-results.xml
set RESHARPER_OUTPUT_FILE=%APP_BIN_DIR%/resharper-results.xml
set RESHARPER_BIN="L:\tools\ReSharper-cmd\inspectcode.exe"

set FOUND_DLLS=
FOR %%i in (%APP_BIN_DIR%\*Test*.dll) DO set FOUND_DLLS=!FOUND_DLLS! %%i

%NUNIT_RUNNER% %APP_BIN_DIR%\NHS111.Models.Test.dll

REM %OPENCOVER_BIN% -target:%NUNIT_RUNNER% -targetdir:%APP_BIN_DIR% -targetargs:"/nologo /noshadow %FOUND_DLLS%" -register -output:%OPENCOVER_OUTPUT_FILE%
REM %RESHARPER_BIN% /o:%RESHARPER_OUTPUT_FILE% NHS111/NHS111.sln