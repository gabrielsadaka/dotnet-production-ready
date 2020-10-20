#!/bin/bash

set -e

echo "Restoring packages"

npm --prefix infra/deploy-iam install infra/deploy-iam

echo "Applying changes"

pulumi stack select dev -c --non-interactive --cwd infra/deploy-iam

pulumi up --stack dev --non-interactive --yes --cwd infra/deploy-iam