import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";
import * as enableGcpApis from "./enableGcpApis";
import * as config from "./config";
import * as secrets from "./secrets";

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

const cloudSecretAccessorIamBinding = new gcp.secretmanager.SecretIamBinding(`ci-svc-${config.appName}-db-pass`, {
    secretId: secrets.databasePasswordSecret.secretId,
    members: [ciServiceAccountEmail],
    role: "roles/secretmanager.secretAccessor"
}, {parent: ciServiceAccount, dependsOn: enableGcpApis.enableIamApi});

// Setup cloud run service account
const cloudRunServiceAccount = new gcp.serviceaccount.Account(`${config.appName}-cloud-run`, {
    accountId: `${config.appName}-cloud-run`,
    description: `${config.appName} cloud run service account`,
    displayName: `${config.appName} cloud run`
}, {dependsOn: enableGcpApis.enableIamApi});

const cloudRunServiceAccountBinding = new gcp.serviceaccount.IAMBinding(`${config.appName}-cloud-run-ci-act-as-user`, {
    serviceAccountId: cloudRunServiceAccount.id,
    members: [ ciServiceAccountEmail ],
    role: "roles/iam.serviceAccountUser"
}, {parent: cloudRunServiceAccount, dependsOn: enableGcpApis.enableIamApi});

const cloudRunServiceAccountSqlClientIamBinding = new gcp.projects.IAMBinding(`${config.appName}-cloud-run-cloud-sql`, {
    members: [pulumi.interpolate`serviceAccount:${cloudRunServiceAccount.email}`],
    role: "roles/cloudsql.client"
}, {parent: cloudRunServiceAccount, dependsOn: enableGcpApis.enableIamApi});
