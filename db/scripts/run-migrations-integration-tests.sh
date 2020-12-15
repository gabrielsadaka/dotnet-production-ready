#!/bin/bash

set -e

RETRIES=10
until PGPASSWORD=$POSTGRES_PASSWORD psql -h $POSTGRES_HOST -U $POSTGRES_USER -d $POSTGRES_DB -c "select 1" > /dev/null 2>&1 || [ $RETRIES -eq 0 ]; do
  echo "Waiting for postgres server, $((RETRIES--)) remaining attempts..."
  sleep 1
done

echo "Postgres ready, running migrations"

# migrate the database
>&2 echo "Flyway is running pending migrations -if any- on the database"
flyway -url=jdbc:postgresql://"$POSTGRES_HOST":"$POSTGRES_PORT"/"$POSTGRES_DB" -schemas="$POSTGRES_SCHEMA_NAME" -user="$POSTGRES_USER" -password="$POSTGRES_PASSWORD" -locations="$POSTGRES_MIGRATIONS_LOCATION" migrate