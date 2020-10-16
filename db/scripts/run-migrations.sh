#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

# clean the database
>&2 echo "Flyway is cleaning the database first"
flyway -url=jdbc:postgresql://"$HOST":"$PORT"/"$DATABASE_NAME" -schemas="$SCHEMA_NAME" -user="$USERNAME" -password="$PASSWORD" clean

# migrate the database
>&2 echo "Flyway is running pending migrations -if any- on the database"
flyway -url=jdbc:postgresql://"$HOST":"$PORT"/"$DATABASE_NAME" -schemas="$SCHEMA_NAME" -user="$USERNAME" -password="$PASSWORD" -locations="$MIGRATIONS_LOCATION" migrate