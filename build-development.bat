@echo off

setlocal enabledelayedexpansion

set MSBUILDER="C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"
set NUGET="NHS111\.NuGet\NuGet.exe"
set OUTPUTDIR="L:\Work\111 Online\nhs111-build-dir\nhs111-beta"

%NUGET% restore NHS111\NHS111.sln

%MSBUILDER% NHS111\NHS111.sln /t:Rebuild /p:Configuration=Release /p:VisualStudioVersion=12.0 /p:BuildingProject=true;OutDir=%OUTPUTDIR%