# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/FlatOut2_WindUpBoost/*" -Force -Recurse
dotnet publish "./FlatOut2_WindUpBoost.csproj" -c Release -o "$env:RELOADEDIIMODS/FlatOut2_WindUpBoost" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location