#!/bin/bash

set -e

IMAGE_TAG=gcr.io/"$GOOGLE_PROJECT"/"$APP_NAME":"$GITHUB_SHA"

docker build . -t "$IMAGE_TAG"

docker run --rm -v /var/run/docker.sock:/var/run/docker.sock aquasec/trivy:0.12.0 \
    --exit-code 1 --no-progress --severity CRITICAL "$IMAGE_TAG"

docker run --rm  --rm -v /var/run/docker.sock:/var/run/docker.sock -i goodwithtech/dockle:v0.3.1 \
     --exit-code 1 --exit-level warn "$IMAGE_TAG"
