# Deploy HoangZoho1 to Azure App Service

Use the checked-in deployment script for each deployment. It publishes a clean Release build, uploads files individually over FTPS to avoid Kudu ZIP deployment disk-space failures, and checks the live Swagger endpoint when complete.

## Prerequisites

- .NET 8 SDK
- `curl`, `jq`, `perl`, and `xmllint`
- Azure publish settings containing `FTP` and `ZipDeploy` profiles
- The ignored `.env.azure.local` fallback file at the repository root

Keep `~/Downloads/HoangZoho1.PublishSettings` and `.env.azure.local` out of Git. The deploy script sends `.env.azure.local` directly to the App Service's private `D:\home\data` directory using Kudu; it is not put in the publish output or under the public website root. The generated `web.config` points the existing fallback loader at that private path. If publish settings are missing or expired, download fresh publish settings from the Azure App Service. Never paste credentials into this document or commit secret files.

## Deploy

From the repository root:

```bash
./scripts/deploy-azure.sh
```

For a non-default publish settings location:

```bash
./scripts/deploy-azure.sh /path/to/HoangZoho1.PublishSettings
```

The script validates prerequisites, uploads the fallback file to App Service private data storage, publishes `HoangZoho1/HoangZoho1.csproj` in Release mode to a temporary directory, configures the fallback path in the generated `web.config`, takes the app offline with `app_offline.htm`, uploads published files one at a time, restores service even if an upload fails, and checks that the Swagger endpoint returns HTTP 200.

The deployment is complete only when the script prints `Deployment succeeded`. The current App Service health URL is:

```text
https://hoangzoho1.azurewebsites.net/swagger/v1/swagger.json
```

## Recovery

If deployment exits during upload, the script attempts to remove `app_offline.htm` so the app can restart. Check the deployment output and Azure App Service logs before retrying. If NuGet restore fails, ensure the machine can reach `https://api.nuget.org/v3/index.json`, then rerun the same command. If deployment credentials were rotated, download fresh publish settings.

This script does not use ZIP deployment, so it avoids the Kudu temporary disk usage that caused the earlier deployment failure. It briefly takes the app offline while replacing files. It uploads the ignored fallback file only to private App Service data storage, never to the website's public root or Git.
