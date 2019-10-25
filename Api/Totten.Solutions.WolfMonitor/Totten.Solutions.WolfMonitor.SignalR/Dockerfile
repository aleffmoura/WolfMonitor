FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Totten.Solutions.WolfMonitor.SignalR/Totten.Solutions.WolfMonitor.SignalR.csproj", "Totten.Solutions.WolfMonitor.SignalR/"]
RUN dotnet restore "Totten.Solutions.WolfMonitor.SignalR/Totten.Solutions.WolfMonitor.SignalR.csproj"
COPY . .
WORKDIR "/src/Totten.Solutions.WolfMonitor.SignalR"
RUN dotnet build "Totten.Solutions.WolfMonitor.SignalR.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Totten.Solutions.WolfMonitor.SignalR.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Totten.Solutions.WolfMonitor.SignalR.dll"]