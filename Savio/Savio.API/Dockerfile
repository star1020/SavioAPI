# Base image with SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# Copy only the API project
COPY Savio/Savio.API/*.csproj ./Savio.API/

RUN dotnet restore Savio.API/Savio.API.csproj

# Copy full source of API project
COPY Savio/Savio.API/ ./Savio.API/

WORKDIR /src/Savio.API
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000
EXPOSE 1001

ENTRYPOINT ["dotnet", "Savio.API.dll"]
