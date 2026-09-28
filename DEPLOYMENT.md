# Azure deployment

The project includes a deployment script that avoids the App Service ZIP deployment disk-space failure by uploading the clean publish output incrementally.

## One-time setup

Install the .NET 8 SDK and make sure macOS has `curl`, `jq`, `python3`, and `xmllint`. Keep these two secret files outside Git:

- `~/Downloads/HoangZoho1.PublishSettings`
- `.env.azure.local` in the repository root

Both paths are already ignored by Git. Download a fresh publish-settings file from Azure if the deployment credentials are rotated.

## Deploy

From the repository root, run:

```bash
./scripts/deploy-azure.sh
```

To use files in different locations:

```bash
./scripts/deploy-azure.sh /path/to/HoangZoho1.PublishSettings /path/to/.env.azure.local
```

The script performs a clean Release publish, uploads the fallback secret file outside the public web root, briefly takes the application offline, replaces the application files, restores service even if an upload fails, and verifies `/swagger/v1/swagger.json` returns HTTP 200.

Do not commit or share either secret file. Rotate the Azure publishing credentials immediately if the publish-settings file is exposed.
