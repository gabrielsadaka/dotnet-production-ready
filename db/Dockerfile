FROM flyway/flyway:7.15

USER root

RUN apt-get update -y && apt-get install -y gnupg2 && \
    echo "deb [signed-by=/usr/share/keyrings/cloud.google.gpg] http://packages.cloud.google.com/apt cloud-sdk main" | tee -a /etc/apt/sources.list.d/google-cloud-sdk.list && \
    curl https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key --keyring /usr/share/keyrings/cloud.google.gpg  add - && \
    apt-get update -y && apt-get install google-cloud-sdk -y

RUN apt-get update -y && apt-get install -y postgresql-client

RUN curl https://dl.google.com/cloudsql/cloud_sql_proxy.linux.amd64 --output cloud_sql_proxy && \
    chmod +x cloud_sql_proxy

USER flyway