# .NET Core Production Ready Sample

## Local Environment Variables

* `POSTGRES_PASSWORD` - Database password used in Postgres container
* `WEATHERDB__PASSWORD` - Password used to authenticate with Postgres container, should be the same as `POSTGRES_PASSWORD`

## Build

```
./batect build
```

## Test
```
./batect test
```

## Run integration test database
```
./batect start-integration-test-db
```

## How to deploy from scratch

### Prerequisites

* Google Cloud CLI
* Pulumi CLI

### GitHub Actions Secrets

* `PULUMI_ACCESS_TOKEN` - Access token used to authenticate with Pulumi
* `POSTGRES_PASSWORD` - Password used for postgres integration test container
* `IAM_GOOGLE_CREDENTIALS` - JSON credentials for IAM service account
* `CI_GOOGLE_CREDENTIALS` - JSON credentials for CI service account

### Steps

1. Fork repository
2. Create new Google Cloud Project
    * ```
      gcloud projects create [project-name]
      gcloud config set project [project-name]
      ```
    * Link the new project to a billing account in the Google Cloud Console
3. Create IAM service account
    * `gcloud iam service-accounts create iam-svc`
4. Assign required roles to IAM service account
    * ```
        gcloud projects add-iam-policy-binding [project-name] --member "serviceAccount:iam-svc@[project-name].iam.gserviceaccount.com" --role "roles/iam.securityAdmin"
        gcloud projects add-iam-policy-binding [project-name] --member "serviceAccount:iam-svc@[project-name].iam.gserviceaccount.com" --role "roles/iam.serviceAccountAdmin"
        gcloud projects add-iam-policy-binding [project-name] --member "serviceAccount:iam-svc@[project-name].iam.gserviceaccount.com" --role "roles/serviceusage.serviceUsageAdmin"
        gcloud projects add-iam-policy-binding [project-name] --member "serviceAccount:iam-svc@[project-name].iam.gserviceaccount.com" --role "roles/secretmanager.admin"
      ```
5. Generate keyfile for IAM service account
    * `gcloud iam service-accounts keys create keyfile.json --iam-account iam-svc@[project-name].iam.gserviceaccount.com`
6. Set GitHub Action secrets
    1. `PULUMI_ACCESS_TOKEN` - Set to your access token for your Pulumi account
    2. `POSTGRES_PASSWORD` - Choose a random password for the integration test database
    3. `IAM_GOOGLE_CREDENTIALS` - Set to the contents of the keyfile generated in the previous step
    4. Remove the generated keyfile
7. Commit a change to update the environment variables in `.github/workflows/main.yml` to match new project details
8. Commit a change to update the files in infra folder to match new project details
13. Commit a change to update the `src/WeatherApi/appsettings.dev.json` to match the database details
9. Run the GitHub actions workflow - it should fail at the Deploy Infra step failing to authenticate the newly created CI account
10. Generate keyfile for CI service account
    * `gcloud iam service-accounts keys create keyfile.json --iam-account ci-svc@[project-name].iam.gserviceaccount.com`
11. Set GitHub Action secrets
    1. `CI_GOOGLE_CREDENTIALS` - Set to the contents of the keyfile generated in the previous step
    2. Remove the generated keyfile
12. Run the GitHub actions workflow - it should fail at the Migrate Database step failing to authenticate the newly created database instance
14. Create a secret in GCP secret manager to store the database password
   * `gcloud secrets versions add weather-api-db-password --data-file="dbpass.txt"`
15. Create a database user with the database password stored in secret manager
   * `gcloud sql users create weather-api-user --instance=weather-api-db --password $(cat dbpass.txt)`
16. Run the GitHub actions workflow - it should now succeed