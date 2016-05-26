@echo off

setlocal enabledelayedexpansion

set MSBUILDER="C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"
set NUGET="C:\tools\nuget.exe"
set OUTPUTDIR="C:\Temp\build\nhs111_dev"
REM C:\Windows\Microsoft.NET\Framework64\v4.0.30319

%NUGET% restore NHS111\NHS111.sln

%MSBUILDER% NHS111\NHS111.sln /t:Rebuild /p:Configuration=Release /p:VisualStudioVersion=12.0 /p:BuildingProject=true;OutDir=%OUTPUTDIR%