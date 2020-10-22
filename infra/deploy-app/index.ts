import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

const location = gcp.config.region || "australia-southeast1";

const config = new pulumi.Config();

const configFile = config.require("dockerConfigFile");
const appName = config.require("appName");
const gitSha = config.require("gitSha");
const googleCloudRunServiceAccount = config.require("googleRunServiceAccount");

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

