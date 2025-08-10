#!/usr/bin/env bash
set -euo pipefail

dotnet publish -c Release -r "$1" -p:PublishAot=true --self-contained true

