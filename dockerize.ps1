dotnet publish -c Release
Set-Location ./check-yo-self-api
docker build . -t check-yo-self-api:1.0.0 --build-arg Configuration=Release