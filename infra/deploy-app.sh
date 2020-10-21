#!/bin/bash

set -e

cd deploy-app

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi stack select dev -c --non-interactive

pulumi config set gcp:project "$GOOGLE_PROJECT"
pulumi config set gcp:region "$GOOGLE_REGION"
pulumi config set dotnet-prod-deploy-app:app-name "$APP_NAME"
pulumi config set dotnet-prod-deploy-app:git-sha "$GITHUB_SHA"
pulumi config set dotnet-prod-deploy-app:google-run-service-account "$GOOGLE_RUN_SERVICE_ACCOUNT"

pulumi up --stack dev --non-interactive --yes