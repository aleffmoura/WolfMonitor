FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Totten.Solutions.WolfMonitor.IdentityServer/Totten.Solutions.WolfMonitor.IdentityServer.csproj", "Totten.Solutions.WolfMonitor.IdentityServer/"]
RUN dotnet restore "Totten.Solutions.WolfMonitor.IdentityServer/Totten.Solutions.WolfMonitor.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Totten.Solutions.WolfMonitor.IdentityServer"
RUN dotnet build "Totten.Solutions.WolfMonitor.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Totten.Solutions.WolfMonitor.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Totten.Solutions.WolfMonitor.IdentityServer.dll"]