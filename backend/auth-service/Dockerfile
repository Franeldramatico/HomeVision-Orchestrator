FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HomeVision.Auth/HomeVision.Auth.csproj", "HomeVision.Auth/"]
COPY ["../shared/HomeVision.Common/Home Vision.Common.csproj", "../shared/HomeVision.Common/"]
RUN dotnet restore "HomeVision.Auth/HomeVision.Auth.csproj"
COPY . .
WORKDIR "/src/HomeVision.Auth"
RUN dotnet build "HomeVision.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeVision.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeVision.Auth.dll"]
