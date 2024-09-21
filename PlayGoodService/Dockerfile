# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore only the PlayGoodService project
COPY PlayGoodService/PlayGoodService.csproj ./PlayGoodService/
RUN dotnet restore ./PlayGoodService/PlayGoodService.csproj

# Copy the entire PlayGoodService directory after restoring the dependencies
COPY ./PlayGoodService ./PlayGoodService
WORKDIR /src/PlayGoodService

# Build the project in Release mode
RUN dotnet build PlayGoodService.csproj -c Release -o /app/build

# Publish the project
RUN dotnet publish PlayGoodService.csproj -c Release -o /app/publish

# Use the ASP.NET Core runtime to run the published app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "PlayGoodService.dll"]
