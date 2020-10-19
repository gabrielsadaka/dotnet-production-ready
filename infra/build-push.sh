#!/bin/bash

set -e

echo "Restoring packages"

npm --prefix infra/build-push install infra/build-push

echo "Applying changes"

echo $GOOGLE_CREDENTIALS | docker login -u _json_key --password-stdin https://gcr.io

pulumi stack select dev -c --non-interactive --cwd infra/build-push

pulumi up --stack dev --non-interactive --yes --cwd infra/build-push