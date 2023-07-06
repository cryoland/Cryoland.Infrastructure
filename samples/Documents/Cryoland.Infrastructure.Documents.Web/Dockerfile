# Use it with docker-compose at ../
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS publish

WORKDIR /src/Documents/Cryoland.Infrastructure.Documents.Abstractions
COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Abstractions/Cryoland.Infrastructure.Documents.Abstractions.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.Abstractions.csproj" --runtime alpine-x64
COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Abstractions", "."]

WORKDIR /src/Documents/Cryoland.Infrastructure.Documents.Services
COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Services/Cryoland.Infrastructure.Documents.Services.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.Services.csproj" --runtime alpine-x64
COPY ["./src/Documents/Cryoland.Infrastructure.Documents.Services", "."]

WORKDIR /src/Documents/Cryoland.Infrastructure.Documents
COPY ["./src/Documents/Cryoland.Infrastructure.Documents/Cryoland.Infrastructure.Documents.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.csproj" --runtime alpine-x64
COPY ["./src/Documents/Cryoland.Infrastructure.Documents", "."]

WORKDIR /samples/Documents/Cryoland.Infrastructure.Documents.Web
COPY ["./samples/Documents/Cryoland.Infrastructure.Documents.Web/Cryoland.Infrastructure.Documents.Web.csproj", "."]
RUN dotnet restore "./Cryoland.Infrastructure.Documents.Web.csproj" --runtime alpine-x64
COPY ["./samples/Documents/Cryoland.Infrastructure.Documents.Web", "."]

RUN dotnet publish "Cryoland.Infrastructure.Documents.Web.csproj" --runtime alpine-x64 \
	-c Release \
	-o /publish \
	--self-contained true \
	-p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine AS final
WORKDIR /application
# create a new user and change directory ownership
RUN adduser --disabled-password \
  --home /application \
  --gecos '' dotnetuser && chown -R dotnetuser /application
# upgrade musl to remove potential vulnerability
RUN apk upgrade musl
# impersonate into the new user
USER dotnetuser
EXPOSE 80
COPY --from=publish /publish .
ENTRYPOINT ["./Cryoland.Infrastructure.Documents.Web"]
