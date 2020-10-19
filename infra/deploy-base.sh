#!/bin/bash

set -e

cd base

echo "Restoring packages"

npm install

echo "Previewing changes"

pulumi up --stack dev --non-interactive --yes