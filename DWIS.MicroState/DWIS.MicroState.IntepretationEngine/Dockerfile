#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["DWIS.MicroState.IntepretationEngine/DWIS.MicroState.IntepretationEngine.csproj", "DWIS.MicroState.IntepretationEngine/"]
RUN dotnet restore "DWIS.MicroState.IntepretationEngine/DWIS.MicroState.IntepretationEngine.csproj"
COPY . .
WORKDIR "/src/DWIS.MicroState.IntepretationEngine"
RUN dotnet build "DWIS.MicroState.IntepretationEngine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DWIS.MicroState.IntepretationEngine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DWIS.MicroState.IntepretationEngine.dll"]