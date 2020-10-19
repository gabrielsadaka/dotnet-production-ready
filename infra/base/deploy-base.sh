#!/bin/bash

set -e

echo "Restoring packages"

npm install

echo "Previewing changes"

pulumi preview --stack dev --non-interactive