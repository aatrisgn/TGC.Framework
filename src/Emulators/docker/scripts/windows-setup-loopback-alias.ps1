<#
.SYNOPSIS
    Native Windows equivalent of mac-setup-loopback-alias.sh / linux-setup-loopback-alias.sh.

.DESCRIPTION
    Windows is documented to route the entire 127.0.0.0/8 block to loopback by default
    (like Linux, unlike macOS), so 127.0.0.2 should already be bindable with no setup at
    all. This script verifies that with a real TCP bind (not just a routing-table check),
    and only falls back to explicitly adding the address if the bind actually fails.

    The fallback doesn't hardcode the loopback interface's name ("Loopback Pseudo-Interface
    1" isn't guaranteed stable across Windows versions/locales) - it looks up whichever
    interface already owns 127.0.0.1 and adds the alias there instead.

    Unlike the macOS script, this is NOT guaranteed to be non-persistent - addresses added
    via New-NetIPAddress typically survive a reboot as part of normal interface config
    (this hasn't been verified end-to-end on a real Windows machine). If you find it does
    NOT survive a reboot, just re-run this script the same way as the macOS/Linux ones.

    NOTE: only verified conceptually against documented Windows loopback behavior, not
    tested on a real Windows machine (this stack was built on a Mac) - please report back
    if this doesn't behave as expected.

.NOTES
    Run this from a regular PowerShell prompt first. It only asks for an elevated
    (Administrator) session if the fallback path is actually needed.
#>

$ErrorActionPreference = 'Stop'
$AliasIp = '127.0.0.2'

function Test-LoopbackBind {
    param([string]$IpAddress)

    try {
        $listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Parse($IpAddress), 0)
        $listener.Start()
        $listener.Stop()
        return $true
    }
    catch {
        return $false
    }
}

if (Test-LoopbackBind -IpAddress $AliasIp) {
    Write-Host "$AliasIp is already bindable (the standard Windows default) - nothing to do."
    exit 0
}

Write-Host "$AliasIp is not bindable on this system (unusual) - adding it explicitly..."

$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Error "Adding a loopback address requires an elevated (Administrator) PowerShell session. Re-run this script as Administrator."
    exit 1
}

$loopbackAddress = Get-NetIPAddress -AddressFamily IPv4 -IPAddress '127.0.0.1' -ErrorAction Stop | Select-Object -First 1
New-NetIPAddress -IPAddress $AliasIp -PrefixLength 8 -InterfaceIndex $loopbackAddress.InterfaceIndex | Out-Null

if (Test-LoopbackBind -IpAddress $AliasIp) {
    Write-Host "Done. $AliasIp is now bindable."
}
else {
    Write-Error "Added the address but $AliasIp still isn't bindable - something else (firewall?) may be blocking it."
    exit 1
}
