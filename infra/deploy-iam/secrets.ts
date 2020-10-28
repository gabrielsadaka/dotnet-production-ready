import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as config from "./config";
import * as enableGcpApis from "./enableGcpApis";

export const databasePasswordSecret = new gcp.secretmanager.Secret(`${config.appName}-db-password`, {
    secretId: `${config.appName}-db-password`,
    replication: {
        automatic: true
    }
}, {dependsOn: enableGcpApis.enableSecretManagerApi});
