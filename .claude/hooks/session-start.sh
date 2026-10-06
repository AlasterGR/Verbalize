#!/bin/bash
#  Prepares a Claude Code cloud session so the solution can be built and tested.
set -euo pipefail

#  Do nothing outside cloud sessions, since local machines manage their own tools.
if [ "${CLAUDE_CODE_REMOTE:-}" != "true" ]; then
  exit 0
fi

#  Install the .NET 10 SDK from Ubuntu's archive if it is not already there.
if ! dotnet --list-sdks 2>/dev/null | grep -q '^10\.'; then
  export DEBIAN_FRONTEND=noninteractive
  apt-get install -y -q dotnet-sdk-10.0 >/dev/null 2>&1 || { apt-get update -q >/dev/null && apt-get install -y -q dotnet-sdk-10.0 >/dev/null; }
fi

#  Keep the .NET tools quiet for the rest of the session.
if [ -n "${CLAUDE_ENV_FILE:-}" ]; then
  echo 'export DOTNET_NOLOGO=1' >> "$CLAUDE_ENV_FILE"
  echo 'export DOTNET_CLI_TELEMETRY_OPTOUT=1' >> "$CLAUDE_ENV_FILE"
fi

#  Download the solution's packages so building and testing work straight away.
cd "${CLAUDE_PROJECT_DIR:-$(dirname "$0")/../..}"
DOTNET_NOLOGO=1 DOTNET_CLI_TELEMETRY_OPTOUT=1 dotnet restore Verbalize.sln
