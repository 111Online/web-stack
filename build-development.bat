REM cd NHS111
C:\tools\nuget.exe restore NHS111\NHS111.sln

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe NHS111\NHS111.sln /p:Configuration=Development /p:VisualStudioVersion=12.0 /p:BuildingProject=true;OutDir=C:\Temp\build\nhs111_dev