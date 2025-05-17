FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["TennisTournament.API/TennisTournament.API.csproj", "TennisTournament.API/"]
RUN dotnet restore "TennisTournament.API/TennisTournament.API.csproj"
COPY . .
WORKDIR "/src/TennisTournament.API"
RUN dotnet build "TennisTournament.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "TennisTournament.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TennisTournament.API.dll"]
