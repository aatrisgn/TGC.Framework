# Local Azure emulator stack

Docker Compose stack running the Azure Cosmos DB, Service Bus, and Event Hubs
emulators at the same time, for local development and Azure Function debugging.

## Why Event Hubs is on a different IP

The Service Bus and Event Hubs emulators both use the `UseDevelopmentEmulator=true`
connection string flag, and both .NET SDKs hardcode the AMQP port to **5672**,
ignoring whatever port is actually in the connection string. That means two
emulators can't both publish AMQP on `5672` on the same host address — see
[azure-event-hubs-emulator-installer#73](https://github.com/Azure/azure-event-hubs-emulator-installer/issues/73)
(open, unresolved upstream).

The SDK does respect the *host* in the connection string, though, so this stack
binds:

| Service | AMQP endpoint |
|---|---|
| Service Bus | `127.0.0.1:5672` |
| Event Hubs | `127.0.0.2:5672` |

## Loopback alias setup

### macOS (verified — an M3 Pro on macOS 26.5)

macOS doesn't route extra `127.x` addresses to loopback by default (unlike
Linux), so `127.0.0.2` has to be added manually — **every time after a reboot**,
before bringing the stack up:

```bash
./scripts/mac-setup-loopback-alias.sh
```

This is a one-shot script (prompts for `sudo`), not an installed system service —
it won't survive a reboot, and it's not meant to.

### Linux / WSL2 (untested — conceptually should just work)

Unlike macOS, Linux (and WSL2, whose networking is a real Linux kernel) routes
the entire `127.0.0.0/8` block to loopback by default, so `127.0.0.2` should
already be usable with no setup at all. Run this anyway before bringing the
stack up — it verifies that and only falls back to adding the address if
something on the system has been configured not to route it:

```bash
./scripts/linux-setup-loopback-alias.sh
```

Same one-shot, non-persistent behavior as the macOS script. I haven't been able
to verify this on a real Linux/WSL2 machine (this stack was built on a Mac) —
please report back if it doesn't behave as expected.

### Native Windows, not WSL2 (untested — conceptually should just work)

Native Windows also routes the entire `127.0.0.0/8` block to loopback by
default, so the same "should just work, no setup needed" reasoning as Linux
applies here too. Run this anyway from a regular (non-elevated) PowerShell
prompt before bringing the stack up — it verifies that with a real TCP bind
and only falls back to adding the address if the bind actually fails:

```powershell
.\scripts\windows-setup-loopback-alias.ps1
```

It doesn't hardcode the loopback interface's name (`"Loopback Pseudo-Interface
1"` isn't guaranteed stable across Windows versions/locales) — the fallback
looks up whichever interface already owns `127.0.0.1` and adds the alias
there instead, and only asks to be re-run elevated (as Administrator) if that
fallback path actually needs to run.

Unlike the macOS/Linux scripts, this one is **not guaranteed to be
non-persistent** — Windows-added loopback addresses typically survive a
reboot as part of normal interface config, though that hasn't been verified
end-to-end. If it turns out not to survive a reboot, just re-run the script
the same way as the others. I haven't been able to verify this script on a
real Windows machine (this stack was built on a Mac; only the bind-check
logic itself, which is plain cross-platform .NET, was tested) — please report
back if it doesn't behave as expected.

## Running the stack

1. Copy `.env.example` to `.env` and fill in `ACCEPT_EULA=Y` and a `MSSQL_SA_PASSWORD`.
   The stack won't start without `ACCEPT_EULA=Y` — Service Bus and Event Hubs both refuse
   to launch and exit immediately if it's missing/not `Y`.
2. Run the loopback alias script for your OS (see above).
3. `docker compose -f compose.yaml up -d`
4. `docker ps` to confirm all five containers (`cosmos-emulator`, `servicebus-emulator`,
   `mssql`, `eventhubs-emulator`, `azurite`) are `Up`.

### Why `mssql/server:2022-latest` instead of `azure-sql-edge`

The Service Bus emulator needs a SQL backend. The installer repo's own Docker Compose
template defaults to `mcr.microsoft.com/azure-sql-edge`, but on Apple Silicon (confirmed
on an M3 Pro, macOS 26.5) its ARM64 build reliably crashes on startup with `SIGABRT`
inside its internal binary-translation layer — reproduced on both the `latest`/`2.0.0`
and `1.0.7` tags, so it's not a version-pinning fix. `mcr.microsoft.com/mssql/server:2022-latest`
is amd64-only and runs under Docker Desktop's Rosetta emulation on Apple Silicon instead,
but is stable there — this is also what Microsoft's own "test locally with the Service Bus
emulator" doc uses in its compose sample, rather than the installer repo's template default.

Cosmos DB data is persisted to `./.data/cosmos` (gitignored) via a bind mount, so it
survives `docker compose down`/`up`. The Service Bus and Event Hubs emulators do not
support data persistence — their entities and messages reset on every container restart,
which is why they're declared up front in `config/servicebus.Config.json` and
`config/eventhub.Config.json` rather than created at runtime.

## Endpoints & well-known connection strings

| Service | Endpoint | Connection string |
|---|---|---|
| Cosmos DB (gateway) | `https://localhost:8081/` | `AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==` |
| Cosmos Data Explorer | `http://localhost:1234/` | — |
| Service Bus | `127.0.0.1:5672` | `Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;` |
| Service Bus management/health | `http://127.0.0.1:5300` | — |
| Event Hubs | `127.0.0.2:5672` | `Endpoint=sb://127.0.0.2;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;EntityPath=eh1` |
| Event Hubs (Kafka protocol) | `127.0.0.2:9092` | not used by the test project, exposed by the image regardless |
| Event Hubs management/health | `http://127.0.0.2:5300` | — |

The Cosmos DB `.NET`/Java SDKs require HTTPS, hence `--protocol https`, so the emulator's
self-signed certificate needs `ServerCertificateCustomValidationCallback` bypassed on the client.

`SharedAccessKey=SAS_KEY_VALUE` and the Cosmos account key above are Microsoft's own
documented, publicly known emulator placeholders — not real secrets.

Pre-declared entities:
- Service Bus: queue `queue.1`, topic `topic.1` with subscription `subscription.1`.
- Event Hubs: namespace `emulatorNs1`, hub `eh1` (2 partitions), consumer group `cg1`.
