#!/bin/bash

dotnet publish ../../check-yo-self-api/check-yo-self-api.csproj -c Release
dotnet publish ../check-yo-self-api-sdk.csproj -o ../out
dotnet pack ../check-yo-self-api-sdk.csproj /p:PackageVersion=1.0.9-beta --configuration Debug --include-source --include-symbols --output ../nupkg --version-suffix "beta"