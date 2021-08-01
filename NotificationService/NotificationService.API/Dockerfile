#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NotificationService/NotificationService.API/NotificationService.API.csproj", "NotificationService/NotificationService.API/"]
COPY ["NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "NotificationService/NotificationService.Infrastructure/"]
COPY ["NotificationService/NotificationService.Domain/NotificationService.Domain.csproj", "NotificationService/NotificationService.Domain/"]
COPY ["Common/Common.Domain/Common.Domain.csproj", "Common/Common.Domain/"]
COPY ["Common/Common.Grpc/Common.Grpc.csproj", "Common/Common.Grpc/"]
COPY ["Common/Common.Utilities/Common.Utilities.csproj", "Common/Common.Utilities/"]
COPY ["Common/Common.Dto/Common.Dto.csproj", "Common/Common.Dto/"]
RUN dotnet restore "NotificationService/NotificationService.API/NotificationService.API.csproj"
COPY . .
WORKDIR "/src/NotificationService/NotificationService.API"
RUN dotnet build "NotificationService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]