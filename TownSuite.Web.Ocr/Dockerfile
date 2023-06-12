FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TownSuite.Web.Ocr/TownSuite.Web.Ocr.csproj", "TownSuite.Web.Ocr/"]
RUN dotnet restore "TownSuite.Web.Ocr/TownSuite.Web.Ocr.csproj"
COPY . .
WORKDIR "/src/TownSuite.Web.Ocr"
RUN dotnet build "TownSuite.Web.Ocr.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TownSuite.Web.Ocr.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TownSuite.Web.Ocr.dll"]
