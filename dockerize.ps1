dotnet publish -c Release -r linux-musl-x64
Set-Location ./check-yo-self-api
docker build . -t check-yo-self-api:1.0.0 --build-arg Configuration=Release