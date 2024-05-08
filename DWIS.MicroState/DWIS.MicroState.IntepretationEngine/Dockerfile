#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DWIS.MicroState/DWIS.MicroState.Model/DWIS.MicroState.Model.csproj", "DWIS.MicroState.Model/"]
COPY ["DWIS.MicroState/DWIS.MicroState.IntepretationEngine/DWIS.MicroState.IntepretationEngine.csproj", "DWIS.MicroState.IntepretationEngine/"]
RUN dotnet restore "DWIS.MicroState.IntepretationEngine/DWIS.MicroState.IntepretationEngine.csproj"
COPY . .
WORKDIR "/src/DWIS.MicroState.IntepretationEngine"
RUN dotnet build "DWIS.MicroState.IntepretationEngine.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DWIS.MicroState.IntepretationEngine.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
VOLUME /home
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DWIS.MicroState.IntepretationEngine.dll"]