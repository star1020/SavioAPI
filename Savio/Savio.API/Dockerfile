# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore
COPY ../../SavioAPI.sln ./
COPY ../Savio.API/Savio.API.csproj ./Savio.API/
RUN dotnet restore

# Copy the rest of the source code
COPY ../Savio.API/. ./Savio.API/
WORKDIR /src/Savio.API
RUN dotnet publish -c Release -o /app/publish

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Savio.API.dll"]
