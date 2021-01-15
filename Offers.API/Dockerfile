#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Offers.API/Offers.API.csproj", "Offers.API/"]
RUN dotnet restore "Offers.API/Offers.API.csproj"
COPY . .
WORKDIR "/src/Offers.API"
RUN dotnet build "Offers.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Offers.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Offers.API.dll"]