import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as enableGcpApis from "./enableGcpApis";
import * as config from "./config";

// Setup CI service account
const ciServiceAccount = new gcp.serviceaccount.Account(`ci-svc`, {
    accountId: `ci-svc`,
    description: `CI Service Account`,
    displayName: `CI Service Account`
}, {dependsOn: enableGcpApis.enableIamApi});

const ciServiceAccountEmail = pulumi.interpolate`serviceAccount:${ciServiceAccount.email}`;

const storageAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-storage-admin`, {
    members: [ciServiceAccountEmail],
    role: "roles/storage.admin"
}, {parent: ciServiceAccount, dependsOn: enableGcpApis.enableIamApi});

const cloudRunAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-run-admin`, {
    members: [ciServiceAccountEmail],
    role: "roles/run.admin"
}, {parent: ciServiceAccount, dependsOn: enableGcpApis.enableIamApi});

const cloudSqlAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-cloud-sql-admin`, {
    members: [ciServiceAccountEmail],
    role: "roles/cloudsql.admin"
}, {parent: ciServiceAccount, dependsOn: enableGcpApis.enableIamApi});

const cloudSecretAdminIamBinding = new gcp.projects.IAMBinding(`ci-svc-cloud-secret-admin`, {
    members: [ciServiceAccountEmail],
    role: "roles/secretmanager.admin"
}, {parent: ciServiceAccount, dependsOn: enableGcpApis.enableIamApi});

// Setup cloud run service account
const appName = config.appName;

const cloudRunServiceAccount = new gcp.serviceaccount.Account(`${appName}-cloud-run`, {
    accountId: `${appName}-cloud-run`,
    description: `${appName} cloud run service account`,
    displayName: `${appName} cloud run`
}, {dependsOn: enableGcpApis.enableIamApi});

const cloudRunServiceAccountBinding = new gcp.serviceaccount.IAMBinding(`${appName}-cloud-run-ci-act-as-user`, {
    serviceAccountId: cloudRunServiceAccount.id,
    members: [ ciServiceAccountEmail ],
    role: "roles/iam.serviceAccountUser"
}, {parent: cloudRunServiceAccount, dependsOn: enableGcpApis.enableIamApi});

const cloudRunServiceAccountSqlClientIamBinding = new gcp.projects.IAMBinding(`${appName}-cloud-run-cloud-sql`, {
    members: [pulumi.interpolate`serviceAccount:${cloudRunServiceAccount.email}`],
    role: "roles/cloudsql.client"
}, {parent: cloudRunServiceAccount, dependsOn: enableGcpApis.enableIamApi});
