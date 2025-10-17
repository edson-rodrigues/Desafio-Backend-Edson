# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Mottu.RentalService.sln", "./"]
COPY ["src/Mottu.API/Mottu.API.csproj", "src/Mottu.API/"]
COPY ["src/Mottu.Application/Mottu.Application.csproj", "src/Mottu.Application/"]
COPY ["src/Mottu.Domain/Mottu.Domain.csproj", "src/Mottu.Domain/"]
COPY ["src/Mottu.Infrastructure/Mottu.Infrastructure.csproj", "src/Mottu.Infrastructure/"]
COPY ["src/Mottu.Shared/Mottu.Shared.csproj", "src/Mottu.Shared/"]

# Restore dependencies
RUN dotnet restore "src/Mottu.API/Mottu.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/Mottu.API"
RUN dotnet build "Mottu.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Mottu.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

# Create storage directory
RUN mkdir -p /app/storage

ENTRYPOINT ["dotnet", "Mottu.API.dll"]

