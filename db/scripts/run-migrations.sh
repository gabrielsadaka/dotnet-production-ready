#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

RETRIES=10
until PGPASSWORD=$PASSWORD psql -h $HOST -U $USERNAME -d $DATABASE_NAME -c "select 1" > /dev/null 2>&1 || [ $RETRIES -eq 0 ]; do
  echo "Waiting for postgres server, $((RETRIES--)) remaining attempts..."
  sleep 1
done

echo "Postgres ready, running migrations"

# clean the database
>&2 echo "Flyway is cleaning the database first"
flyway -url=jdbc:postgresql://"$HOST":"$PORT"/"$DATABASE_NAME" -schemas="$SCHEMA_NAME" -user="$USERNAME" -password="$PASSWORD" clean

# migrate the database
>&2 echo "Flyway is running pending migrations -if any- on the database"
flyway -url=jdbc:postgresql://"$HOST":"$PORT"/"$DATABASE_NAME" -schemas="$SCHEMA_NAME" -user="$USERNAME" -password="$PASSWORD" -locations="$MIGRATIONS_LOCATION" migrate