FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HomeVision.Gateway/HomeVision.Gateway.csproj", "HomeVision.Gateway/"]
COPY ["../shared/HomeVision.Common/HomeVision.Common.csproj", "../shared/HomeVision.Common/"]
RUN dotnet restore "HomeVision.Gateway/HomeVision.Gateway.csproj"
COPY . .
WORKDIR "/src/HomeVision.Gateway"
RUN dotnet build "HomeVision.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeVision.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeVision.Gateway.dll"]
