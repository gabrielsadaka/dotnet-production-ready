import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

const config = new pulumi.Config();

export const appName = config.require("appName");
export const dbName = config.require("dbName");
export const dbUsername = config.require("dbUsername");
export const dbPassword = config.require("dbPassword");