@ECHO OFF
ECHO Publishing the newest nupkg file.
FOR /F "delims=|" %%I IN ('DIR "Nupkg\*.nupkg" /B /O:D') DO SET NewestFile=%%I
start ./nuget.exe push Nupkg\%NewestFile% %CUSTOMCOMMANDSYSTEM_NUGET_KEY% -Source https://api.nuget.org/v3/index.json
PAUSE