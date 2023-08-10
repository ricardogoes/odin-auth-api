FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY *.sln .
COPY src/Odin.Auth.Api/*.csproj ./src/Odin.Auth.Api/
COPY src/Odin.Auth.Service/*.csproj ./src/Odin.Auth.Service/
COPY src/Odin.Auth.Domain/*.csproj ./src/Odin.Auth.Domain/
COPY tests/Odin.Auth.UnitTests/*.csproj ./tests/Odin.Auth.UnitTests/ 

RUN dotnet restore

# copy everything else and build app
COPY src/Odin.Auth.Api/. ./src/Odin.Auth.Api/
COPY src/Odin.Auth.Service/. ./src/Odin.Auth.Service/
COPY src/Odin.Auth.Domain/. ./src/Odin.Auth.Domain/ 

#WORKDIR "/src/Odin.Auth.Api"
RUN dotnet build "./src/Odin.Auth.Api/Odin.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/Odin.Auth.Api/Odin.Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Odin.Auth.Api.dll"]