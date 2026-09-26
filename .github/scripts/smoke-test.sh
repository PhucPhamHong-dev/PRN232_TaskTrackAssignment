#!/usr/bin/env bash
set -Eeuo pipefail

: "${REPOSITORY_URL:?REPOSITORY_URL is required}"
: "${FRONTEND_URL:?FRONTEND_URL is required}"
: "${BACKEND_URL:?BACKEND_URL is required}"
: "${SWAGGER_URL:?SWAGGER_URL is required}"
: "${HEALTH_URL:?HEALTH_URL is required}"

check_url() {
  local label="$1"
  local url="$2"
  local expected_text="$3"
  local attempts=24
  local delay_seconds=15
  local response_file
  local status_code
  response_file="$(mktemp)"

  for attempt in $(seq 1 "$attempts"); do
    status_code="$(curl --silent --show-error --location --max-time 40 --output "$response_file" --write-out '%{http_code}' "$url" || true)"

    if [[ "$status_code" == "200" ]] && grep --fixed-strings --quiet "$expected_text" "$response_file"; then
      echo "✓ $label is healthy ($url)"
      rm -f "$response_file"
      return 0
    fi

    echo "Attempt $attempt/$attempts: $label returned HTTP ${status_code:-000}; retrying in ${delay_seconds}s..."
    sleep "$delay_seconds"
  done

  echo "::error::$label did not become healthy: $url"
  echo "Last response:"
  head -c 500 "$response_file" || true
  rm -f "$response_file"
  return 1
}

check_url "GitHub repository" "$REPOSITORY_URL" "PRN232_TaskTrackAssignment"
check_url "Vercel frontend" "$FRONTEND_URL" "TaskTrack"
check_url "Render backend" "$BACKEND_URL" "Swagger UI"
check_url "Render database health" "$HEALTH_URL" '"status":"healthy"'
check_url "Swagger" "$SWAGGER_URL" "Swagger UI"

echo "All four submission links are operational."
