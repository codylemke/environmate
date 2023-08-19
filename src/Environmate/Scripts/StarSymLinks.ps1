# Global Paths
$RepoDir = "C:\BroadInstitute\GPAuto"
$StarDir = "$RepoDir\Environments\Hamilton\Star"
$InstallationDir = "C:\Program Files (x86)\HAMILTON"
# SymLink Definitions
# @(Symlink Path, Target Path, Symlink Type)
$SymLinks = @(
    # Global Scope
    @("$InstallationDir\Help\", "Z:\Help\", "Directory"),
    @("$InstallationDir\LabWare\", "$StarDir\Labware\", "Directory"),
    @("$InstallationDir\Library\", "$StarDir\Libraries\", "Directory"),
    @("$InstallationDir\Methods\", "$StarDir\Methods\", "Directory"),
    # Group Scope
    @("$InstallationDir\Config\", "$StarDir\Config\", "Directory"),
    # Local Scope
    @("$InstallationDir\LogFiles\", "$RepoDir\Logs\Hamilton\Star\", "Directory"),
)
# Create SymLinks
foreach ($SymLink in $SymLinks) {
    if (Test-Path -Path $SymLink[0]) {
        $ExistingItem = Get-Item -Path $SymLink[0]
        if ($ExistingItem.Mode[0] -ne 'l') {
            Remove-Item -Path $SymLink[0]
            New-Item -ItemType SymbolicLink -Path $SymLink[0] -Target $SymLink[1]
        }
        elseif ($SymLink[2] -eq "File" -and $ExistingFile.Mode[1] -ne 'a') {
            Remove-Item -Path $SymLink[0]
            New-Item -ItemType SymbolicLink -Path $SymLink[0] -Target $SymLink[1]
        }
        elseif ($SymLink[2] -eq "Directory" -and $ExistingFile.Mode[1] -ne '-') {
            Remove-Item -Path $SymLink[0] -Recurse
            New-Item -ItemType SymbolicLink -Path $SymLink[0] -Target $SymLink[1]
        }
        elseif ($ExistingFile.LinkTarget -ne $SymLink[1]) {
            Remove-Item -Path $SymLink[0]
            New-Item -ItemType SymbolicLink -Path $SymLink[0] -Target $SymLink[1]
        }
    }
    else {
        New-Item -ItemType SymbolicLink -Path $SymLink[0] -Target $SymLink[1]
    }
}
