#!/usr/bin/env bash
# Linux / WSL2 equivalent of mac-setup-loopback-alias.sh.
#
# Unlike macOS, Linux (and WSL2, whose networking is a real Linux kernel) routes
# the entire 127.0.0.0/8 block to loopback by default, so 127.0.0.2 is normally
# already usable with no setup at all. This script just verifies that, and only
# falls back to adding the address if something on this system has been
# configured not to route it (uncommon).
#
# Not persisted across reboots on purpose, same as the macOS script — if the
# fallback path ever runs, re-run this after a reboot before `docker compose up`.
#
# NOTE: only verified conceptually against documented Linux/WSL2 loopback
# behavior, not tested on a real Linux/WSL2 machine — please report back if this
# doesn't work as expected.
set -euo pipefail

ALIAS_IP="127.0.0.2"

if ip route get "${ALIAS_IP}" 2>/dev/null | grep -q "dev lo"; then
  echo "${ALIAS_IP} already routes to loopback (the standard Linux/WSL2 default) — nothing to do."
  exit 0
fi

echo "${ALIAS_IP} does not route to loopback on this system (unusual) — adding it explicitly (requires sudo)..."
sudo ip addr add "${ALIAS_IP}/32" dev lo

echo "Done. ${ALIAS_IP} will be reachable until the next reboot."
