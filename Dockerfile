# --- FRONTEND BUILD ---
FROM node:20-alpine AS frontend
WORKDIR /frontend
COPY DevEstate.Web/package*.json ./
RUN npm ci
COPY DevEstate.Web/ ./
RUN npm run build

# --- BACKEND BUILD ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore DevEstate.Api/DevEstate.Api.csproj
RUN dotnet publish DevEstate.Api/DevEstate.Api.csproj -c Release -o /app/publish

# --- RUNTIME ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY --from=frontend /frontend/dist ./wwwroot

# Uploads folder (opcjonalnie – ale uwaga niżej)
COPY DevEstate.Api/Uploads ./Uploads

# Render daje PORT w env, najczęściej 10000
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "DevEstate.Api.dll"]
