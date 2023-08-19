function MountNetworkDrive {
    param([String]$Drive, [String]$Root)
    if (-Not (Test-Path -Path "$Drive`:\")) {
        New-PSDrive -Name $Drive -PSProvider FileSystem -Root $Root -Scope Global -Persist
        return "The '$Drive' drive has been mapped to '$Root'"
    }
    else {
        if ((Get-PSDrive -Name $Drive).DisplayRoot -ne $Root) {
            Remove-PSDrive -Name $Drive -PSProvider FileSystem -Scope Global
            New-PSDrive -Name $Drive -Root $Root -PSProvider FileSystem -Scope Global -Persist
            return "The '$Drive' drive has been re-mapped to '$Root'"
        }
        else {
            return "The '$Drive' drive is already properly mounted ($Root)"
        }
    }
}