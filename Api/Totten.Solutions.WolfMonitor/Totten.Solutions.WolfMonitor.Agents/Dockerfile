FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Totten.Solutions.WolfMonitor.Agents/Totten.Solutions.WolfMonitor.Agents.csproj", "Totten.Solutions.WolfMonitor.Agents/"]
RUN dotnet restore "Totten.Solutions.WolfMonitor.Agents/Totten.Solutions.WolfMonitor.Agents.csproj"
COPY . .
WORKDIR "/src/Totten.Solutions.WolfMonitor.Agents"
RUN dotnet build "Totten.Solutions.WolfMonitor.Agents.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Totten.Solutions.WolfMonitor.Agents.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Totten.Solutions.WolfMonitor.Agents.dll"]