# Global Paths
$RepoDir = "C:\BroadInstitute\GPAuto"
$LynxDir = "$RepoDir\Environments\DynamicDevices\Lynx"
$ConfigDir = "$LynxDir\Config"
$WorkspacesDir = "$LynxDir\Workspaces"
# Workspace Definitions
$Workspaces = @('Beedle','Link','Ganondorf','Impa')
$Group = "VVP"
# Create Workspaces
foreach ($Workspace in $Workspaces) {
    $WorkspaceDir = "$WorkspacesDir\$Workspace"
    if (-Not (Test-Path -Path $WorkspaceDir -PathType Container)) {
        New-Item -Path $WorkspacesDir -Name $Workspace -ItemType Directory
    }
    if (-Not (Test-Path -Path "$WorkspaceDir\Configuration" -PathType Container)) {
        New-Item -Path $WorkspaceDir -Name 'Configuration' -ItemType Directory
    }
    # SymLink Definitions
    # @(Symlink Path, Target Path, Symlink Type)
    $SymLinks = @(
        # Global Scope
        @("$WorkspaceDir\Scripts", "$LynxDir\Scripts\", "Directory"),
        @("$WorkspaceDir\User.AxisSpeed.config", "$ConfigDir\Global\User.AxisSpeed.config", "File"),
        @("$WorkspaceDir\User.GripperSpeed.config", "$ConfigDir\Global\User.GripperSpeed.config", "File"),
        @("$WorkspaceDir\User.Labware.config", "$ConfigDir\Global\User.Labware.config", "File"),
        @("$WorkspaceDir\User.MotionProfile.config", "$ConfigDir\Global\User.MotionProfile.config", "File"),
        @("$WorkspaceDir\User.TipboxType.config", "$ConfigDir\Global\User.TipboxType.config", "File"),
        @("$WorkspaceDir\User.TipType.config", "$ConfigDir\Global\User.TipType.config", "File"),
        @("$WorkspaceDir\User.WasteType.config", "$ConfigDir\Global\User.WasteType.config", "File"),
	    @("$WorkspaceDir\CustomScriptReferences.config", "$ConfigDir\Global\CustomScriptReferences.config", "File"),
        # Group Scope
        @("$WorkspaceDir\Methods", "$LynxDir\Methods\VVP", "Directory"),
        @("$WorkspaceDir\User.PipetteScheme.config", "$ConfigDir\Group\$Group\User.PipetteScheme.config", "File"),
        @("$WorkspaceDir\User.VVPPressure.config", "$ConfigDir\Group\$Group\User.VVPPressure.config", "File"),
        @("$WorkspaceDir\Configuration\X Axis.xml", "$ConfigDir\Group\$Group\X Axis.xml", "File"),
        @("$WorkspaceDir\Configuration\Y Axis.xml", "$ConfigDir\Group\$Group\Y Axis.xml", "File"),
        @("$WorkspaceDir\Configuration\Z Axis.xml", "$ConfigDir\Group\$Group\Z Axis.xml", "File"),
        @("$WorkspaceDir\Configuration\GripperZ Axis.xml", "$ConfigDir\Group\$Group\GripperZ Axis.xml", "File"),
        @("$WorkspaceDir\WorkspaceSettings.config", "$ConfigDir\Group\$Group\WorkspaceSettings.config", "File"),
        @("$WorkspaceDir\InitializeOrder.xml", "$ConfigDir\Group\$Group\InitializeOrder.xml", "File"),
        @("$WorkspaceDir\WorkspaceIoLabels.config", "$ConfigDir\Group\$Group\WorkspaceIoLabels.config", "File"),
        @("$WorkspaceDir\Configuration\Lynx.Main.Worktable.ToolFilter.config", "$ConfigDir\Group\$Group\Lynx.Main.Worktable.ToolFilter.config", "File"),
        # Local Scope
        @("$WorkspaceDir\Workspace.config", "$ConfigDir\Local\$Workspace\Workspace.config", "File"),
        @("$WorkspaceDir\Configuration\Lynx.config", "$ConfigDir\Local\$Workspace\Lynx.config", "File"),
        @("$WorkspaceDir\Configuration\Lynx.Main.Worktable.config", "$ConfigDir\Local\$Workspace\Lynx.Main.Worktable.config", "File"),
        @("$WorkspaceDir\WorkspaceRecentFiles.config", "$ConfigDir\Local\$Workspace\WorkspaceRecentFiles.config", "File"),
        @("$WorkspaceDir\WorkspaceVariables.config", "$ConfigDir\Local\$Workspace\WorkspaceVariables.config", "File"),
        # Network Drive
        @("$WorkspaceDir\Output", "$RepoDir\Logs\DynamicDevices\Lynx\$Workspace\", "Directory")
    )
    # Create SymLinks
    foreach ($SymLink in $SymLinks) {
        if (Test-Path -Path $SymLink[0]) {
            $ExistingFile = Get-Item -Path $SymLink[0]
            if (($ExistingFile).Mode[0] -ne 'l') {
                Remove-Item -Path $SymLink[0]
                New-Item -ItemType SymbolicLink -Path $SymLink[0] -Target $SymLink[1]
            }
            elseif ($SymLink[2] -eq "File" -and ($ExistingFile).Mode[1] -ne 'a') {
                Remove-Item -Path $SymLink[0]
                New-Item -ItemType SymbolicLink -Path $SymLink[0] -Target $SymLink[1]
            }
            elseif ($SymLink[2] -eq "Directory" -and ($ExistingFile).Mode[1] -ne '-') {
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
}