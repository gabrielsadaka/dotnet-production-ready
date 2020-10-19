#!/bin/bash

set -e

echo "Restoring packages"

npm --prefix infra/base install infra/base

echo "Applying changes"

pulumi stack select dev -c --non-interactive -cwd infra/base

pulumi up --stack dev --non-interactive --yes -cwd infra/base