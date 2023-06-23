#!/bin/bash
set -e # exit on first error
set -u # exit on using unset variable

VERSION=`cat build/version.txt`
GITHASH=`cat build/githash.txt`

# https://github.com/google/go-containerregistry/blob/main/cmd/crane/doc/crane.md
crane push build/ocr.tar townsuite/ocr:latest --insecure
crane push build/ocr.tar townsuite/ocr:$VERSION-$GITHASH --insecure

