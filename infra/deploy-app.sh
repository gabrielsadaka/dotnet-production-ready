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
pulumi config set dotnet-prod-deploy-app:dbInstance "$DB_INSTANCE"
pulumi config set dotnet-prod-deploy-app:dbName "$DB_NAME"
pulumi config set dotnet-prod-deploy-app:dbUsername "$DB_USERNAME"
pulumi config set dotnet-prod-deploy-app:dbPassword "$DB_PASSWORD" --secret


pulumi up --stack dev --non-interactive --yes