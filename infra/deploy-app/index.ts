import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

const location = gcp.config.region || "australia-southeast1";

const config = new pulumi.Config();

const configFile = config.require("dockerConfigFile");
const appName = config.require("appName");
const gitSha = config.require("gitSha");
const googleCloudRunServiceAccount = config.require("googleRunServiceAccount");
const dbInstance = config.require("dbInstance");
const dbName = config.require("dbName");
const dbUsername = config.require("dbUsername");
const dbPassword = config.require("dbPassword");

const cloudSqlInstance = `${gcp.config.project}:${gcp.config.region}:${dbInstance}`;

const weatherApi = new gcp.cloudrun.Service(appName, {
    location,
    name: appName,
    template: {
        spec: {
            containers: [{
                image: `gcr.io/${gcp.config.project}/${appName}:${gitSha}`,
                envs: [
                    {
                        name: "WEATHERDB__SOCKET_PATH",
                        value: "/cloudsql"
                    },
                    {
                        name: "WEATHERDB__INSTANCE_CONNECTION_NAME",
                        value: cloudSqlInstance
                    },
                    {
                        name: "WEATHERDB__NAME",
                        value: dbName
                    },
                    {
                        name: "WEATHERDB__USER",
                        value: dbUsername
                    },
                    {
                        name: "WEATHERDB__PASSWORD",
                        value: dbPassword
                    }
                ]
            }],
            serviceAccountName: googleCloudRunServiceAccount
        },
        metadata: {
            annotations: {
                "autoscaling.knative.dev/maxScale": "2",
                "run.googleapis.com/cloudsql-instances": cloudSqlInstance,
            }
        }
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

