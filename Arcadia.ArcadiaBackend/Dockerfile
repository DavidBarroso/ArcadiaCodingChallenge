#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Arcadia.ArcadiaBackend/Arcadia.ArcadiaBackend.csproj", "Arcadia.ArcadiaBackend/"]
COPY ["Arcadia.OpenSkyRestClient/Arcadia.OpenSkyRestClient.csproj", "Arcadia.OpenSkyRestClient/"]
COPY ["Arcadia.Model/Arcadia.Model.csproj", "Arcadia.Model/"]
RUN dotnet restore "Arcadia.ArcadiaBackend/Arcadia.ArcadiaBackend.csproj"
COPY . .
WORKDIR "/src/Arcadia.ArcadiaBackend"
RUN dotnet build "Arcadia.ArcadiaBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arcadia.ArcadiaBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arcadia.ArcadiaBackend.dll"]
