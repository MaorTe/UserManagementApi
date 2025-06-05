################################################################
# 1. Build stage
################################################################
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["UserManagementApi.csproj", "./"]
RUN dotnet restore "./UserManagementApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "UserManagementApi.csproj" -c Release -o /app/publish

################################################################
# 2. Runtime stage
################################################################
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Render routes 100% HTTPS ? you can still listen on 80 or 5000)
EXPOSE 80

# Run the .NET API
ENTRYPOINT ["dotnet", "UserManagementApi.dll"]