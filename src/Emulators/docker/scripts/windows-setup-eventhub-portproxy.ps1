<#
.SYNOPSIS
    Windows-only: relays 127.0.0.2 traffic to the Event Hubs emulator, which on
    Windows is actually published on 127.0.0.1 at alternate ports.

.DESCRIPTION
    Rancher Desktop's Windows port-forwarding does not reliably distinguish
    127.0.0.1:PORT from 127.0.0.2:PORT when the port number is identical -
    both addresses end up answered by whichever container's mapping wins
    internally. See README.md for the investigation. macOS/Linux don't have
    this problem, so this workaround is Windows-only.

    compose.windows.yaml publishes Event Hubs on 127.0.0.1 at alternate host
    ports (15672/19092/15300 instead of 5672/9092/5300) rather than on
    127.0.0.2 at the standard ports. This script uses netsh's built-in
    portproxy - a plain OS-level TCP relay with nothing to do with
    Rancher/Docker - to make 127.0.0.2 keep answering on the standard ports
    that the connection string / SDK actually expect.

.NOTES
    Requires an elevated (Administrator) PowerShell session - netsh interface
    portproxy needs it unconditionally, unlike windows-setup-loopback-alias.ps1
    which only needs elevation in its fallback path.

    Run this once per boot, alongside windows-setup-loopback-alias.ps1, before
    bringing the stack up with:
      docker compose -f compose.yaml -f compose.windows.yaml up -d

    Portproxy rules persist across reboots (unlike the loopback alias script's
    New-NetIPAddress addition, which typically also persists, but is
    documented there as unverified). This script is idempotent - it removes
    any existing rule for the same listen address/port before re-adding it, so
    it's safe to re-run.

    To remove the rules this script adds:
      netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=5672
      netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=9092
      netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=5300
#>

$ErrorActionPreference = 'Stop'

$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Error "This script requires an elevated (Administrator) PowerShell session. Re-run as Administrator."
    exit 1
}

# Maps the standard port (what 127.0.0.2 must keep answering on) to the
# alternate port compose.windows.yaml actually publishes on 127.0.0.1.
$rules = @(
    @{ Name = "AMQP"; ListenPort = 5672; ConnectPort = 15672 }
    @{ Name = "Kafka"; ListenPort = 9092; ConnectPort = 19092 }
    @{ Name = "management/health"; ListenPort = 5300; ConnectPort = 15300 }
)

foreach ($rule in $rules) {
    # Ignore failure: this only errors if the rule doesn't exist yet, which is fine.
    netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=$($rule.ListenPort) 2>$null | Out-Null

    netsh interface portproxy add v4tov4 listenaddress=127.0.0.2 listenport=$($rule.ListenPort) connectaddress=127.0.0.1 connectport=$($rule.ConnectPort)
    Write-Host "$($rule.Name): 127.0.0.2:$($rule.ListenPort) -> 127.0.0.1:$($rule.ConnectPort)"
}

Write-Host ""
Write-Host "Done. Current v4tov4 portproxy rules:"
netsh interface portproxy show v4tov4
