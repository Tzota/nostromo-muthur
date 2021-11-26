#!/usr/bin/env bash
set -euo pipefail

_dirname="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${_dirname}/_common/get_arch.sh" || exit 1

docker images -a | grep -E "nostromo-muthur-$ARCH" | awk '{ print $1":"$2 }' | xargs --no-run-if-empty docker rmi
