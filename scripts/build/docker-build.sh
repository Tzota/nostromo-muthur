#!/usr/bin/env bash
set -euo pipefail

_dirname="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${_dirname}/_common/get_arch.sh" || exit 1

pushd ../../build/docker/$ARCH/

if [ -d "./app" ]
then
rm -rf ./app
fi

mkdir app

cp -r ../../../bin/Release/netcoreapp3.1/publish/$ARCH/* ./app

SEMVER="0.0.3"
docker build -t nostromo-muthur-$ARCH:$SEMVER .

docker image tag nostromo-muthur-$ARCH:$SEMVER tzota/nostromo-muthur-$ARCH:$SEMVER
docker image tag nostromo-muthur-$ARCH:$SEMVER tzota/nostromo-muthur-$ARCH:latest

docker push --all-tags tzota/nostromo-muthur-$ARCH

rm -rf ./app





