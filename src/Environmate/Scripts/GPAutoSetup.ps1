# TODO -------------------------------------------------------------------------
# Make sure none of the used variables are default system variables
# Talk to teams about any configurations that might be missing

# Initialize -------------------------------------------------------------------
Write-Host "Starting the GP-Automation automated installer...`n"
# --- This `if` statement may be able to be deleted in a future version. It is included to avoid a current bug in the Appx module for PowerShell Core
if ($PSEdition -eq "Core") {
    Import-Module -Name Appx -UseWindowsPowerShell -WarningAction SilentlyContinue
}
# ---
$WebRequest = Invoke-WebRequest -URI "http://www.google.com/"
if ($WebRequest.StatusDescription -eq "OK") {
    $InternetAccessible = $true
}
else {
    $InternetAccessible = $false
}
$WindowsVersion = (Get-CimInstance Win32_OperatingSystem).Version
$InstallersDir = "Z:\GPAutoStatic\Installers"


# Configure Network Drives -----------------------------------------------------
Write-Host "`nConfiguring Network Drives..."

$NetworkDrives = @(
    @('X', '\\gpops\tableau_files'),
    @('Y', '\\bdrop\dropbox'),
    @('Z', '\\gpauto\automation')
)

function MountDrive {
    param(
        [String]$Drive,
        [String]$Root
    )
    if (-Not (Test-Path -Path "$Drive`:\")) {
        New-PSDrive -Name $Drive -PSProvider FileSystem -Root $Root -Scope Global -Persist
    }
    else {
        if ((Get-PSDrive -Name $Drive).DisplayRoot -ne $Root) {
            Remove-PSDrive -Name $Drive -PSProvider FileSystem -Scope Global
            New-PSDrive -Name $Drive -Root $Root -PSProvider FileSystem -Scope Global -Persist
            Write-Host "The '$Drive' drive has been re-mapped to '$Root'"
        }
        else {
            Write-Host "The '$Drive' drive is already properly mounted ($Root)"
        }
    }
}

foreach ($NetworkDrive in $NetworkDrives) {
    MountDrive -Drive $NetworkDrive[0] -Root $NetworkDrive[1]
}


# Check Software Installation Status -------------------------------------------
Write-Host "`nChecking general software installation status..."
# Exit if `InstallersDir` does not exist
if (-Not (Test-Path -Path $InstallersDir -PathType Container)) {
    Write-Host "The installers directory '$InstallersDir' could not be found."
    Read-Host -Prompt “Press Enter to abort the setup script”
    exit
}

[System.Collections.ArrayList]$SoftwareList = @()
# Windows Terminal
if ((Get-AppxPackage | Where-Object Name -eq "Microsoft.WindowsTerminal").Status -eq 'Ok') {
    $WindowsTerminalInstalledVersion = (Get-AppxPackage | Where-Object Name -eq "Microsoft.WindowsTerminal").Version
}
else {
    $WindowsTerminalInstalledVersion = '0'
}
if ($WindowsVersion.split('.')[0] -eq '10') {
    $WindowsTerminalInstaller = (Get-Command "$InstallersDir\*Terminal_Win10*.msixbundle")
}
elseif ($WindowsVersion.split('.')[0] -eq '11') {
    $WindowsTerminalInstaller = (Get-Command "$InstallersDir\*Terminal_Win11*.msixbundle")
}
$WindowsTerminal = [PSCustomObject]@{
    Name = 'Windows Terminal'
    Version = $WindowsTerminalInstalledVersion
    WingetID = "Microsoft.WindowsTerminal"
    LocalInstaller = $WindowsTerminalInstaller.Source
    LocalInstallerVersion = $WindowsTerminalInstaller.Name.Split('_')[2]
    InstallAsAdmin = $false
}
$SoftwareList.Add($WindowsTerminal) | Out-Null

# Microsoft .Net SDK
$MicrosoftDotNetSDKInstalled = Test-Path -Path 'C:\Program Files\dotnet\sdk' -PathType Container
if ($MicrosoftDotNetSDKInstalled) {
    $MicrosoftDotNetSDKInstalledVersion = (Get-ChildItem 'C:\Program Files\dotnet\sdk').Name
}
else {
    $MicrosoftDotNetSDKInstalledVersion = '0'
}
$MicrosoftDotNetSDKInstaller = (Get-Command "$InstallersDir\*dotnet*.exe")
$MicrosoftDotNetSDK = [PSCustomObject]@{
    Name = 'Microsoft .NET SDK'
    Version = $MicrosoftDotNetSDKInstalledVersion
    WingetID = "Microsoft.DotNet.SDK.7"
    LocalInstaller = $MicrosoftDotNetSDKInstaller.Source
    LocalInstallerVersion = $MicrosoftDotNetSDKInstaller.Name.Split('-')[2]
    InstallAsAdmin = $true
}
$SoftwareList.Add($MicrosoftDotNetSDK) | Out-Null

# Visual Studio Build Tools
# $VisualStudioBuildToolsInstalled = Test-Path -Path '' -PathType Container
# if ($VisualStudioBuildToolsInstalled) {
#     $VisualStudioBuildToolsInstalledVersion = (Get-ChildItem 'C:\Program Files\dotnet\sdk').Name
# }
# else {
#     $VisualStudioBuildToolsInstalledVersion = '0'
# }
# $VisualStudioBuildToolsInstaller = (Get-Command "$InstallersDir\*dotnet*.exe")
# $VisualStudioBuildTools = [PSCustomObject]@{
#     Name = 'Visual Studio Build Tools'
#     Version = $VisualStudioBuildToolsInstalledVersion
#     WingetID = 'Microsoft.VisualStudio.2022.BuildTools'
#     LocalInstaller = $VisualStudioBuildToolsInstaller.Source
#     LocalInstallerVersion = $VisualStudioBuildToolsInstaller.Name.Split('-')[2]
#     InstallAsAdmin = $true
# }
# # Under `Workloads` Select .NET desktop build tools
# # Under `Individual components` select ['.NET Framework 4.8.1 development tools', 'F# compiler', 'NuGet package manager', 'C# and Visual Basic']
# # Install
# $SoftwareList.Add($VisualStudioBuildTools) | Out-Null

# NuGet
# $NuGetInstalled = Test-Path -Path '' -PathType Container
# if ($NuGetInstalled) {
#     $NuGetInstalledVersion = (Get-ChildItem 'C:\Program Files\dotnet\sdk').Name
# }
# else {
#     $NuGetInstalledVersion = '0'
# }
# $NuGetInstaller = (Get-Command "$InstallersDir\*dotnet*.exe")
# $NuGet = [PSCustomObject]@{
#     Name = 'NuGet'
#     Version = $NuGetInstalledVersion
#     WingetID = 'Microsoft.NuGet'
#     LocalInstaller = $NuGetInstaller.Source
#     LocalInstallerVersion = $NuGetInstaller.Name.Split('-')[2]
#     InstallAsAdmin = $true
# }
# $SoftwareList.Add($NuGet) | Out-Null

# PowerShell Core
$PowerShellCoreInstalled = Test-Path -Path 'C:\Program Files\PowerShell\' -PathType Container
if ($PowerShellCoreInstalled) {
    $PowerShellCoreLookup = (Get-ChildItem 'C:\Program Files\PowerShell\').Name
    if ($PowerShellCoreLookup.GetType().BaseType.Name -eq 'Array') {
        $PowerShellCoreLookup = ($PowerShellCoreLookup | Sort-Object -Descending)
        if ($PowerShellCoreLookup[0] -ne 'Scripts') {
            $PowerShellCoreLookup = $PowerShellCoreLookup[0]
        }
        else {
            $PowerShellCoreLookup = $PowerShellCoreLookup[1]
        }
    }
    $PowerShellCoreInstalledVersion = (Get-Command "C:\Program Files\PowerShell\$PowerShellCoreLookup\pwsh.exe").Version.ToString()
}
else {
    $PowerShellCoreInstalledVersion = '0'
}
$PowerShellCoreInstaller = (Get-Command "$InstallersDir\*PowerShell*.msi")
$PowerShellCore = [PSCustomObject]@{
    Name = 'Powershell Core'
    Version = $PowerShellCoreInstalledVersion
    WingetID = "Microsoft.PowerShell"
    LocalInstaller = $PowerShellCoreInstaller.Source
    LocalInstallerVersion = $PowerShellCoreInstaller.Name.Split('-')[1]
    InstallAsAdmin = $true
}
$SoftwareList.Add($PowerShellCore) | Out-Null

# Python3
$Python3Installed = Test-Path -Path "$HOME\AppData\Local\Programs\Python\" -PathType Container
if ($Python3Installed) {
    $Python3Lookup = (Get-ChildItem "$HOME\AppData\Local\Programs\Python\").Name
    if ($Python3Lookup.GetType().BaseType.Name -eq 'Array') {
        $Python3Lookup = ($Python3Lookup | Sort-Object -Descending)[0]
    }
    $Python3InstalledVersion = (Get-Command "$HOME\AppData\Local\Programs\Python\$Python3Lookup\python.exe").Version.ToString()
}
else {
    $Python3InstalledVersion = '0'
}
$Python3Installer = (Get-Command "$InstallersDir\*python*.exe")
$Python3 = [PSCustomObject]@{
    Name = 'Python3'
    Version = $Python3InstalledVersion
    WingetID = "Python.Python.3.11"
    LocalInstaller = $Python3Installer.Source
    LocalInstallerVersion = $Python3Installer.Version
    InstallAsAdmin = $true
}
$SoftwareList.Add($Python3) | Out-Null

# Miniconda

# Java SDK
$JavaSDKInstalled = Test-Path -Path 'C:\Program Files\Amazon Corretto\' -PathType Container
if ($JavaSDKInstalled) {
    $JavaSDKLookup = (Get-ChildItem 'C:\Program Files\Amazon Corretto\').Name
    if ($JavaSDKLookup.GetType().BaseType -eq 'System.Array') {
        $JavaSDKLookup = ($JavaSDKLookup | Sort-Object -Descending)[0]
    }
    $JavaSDKInstalledVersion = (Get-Command "C:\Program Files\Amazon Corretto\$JavaSDKLookup\bin\java.exe").Version.ToString().Split('.')[0..2] -join '.'
}
else {
    $JavaSDKInstalledVersion = '0'
}
$JavaSDKInstaller = (Get-Command "$InstallersDir\*corretto*.msi")
$JavaSDK = [PSCustomObject]@{
    Name = 'OpenJDK/Amazon Corretto'
    Version = $JavaSDKInstalledVersion
    WingetID = "Amazon.Corretto.17"
    LocalInstaller = $JavaSDKInstaller.Source
    LocalInstallerVersion = $JavaSDKInstaller.Name.Split('-')[2].Split('.')[0..2] -join '.'
    InstallAsAdmin = $true
}
$SoftwareList.Add($JavaSDK) | Out-Null
# ***TODO*** - Look into if there are additional steps for registering corretto as the default JDK

# # Microsoft Office

# ***TODO*** - Obtain an offline Microsoft Office Installer

# $MicrosoftOfficeInstaller = ?
# $MicrosoftOffice = [PSCustomObject]@{
#     Name = 'Microsoft Office'
#     Version = (Get-Command "C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE").Version.ToString()
#     Installer = $MicrosoftOfficeInstaller.Source
#     InstallerVersion = ?
# }
# $SoftwareList.Add($MicrosoftOffice) | Out-Null

# Git
$GitInstalled = Test-Path -Path "C:\Program Files\Git\" -PathType Container
if ($GitInstalled) {
    $GitInstalledVersion = (Get-Command "C:\Program Files\Git\git-bash.exe").Version.ToString()
}
else {
    $GitInstalledVersion = '0'
}
$GitInstaller = (Get-Command "$InstallersDir\*Git*.exe")
$Git = [PSCustomObject]@{
    Name = 'Git'
    Version = $GitInstalledVersion
    WingetID = "Git.Git"
    LocalInstaller = $GitInstaller.Source
    LocalInstallerVersion = $GitInstaller.Version
    InstallAsAdmin = $true
}
$SoftwareList.Add($Git) | Out-Null

# VS Code
$VSCodeInstalled = Test-Path -Path "C:\Users\$env:username\AppData\Local\Programs\Microsoft VS Code\" -PathType Container
if ($VSCodeInstalled) {
    $VSCodeInstalledVersion = (Get-Command "C:\Users\$env:username\AppData\Local\Programs\Microsoft VS Code\Code.exe").Version.ToString()
}
else {
    $VSCodeInstalledVersion = '0'
}
$VSCodeInstaller = (Get-Command "$InstallersDir\*VSCode*.exe")
$VSCode = [PSCustomObject]@{
    Name = 'Visual Studio Code'
    Version = $VSCodeInstalledVersion
    WingetID = "Microsoft.VisualStudioCode"
    LocalInstaller = $VSCodeInstaller.Source
    LocalInstallerVersion = $VSCodeInstaller.Version
    InstallAsAdmin = $false
}
$SoftwareList.Add($VSCode) | Out-Null

# Notepad++
$NotePadPlusPlusInstalled = Test-Path -Path "C:\Program Files\Notepad++\" -PathType Container
if ($NotePadPlusPlusInstalled) {
    $NotePadPlusPlusInstalledVersion = (Get-Command "C:\Program Files\Notepad++\notepad++.exe").Version.ToString()
}
else {
    $NotePadPlusPlusInstalledVersion = '0'
}
$NotepadPlusPlusInstaller = (Get-Command "$InstallersDir\*npp*.exe")
$NotepadPlusPlus = [PSCustomObject]@{
    Name = 'Notepad++'
    Version = $NotePadPlusPlusInstalledVersion
    WingetID = "Notepad++.Notepad++"
    LocalInstaller = $NotepadPlusPlusInstaller.Source
    LocalInstallerVersion = $NotepadPlusPlusInstaller.Version
    InstallAsAdmin = $true
}
$SoftwareList.Add($NotepadPlusPlus) | Out-Null

# Nano
# $NanoInstalled = Test-Path -Path "C:\Program Files\Notepad++\" -PathType Container
# if ($NanoInstalled) {
#     $NanoInstalledVersion = (Get-Command "C:\Program Files\Notepad++\notepad++.exe").Version.ToString()
# }
# else {
#     $NanoInstalledVersion = '0'
# }
# $NanoInstaller = (Get-Command "$InstallersDir\*npp*.exe")
# $Nano = [PSCustomObject]@{
#     Name = 'Nano'
#     Version = $NanoInstalledVersion
#     WingetID = "GNU.Nano"
#     LocalInstaller = $NanoInstaller.Source
#     LocalInstallerVersion = $NanoInstaller.Version
#     InstallAsAdmin = $true
# }
# $SoftwareList.Add($Nano) | Out-Null

# Vim (future effort)

# Emacs (future effort)

# 7-Zip
$7ZipInstalled = Test-Path -Path "C:\Program Files\7-Zip\" -PathType Container
if ($7ZipInstalled) {
    $7ZipInstalledVersion = (Get-Command "C:\Program Files\7-Zip\7zFM.exe").Version.ToString()
}
else {
    $7ZipInstalledVersion = '0'
}
$7ZipInstaller = (Get-Command "$InstallersDir\*7z*.exe")
$7Zip = [PSCustomObject]@{
    Name = '7-Zip'
    Version = $7ZipInstalledVersion
    WingetID = '7zip.7zip'
    LocalInstaller = $7ZipInstaller.Source
    LocalInstallerVersion = $7ZipInstaller.Version
    InstallAsAdmin = $true
}
$SoftwareList.Add($7Zip) | Out-Null

# Revo Uninstaller
$RevoUninstallerInstalled = Test-Path -Path "C:\Program Files\VS Revo Group\Revo Uninstaller\" -PathType Container
if ($RevoUninstallerInstalled) {
    $RevoUninstallerInstalledVersion = (Get-Command "C:\Program Files\VS Revo Group\Revo Uninstaller\RevoUnin.exe").Version.ToString()
}
else {
    $RevoUninstallerInstalledVersion = '0'
}
$RevoUninstallerInstaller = (Get-Command "$InstallersDir\*revo*.exe")
$RevoUninstaller = [PSCustomObject]@{
    Name = 'Revo Uninstaller'
    Version = $RevoUninstallerInstalledVersion
    WingetID = 'RevoUninstaller.RevoUninstaller'
    LocalInstaller = $RevoUninstallerInstaller.Source
    LocalInstallerVersion = $RevoUninstallerInstaller.Version
    InstallAsAdmin = $true
}
$SoftwareList.Add($RevoUninstaller) | Out-Null

# Handle suggested installations/updates
function UpdateSoftware {
    param([Object]$Software)
    if ($false) {
        if ($Software.Version -eq 0) {
            Invoke-Expression "winget install --id $($Software.WingetID)"
        }
        else {
            Invoke-Expression "winget upgrade --id $($Software.WingetID)"
        }
    }
    else {
        if ($Software.Version -lt $Software.LocalInstallerVersion) {
            while ($true) {
                $UserInput = Read-Host "`nAn update is available for $($Software.Name) ($($Software.Version) -> $($Software.LocalInstallerVersion)).`nWould you like to perform the installation?`n[Y] Yes`n[N] No`nSelection"
                Write-Host ""
                if ($UserInput.ToUpper() -eq 'Y') {
                    if ($Software.Name -eq 'Dynamic Devices MethodManager4' -and $Software.Version -ne '0') {
                        $MethodManagerInstance = Get-CimInstance Win32_Product | Where-Object {$_.Name -eq 'MethodManager4'}
                        msiexec /x $MethodManagerInstance.InstallSource
                    }
                    Start-Process -FilePath $Software.LocalInstaller -Wait
                    break
                }
                elseif ($UserInput.ToUpper() -eq 'N') {
                    break
                }
            }
        }
        else {
            Write-Host "$($Software.Name) is up to date (v$($Software.Version))"
        }
    }
    
}
foreach ($Software in $SoftwareList) {
    UpdateSoftware -Software $Software
}

# Windows Terminal Configuration -----------------------------------------------
Write-Host "`nPerforming Windows Terminal Configuration..."
function InstallFont {
    param(
        [String]$FontFamily
    )
    $WindowsFontDir = "C:\Windows\Fonts"
    $CustomFontDir = "$InstallersDir\Fonts"
    Expand-Archive -Path "$CustomFontDir\$FontFamily.zip" -DestinationPath "$CustomFontDir\$FontFamily"
    $Fonts = Get-ChildItem -Path "$CustomFontDir\$FontFamily" -Include '*.ttf'
    foreach ($Font in $Fonts) {
        if (-Not (Test-Path -Path "$WindowsFontDir\$Font")) {
            Copy-Item -Path "$CustomFontDir\$Font" -Destination "$WindowsFontDir\$Font"
        }
    }
    Remove-Item -Path "$CustomFontDir\$FontFamily" -Recurse
}
$WindowsTerminalDefaultSettingsFile = "$HOME\AppData\Local\Packages\Microsoft.WindowsTerminal_8wekyb3d8bbwe\LocalState\settings.json"
$WindowsTerminalDefaultSettings = Get-Content $WindowsTerminalDefaultSettingsFile | ConvertFrom-Json
$WindowsTerminalDefaultGUID = $WindowsTerminalDefaultSettings.defaultProfile
foreach ($WindowsTerminalProfile in $WindowsTerminalDefaultSettings.profiles.list) {
    switch ($WindowsTerminalProfile.name) {
        "PowerShell Core" {$WindowsTerminalPowerShellGUID = $WindowsTerminalProfile.guid}
        "Ubuntu" {$WindowsTerminalUbuntuGUID = $WindowsTerminalProfile.guid}
        "Command Prompt" {$WindowsTerminalCommandPromptGUID = $WindowsTerminalProfile.guid}
        "Git Bash" {$WindowsTerminalGitGUID = $WindowsTerminalProfile.guid}
        "Windows PowerShell" {$WindowsTerminalWindowsPowerShellGUID = $WindowsTerminalProfile.guid}
    }
}
$WindowsTerminalCustomSettingsFile = "Z:\GPAutoSetup\Settings\settings.json"
$WindowsTerminalCustomSettings = Get-Content $WindowsTerminalCustomSettingsFile | ConvertFrom-Json
$WindowsTerminalCustomSettings.defaultProfile = $WindowsTerminalDefaultGUID
foreach ($WindowsTerminalProfile in $WindowsTerminalCustomSettings.list) {
    switch ($WindowsTerminalProfile.name) {
        "PowerShell Core" {$WindowsTerminalProfile.guid = $WindowsTerminalPowerShellGUID}
        "Ubuntu" {$WindowsTerminalProfile.guid = $WindowsTerminalUbuntuGUID}
        "Command Prompt" {$WindowsTerminalProfile.guid = $WindowsTerminalCommandPromptGUID}
        "Git Bash" {$WindowsTerminalProfile.guid = $WindowsTerminalGitGUID}
        "Windows PowerShell" {$WindowsTerminalProfile.guid = $WindowsTerminalWindowsPowerShellGUID}
    }
}
$WindowsTerminalCustomSettings | ConvertTo-Json -Depth 10 | Out-File $WindowsTerminalDefaultSettingsFile -Force


# PowerShell Configuration -----------------------------------------------------
Write-Host "`nPerforming PowerShell Configuration..."
Write-Host 'Installing `Oh My Posh`...'
$OhMyPoshInstalled = Test-Path -Path "$HOME\AppData\Local\Programs\oh-my-posh" -PathType Container
if ($OhMyPoshInstalled) {
    $OhMyPoshInstalledVersion = (Get-Command "$HOME\AppData\Local\Programs\oh-my-posh\bin\oh-my-posh.exe").Version.ToString()
}
else {
    $OhMyPoshInstalledVersion = '0'
}
$OhMyPoshInstaller = (Get-Command "$InstallersDir\*oh-my-posh*.exe")
$OhMyPosh = [PSCustomObject]@{
    Name = 'Oh My Posh'
    Version = $OhMyPoshInstalledVersion
    Installer = $OhMyPoshInstaller.Source
    InstallerVersion = $OhMyPoshInstaller.FileVersionInfo.ProductVersion
}
UpdateSoftware -Software $OhMyPosh
if (-Not (Test-Path -Path "$HOME\Documents\PowerShell\Modules\Terminal-Icons" -PathType Container)) {
    Copy-Item -Path "$InstallersDir\Terminal-Icons" -Destination "$HOME\Documents\PowerShell\Modules\" -Recurse
    Import-Module -Name Terminal-Icons
    Write-Host '`Terminal-Icons` was successfully installed.'
}
function EditConfigFile {
    param(
        [String]$File,
        [String]$Command,
        [Int]$Line
    )
    [System.Collections.ArrayList]$NewFileContents = @(Get-Content $File)
    if (-Not ($Command -in $NewFileContents)) {
        $NewFileContents.Insert($Line, $Command)
    }
    elseif ($NewFileContents.IndexOf($Command) -ne $Line) {
        $NewFileContents.Remove($NewFileContents.IndexOf($Command))
        $NewFileContents.Insert($Line, $Command)
    }
    else {return}
    $NewFileContents | Out-File $File
}
$PowerShellProfileFile = "$HOME\Documents\PowerShell\profile.ps1"
if (-Not (Test-Path -Path $PowerShellProfileFile -PathType Leaf)) {
    New-Item -Path "$HOME\Documents\PowerShell\" -Name 'profile.ps1' -ItemType File
}
$OhMyPoshPowerShellInit = 'oh-my-posh init pwsh --config "$env:POSH_THEMES_PATH\powerline.omp.json" | Invoke-Expression'
$TerminalIconsInit = 'Import-Module -Name Terminal-Icons'
EditConfigFile -File $PowerShellProfileFile -Command $OhMyPoshPowerShellInit -Line 0
EditConfigFile -File $PowerShellProfileFile -Command $TerminalIconsInit -Line 1


# Windows PowerShell Configuration ---------------------------------------------
Write-Host "`nPerforming Windows PowerShell Configuration..."
if (-Not (Test-Path -Path "$HOME\Documents\WindowsPowerShell\Modules\Terminal-Icons" -PathType Container)) {
    Copy-Item -Path "$InstallersDir\Terminal-Icons" -Destination "$HOME\Documents\WindowsPowerShell\Modules\"
    Import-Module -Name Terminal-Icons
    Write-Host '`Terminal-Icons` was successfully installed.'
}
$WindowsPowerShellProfileFile = "$HOME\Documents\WindowsPowerShell\profile.ps1"
if (-Not (Test-Path -Path $WindowsPowerShellProfileFile -PathType Leaf)) {
    New-Item -Path "$HOME\Documents\WindowsPowerShell\" -Name 'profile.ps1' -ItemType File
}
$OhMyPoshWindowsPowerShellInit = 'oh-my-posh init powershell --config "$env:POSH_THEMES_PATH\powerline.omp.json" | Invoke-Expression'
EditConfigFile -File $WindowsPowerShellProfileFile -Command $OhMyPoshWindowsPowerShellInit -Line 0
EditConfigFile -File $WindowsPowerShellProfileFile -Command $TerminalIconsInit -Line 1


# DotNet Configuration ---------------------------------------------------------



# Git Configuration ------------------------------------------------------------
Write-Host "`nPerforming Git Configuration..."
if (-Not (Test-Path -Path "$HOME\.ssh" -PathType Container)) {
    New-Item -Path "$HOME" -Name '.ssh' -ItemType Directory
}
if (-Not (Test-Path -Path "$HOME\.ssh\config" -PathType Leaf)) {
    New-Item -Path "$HOME\.ssh" -Name 'config' -ItemType File
}

# Configure SSH
# Configure SSH or personal access token
    # Configure Deploy Key for origin
        # Requires that the GPAuto private deploy key be moved into .ssh/
        # TODO - Figure out where to store SSH keys
# if ($env:username -eq 'gpautomation') {
#     git config --global user.name 'GP Automation'
# }
# else {
#     $GitUserName = Read-Host "Please provide username associated with this device`nuser.name"
#     git config --global user.name $GitUserName
#     $GitUserEmail = Read-Host "Please provide user email associated with this device`nuser.email"
#     git config --global user.email $GitUserEmail
# }

# Ensure git symlinks are enabled
$GitConfigs = git config --list --show-scope
$GitConfigScopes = @('system', 'global', 'local', 'worktree')
foreach ($GitConfigScope in $GitConfigScopes) {
    if ("$GitConfigScope`tcore.symlinks=false" -in $GitConfigs) {
        "git config --$GitConfigScope core.symlinks true" | Invoke-Expression
    }
}

# Clone Repositories
# git clone git@github.com:broadinstitute/gp-automationprotocols.git $HOME\repos
# $NewUser = Read-Host "Would you like to configure a user remote configuration for `gp-automationprotocols`?`n[Y] Yes`n[N] No`nSelection"
# if ($NewUser.ToUpper() -eq 'Y') {
#     # Once SSH is setup, configure how it will be setup here
# }

# VSCode Configuration ---------------------------------------------------------
Write-Host "`nPerforming VSCode Configuration..."
# TODO - This will likely be a larger undertaking
<#
VSCode Extensions
- [ ] Python
- [ ] Jupyter
- [ ] Pylance
- [ ] C/C++
- [ ] Live Server
- [ ] Prettier
- [ ] IntelliCode
- [ ] ESLint
- [ ] Docker
- [ ] (Maybe) GitLens
- [ ] (Maybe) Extension Pack for Java
- [ ] C#
- [ ] WSL
- [ ] Code Runner
- [ ] Dev Containers
- [ ] Material Icon Theme
- [ ] Remote - SSH
- [ ] CMake
- [ ] vscode-icons
- [ ] HTML CSS
- [ ] Remote - SSH: Editing Configuration Files
- [ ] Auto Rename Tag
- [ ] Auto Close Tag
- [ ] Live Share
- [ ] isort
- [ ] YAML
- [ ] GitHub Pull Requests and Issues
- [ ] Path Intellisense
- [ ] Go
- [ ] IntelliCode API Usage Examples
- [ ] Maybe Git History
- [ ] PowerShell
- [ ] Open in browser
- [ ] Code Spell Checker
- [ ] Dart
- [ ] Django
- [ ] IntelliSense for CSS class names in HTML
- [ ] Markdown All in One
- [ ] Flutter
- [ ] npm Intellisense
- [ ] Jinja
- [ ] Tabnine
- [ ] Markdown Lint
- [ ] Better Comments
- [ ] Git Graph
- [ ] autoDocstring
- [ ] Markdown Preview Enhanced
- [ ] Python Indent
- [ ] XML
- [ ] DotENV
- [ ] Remote Development
- [ ] (Maybe) Remote Develooment
- [ ] React Native Tools
- [ ] Remote Explorer
- [ ] GitHub Copilot
- [ ] Excel Viewer
- [ ] Python Environment Manager
- [ ] vscode-pdf
- [ ] Todo Tree
- [ ] Kubernetes
- [ ] Rainbow CSV
- [ ] Tailwind CSS
- [ ] Sass
- [ ] Hex Editor
- [ ] (Maybe) Edge and Firefox
- [ ] rust-analyzer
- [ ] Trailing Spaces
- [ ] solidity
- [ ] R
- [ ] MongoDB
- [ ] Django Template
- [ ] PostgreSQL
- [ ] Pylint
- [ ] JavaScript Debugger
#>

# Perform Device Specific Configurations and Updates ---------------------------
Write-Host "`nPerforming device specific configuration..."

function ConfigureHamiltonStar {
    # ***TODO*** - Understand how to install and configure Hamilton software to get this information
    # $HamiltonMethodEditorInstaller = ?
    # $HamiltonMethodEditor = [PSCustomObject]@{
    #     Name = 'Hamilton Method Editor'
    #     Version = (Get-Command "C:\Program Files (x86)\HAMILTON\Bin\HxMetEd.exe").Version.ToString()
    #     Installer = $HamiltonMethodEditorInstaller.Source
    #     InstallerVersion = ?
    # }
    # UpdateSoftware -Software $HamiltonMethodEditor

    # ***TODO*** - Write additional configurations for Hamiltons
}

function ConfigureAgilentBravo {
    $AgilentVWorksInstaller = (Get-Command "$InstallersDir\*VWorks*.exe")
    $AgilentVWorks = [PSCustomObject]@{
        Name = 'Agilent VWorks'
        Version = (Get-Command "C:\Program Files (x86)\Agilent Technologies\VWorks\VWorks.exe").Version.ToString()
        Installer = $AgilentVWorksInstaller.Source
        InstallerVersion = $AgilentVWorksInstaller.Version
    }
    UpdateSoftware -Software $AgilentVWorks

    # ***TODO*** - Write additional configurations for Bravos
}

function ConfigureDynamicDevicesLynx {
    Write-Host "Configuring the DynamicDevicesLynx environment..."
    # Install or updated method manager
    $DDMethodManager4Installer = (Get-Command "$InstallersDir\*MethodManager4*.exe")
    $DDMethodManager4 = [PSCustomObject]@{
        Name = 'Dynamic Devices MethodManager4'
        Version = (Get-Command "C:\Program Files (x86)\Dynamic Devices\MethodManager4\MethodManager.DX.exe").Version.ToString()
        LocalInstaller = $DDMethodManager4Installer.Source
        LocalInstallerVersion = $DDMethodManager4Installer.Name.Split('-')[1]
        InstallAsAdmin = $true
    }
    UpdateSoftware -Software $DDMethodManager4
    # Enable remote symlinks
    $SymlinkPermissions = fsutil behavior query symlinkEvaluation
    if ($SymlinkPermissions[2] -ne 'Remote to local symbolic links are enabled.') {
        fsutil behavior set symlinkEvaluation R2L:1
    }
    if ($SymlinkPermissions[3] -ne 'Remote to remote symbolic links are enabled.') {
        fsutil behavior set symlinkEvaluation R2R:1
    }        
    # Create local workspace variables file and Logs output folder
    if (-Not (Test-Path -Path "C:\MethodManager4\" -PathType Container)) {
        New-Item -Path "C:\" -Name 'MethodManager4' -ItemType Directory
    }
    if (-Not (Test-Path -Path "C:\MethodManager4\Output\" -PathType Container)) {
        New-Item -Path "C:\MethodManager4\" -Name 'Output' -ItemType Directory
    }
    if (-Not (Test-Path -Path "C:\MethodManager4\Output\Logs" -PathType Container)) {
        New-Item -Path "C:\MethodManager4\Output\" -Name 'Logs' -ItemType Directory
    }
    if (-Not (Test-Path -Path "C:\MethodManager4\Output\LLD" -PathType Container)) {
        New-Item -Path "C:\MethodManager4\Output\" -Name 'LLD' -ItemType Directory
    }
    if (-Not (Test-Path -Path "C:\MethodManager4\WorkspaceVariables.config" -PathType Leaf)) {
        New-Item -Path "C:\MethodManager4\" -Name 'WorkspaceVariables.config' -ItemType File
    }
    if (-Not (Test-Path -Path "C:\MethodManager4\WorkspaceRecentFiles.config" -PathType Leaf)) {
        New-Item -Path "C:\MethodManager4\" -Name 'WorkspaceRecentFiles.config' -ItemType File
    }
    # Register help files using symlinks
    if (Test-Path -Path "C:\MethodManager4\Help\" -PathType Container) {
        if ((Get-Item -Path "C:\MethodManager4\Help\").Mode[0] -ne 'l') {
            Remove-Item -Path "C:\MethodManager4\Help\"
            New-Item -ItemType SymbolicLink -Path "C:\MethodManager4\Help\" -Target "Z:\automationprotocols\DynamicDevicesLynx\Help\"
        }
    }
    else {
        New-Item -ItemType SymbolicLink -Path "C:\MethodManager4\Help\" -Target "Z:\automationprotocols\DynamicDevicesLynx\Help\"
    }
    # Register DDI and DDX labware using symlinks
    if (-Not (Test-Path -Path "C:\ProgramData\MethodManager4\")) {
        New-Item -Path "C:\ProgramData\" -Name "MethodManager4" -ItemType Directory
    }
    if ( -Not (Test-Path -Path "C:\ProgramData\MethodManager4\Labware\" -PathType Container)) {
        New-Item -Path "C:\ProgramData\MethodManager4\" -Name 'Labware' -ItemType Directory
    }
    function RegisterDDLabware {
        param([String]$FileName)
        if (Test-Path -Path "C:\ProgramData\MethodManager4\Labware\$FileName" -PathType Leaf) {
            if (-Not ((Get-ChildItem "C:\ProgramData\MethodManager4\Labware\$FileName").Mode[0] -eq 'l')) {
                Remove-Item -Path "C:\ProgramData\MethodManager4\Labware\$FileName" -ItemType File
                New-Item -Path "C:\ProgramData\MethodManager4\Labware\$FileName" -ItemType SymbolicLink `
                -Target "Z:\automationprotocols\DynamicDevicesLynx\Configurations\Global\$FileName"
            }
        }
        else {
            New-Item -Path "C:\ProgramData\MethodManager4\Labware\$FileName" -ItemType SymbolicLink `
            -Target "Z:\automationprotocols\DynamicDevicesLynx\Configurations\Global\$FileName"
        }
    }
    $DDLabwareFiles = 'DDI.TipType.config', 'DDI.TipboxType.config', 'DDX.TipType.config', 'DDX.TipboxType.config'
    foreach ($DDLabwareFile in $DDLabwareFiles) {
        RegisterDDLabware -FileName $DDLabwareFile
    }
}

function ConfigureLimsService {
# Register dlls
# ***TODOS***
# $LimsServiceDLL = Get-Command "Z:\New Computer Configuration Files\Lims DLL\Release\LimsService.dll"
    # 1. Decide on Z location to store LimsService.dll
    # 2. Decide on C location to store LimsService.dll
    # 3. Write code that copies LimsService.dll if it is not already on the C drive
    # 4. Write code that registers LimsService.dll
# ***TODO*** Figure out how to setup barcode scanner
# C:\gpauto\lib,bin,etc,var,tmp

# Understand C:\MESSAGING and implement it
}

function ConfigureDeveloperEnvironment {
    # Create section for the setup of a python environment
    # configure conda and mamba
    # include ipython configurations
    # include the creation of a dev conda environment

# Create section for the setup of a Linux environment

# Create section for the setup of a PowerShell environment

# Create section for the setup of a C/C++ environment

# Create section for the setup of a Node environment
}

# Choose Configuration Device
while ($true) {
    $UserInput = Read-Host "Please select the instrument to configure.`n[N] None`n[H] Hamilton Star/Starlett`n[B] Agilent Bravo`n[L] Dynamic Devices Lynx`n[D] Developer Environment`nSelection"
    Write-Host ""
    if ($UserInput.ToUpper() -eq 'N') {
        break
    }
    elseif ($UserInput.ToUpper() -eq 'H') {
        ConfigureHamiltonStar
        ConfigureLimsService
        break
    }
    elseif ($UserInput.ToUpper() -eq 'B') {
        ConfigureAgilentBravo
        ConfigureLimsService
        break
    }
    elseif ($UserInput.ToUpper() -eq 'L') {
        ConfigureDynamicDevicesLynx
        ConfigureLimsService
        break
    }
    elseif ($UserInput.ToUpper() -eq 'D') {
        ConfigureHamiltonStar
        ConfigureAgilentBravo
        ConfigureDynamicDevicesLynx
        ConfigureLimsService
        ConfigureDeveloperEnvironment
        break
    }
}

# Exit Script ------------------------------------------------------------------
Write-Host "`nAutomated device setup complete."
Read-Host -Prompt “Press Enter to exit”
