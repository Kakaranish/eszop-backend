#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Carts/Carts.API/Carts.API.csproj", "Carts/Carts.API/"]
COPY ["Common/Common.Grpc/Common.Grpc.csproj", "Common/Common.Grpc/"]
COPY ["Common/Common.Utilities/Common.Utilities.csproj", "Common/Common.Utilities/"]
COPY ["Common/Common.Domain/Common.Domain.csproj", "Common/Common.Domain/"]
COPY ["Common/Common.Dto/Common.Dto.csproj", "Common/Common.Dto/"]
COPY ["Carts/Carts.Infrastructure/Carts.Infrastructure.csproj", "Carts/Carts.Infrastructure/"]
COPY ["Carts/Carts.Domain/Carts.Domain.csproj", "Carts/Carts.Domain/"]
RUN dotnet restore "Carts/Carts.API/Carts.API.csproj"
COPY . .
WORKDIR "/src/Carts/Carts.API"
RUN dotnet build "Carts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Carts.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carts.API.dll"]