
rem test
dotnet test DemoNetCoreProject.UnitTest

echo %ERRORLEVEL%

rem build and publish
if %ERRORLEVEL% == 0 dotnet build DemoNetCoreProject.Backend -c Release /p:DeployOnBuild=true /p:PublishProfile=Publish /p:EnvironmentName=Production

if %ERRORLEVEL% == 0 dotnet publish DemoNetCoreProject.Batch -c Release /p:PublishProfile=Publish

pause
