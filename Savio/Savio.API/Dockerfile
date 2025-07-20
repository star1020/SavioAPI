# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Savio/Savio.API/Savio.API.csproj", "Savio/Savio.API/"]
COPY ["Savio/Savio.Core.Data/Savio.Core.Data.csproj", "Savio/Savio.Core.Data/"]
# ... copy other .csproj files as needed

# Restore dependencies
RUN dotnet restore "Savio/Savio.API/Savio.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/Savio/Savio.API"
RUN dotnet build "Savio.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
WORKDIR "/src/Savio/Savio.API"
RUN dotnet publish "Savio.API.csproj" -c Release -o /app/publish

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Savio.API.dll"]