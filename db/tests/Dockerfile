FROM postgres:13

RUN apt-get update -y

RUN apt-get install -y curl && apt-get install -y default-jre

RUN curl https://repo1.maven.org/maven2/org/flywaydb/flyway-commandline/7.3.1/flyway-commandline-7.3.1-linux-x64.tar.gz \
    | tar xvz && ln -s `pwd`/flyway-7.3.1/flyway /usr/local/bin