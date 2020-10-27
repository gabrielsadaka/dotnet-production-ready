import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as config from "./config";
import * as database from "./database";

const registry = new gcp.container.Registry("weather-registry");

const databasePasswordSecret = new gcp.secretmanager.Secret(`${config.appName}-db-pass`, {
    secretId: `${config.appName}-db-pass`,
    replication: {
        automatic: true
    }
});

export const registryUrn = registry.urn;
export const dbAddress = database.databaseInstance.firstIpAddress;
export const dbName = database.database.name;