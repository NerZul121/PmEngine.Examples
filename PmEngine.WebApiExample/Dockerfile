#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN dotnet nuget add source http://87.249.53.45:5555/v3/index.json
COPY ["PmEngine.WebApiExample/PmEngine.WebApiExample.csproj", "PmEngine.WebApiExample/"]
RUN dotnet restore "PmEngine.WebApiExample/PmEngine.WebApiExample.csproj"
COPY . .
WORKDIR "/src/PmEngine.WebApiExample"
RUN dotnet build "PmEngine.WebApiExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PmEngine.WebApiExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PmEngine.WebApiExample.dll"]