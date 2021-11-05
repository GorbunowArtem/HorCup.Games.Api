FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src

COPY "HorCup.Games.Api.sln" "HorCup.Games.Api.sln"
COPY "src/HorCup.Games/HorCup.Games.csproj" "src/HorCup.Games/HorCup.Games.csproj"
COPY "src/HorCup.Games.External/HorCup.Games.External.csproj" "src/HorCup.Games.External/HorCup.Games.External.csproj"

COPY "tests/HorCup.Games.Tests/HorCup.Games.Tests.csproj" "tests/HorCup.Games.Tests/HorCup.Games.Tests.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

RUN dotnet restore "HorCup.Games.Api.sln"

COPY . .
WORKDIR /src/src/HorCup.Games

FROM build AS publish
RUN dotnet publish "HorCup.Games.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HorCup.Games.dll"]
