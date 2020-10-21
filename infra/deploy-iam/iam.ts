import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as config from "./config";

// Setup CI service account
const ciServiceAccount = new gcp.serviceaccount.Account(`ci-svc`, {
    accountId: `ci-svc`,
    description: `CI Service Account`,
    displayName: `CI Service Account`
});

const ciServiceAccountEmail = pulumi.interpolate`serviceAccount:${ciServiceAccount.email}`;

const storageAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-storage-admin`, {
    members: [ciServiceAccountEmail],
    role: "roles/storage.admin"
}, {parent: ciServiceAccount});

const cloudRunAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-run-admin`, {
    members: [ciServiceAccountEmail],
    role: "roles/run.admin"
}, {parent: ciServiceAccount});

// Setup cloud run service account
const appName = config.appName;

const cloudRunServiceAccount = new gcp.serviceaccount.Account(`${appName}-cloud-run`, {
    accountId: `${appName}-cloud-run`,
    description: `${appName} cloud run service account`,
    displayName: `${appName} cloud run`
});

const cloudRunServiceAccountBinding = new gcp.serviceaccount.IAMBinding(`${appName}-cloud-run`, {
    serviceAccountId: cloudRunServiceAccount.id,
    members: [ ciServiceAccountEmail ],
    role: "roles/iam.serviceAccountUser"
});
