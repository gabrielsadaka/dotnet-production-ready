#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

echo "Connecting to Google Cloud SQL Proxy"

echo $GOOGLE_CREDENTIALS > keyfile.json

./cloud_sql_proxy -credential_file keyfile.json -instances="$GOOGLE_PROJECT:$GOOGLE_REGION:$DB_INSTANCE"=tcp:"$DB_PORT" &

RETRIES=10
until PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USERNAME -d $DB_NAME -c "select 1" > /dev/null 2>&1 || [ $RETRIES -eq 0 ]; do
  echo "Waiting for Google Cloud Run Proxy, $((RETRIES--)) remaining attempts..."
  sleep 1
done

echo "Google Cloud Run Proxy ready, running migrations"

# migrate the database
echo "Flyway is running pending migrations -if any- on the database"
flyway -url=jdbc:postgresql://"$DB_HOST":"$DB_PORT"/"$DB_NAME" -schemas="$DB_SCHEMA_NAME" -user="$DB_USERNAME" -password="$DB_PASSWORD" -locations="$DB_MIGRATIONS_LOCATION" migrate