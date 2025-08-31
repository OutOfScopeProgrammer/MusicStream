

#===================================
# Stage 1: Build
#===================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first to cache restore
COPY MusicStream.Domain/MusicStream.Domain.csproj MusicStream.Domain/
COPY MusicStream.Application/MusicStream.Application.csproj MusicStream.Application/
COPY MusicStream.Infrastructure/MusicStream.Infrastructure.csproj MusicStream.Infrastructure/
COPY MusicStream.API/MusicStream.API.csproj MusicStream.API/
RUN dotnet restore MusicStream.API/MusicStream.API.csproj

# Copy the rest of the code
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


# Copy published app
COPY --from=build /src/MusicStream.API/app/publish .
COPY docker-binaries/ffmpeg /usr/local/bin/ffmpeg
COPY docker-binaries/ffprobe /usr/local/bin/ffprobe 

EXPOSE 5000
ENTRYPOINT ["dotnet", "MusicStream.API.dll"]
