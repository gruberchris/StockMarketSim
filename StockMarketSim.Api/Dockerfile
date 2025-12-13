FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StockMarketSim.Api/StockMarketSim.Api.csproj", "StockMarketSim.Api/"]
RUN dotnet restore "StockMarketSim.Api/StockMarketSim.Api.csproj"
COPY . .
WORKDIR "/src/StockMarketSim.Api"
RUN dotnet build "./StockMarketSim.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StockMarketSim.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockMarketSim.Api.dll"]
