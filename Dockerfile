# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# Copy csproj files first (for faster restore)
COPY Savio/Savio.API/*.csproj Savio/Savio.API/
COPY Savio/Savio.Core/*.csproj Savio/Savio.Core/
COPY Savio/User.Contract/*.csproj Savio/User.Contract/
COPY Savio/User.Proxy/*.csproj Savio/User.Proxy/

# Restore dependencies
RUN dotnet restore Savio/Savio.API/Savio.API.csproj

# Copy full source
COPY Savio/. ./Savio/

WORKDIR /src/Savio/Savio.API
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000
EXPOSE 1001

ENTRYPOINT ["dotnet", "Savio.API.dll"]
