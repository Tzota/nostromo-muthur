#!/usr/bin/env bash
set -euo pipefail

_dirname="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${_dirname}/_common/get_arch.sh" || exit 1

NAME="tzota/nostromo-muthur-$ARCH"
docker run --rm --name nostromo-muthur -it --link nostromo-brett:redis -e REDIS_SERVER=redis -e BOT_ID=$BOT_ID $NAME:@latest
