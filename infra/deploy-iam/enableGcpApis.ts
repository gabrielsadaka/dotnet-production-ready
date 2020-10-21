import * as gcp from "@pulumi/gcp";

// Enable required GCP APIs
const enableCloudRunApi= new gcp.projects.Service("EnableCloudRunApi", {
    service: "run.googleapis.com",
});

const enableIamApi = new gcp.projects.Service("EnableIamApi", {
    service: "iam.googleapis.com",
});

const enableCloudResourceManagerApi = new gcp.projects.Service("EnableCloudResourceManagerApi", {
    service: "cloudresourcemanager.googleapis.com",
});

const enableContainerRegistryApi = new gcp.projects.Service("EnableContainerRegistryApi", {
    service: "containerregistry.googleapis.com",
});
