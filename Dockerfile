# Stage 1: Build both APIs
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy and build Savio.API (port 5000)
COPY ./Savio/Savio.API/ ./Savio/Savio.API/
RUN dotnet publish "./Savio/Savio.API/Savio.API.csproj" -c Release -o /app/savio

# Copy and build User.API (port 5001)
COPY ./Savio/User.API/ ./Savio/User.API/
RUN dotnet publish "./Savio/User.API/User.API.csproj" -c Release -o /app/user

# Stage 2: Runtime with Nginx
FROM nginx:alpine

# Copy Nginx config
COPY ./Savio/nginx.conf /etc/nginx/conf.d/default.conf

# Copy published APIs
COPY --from=build /app/savio /usr/share/nginx/html/savio
COPY --from=build /app/user /usr/share/nginx/html/user

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]