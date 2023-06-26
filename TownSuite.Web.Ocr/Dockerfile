FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN apt update && apt install apt-transport-https -y \
    && echo "deb https://notesalexp.org/tesseract-ocr5/bullseye/ bullseye main" | tee /etc/apt/sources.list.d/notesalexp.list > /dev/null \
    && apt update -oAcquire::AllowInsecureRepositories=true \
    && apt install notesalexp-keyring -y --allow-unauthenticated \
    && apt update \ 
    && apt install libgdiplus libc6-dev tesseract-ocr libleptonica-dev libtesseract5 --allow-unauthenticated -y \
    && rm -rf /tmp/* \
    && apt-get purge -y --auto-remove wget \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TownSuite.Web.Ocr/TownSuite.Web.Ocr.csproj", "TownSuite.Web.Ocr/"]
RUN dotnet restore "TownSuite.Web.Ocr/TownSuite.Web.Ocr.csproj"
COPY . .
WORKDIR "/src/TownSuite.Web.Ocr"
RUN dotnet build "TownSuite.Web.Ocr.csproj" -c Release -o /app/build
RUN dotnet dev-certs https -ep /app/certs/aspnetapp.pfx -p A_W3AK_PLACEHOLDER_DEV_PASSWORD

FROM build AS publish
RUN dotnet publish "TownSuite.Web.Ocr.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /app/certs .
RUN mkdir -p x64 \
    && mkdir -p tessdata \
    && cd x64 \
    && ln -s /usr/lib/x86_64-linux-gnu/liblept.so.5.0.4 libleptonica-1.82.0.so \
    && ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.5.0.3 libtesseract50.so \
    && ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.5.0.3 tesseract50.so

ENTRYPOINT ["dotnet", "TownSuite.Web.Ocr.dll"]

# ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.5.0.3 tesseract50.so