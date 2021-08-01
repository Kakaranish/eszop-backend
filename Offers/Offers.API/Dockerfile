#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Offers/Offers.API/Offers.API.csproj", "Offers/Offers.API/"]
COPY ["Offers/Offers.Infrastructure/Offers.Infrastructure.csproj", "Offers/Offers.Infrastructure/"]
COPY ["Common/Common.Grpc/Common.Grpc.csproj", "Common/Common.Grpc/"]
COPY ["Offers/Offers.Domain/Offers.Domain.csproj", "Offers/Offers.Domain/"]
COPY ["Common/Common.Domain/Common.Domain.csproj", "Common/Common.Domain/"]
COPY ["Common/Common.Utilities/Common.Utilities.csproj", "Common/Common.Utilities/"]
COPY ["Common/Common.Dto/Common.Dto.csproj", "Common/Common.Dto/"]
RUN dotnet restore "Offers/Offers.API/Offers.API.csproj"
COPY . .
WORKDIR "/src/Offers/Offers.API"
RUN dotnet build "Offers.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Offers.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Offers.API.dll"]