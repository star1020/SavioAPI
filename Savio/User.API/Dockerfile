FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["User.API/User.API.csproj", "User.API/"]
RUN dotnet restore "User.API/User.API.csproj"

COPY . .
WORKDIR "/src/User.API"
RUN dotnet build "User.API.csproj" -c Release -o /app/build
RUN dotnet publish "User.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "User.API.dll"]
