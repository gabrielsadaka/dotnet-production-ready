FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY .editorconfig .
COPY src/WeatherApi/WeatherApi.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY src/WeatherApi/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

RUN useradd -m -s /bin/bash dotnet-user
USER dotnet-user

ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "WeatherApi.dll"]