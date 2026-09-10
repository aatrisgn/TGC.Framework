#!/usr/bin/env bash
# Entrypoint for Dockerfile.dind: starts a nested Docker engine, brings up
# the emulator stack inside it, then runs the Emulators unit tests against
# that stack. See README.md.
set -euo pipefail

REPO_DIR="/repo"

echo "==> Starting nested dockerd..."
# vfs, not the overlay2 default: overlay-on-overlay (this container's nested
# engine on top of the host's own overlay-based backing filesystem, e.g.
# Docker Desktop) fails extracting image layers ("operation not permitted"
# on whiteout files). fuse-overlayfs avoids that and is more disk-efficient,
# but its FUSE layer breaks `exec` for these particular emulator images
# ("invalid argument" on container start, reproduced against all 5). vfs has
# no layer sharing (needs real disk headroom on the host — tens of GB for
# this 5-image stack) but uses plain files, so `exec` behaves normally.
dockerd --storage-driver=vfs >/var/log/dockerd.log 2>&1 &

echo "==> Waiting for nested dockerd to be ready..."
until docker info >/dev/null 2>&1; do
  sleep 1
done
echo "==> dockerd is ready."

echo "==> Setting up the 127.0.0.2 loopback alias for the Event Hubs emulator..."
"${REPO_DIR}/src/Emulators/docker/scripts/linux-setup-loopback-alias.sh"

echo "==> Bringing up the emulator stack..."
cd "${REPO_DIR}/src/Emulators/docker"
docker compose -f compose.yaml up -d

./scripts/wait-for-emulators.sh

echo "==> Running Emulators unit tests..."
cd "${REPO_DIR}/src/Emulators/TGC.Emulators"
dotnet test TGC.Emulators.Tests/TGC.Emulators.Tests.csproj
