dotnet publish -c Debug -r linux-musl-x64
Set-Location ./check-yo-self-api
docker build --build-arg Configuration=Debug -t check-yo-self-api:1.0.0 .
Set-Location ../