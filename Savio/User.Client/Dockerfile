FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["User.Client/User.Client.csproj", "User.Client/"]
RUN dotnet restore "User.Client/User.Client.csproj"

COPY . .
WORKDIR "/src/User.Client"
RUN dotnet build "User.Client.csproj" -c Release -o /app/build
RUN dotnet publish "User.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "User.Client.dll"]
