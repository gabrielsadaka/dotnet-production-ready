#!/bin/bash

set -e

echo "Restoring packages"

npm --prefix infra/build-push install infra/deploy

echo "Applying changes"

pulumi stack select dev -c --non-interactive --cwd infra/deploy

pulumi up --stack dev --non-interactive --yes --cwd infra/deploy