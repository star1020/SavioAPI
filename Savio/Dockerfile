FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .

# Build both APIs
RUN dotnet publish Savio/Savio.API/Savio.API.csproj -c Release -o /app/SavioAPI
RUN dotnet publish Savio/User.API/User.API.csproj -c Release -o /app/UserAPI

FROM base AS final
WORKDIR /app
COPY --from=build /app/SavioAPI ./SavioAPI
COPY --from=build /app/UserAPI ./UserAPI
COPY start.sh .

RUN chmod +x start.sh
CMD ["./start.sh"]
