FROM flyway/flyway:7.0

USER root

RUN apt-get update && apt-get install -y postgresql-client

RUN curl https://dl.google.com/cloudsql/cloud_sql_proxy.linux.amd64 --output cloud_sql_proxy

RUN chmod +x cloud_sql_proxy

USER flyway