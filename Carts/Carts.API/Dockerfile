#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Carts.API/Carts.API.csproj", "Carts.API/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "Carts.API/Carts.API.csproj"
COPY . .
WORKDIR "/src/Carts.API"
RUN dotnet build "Carts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Carts.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carts.API.dll"]