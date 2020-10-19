#!/bin/bash

set -e

cd infra/build-push

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi stack select dev -c --non-interactive

pulumi up --stack dev --non-interactive --yes