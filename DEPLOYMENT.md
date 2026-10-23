# Deploy HoangZoho1 to Azure App Service

Use the checked-in deployment script for each deployment. It publishes a clean Release build, uploads files individually over FTPS to avoid Kudu ZIP deployment disk-space failures, and checks the live Swagger endpoint when complete.

## Prerequisites

- .NET 8 SDK
- `curl`, `jq`, and `xmllint`
- Azure publish settings containing `FTP` and `ZipDeploy` profiles
- Production secrets already configured as Azure App Service environment variables

Keep `~/Downloads/HoangZoho1.PublishSettings` out of Git. The deployment script does not read or upload `.env.azure.local`; production credentials come from Azure App Settings. If publish settings are missing or expired, download fresh publish settings from the Azure App Service. Never paste credentials into this document or commit secret files.

## Deploy

From the repository root:

```bash
./scripts/deploy-azure.sh
```

For a non-default publish settings location:

```bash
./scripts/deploy-azure.sh /path/to/HoangZoho1.PublishSettings
```

The script validates prerequisites, publishes `HoangZoho1/HoangZoho1.csproj` in Release mode to a temporary directory, takes the app offline with `app_offline.htm`, uploads published files one at a time, restores service even if an upload fails, and checks that the Swagger endpoint returns HTTP 200.

The deployment is complete only when the script prints `Deployment succeeded`. The current App Service health URL is:

```text
https://hoangzoho1.azurewebsites.net/swagger/v1/swagger.json
```

## Recovery

If deployment exits during upload, the script attempts to remove `app_offline.htm` so the app can restart. Check the deployment output and Azure App Service logs before retrying. If NuGet restore fails, ensure the machine can reach `https://api.nuget.org/v3/index.json`, then rerun the same command. If deployment credentials were rotated, download fresh publish settings.

This script does not use ZIP deployment, so it avoids the Kudu temporary disk usage that caused the earlier deployment failure. It briefly takes the app offline while replacing files. It does not transfer local secrets; Azure App Settings must already contain the required environment variables.
