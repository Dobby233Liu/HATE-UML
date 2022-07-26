param(
    [Parameter(Mandatory=$true)]
    [String]$SolutionDir,
    [Parameter(Mandatory=$true)]
    [String]$TargetDir
)

function Copy-Item-IfExists {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true)]
        [string[]]$Source,
        [String]$Destination
    )

    foreach ($file in $Source)
    {
        if (-not (Test-Path -Path $file))
        {
            continue
        }
        Copy-Item -Force -Path $file -Destination $Destination
        Write-Host "${file} -> ${Destination}"
    }
}

$files = @(
    (Join-Path $SolutionDir "README.md"),
    (Join-Path $SolutionDir "COPYING.txt"),
    (Join-Path $SolutionDir "LICENSE.MIT.txt")
)

Copy-Item-IfExists $files $TargetDir