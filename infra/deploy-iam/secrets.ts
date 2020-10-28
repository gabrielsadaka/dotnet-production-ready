import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as config from "./config";

export const enableSecretManagerApi = new gcp.projects.Service("EnableSecretManagerApi", {
    service: "secretmanager.googleapis.com",
});

export const databasePasswordSecret = new gcp.secretmanager.Secret(`${config.appName}-db-password`, {
    secretId: `${config.appName}-db-password`,
    replication: {
        automatic: true
    }
}, {dependsOn: enableSecretManagerApi});
