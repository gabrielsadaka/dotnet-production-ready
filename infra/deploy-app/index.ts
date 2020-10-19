// Copyright 2016-2020, Pulumi Corporation.  All rights reserved.

import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as docker from "@pulumi/docker";

const location = gcp.config.region || "australia-southeast1";

const config = new pulumi.Config();

const configFile = config.require("docker-config-file");

const imageName = config.require("image-name");

const gcrDockerProvider = new docker.Provider('gcr', {
    registryAuth: [{
        address: "gcr.io",
        configFile: configFile
    }],
});

// Used to get the image from the google cloud registry.  Output is required to make sure that the provider is in sync with this call.
const registryImage = pulumi.output(
    docker.getRegistryImage({
    name: `gcr.io/${gcp.config.project}/${imageName}:latest`,
}, {provider: gcrDockerProvider}));


// Using the value from the registryImage to pull the image if it's new, pullTriggers looks for a new sha.
var dockerImage = registryImage.apply(r => new docker.RemoteImage(`${imageName}-docker-image`, {
    name: r.name!,
    pullTriggers: [registryImage.sha256Digest!],
    keepLocally: true
}, {provider: gcrDockerProvider}));

// String used to force the update using the new image.
var truncatedSha = registryImage.sha256Digest.apply(d => imageName + "-" + d.substr(8,20));

const enableCloudRun = new gcp.projects.Service("EnableCloudRun", {
    service: "run.googleapis.com",
});

const enableIamAPI = new gcp.projects.Service("EnableIamApi", {
    service: "iam.googleapis.com",
});

const cloudRunServiceAccount = new gcp.serviceaccount.Account("weather-api-cloud-run", {
    accountId: "weather-api-cloud-run",
    description: "Weather API Cloud Run Service Account",
    displayName: "Weather API Cloud Run"
});

const cloudRunServiceAccountBinding = new gcp.serviceaccount.IAMBinding("weather-api-cloud-run", {
    serviceAccountId: cloudRunServiceAccount.email,
    members: [ "dotnet-production-ready-svc@dotnetproductionready.iam.gserviceaccount.com" ],
    role: "roles/iam.serviceAccountUser"
});

// Deploy to Cloud Run if there is a difference in the sha, denoted above.
const weatherApi = new gcp.cloudrun.Service("weather-api", {
    location,
    name: truncatedSha,
    template: {
        spec: {
            containers: [{
                image: dockerImage.name,
            }],
            serviceAccountName: cloudRunServiceAccount.email
        },
    },
}, {dependsOn: dockerImage});

// Open the service to public unrestricted access
const iamWeatherApi = new gcp.cloudrun.IamMember("weather-api-everyone", {
    service: weatherApi.name,
    location,
    role: "roles/run.invoker",
    member: "allUsers",
});

export const weatherApiUrl = weatherApi.status.url;

