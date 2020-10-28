import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as config from "./config";

export const databaseInstance = new gcp.sql.DatabaseInstance(`${config.appName}-db`, {
    name: `${config.appName}-db`,
    databaseVersion: "POSTGRES_12",
    settings: {
        tier: "db-f1-micro",
        ipConfiguration: {
            ipv4Enabled: true,
            requireSsl: true
        }
    },
});

export const database = new gcp.sql.Database(`${config.appName}-db`, {
    name: config.dbName,
    instance: databaseInstance.id
});