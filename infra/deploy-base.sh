#!/bin/bash

set -e

cd infra/base

echo "Restoring packages"

npm install

echo "Applying changes"

pulumi stack init dev

pulumi up --stack dev --non-interactive --yes