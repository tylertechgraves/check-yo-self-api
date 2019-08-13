Set-Location ./check-yo-self-api
dotnet publish -c Debug -r linux-musl-x64
docker build --build-arg Configuration=Debug -t check-yo-self-api:1.0.2 .
Set-Location ../