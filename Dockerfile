FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app

EXPOSE 3001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src

COPY ["WebBanHang.csproj", ""]

RUN dotnet restore "./WebBanHang.csproj"

COPY . .

WORKDIR "/src/."

RUN dotnet build "WebBanHang.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "WebBanHang.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebBanHang.dll"]
