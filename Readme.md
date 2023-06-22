ubuntu 20.04

```bash
apt install libleptonica-dev tesseract-ocr -y
cd TownSuite.Web.Ocr # Place "x64" directory on the same level with "tessdata"
mkdir -p x64
mkdir -p tessdata
cd x64
ln -s /usr/lib/x86_64-linux-gnu/libleptonica.so libleptonica-1.82.0.so
ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.4 libtesseract41.so
cd ..

echo "download trained language models from https://github.com/tesseract-ocr/tessdata_fast"

```

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
