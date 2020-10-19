#!/bin/bash

set -e

echo "Restoring packages"

npm --prefix infra/deploy-app install infra/deploy-app

echo "Applying changes"

pulumi stack select dev -c --non-interactive --cwd infra/deploy-app

pulumi up --stack dev --non-interactive --yes --cwd infra/deploy-app