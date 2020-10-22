#!/bin/bash

set -e

cd deploy-app

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi stack select dev -c --non-interactive

pulumi config set gcp:project "$GOOGLE_PROJECT"
pulumi config set gcp:region "$GOOGLE_REGION"
pulumi config set dotnet-prod-deploy-app:appName "$APP_NAME"
pulumi config set dotnet-prod-deploy-app:gitSha "$GITHUB_SHA"
pulumi config set dotnet-prod-deploy-app:googleRunServiceAccount "$GOOGLE_RUN_SERVICE_ACCOUNT"

pulumi up --stack dev --non-interactive --yes