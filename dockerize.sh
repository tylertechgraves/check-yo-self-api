#!/bin/bash

cd ./check-yo-self-api || exit
dotnet publish -c Debug -r linux-musl-x64
docker build --build-arg Configuration=Debug -t check-yo-self-api:1.0.0 .
cd .. || exit