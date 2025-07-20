# Must use Windows Server Core base image
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /src

# Copy and restore NuGet packages
COPY ./Savio/Savio.API/ .
RUN nuget restore

# Build the project
RUN msbuild "Savio.API.csproj" /p:Configuration=Release /p:OutputPath=/app

# Runtime stage
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8
WORKDIR /inetpub/wwwroot
COPY --from=build /app .

# Expose port (IIS will listen on 80)
EXPOSE 80