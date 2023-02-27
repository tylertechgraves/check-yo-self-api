#!/bin/bash

cd ./check-yo-self-api || exit
dotnet publish -c Debug
docker build --build-arg Configuration=Debug -t check-yo-self-api:1.0.4 .
cd .. || exit