@ECHO OFF
ECHO Creating nupgk file.
start ./nuget.exe pack .\Integration\Integration.csproj -Build -NonInteractive -IncludeReferencedProjects -OutputDirectory .\Nupkg -Properties Configuration=Release
EXIT