import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

const registry = new gcp.container.Registry("weather-registry");

export const bucketName = registry.urn;
