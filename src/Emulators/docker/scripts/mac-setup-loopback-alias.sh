#!/usr/bin/env bash
# One-shot setup so the Event Hubs emulator can bind AMQP on 127.0.0.2:5672
# without colliding with the Service Bus emulator on 127.0.0.1:5672.
#
# macOS does not route extra 127.x loopback addresses by default (unlike Linux),
# so this alias has to be added explicitly. It is NOT persisted across reboots
# on purpose (no LaunchDaemon) — re-run this script after every reboot, before
# `docker compose up`.
set -euo pipefail

ALIAS_IP="127.0.0.2"

if ifconfig lo0 | grep -q "inet ${ALIAS_IP} "; then
  echo "${ALIAS_IP} is already aliased on lo0 — nothing to do."
  exit 0
fi

echo "Adding ${ALIAS_IP} as a loopback alias on lo0 (requires sudo)..."
sudo ifconfig lo0 alias "${ALIAS_IP}" up

echo "Done. ${ALIAS_IP} will be reachable until the next reboot."
