#!/bin/bash

set -e

echo $GOOGLE_CREDENTIALS | docker login -u _json_key --password-stdin https://gcr.io

docker build . -t gcr.io/dotnet-prod/weather-api:"$GITHUB_SHA"

docker push gcr.io/dotnet-prod/weather-api:"$GITHUB_SHA"