#!/bin/bash

set -e

echo "Getting database password from Google Cloud Secrets Manager"

echo $GOOGLE_CREDENTIALS > keyfile.json

gcloud auth activate-service-account --key-file keyfile.json

DB_PASSWORD=$(gcloud secrets versions access "$DB_PASSWORD_SECRET_VERSION" --project "$GOOGLE_PROJECT" --secret="$DB_PASSWORD_SECRET_ID")

echo "Connecting to Google Cloud SQL Proxy"

./cloud_sql_proxy -credential_file keyfile.json -instances="$GOOGLE_PROJECT:$GOOGLE_REGION:$DB_INSTANCE"=tcp:"$DB_PORT" &

RETRIES=10
until PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USERNAME -d $DB_NAME -c "select 1" > /dev/null 2>&1 || [ $RETRIES -eq 0 ]; do
  echo "Waiting for Google Cloud Run Proxy, $((RETRIES--)) remaining attempts..."
  sleep 1
done

echo "Google Cloud Run Proxy ready, running migrations using flyway"

flyway -url=jdbc:postgresql://"$DB_HOST":"$DB_PORT"/"$DB_NAME" -schemas="$DB_SCHEMA_NAME" -user="$DB_USERNAME" -password="$DB_PASSWORD" -locations="$DB_MIGRATIONS_LOCATION" migrate