#===================================
# Stage 1: Build 
#===================================


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
#===================================
# Copy Dependencies
#===================================

COPY MusicStream.Domain/MusicStream.Domain.csproj MusicStream.Domain/
COPY MusicStream.Application/MusicStream.Application.csproj MusicStream.Application/
COPY MusicStream.Infrastructure/MusicStream.Infrastructure.csproj MusicStream.Infrastructure/
COPY MusicStream.API/MusicStream.API.csproj MusicStream.API/
RUN dotnet restore MusicStream.API/MusicStream.API.csproj
#===================================
# Copy rest of the codebase
#===================================
COPY MusicStream.Domain/. ./MusicStream.Domain/
COPY MusicStream.Application/. ./MusicStream.Application/
COPY MusicStream.Infrastructure/. ./MusicStream.Infrastructure/
COPY MusicStream.API/. ./MusicStream.API/

WORKDIR /src/MusicStream.API
RUN dotnet publish -c Release -o app/publish
#===================================
# Stage 2: Runtime
#===================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY  --from=build /src/MusicStream.API/app/publish .
EXPOSE 80
ENTRYPOINT [ "dotnet","MusicStream.API.dll" ]