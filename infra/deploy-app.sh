#!/bin/bash

set -e

cd deploy-app

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi stack select dev -c --non-interactive

pulumi config set gcp:project "$GOOGLE_PROJECT"
pulumi config set gcp:region "$GOOGLE_REGION"
pulumi config set weather-api-dev-deploy-app:appName "$APP_NAME"
pulumi config set weather-api-dev-deploy-app:gitSha "$GITHUB_SHA"
pulumi config set weather-api-dev-deploy-app:googleRunServiceAccount "$GOOGLE_RUN_SERVICE_ACCOUNT"
pulumi config set weather-api-dev-deploy-app:dbInstance "$DB_INSTANCE"
pulumi config set weather-api-dev-deploy-app:environment "$ENVIRONMENT"


pulumi up --stack dev --non-interactive --yes