FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-alpine
ARG Configuration=Debug
WORKDIR /app
EXPOSE 80
COPY bin/$Configuration/netcoreapp2.1/linux-musl-x64/publish .
ENV urls "http://*:80"
ENTRYPOINT ["dotnet", "check-yo-self-api.dll"]