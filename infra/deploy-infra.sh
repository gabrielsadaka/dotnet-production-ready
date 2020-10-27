#!/bin/bash

set -e

cd deploy-infra

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi stack select dev -c --non-interactive

pulumi config set gcp:project "$GOOGLE_PROJECT"
pulumi config set gcp:region "$GOOGLE_REGION"
pulumi config set dotnet-prod-deploy-infra:appName "$APP_NAME"
pulumi config set dotnet-prod-deploy-infra:dbName "$DB_NAME"
pulumi config set dotnet-prod-deploy-infra:dbUsername "$DB_USERNAME"

pulumi up --stack dev --non-interactive --yes