import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

const location = gcp.config.region || "australia-southeast1";

const config = new pulumi.Config();

const configFile = config.require("docker-config-file");
const appName = config.require("app-name");
const gitSha = config.require("git-sha");
const googleCloudRunServiceAccount = config.require("google-run-service-account");

const weatherApi = new gcp.cloudrun.Service(appName, {
    location,
    name: appName,
    template: {
        spec: {
            containers: [{
                image: `gcr.io/${gcp.config.project}/${appName}:${gitSha}`,
            }],
            serviceAccountName: googleCloudRunServiceAccount
        },
    },
});

// Open the service to public unrestricted access
const iamWeatherApi = new gcp.cloudrun.IamMember(`${appName}-everyone`, {
    service: weatherApi.name,
    location,
    role: "roles/run.invoker",
    member: "allUsers",
});

export const weatherApiUrl = weatherApi.status.url;

