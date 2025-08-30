#===================================
# Stage 1: Build 
#===================================


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
#===================================
# Copy Dependencies
#===================================

COPY Music.Domain/Music.Domain.csproj ./Music.Domain
RUN dotnet restore Music.Domain/Music.Domain.csproj

COPY Music.Application/Music.Application.csproj ./Music.Application
RUN dotnet restore Music.Application/Music.Application.csproj

COPY Music.Infrastructure/Music.Infrastructure.csproj ./Music.Infrastructure
RUN dotnet restore Music.Infrastructure/Music.Infrastructure.csproj

COPY Music.API/Music.API.csproj ./Music.API
RUN dotnet restore Music.API/Music.API.csproj
#===================================
# Copy rest of the codebase
#===================================
COPY Music.Domain/. ./Music.Domain/
COPY Music.Application/. ./Music.Application/
COPY Music.Infrastructure/. ./Music.Infrastructure/
COPY Music.API/. ./Music.API/

WORKDIR /src/Music.API
RUN dotnet publish -c Release -o app/publish --no-restore
#===================================
# Stage 2: Runtime
#===================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY  --from=build /src/Music.API/app/publish .
EXPOSE 80
ENTRYPOINT [ "dotnet","Music.API.dll" ]