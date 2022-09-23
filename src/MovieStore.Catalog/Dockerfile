#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MovieStore.Catalog/MovieStore.Catalog.csproj", "MovieStore.Catalog/"]
COPY ["MovieStore.Catalog.Services/MovieStore.Catalog.Services.csproj", "MovieStore.Catalog.Services/"]
COPY ["MovieStore.Catalog.Repository/MovieStore.Catalog.Repository.csproj", "MovieStore.Catalog.Repository/"]
COPY ["MovieStore.Catalog.Common/MovieStore.Catalog.Common.csproj", "MovieStore.Catalog.Common/"]
RUN dotnet restore "MovieStore.Catalog/MovieStore.Catalog.csproj"
COPY . .
WORKDIR "/src/MovieStore.Catalog"
RUN dotnet build "MovieStore.Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieStore.Catalog.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieStore.Catalog.dll"]