# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["WebApplication1/ForgottenEmpire.csproj", "WebApplication1/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "WebApplication1/ForgottenEmpire.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
WORKDIR "/src/WebApplication1"
RUN dotnet publish "ForgottenEmpire.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Install curl for Docker healthcheck
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Create database directory
RUN mkdir -p /app/database

ENTRYPOINT ["dotnet", "ForgottenEmpire.dll"]
