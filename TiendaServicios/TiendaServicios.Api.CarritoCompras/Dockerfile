#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TiendaServicios.Api.CarritoCompras/TiendaServicios.Api.CarritoCompra.csproj", "TiendaServicios.Api.CarritoCompras/"]
RUN dotnet restore "TiendaServicios.Api.CarritoCompras/TiendaServicios.Api.CarritoCompra.csproj"
COPY . .
WORKDIR "/src/TiendaServicios.Api.CarritoCompras"
RUN dotnet build "TiendaServicios.Api.CarritoCompra.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TiendaServicios.Api.CarritoCompra.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TiendaServicios.Api.CarritoCompra.dll"]
