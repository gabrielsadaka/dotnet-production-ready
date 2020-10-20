import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

const config = new pulumi.Config();

const imageName = config.require("image-name");

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

// Setup CI service account
const ciServiceAccount = new gcp.serviceaccount.Account(`ci-svc`, {
    accountId: `ci-svc`,
    description: `CI Service Account`,
    displayName: `CI Service Account`
});

const storageAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-storage-admin`, {
    members: [pulumi.interpolate`serviceAccount:${ciServiceAccount.email}`],
    role: "roles/storage.admin"
}, {parent: ciServiceAccount});

const cloudRunAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-run-admin`, {
    members: [pulumi.interpolate`serviceAccount:${ciServiceAccount.email}`],
    role: "roles/run.admin"
}, {parent: ciServiceAccount});

// Setup cloud run service account
const cloudRunServiceAccount = new gcp.serviceaccount.Account(`${imageName}-cloud-run`, {
    accountId: `${imageName}-cloud-run`,
    description: `${imageName} cloud run service account`,
    displayName: `${imageName} cloud run`
});

export const cloudRunServiceAccountName = cloudRunServiceAccount.name;