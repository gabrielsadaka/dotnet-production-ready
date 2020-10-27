import * as gcp from "@pulumi/gcp";

// Enable required GCP APIs
export const enableCloudRunApi= new gcp.projects.Service("EnableCloudRunApi", {
    service: "run.googleapis.com",
});

export const enableIamApi = new gcp.projects.Service("EnableIamApi", {
    service: "iam.googleapis.com",
});

export const enableCloudResourceManagerApi = new gcp.projects.Service("EnableCloudResourceManagerApi", {
    service: "cloudresourcemanager.googleapis.com",
});

export const enableContainerRegistryApi = new gcp.projects.Service("EnableContainerRegistryApi", {
    service: "containerregistry.googleapis.com",
});

export const enableSqlAdminApi = new gcp.projects.Service("EnableSqlAdminApi", {
    service: "sqladmin.googleapis.com",
});

export const enableSecretManagerApi = new gcp.projects.Service("EnableSecretManagerApi", {
    service: "secretmanager.googleapis.com",
});
