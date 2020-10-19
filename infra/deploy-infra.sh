#!/bin/bash

set -e

echo "Restoring packages"

npm --prefix infra/deploy-infra install infra/deploy-infra

echo "Applying changes"

pulumi stack select dev -c --non-interactive --cwd infra/deploy-infra

pulumi up --stack dev --non-interactive --yes --cwd infra/deploy-infra