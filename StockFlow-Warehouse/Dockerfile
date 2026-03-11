FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StockFlow-Warehouse/StockFlow-Warehouse.csproj", "StockFlow-Warehouse/"]
RUN dotnet restore "StockFlow-Warehouse/StockFlow-Warehouse.csproj"
COPY . .
WORKDIR "/src/StockFlow-Warehouse"
RUN dotnet build "./StockFlow-Warehouse.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StockFlow-Warehouse.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockFlow-Warehouse.dll"]
