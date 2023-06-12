
ubuntu 20.04

```bash
apt install libleptonica-dev tesseract-ocr -y
cd ~/YourProject # Place "x64" directory on the same level with "tessdata"
mkdir -p x64
mkdir -p tessdata
cd x64
ln -s /usr/lib/x86_64-linux-gnu/libleptonica.so libleptonica-1.82.0.so
ls -s /usr/lib/x86_64-linux-gnu/libtesseract.so.4 libtesseract41.so
cd ..

```


https://stackoverflow.com/questions/62614383/c-sharp-run-tesseract-or-any-command-in-linux

