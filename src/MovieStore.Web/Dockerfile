FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MovieStore.Web/MovieStore.Web.csproj", "MovieStore.Web/"]
RUN dotnet restore "MovieStore.Web/MovieStore.Web.csproj"
COPY . .
WORKDIR "/src/MovieStore.Web"
RUN dotnet build "MovieStore.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieStore.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieStore.Web.dll"]