<#
.SYNOPSIS
    Windows-only: relays 127.0.0.2 traffic to the Event Hubs emulator, which on
    Windows is actually published on the Rancher Desktop WSL2 VM's real IP at
    alternate ports.

.DESCRIPTION
    Rancher Desktop's Windows port-forwarding does not reliably distinguish
    127.0.0.1:PORT from 127.0.0.2:PORT when the port number is identical -
    both addresses end up answered by whichever container's mapping wins
    internally. See README.md for the investigation. macOS/Linux don't have
    this problem, so this workaround is Windows-only.

    A first attempt at this script relayed loopback-to-loopback
    (127.0.0.2 -> 127.0.0.1:<alternate port>) via netsh portproxy. That did
    NOT work, even with the IP Helper service (iphlpsvc) confirmed running -
    connections to 127.0.0.2 were refused outright. Loopback-to-loopback
    relaying appears to be an unsupported combination for netsh portproxy on
    this setup.

    What IS a well-documented, supported netsh portproxy pattern - the same
    one used to expose a WSL2-hosted server to the LAN - is relaying a
    Windows-side loopback listener to the WSL2 VM's actual assigned IP
    address. compose.windows.yaml publishes Event Hubs unbound to any
    specific host IP (default: all interfaces inside the VM) at alternate
    ports (15672/19092/15300 instead of 5672/9092/5300), which makes it
    reachable via the VM's real IP, not just its own loopback. This script
    resolves that real IP dynamically (it is NOT stable across WSL/Rancher
    restarts) and relays 127.0.0.2's standard ports there.

.NOTES
    Requires an elevated (Administrator) PowerShell session - netsh interface
    portproxy needs it unconditionally, unlike windows-setup-loopback-alias.ps1
    which only needs elevation in its fallback path.

    Run this every time after Rancher Desktop (re)starts, alongside
    windows-setup-loopback-alias.ps1, before bringing the stack up with:
      docker compose -f compose.yaml -f compose.windows.yaml up -d
    The VM's IP can change on restart, which is exactly why this resolves it
    fresh each run rather than hardcoding it - re-run this script whenever
    Rancher Desktop restarts, even if you skip it after every reboot.

    This script is idempotent - it removes any existing rule for the same
    listen address/port before re-adding it, so it's safe to re-run.

    To remove the rules this script adds:
      netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=5672
      netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=9092
      netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=5300
#>

$ErrorActionPreference = 'Stop'
$DistroName = 'rancher-desktop'

$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Error "This script requires an elevated (Administrator) PowerShell session. Re-run as Administrator."
    exit 1
}

# The Rancher Desktop WSL2 VM's IP is not stable across restarts, so it's
# resolved fresh on every run rather than hardcoded.
$vmIpRaw = & wsl.exe -d $DistroName -- hostname -I 2>$null
if ([string]::IsNullOrWhiteSpace($vmIpRaw)) {
    Write-Error @"
Could not resolve an IP for the '$DistroName' WSL distro (wsl -d $DistroName -- hostname -I returned nothing).
Run 'wsl --list --verbose' to confirm the actual distro name Rancher Desktop is using and update
`$DistroName at the top of this script if it differs from '$DistroName'.
"@
    exit 1
}

$vmIp = ($vmIpRaw -split '\s+')[0].Trim()
Write-Host "Rancher Desktop VM ('$DistroName') resolved to: $vmIp"

# Maps the standard port (what 127.0.0.2 must keep answering on) to the
# alternate port compose.windows.yaml actually publishes on the VM.
$rules = @(
    @{ Name = "AMQP"; ListenPort = 5672; ConnectPort = 15672 }
    @{ Name = "Kafka"; ListenPort = 9092; ConnectPort = 19092 }
    @{ Name = "management/health"; ListenPort = 5300; ConnectPort = 15300 }
)

foreach ($rule in $rules) {
    # Ignore failure: this only errors if the rule doesn't exist yet, which is fine.
    netsh interface portproxy delete v4tov4 listenaddress=127.0.0.2 listenport=$($rule.ListenPort) 2>$null | Out-Null

    netsh interface portproxy add v4tov4 listenaddress=127.0.0.2 listenport=$($rule.ListenPort) connectaddress=$vmIp connectport=$($rule.ConnectPort)
    Write-Host "$($rule.Name): 127.0.0.2:$($rule.ListenPort) -> ${vmIp}:$($rule.ConnectPort)"
}

Write-Host ""
Write-Host "Done. Current v4tov4 portproxy rules:"
netsh interface portproxy show v4tov4
