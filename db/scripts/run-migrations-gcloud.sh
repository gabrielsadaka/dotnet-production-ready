#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

echo "Connecting to Google Cloud SQL Proxy"

echo $GOOGLE_CREDENTIALS > keyfile.json

./cloud_sql_proxy -credential_file keyfile.json -instances="$GOOGLE_PROJECT:$GOOGLE_REGION:$DB_INSTANCE"=tcp:"$DB_PORT"

# migrate the database
>&2 echo "Flyway is running pending migrations -if any- on the database"
flyway -url=jdbc:postgresql://"$DB_HOST":"$DB_PORT"/"$DB_NAME" -schemas="$DB_SCHEMA_NAME" -user="$DB_USERNAME" -password="$DB_PASSWORD" -locations="$DB_MIGRATIONS_LOCATION" migrate