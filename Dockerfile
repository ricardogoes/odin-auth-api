FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY *.sln .
COPY src/Odin.Auth.Api/*.csproj ./src/Odin.Auth.Api/
COPY src/Odin.Auth.Application/*.csproj ./src/Odin.Auth.Application/
COPY src/Odin.Auth.Domain/*.csproj ./src/Odin.Auth.Domain/
COPY src/Odin.Auth.Infra.Data/*.csproj ./src/Odin.Auth.Data.Keycloak/
COPY src/Odin.Auth.Infra.Keycloak/*.csproj ./src/Odin.Auth.Infra.Keycloak/
COPY src/Odin.Auth.Infra.Messaging/*.csproj ./src/Odin.Auth.Infra.Messaging/
COPY tests/Odin.Auth.UnitTests/*.csproj ./tests/Odin.Auth.UnitTests/
COPY tests/Odin.Auth.EndToEndTests/*.csproj ./tests/Odin.Auth.EndToEndTests/

RUN dotnet restore

# copy everything else and build app
COPY src/Odin.Auth.Api/. ./src/Odin.Auth.Api/
COPY src/Odin.Auth.Application/. ./src/Odin.Auth.Application/
COPY src/Odin.Auth.Domain/. ./src/Odin.Auth.Domain/
COPY src/Odin.Auth.Infra.Data/. ./src/Odin.Auth.Infra.Data/ 
COPY src/Odin.Auth.Infra.Keycloak/. ./src/Odin.Auth.Infra.Keycloak/ 
COPY src/Odin.Auth.Infra.Messaging/. ./src/Odin.Auth.Infra.Messaging/ 

#WORKDIR "/src/Odin.Auth.Api"
RUN dotnet build "./src/Odin.Auth.Api/Odin.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/Odin.Auth.Api/Odin.Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Odin.Auth.Api.dll"]