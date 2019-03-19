#!/bin/sh

dotnet publish -c Release
cd ./check-yo-self-api
docker build . -t check-yo-self-api:1.0.0 --build-arg Configuration=Release