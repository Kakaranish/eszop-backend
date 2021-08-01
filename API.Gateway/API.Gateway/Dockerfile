#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API.Gateway/API.Gateway/API.Gateway.csproj", "API.Gateway/API.Gateway/"]
COPY ["Common/Common.Utilities/Common.Utilities.csproj", "Common/Common.Utilities/"]
COPY ["Common/Common.Domain/Common.Domain.csproj", "Common/Common.Domain/"]
COPY ["Common/Common.Dto/Common.Dto.csproj", "Common/Common.Dto/"]
RUN dotnet restore "API.Gateway/API.Gateway/API.Gateway.csproj"
COPY . .
WORKDIR "/src/API.Gateway/API.Gateway"
RUN dotnet build "API.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Gateway.dll"]