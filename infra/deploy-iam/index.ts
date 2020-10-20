import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

const config = new pulumi.Config();

const appName = process.env.APP_NAME;

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
const cloudRunServiceAccount = new gcp.serviceaccount.Account(`${appName}-cloud-run`, {
    accountId: `${appName}-cloud-run`,
    description: `${appName} cloud run service account`,
    displayName: `${appName} cloud run`
});

const cloudRunServiceAccountBinding = new gcp.serviceaccount.IAMBinding(`${appName}-cloud-run`, {
    serviceAccountId: cloudRunServiceAccount.id,
    members: [ pulumi.interpolate`serviceAccount:${ciServiceAccount.email}` ],
    role: "roles/iam.serviceAccountUser"
});