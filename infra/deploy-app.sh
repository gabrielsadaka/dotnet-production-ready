#!/bin/bash

set -e

cd deploy-app

echo "Restoring packages"

npm install

echo "Applying changes"

echo $GOOGLE_CREDENTIALS | docker login -u _json_key --password-stdin https://gcr.io

pulumi stack select dev -c --non-interactive

pulumi config set gcp:project "$GOOGLE_PROJECT"
pulumi config set gcp:region "$GOOGLE_REGION"

pulumi up --stack dev --non-interactive --yes