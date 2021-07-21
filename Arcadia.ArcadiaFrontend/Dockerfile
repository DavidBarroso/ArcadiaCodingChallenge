#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Arcadia.ArcadiaFrontend/Arcadia.ArcadiaFrontend.csproj", "Arcadia.ArcadiaFrontend/"]
COPY ["Arcadia.Model/Arcadia.Model.csproj", "Arcadia.Model/"]
RUN dotnet restore "Arcadia.ArcadiaFrontend/Arcadia.ArcadiaFrontend.csproj"
COPY . .
WORKDIR "/src/Arcadia.ArcadiaFrontend"
RUN dotnet build "Arcadia.ArcadiaFrontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arcadia.ArcadiaFrontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arcadia.ArcadiaFrontend.dll"]
