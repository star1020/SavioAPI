FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Savio.API/Savio.API.csproj", "Savio.API/"]
RUN dotnet restore "Savio.API/Savio.API.csproj"

COPY . .
WORKDIR "/src/Savio.API"
RUN dotnet build "Savio.API.csproj" -c Release -o /app/build
RUN dotnet publish "Savio.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Savio.API.dll"]
