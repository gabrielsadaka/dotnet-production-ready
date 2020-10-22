import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as config from "./config";

const registry = new gcp.container.Registry("weather-registry");

const databaseInstance = new gcp.sql.DatabaseInstance(`${config.appName}-db`, {
    databaseVersion: "POSTGRES_12",
    settings: {
        tier: "db-f1-micro",
        ipConfiguration: {
            ipv4Enabled: true,
            requireSsl: true
        }
    },
});

const databaseUser = new gcp.sql.User(`${config.appName}-db-user`, {
    instance: databaseInstance.name,
    name: config.dbUsername,
    password: config.dbPassword,
});

export const registryUrn = registry.urn;