#!/usr/bin/env pwsh
dotnet pack -c Release

dotnet tool install --global --add-source ./nupkg crit