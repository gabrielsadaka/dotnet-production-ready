#!/bin/bash

set -e

cd infra/build-push

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi up --stack dev --non-interactive --yes