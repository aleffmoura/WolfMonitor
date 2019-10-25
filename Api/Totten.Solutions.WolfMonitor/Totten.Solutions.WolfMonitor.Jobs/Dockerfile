FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Totten.Solutions.WolfMonitor.Jobs/Totten.Solutions.WolfMonitor.Jobs.csproj", "Totten.Solutions.WolfMonitor.Jobs/"]
RUN dotnet restore "Totten.Solutions.WolfMonitor.Jobs/Totten.Solutions.WolfMonitor.Jobs.csproj"
COPY . .
WORKDIR "/src/Totten.Solutions.WolfMonitor.Jobs"
RUN dotnet build "Totten.Solutions.WolfMonitor.Jobs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Totten.Solutions.WolfMonitor.Jobs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Totten.Solutions.WolfMonitor.Jobs.dll"]