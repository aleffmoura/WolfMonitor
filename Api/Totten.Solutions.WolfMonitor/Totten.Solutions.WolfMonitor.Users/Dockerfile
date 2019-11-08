FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Totten.Solutions.WolfMonitor.Users/Totten.Solutions.WolfMonitor.Users.csproj", "Totten.Solutions.WolfMonitor.Users/"]
RUN dotnet restore "Totten.Solutions.WolfMonitor.Users/Totten.Solutions.WolfMonitor.Users.csproj"
COPY . .
WORKDIR "/src/Totten.Solutions.WolfMonitor.Users"
RUN dotnet build "Totten.Solutions.WolfMonitor.Users.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Totten.Solutions.WolfMonitor.Users.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Totten.Solutions.WolfMonitor.Users.dll"]