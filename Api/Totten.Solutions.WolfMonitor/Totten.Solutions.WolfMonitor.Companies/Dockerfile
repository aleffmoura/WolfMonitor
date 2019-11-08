FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Totten.Solutions.WolfMonitor.Companies/Totten.Solutions.WolfMonitor.Companies.csproj", "Totten.Solutions.WolfMonitor.Companies/"]
RUN dotnet restore "Totten.Solutions.WolfMonitor.Companies/Totten.Solutions.WolfMonitor.Companies.csproj"
COPY . .
WORKDIR "/src/Totten.Solutions.WolfMonitor.Companies"
RUN dotnet build "Totten.Solutions.WolfMonitor.Companies.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Totten.Solutions.WolfMonitor.Companies.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Totten.Solutions.WolfMonitor.Companies.dll"]