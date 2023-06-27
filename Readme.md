ubuntu 20.04

```bash
apt install libleptonica-dev tesseract-ocr libgdiplus libc6-dev -y
cd TownSuite.Web.Ocr # Place "x64" directory on the same level with "tessdata"
mkdir -p x64
mkdir -p tessdata
cd x64
ln -s /usr/lib/x86_64-linux-gnu/libleptonica.so libleptonica-1.82.0.so
ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.4 libtesseract41.so
cd ..

echo "download trained language models from https://github.com/tesseract-ocr/tessdata_fast"
```


If using https://notesalexp.org/tesseract-ocr/packages5/, see https://notesalexp.org/tesseract-ocr/html/.

```bash
cd TownSuite.Web.Ocr # Place "x64" directory on the same level with "tessdata"
mkdir -p x64
mkdir -p tessdata
cd x64
ln -s /usr/lib/x86_64-linux-gnu/libleptonica.so libleptonica-1.82.0.so
ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.5.0.3 libtesseract50.so
cd ..

echo "download trained language models from https://github.com/tesseract-ocr/tessdata_fast"
```z

mac

```bash
brew install tesseract

cd TownSuite.Web.Ocr # Place "x64" directory on the same level with "tessdata"
mkdir -p x64
mkdir -p tessdata
cd x64
ln -s /opt/homebrew/Cellar/leptonica/1.82.0_2/lib/liblept.5.dylib libleptonica-1.82.0.dylib
ln -s /opt/homebrew/Cellar/tesseract/5.3.1_1/lib/libtesseract.5.dylib libtesseract50.dylib
cd ..

cd tessdata

echo "download trained language models from https://github.com/tesseract-ocr/tessdata_fast"

```

https://stackoverflow.com/questions/62614383/c-sharp-run-tesseract-or-any-command-in-linux



# Docker


compose

```yaml
version: "3.8"
services:
  portal:
    image: "townsuite/ocr:latest"
    ports:
        - "31006:443"
    environment:
        ASPNETCORE_URLS: "https://+:443;http://+:80"
        # Do not use the default dev cert in production.   Map a volume and use a different cert.
        ASPNETCORE_Kestrel__Certificates__Default__Password: "A_W3AK_PLACEHOLDER_DEV_PASSWORD"
        ASPNETCORE_Kestrel__Certificates__Default__Path: "/app/aspnetapp.pfx"
        ASPNETCORE_URLS: "https://+:443;http://+:80"
        Settings__TessDataFolder: "./tessdata"
        Settings__TessLanguage: "eng"
        JWT__ValidAudience: "https://localhost:7169"
        JWT__ValidIssuer: "https://localhost:7169"
        JWT__Secret: "PLACEHOLDER_PLACEHOLDER_PLACEHOLDER_PLACEHOLDER_PLACEHOLDER_PLACEHOLDER"
        JWT__ValidInMinutes: 525960
        JWT__RefreshTokenValidInMinutes: 578556
```

Run it with:

```bash
docker-compose up
```

# Environment variable note
Note: For environment variables, on Windows you should use :; on non-Windows, use __ (double underscore).



