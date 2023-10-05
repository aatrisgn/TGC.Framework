[CmdletBinding()]
param(
     [Parameter()]
     [string]$AssemblyVersion,
     [Parameter()]
     [string]$RootPath
 )

Write-Host "Running script with the following parameters:"
Write-Host "AssemblyVersion : $AssemblyVersion"
Write-Host "RootPath : $RootPath"

# Define the directory to search for .csproj files
$directoryToSearch = $RootPath
$revisionYear = Get-Date -Format "yy";
$revisionDay = (Get-Date).DayOfYear;

$revisionDate = "$($revisionYear)$($revisionDay)"

Write-Host "Searching for .csproj files in $directoryToSearch"

# Find all .csproj files in the specified directory and its subdirectories
$csprojFiles = Get-ChildItem -Path $directoryToSearch -Filter *.csproj -Recurse | Where-Object { $_ -notcontains 'Tests' }

# Loop through each .csproj file and replace the version number
foreach ($csprojFile in $csprojFiles) {
    # Load the XML file into a variable
    [xml]$xml = Get-Content $csprojFile.FullName

    Write-Host "Updating  version for $csprojFile..."

    # Find the Version element and update its value
    $xml.Project.PropertyGroup[0].Version = $AssemblyVersion
    $xml.Project.PropertyGroup[0].AssemblyVersion = "$($AssemblyVersion).$revisionDate"
    $xml.Project.PropertyGroup[0].FileVersion = "$($AssemblyVersion).$revisionDate"

    # Save the updated XML back to the file
    $xml.Save($csprojFile.FullName)
}

Write-Host "All .csproj files updated to version $newVersion"