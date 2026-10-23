# Deploy HoangZoho1 to Azure App Service

Use the checked-in deployment script every time. It publishes a clean Release build, uploads files individually over FTPS to avoid Kudu ZIP deployment disk-space failures, and checks the live Swagger endpoint when complete.

## Prerequisites

- .NET 8 SDK
- `curl`, `jq`, `python3`, and `xmllint`
- Azure publish settings containing `FTP` and `ZipDeploy` profiles
- The app's secret fallback file

Keep these files out of Git:

- `~/Downloads/HoangZoho1.PublishSettings`
- `.env.azure.local` in the repository root

Both are ignored by Git. If the publish settings are missing or expired, download fresh publish settings from the Azure App Service. Never paste credentials into this document or commit the secret files.

## Deploy

From the repository root:

```bash
./scripts/deploy-azure.sh
```

For non-default secret file locations:

```bash
./scripts/deploy-azure.sh /path/to/HoangZoho1.PublishSettings /path/to/.env.azure.local
```

The script will:

1. Validate prerequisites and both local credential files.
2. Publish `HoangZoho1/HoangZoho1.csproj` in Release mode to a temporary directory.
3. Upload `.env.azure.local` to `D:\home\data\HoangZoho1\.env.azure.local`, outside the public web root.
4. Place `app_offline.htm` to take the app offline, then upload publish output one file at a time.
5. Remove `app_offline.htm`, including during error cleanup, and check the app's Swagger endpoint for HTTP 200.

The deploy is complete only when the script prints `Deployment succeeded`. The current App Service health URL is:

```text
https://hoangzoho1.azurewebsites.net/swagger/v1/swagger.json
```

## Recovery

If deployment exits during upload, the script attempts to remove `app_offline.htm` so the previous app can start. Check the deployment output and Azure App Service logs before retrying. If NuGet restore fails, ensure the machine can reach `https://api.nuget.org/v3/index.json`, then rerun the same command. If deployment credentials were rotated, download a fresh publish-settings file.

The script does not use ZIP deployment, so it avoids the Kudu temporary disk usage that caused the earlier deployment failure. It briefly takes the app offline while replacing files.
