#!/bin/bash

set -e

cd deploy-iam

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi stack select dev -c --non-interactive

pulumi config set gcp:project "$GOOGLE_PROJECT"
pulumi config set gcp:region "$GOOGLE_REGION"

pulumi up --stack dev --non-interactive --yes