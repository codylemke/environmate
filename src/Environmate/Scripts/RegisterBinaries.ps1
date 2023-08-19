# Retrieve COM Assemblies and .NET Framework dependencies from current directory
$ComAssemblies = Get-ChildItem -Path ".\" -Filter '*.dll' | Where-Object {$_.Name.Contains("Broad")}
$DotNetDependencies = Get-ChildItem -Path ".\" -Filter '*.dll' | Where-Object {!$_.Name.Contains("Broad")}

# Register each COM Assembly in the Windows Registry
foreach ($Assembly in $ComAssemblies) {
    $AssemblyName = $Assembly.BaseName
    &"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" "$AssemblyName.dll" "/tlb:$AssemblyName.tlb"
}

# Register each .Net Framework dependency in the Global Assembly Cache
foreach ($Dependency in $DotNetDependencies) {
    $DependencyName = $Dependency.BaseName
    &"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8.1 Tools\gacutil.exe" "/i" "$DependencyName.dll"
}