#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

RUN mkdir -p /app/config
RUN chmod -R 755 /app/config

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DWIS.MicroState.ThresholdsServer/config/Quickstarts.ReferenceClient.Config.xml", "/app/config/"]
COPY ["DWIS.MicroState.Model/DWIS.MicroState.Model.csproj", "DWIS.MicroState.Model/"]
COPY ["DWIS.MicroState.ThresholdsServer/DWIS.MicroState.ThresholdsServer.csproj", "DWIS.MicroState.ThresholdsServer/"]
RUN dotnet restore "DWIS.MicroState.ThresholdsServer/DWIS.MicroState.ThresholdsServer.csproj"
COPY . .
WORKDIR "/src/DWIS.MicroState.ThresholdsServer"
RUN dotnet build "DWIS.MicroState.ThresholdsServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DWIS.MicroState.ThresholdsServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
VOLUME /home
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DWIS.MicroState.ThresholdsServer.dll"]