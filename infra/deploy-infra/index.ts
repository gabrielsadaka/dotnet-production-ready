import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as config from "./config";
import * as database from "./database";

const registry = new gcp.container.Registry("weather-registry");

export const registryUrn = registry.urn;
export const dbAddress = database.databaseInstance.firstIpAddress;
export const dbName = database.database.name;