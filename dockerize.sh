#!/bin/sh

dotnet publish -c Debug
cd ./check-yo-self-api
docker build --build-arg Configuration=Debug -t check-yo-self-api:1.0.0 .