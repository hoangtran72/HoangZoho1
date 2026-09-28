#!/usr/bin/env bash
set -Eeuo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
project_file="$repo_root/HoangZoho1/HoangZoho1.csproj"
publish_settings="${1:-$HOME/Downloads/HoangZoho1.PublishSettings}"
secret_file="${2:-$repo_root/.env.azure.local}"
remote_secret_path='D:\home\data\HoangZoho1\.env.azure.local'

for dependency in curl dotnet jq python3 xmllint; do
  if ! command -v "$dependency" >/dev/null 2>&1; then
    echo "Missing required command: $dependency" >&2
    exit 1
  fi
done

if [[ ! -f "$publish_settings" ]]; then
  echo "Publish settings not found: $publish_settings" >&2
  exit 1
fi

if [[ ! -f "$secret_file" ]]; then
  echo "Secret fallback file not found: $secret_file" >&2
  exit 1
fi

xml_attribute() {
  local method="$1"
  local attribute="$2"
  xmllint --xpath "string(//publishProfile[@publishMethod='$method']/@$attribute)" "$publish_settings"
}

ftp_user="$(xml_attribute FTP userName)"
ftp_password="$(xml_attribute FTP userPWD)"
ftp_url="$(xml_attribute FTP publishUrl)"
ftp_site_path="$(xml_attribute FTP ftpSitePath)"
zip_user="$(xml_attribute ZipDeploy userName)"
zip_password="$(xml_attribute ZipDeploy userPWD)"
zip_host="$(xml_attribute ZipDeploy publishUrl)"
app_url="$(xml_attribute ZipDeploy destinationAppUrl)"

if [[ -z "$ftp_user" || -z "$ftp_password" || -z "$ftp_url" ||
      -z "$zip_user" || -z "$zip_password" || -z "$zip_host" || -z "$app_url" ]]; then
  echo "The publish settings file is missing an FTP or ZipDeploy profile." >&2
  exit 1
fi

zip_host="${zip_host%:443}"
kudu_url="https://$zip_host"
ftp_base="${ftp_url%/}/${ftp_site_path#/}"
publish_dir="$(mktemp -d "${TMPDIR:-/tmp}/hoangzoho1-publish.XXXXXX")"
offline_file="$publish_dir/app_offline.htm"
offline_active=false

remove_offline_marker() {
  if [[ "$offline_active" != true ]]; then
    return
  fi

  local request_body
  request_body="$(jq -n \
    --arg command 'powershell -NoProfile -Command "Remove-Item D:\home\site\wwwroot\app_offline.htm -Force -ErrorAction SilentlyContinue"' \
    --arg dir 'D:\home\site\wwwroot' \
    '{command:$command,dir:$dir}')"
  curl --silent --user "$zip_user:$zip_password" \
    -H 'Content-Type: application/json' \
    -d "$request_body" "$kudu_url/api/command" >/dev/null 2>&1 || true
  offline_active=false
}

cleanup() {
  remove_offline_marker
  find "$publish_dir" -depth -delete
}
trap cleanup EXIT INT TERM

echo "Publishing a clean Release build..."
DOTNET_CLI_HOME="${TMPDIR:-/tmp}/hoangzoho-dotnet" \
DOTNET_NOLOGO=1 \
DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
dotnet publish "$project_file" \
  --configuration Release \
  --no-self-contained \
  --output "$publish_dir"

python3 - "$publish_dir/web.config" "$remote_secret_path" <<'PY'
import sys
import xml.etree.ElementTree as ET

path, secret_path = sys.argv[1:]
tree = ET.parse(path)
root = tree.getroot()
aspnet_core = root.find(".//aspNetCore")
if aspnet_core is None:
    raise SystemExit("Generated web.config does not contain aspNetCore configuration")

environment = aspnet_core.find("environmentVariables")
if environment is None:
    environment = ET.SubElement(aspnet_core, "environmentVariables")

settings = {
    "ALLOW_LOCAL_SECRET_FALLBACK": "true",
    "LOCAL_SECRET_FILE_PATH": secret_path,
}
for element in list(environment):
    if element.get("name") in settings:
        environment.remove(element)
for name, value in settings.items():
    ET.SubElement(environment, "environmentVariable", name=name, value=value)

ET.indent(tree, space="  ")
tree.write(path, encoding="utf-8", xml_declaration=True)
PY

echo "Uploading the protected fallback file outside wwwroot..."
mkdir_request="$(jq -n \
  --arg command 'powershell -NoProfile -Command "New-Item -ItemType Directory -Force D:\home\data\HoangZoho1 | Out-Null"' \
  --arg dir 'D:\home\data' \
  '{command:$command,dir:$dir}')"
curl --fail --silent --show-error --user "$zip_user:$zip_password" \
  -H 'Content-Type: application/json' \
  -d "$mkdir_request" "$kudu_url/api/command" >/dev/null

secret_status="$(curl --silent --show-error --output /dev/null --write-out '%{http_code}' \
  --user "$zip_user:$zip_password" \
  -X PUT -H 'If-Match: *' --upload-file "$secret_file" \
  "$kudu_url/api/vfs/data/HoangZoho1/.env.azure.local")"
if [[ "$secret_status" != 200 && "$secret_status" != 201 && "$secret_status" != 204 ]]; then
  echo "Secret upload failed with HTTP $secret_status" >&2
  exit 1
fi

echo "Taking the application offline for file replacement..."
printf '%s\n' 'Deployment in progress. Please try again shortly.' >"$offline_file"
curl --fail --silent --show-error --user "$ftp_user:$ftp_password" \
  --ssl-reqd --upload-file "$offline_file" "$ftp_base/app_offline.htm" >/dev/null
offline_active=true

file_total="$(find "$publish_dir" -type f ! -name app_offline.htm | wc -l | tr -d ' ')"
file_number=0
while IFS= read -r -d '' local_file; do
  relative_file="${local_file#"$publish_dir"/}"
  file_number=$((file_number + 1))
  curl --fail --silent --show-error --user "$ftp_user:$ftp_password" \
    --ssl-reqd --ftp-create-dirs --upload-file "$local_file" \
    "$ftp_base/$relative_file" >/dev/null
  if (( file_number % 10 == 0 || file_number == file_total )); then
    echo "Uploaded $file_number/$file_total files"
  fi
done < <(find "$publish_dir" -type f ! -name app_offline.htm -print0 | sort -z)

remove_offline_marker
echo "Application files uploaded. Waiting for startup..."

health_url="${app_url%/}/swagger/v1/swagger.json"
for attempt_number in 1 2 3 4 5 6; do
  health_status="$(curl --silent --show-error --location --output /dev/null \
    --write-out '%{http_code}' --max-time 60 "$health_url")"
  if [[ "$health_status" == 200 ]]; then
    echo "Deployment succeeded: $health_url returned HTTP 200"
    exit 0
  fi
  echo "Startup check $attempt_number/6 returned HTTP $health_status"
  sleep 5
done

echo "Files were uploaded, but the health check did not return HTTP 200." >&2
echo "Inspect Azure App Service logs before trying another deployment." >&2
exit 1
