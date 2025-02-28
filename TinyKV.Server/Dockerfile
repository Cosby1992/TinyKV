FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5009

ENV ASPNETCORE_URLS=http://+:5009

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["TinyKV.Server/TinyKV.Server.csproj", "TinyKV.Server/"]
RUN dotnet restore "TinyKV.Server/TinyKV.Server.csproj"
COPY . .
WORKDIR "/src/TinyKV.Server"
RUN dotnet build "TinyKV.Server.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "TinyKV.Server.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TinyKV.Server.dll"]
