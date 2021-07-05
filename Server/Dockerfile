#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Server/Nexus.Server.csproj", "Server/"]
COPY ["Shared/Nexus.Shared.csproj", "Shared/"]
COPY ["Client/Nexus.Client.csproj", "Client/"]
RUN dotnet restore "Server/Nexus.Server.csproj"
COPY . .
WORKDIR "/src/Server"

# Install Node
RUN apt-get update
RUN apt-get install -y nodejs npm


RUN dotnet build "Nexus.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexus.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN if [ ! -f "/app/wwwroot/Nexus.Client.styles.css" ]; then touch "/app/wwwroot/Nexus.Client.styles.css"; fi # 404 If this file is missing
ENTRYPOINT ["dotnet", "Nexus.Server.dll"]