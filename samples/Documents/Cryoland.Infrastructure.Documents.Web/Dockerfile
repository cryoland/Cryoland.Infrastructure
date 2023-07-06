# Use it with docker-compose at ../

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Abstractions/Cryoland.Infrastructure.Documents.Abstractions.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.Abstractions.csproj"
COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Abstractions", "."]

COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Services/Cryoland.Infrastructure.Documents.Services.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.Services.csproj"
COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Services", "."]

COPY ["./src/Documents/Cryoland.Infrastructure.Documents/Cryoland.Infrastructure.Documents.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.csproj"
COPY ["./src/Documents/Cryoland.Infrastructure.Documents", "."]

COPY ["./samples/Documents/Cryoland.Infrastructure.Documents.Web/Cryoland.Infrastructure.Documents.Web.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.csproj"
COPY ["./samples/Documents/Cryoland.Infrastructure.Documents.Web", "."]

WORKDIR "/src/."
RUN dotnet build "Cryoland.Infrastructure.Documents.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cryoland.Infrastructure.Documents.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cryoland.Infrastructure.Documents.Web.dll"]