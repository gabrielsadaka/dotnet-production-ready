#!/bin/bash

set -e

echo $GOOGLE_CREDENTIALS | docker login -u _json_key --password-stdin https://gcr.io

IMAGE_TAG=gcr.io/"$GOOGLE_PROJECT"/"$APP_NAME":"$GITHUB_SHA"

docker build . -t "$IMAGE_TAG"

docker run --rm -v /var/run/docker.sock:/var/run/docker.sock aquasec/trivy \
    --exit-code 1 --no-progress --severity CRITICAL "$IMAGE_TAG"

docker run --rm  --rm -v /var/run/docker.sock:/var/run/docker.sock -i goodwithtech/dockle \
     --exit-code 1 --exit-level warn "$IMAGE_TAG"

docker push "$IMAGE_TAG"