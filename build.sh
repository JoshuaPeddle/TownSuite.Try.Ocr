#!/bin/bash
set -e # exit on first error
set -u # exit on using unset variable

cwd=`pwd`

docker build -f TownSuite.Web.Ocr/Dockerfile -t townsuite/ocr --rm=true .
mkdir -p build

docker save townsuite/ocr > ./build/ocr.tar

VERSION=`cat TownSuite.Web.Ocr/TownSuite.Web.Ocr.csproj | grep "<Version>"  | sed 's/[^0-9.]*//g'`
echo "VERSION=$VERSION" > build/version.txt
GITHASH="$(git rev-parse --short HEAD)"
echo "GITHASH=$GITHASH" >> build/githash.txt

