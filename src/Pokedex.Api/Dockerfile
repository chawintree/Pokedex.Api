# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
COPY . /app/
WORKDIR /app

RUN dotnet publish "/app/src/Pokedex.Api/Pokedex.Api.csproj" -o out -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Pokedex.Api.dll"]