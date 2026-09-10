#!/usr/bin/env bash
# Waits for the three emulator endpoints the test project connects to
# (Cosmos, Service Bus, Event Hubs) to accept TCP connections.
#
# `docker compose up -d` returns once containers have started, not once the
# emulators inside are actually listening — Cosmos DB in particular can take
# 1-2 minutes to come up, longer than the test project's own 60s retry
# budget (EmulatorRetry.cs) allows for. Run this after `docker compose up -d`
# and before `dotnet test` instead of touching that retry budget.
set -euo pipefail

wait_for_tcp() {
  local host="$1" port="$2" label="$3" waited=0
  echo "==> Waiting for ${label} (${host}:${port})..."
  until (exec 3<>"/dev/tcp/${host}/${port}") 2>/dev/null; do
    exec 3<&- 2>/dev/null || true
    waited=$((waited + 3))
    if [ "${waited}" -ge 300 ]; then
      echo "==> ${label} did not come up within 300s; continuing anyway."
      return 0
    fi
    sleep 3
  done
  exec 3<&- 2>/dev/null || true
  echo "==> ${label} is accepting connections."
}

wait_for_tcp 127.0.0.1 8081 "Cosmos DB emulator"
wait_for_tcp 127.0.0.1 5672 "Service Bus emulator"
wait_for_tcp 127.0.0.2 5672 "Event Hubs emulator"
