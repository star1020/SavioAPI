FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only project files first (optimizes layer caching)
COPY ["Savio/Savio.API/Savio.API.csproj", "Savio/Savio.API/"]
COPY ["Savio/Savio.Core.Data/Savio.Core.Data.csproj", "Savio/Savio.Core.Data/"]
# ... (copy all other .csproj files)

# Restore dependencies
RUN dotnet restore "Savio/Savio.API/Savio.API.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/Savio/Savio.API"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Savio.API.dll"]