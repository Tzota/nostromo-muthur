#!/usr/bin/env bash
set -euo pipefail

_dirname="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${_dirname}/_common/get_arch.sh" || exit 1

RID="debian.10-x64"

case "$ARCH" in
  "amd64") RID="debian.10-x64" ;; # bin/Release/netcoreapp3.1/debian.10-x64/...
  "arm") RID="linux-arm" ;; # bin/Release/netcoreapp3.1/linux-arm/...
esac

pushd ../../
# dotnet clean
rm -rf ./bin
rm -rf ./obj
dotnet publish -c Release -r $RID --output bin/Release/netcoreapp3.1/publish/$ARCH

echo "mu/th/ur for $ARCH has been built"
