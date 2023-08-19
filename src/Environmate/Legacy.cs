using System;
using Microsoft.Win32;
using System.Management.Automation;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;

namespace Environmate
{
    internal class DevSetup
    {
        internal static void Main(string[] args)
        {
            // Global constants --------------------------------------------------------------------------------
            const string GpAutoSetupDir = @"C:\BroadInstitute\GPAuto\Repos\GPAutoSetup\";
            const string OfflineInstallersDir = @"\\gpauto\automation\GPAutoStatic\OfflineInstallers";
            const string PsFunctionsFile = @"C:\BroadInstitute\GPAuto\Repos\GPAutoSetup\src\GPAutoSetup.Lib\PowerShell\PSFunctions.ps1";

            // Initialize --------------------------------------------------------------------------------------
            Console.WriteLine("Initializing computer system...");
            Console.WriteLine("-----------------------");
            ComputerSystem computerSystem = new ComputerSystem("localhost");
            Console.WriteLine($"Computer System Info");
            Console.WriteLine($"Machine Name: {computerSystem.Name}");
            Console.WriteLine($"Workgroup: {computerSystem.Workgroup}");
            Console.WriteLine($"Domain: {computerSystem.Domain}");
            Console.WriteLine($"Username: {computerSystem.Username}");
            Console.WriteLine($"Manufacturer: {computerSystem.Manufacturer}");
            Console.WriteLine($"Model: {computerSystem.Model}");
            Console.WriteLine($"Has Internet Access: {computerSystem.HasInternetAccess}");
            Console.WriteLine($"IP Address: {computerSystem.IpAddress}");
            Console.WriteLine($"Number of Physical Processors: {computerSystem.NumberOfPhysicalProcessors}");
            Console.WriteLine($"Number of Logical Processors: {computerSystem.NumberOfLogicalProcessors}");
            Console.WriteLine($"Total Physical Memory: {computerSystem.TotalPhysicalMemoryHR}");
            Console.WriteLine();
            Console.WriteLine($"Operating System Info");
            Console.WriteLine($"Status: {computerSystem.OperatingSystem.Status}");
            Console.WriteLine($"OS: {computerSystem.OperatingSystem.Name}");
            Console.WriteLine($"Manufacturer: {computerSystem.OperatingSystem.Manufacturer}");
            Console.WriteLine($"Version: {computerSystem.OperatingSystem.Version}");
            Console.WriteLine($"Architecture: {computerSystem.OperatingSystem.Architecture}");
            Console.WriteLine($"Install Date: {computerSystem.OperatingSystem.InstallDate}");
            Console.WriteLine($"Local DateTime: {computerSystem.OperatingSystem.LocalDateTime}");
            Console.WriteLine($"Last Bootup Time: {computerSystem.OperatingSystem.LastBootUpTime}");
            Console.WriteLine($"Number of Processes: {computerSystem.OperatingSystem.NumberOfProcesses}");
            Console.WriteLine($"Number of Users: {computerSystem.OperatingSystem.NumberOfUsers}");
            Console.WriteLine();
            Console.WriteLine($"Motherboard Info");
            Console.WriteLine($"Status: {computerSystem.BaseBoard.Status}");
            Console.WriteLine($"Manufacturer: {computerSystem.BaseBoard.Manufacturer}");
            Console.WriteLine($"Product: {computerSystem.BaseBoard.Product}");
            Console.WriteLine($"Version: {computerSystem.BaseBoard.Version}");
            Console.WriteLine();
            Console.WriteLine($"BIOS Info");
            Console.WriteLine($"Status: {computerSystem.BIOS.Status}");
            Console.WriteLine($"Manufacturer: {computerSystem.BIOS.Manufacturer}");
            Console.WriteLine($"Serial Number: {computerSystem.BIOS.SerialNumber}");
            Console.WriteLine($"Version: {computerSystem.BIOS.Version}");
            Console.WriteLine($"Release Date: {computerSystem.BIOS.ReleaseDate}");
            Console.WriteLine();
            Console.WriteLine($"Processor Info");
            Console.WriteLine($"Processor Count: {computerSystem.VideoControllers.Count}");
            foreach (var processor in computerSystem.Processors)
            {
                Console.WriteLine($"DeviceID: {processor.DeviceId}");
                Console.WriteLine($"Status: {processor.Status}");
                Console.WriteLine($"Role: {processor.Role}");
                Console.WriteLine($"Name: {processor.Name}");
                Console.WriteLine($"Manufacturer: {processor.Manufacturer}");
                Console.WriteLine($"Number of Cores: {processor.NumberOfCores}");
                Console.WriteLine($"Number of Logical Processors: {processor.NumberOfLogicalProcessors}");
                Console.WriteLine($"Thread Count: {processor.ThreadCount}");
                Console.WriteLine($"L2CacheSize: {processor.L2CacheSize}");
                Console.WriteLine($"L3CacheSize: {processor.L3CacheSize}");
                Console.WriteLine($"Socket Designation: {processor.SocketDesignation}");
                Console.WriteLine("---");
            }
            Console.WriteLine();
            Console.WriteLine("Physical Memory Info");
            Console.WriteLine($"Physical Memory Count: {computerSystem.PhysicalMemory.Count}");
            foreach (var memory in computerSystem.PhysicalMemory)
            {
                Console.WriteLine($"Tag: {memory.Tag}");
                Console.WriteLine($"Capacity: {memory.CapacityHR}");
                Console.WriteLine($"Configured Clock Speed: {memory.ConfiguredClockSpeed}");
                Console.WriteLine($"Configured Voltage: {memory.ConfiguredVoltage}");
                Console.WriteLine("---");
            }
            Console.WriteLine();
            Console.WriteLine("Cache Memory Info");
            Console.WriteLine($"Cache Memory Count: {computerSystem.CacheMemory.Count}");
            foreach (var cacheMemory in computerSystem.CacheMemory)
            {
                Console.WriteLine($"Device ID: {cacheMemory.DeviceId}");
                Console.WriteLine($"Status: {cacheMemory.Status}");
                Console.WriteLine($"Purpose: {cacheMemory.Purpose}");
                Console.WriteLine($"Block Size: {cacheMemory.BlockSize}");
                Console.WriteLine($"Number of Blocks: {cacheMemory.NumberOfBlocks}");
                Console.WriteLine("---");
            }
            Console.WriteLine();
            Console.WriteLine("Disk Drive Info");
            Console.WriteLine($"Disk Drive Count: {computerSystem.DiskDrives.Count}");
            foreach (var diskDrive in computerSystem.DiskDrives)
            {
                Console.WriteLine($"Device ID: {diskDrive.DeviceId}");
                Console.WriteLine($"Status: {diskDrive.Status}");
                Console.WriteLine($"Model: {diskDrive.Model}");
                Console.WriteLine($"Firmware Revision: {diskDrive.FirmwareRevision}");
                Console.WriteLine($"Size: {diskDrive.SizeHR}");
                Console.WriteLine($"Partitions: {diskDrive.Partitions}");
                Console.WriteLine("---");
            }
            Console.WriteLine();
            Console.WriteLine("Logical Disk Info");
            Console.WriteLine($"Logical Disk Count: {computerSystem.LogicalDisks.Count}");
            foreach (var logicalDisk in computerSystem.LogicalDisks)
            {
                Console.WriteLine($"Name: {logicalDisk.Name}");
                Console.WriteLine($"Volume Name: {logicalDisk.VolumeName}");
                Console.WriteLine($"File System: {logicalDisk.FileSystem}");
                Console.WriteLine($"Size: {logicalDisk.SizeHR}");
                Console.WriteLine($"Free Space: {logicalDisk.FreeSpaceHR}");
                Console.WriteLine("---");
            }
            Console.WriteLine();
            Console.WriteLine("Video Controller Info");
            Console.WriteLine($"Video Controller Count: {computerSystem.VideoControllers.Count}");
            foreach (var videoController in computerSystem.VideoControllers)
            {
                Console.WriteLine($"Device ID: {videoController.DeviceId}");
                Console.WriteLine($"Status: {videoController.Status}");
                Console.WriteLine($"Name: {videoController.Name}");
                Console.WriteLine($"Driver Date: {videoController.DriverDate}");
                Console.WriteLine($"Driver Version: {videoController.DriverVersion}");
                Console.WriteLine($"Adapter RAM: {videoController.AdapterRamHR}");
                Console.WriteLine($"Horizontal Resolution: {videoController.HorizontalResolution}");
                Console.WriteLine($"Vertical Resolution: {videoController.VerticalResolution}");
                Console.WriteLine($"Number of Colors: {videoController.NumberOfColors}");
                Console.WriteLine($"Refresh Rate: {videoController.RefreshRate}");
                Console.WriteLine("---");
            }
            Console.WriteLine();
            Console.WriteLine("Fan Info");
            Console.WriteLine($"Fan Count: {computerSystem.Fans.Count}");
            foreach (var fan in computerSystem.Fans)
            {
                Console.WriteLine($"Device ID: {fan.DeviceId}");
                Console.WriteLine($"Status: {fan.Status}");
                Console.WriteLine($"Active Cooling: {fan.ActiveCooling}");
                Console.WriteLine("---");
            }


            // Fetch installed software ------------------------------------------------------------------------
            Console.WriteLine("\nFetching installed Software...");
            Console.WriteLine("------------------------------");
            computerSystem.FetchInstalledSoftware();

            // Map Network Drives ------------------------------------------------------------------------------
            Console.WriteLine("\nMapping Network Drives...");
            Console.WriteLine("-------------------------");
            var networkDrives = new List<string[]>() {
    new[] { @"X", @"\\gpops\tableau_files" },
    new[] { @"Y", @"\\bdrop\dropbox" },
    new[] { @"Z", @"\\gpauto\automation" }
};

            using (var powershell = PowerShell.Create())
            {
                powershell.AddScript(File.ReadAllText(PsFunctionsFile), false);
                powershell.Invoke();
                powershell.Commands.Clear();
                foreach (var networkDrive in networkDrives)
                {
                    powershell.AddCommand("MountNetworkDrive").AddParameter("Drive", networkDrive[0]).AddParameter("Root", networkDrive[1]);
                    var psOutput = powershell.Invoke();
                    powershell.Commands.Clear();
                    foreach (var output in psOutput)
                    {
                        Console.WriteLine(output.ToString());
                    }
                }
            }

            // Load input --------------------------------------------------------------------------------------
            Console.WriteLine("\nLoading software selection input...");
            Console.WriteLine("--------------------------");
            string softwareJson = Path.Combine(GpAutoSetupDir, @"src\GPAutoDevSetup\software.json");
            if (!File.Exists(softwareJson))
            {
                throw new ItemNotFoundException($"Software file at \"{softwareJson}\" could not be found.");
            }
            JsonNode desiredSoftwares = JsonObject.Parse(File.ReadAllText(softwareJson))!;
            JsonObject generalDesiredSoftwares = desiredSoftwares["General"]!.AsObject();
            Console.WriteLine($"{generalDesiredSoftwares.Count} applications will be checked for installation status / updates.");
            foreach (var desiredSoftware in generalDesiredSoftwares)
            {
                Console.WriteLine($"- {desiredSoftware.Key}");
            }

            // Fetch offline installers ------------------------------------------------------------------------
            Console.WriteLine("\nFetching offline installers...");
            Console.WriteLine("------------------------------");
            if (!Directory.Exists(OfflineInstallersDir))
            {
                throw new FileNotFoundException($"Offline installers directory at \"{OfflineInstallersDir}\" could not be found.");
            }
            var offlineInstallers = new List<FileInfo>();
            foreach (var installerPath in Directory.GetFiles(OfflineInstallersDir))
            {
                offlineInstallers.Add(new FileInfo(installerPath));
            }
            Console.WriteLine($"{offlineInstallers.Count} offline installers were found.");

            // Compile software details ------------------------------------------------------------------------
            Console.WriteLine("\nFinding selected software installations...");
            Console.WriteLine("-----------------------------");
            var filteredSoftware = new List<Software>();
            foreach (var desiredSoftware in generalDesiredSoftwares)
            {
                // Check installation status and select installed software
                List<Software> matchingSoftware;
                string? lookup = desiredSoftware.Value!["Lookup"]?.GetValue<string>();
                if (lookup == null)
                {
                    matchingSoftware = computerSystem.InstalledSoftware.Where(x => x.Name.Contains(desiredSoftware.Key, StringComparison.Ordinal)).ToList();
                }
                else
                {
                    matchingSoftware = computerSystem.InstalledSoftware.Where(x => Regex.IsMatch(x.Name, lookup)).ToList();
                }
                Software currentSoftware;
                if (matchingSoftware.Count == 0)
                {
                    currentSoftware = new Software()
                    {
                        Name = desiredSoftware.Key,
                        Version = new Version()
                    };
                    if (desiredSoftware.Key == ".NET Framework")
                    {
                        using var powershell = PowerShell.Create();
                        // Set execution policy to remote signed
                        string getNetFrameworkVersion = @"(Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP' -Recurse | Get-ItemProperty -Name version -EA 0 | Where { $_.PSChildName -Match '^(?!S)\p{L}'} | Select Version | Measure-Object -Property Version -maximum).Maximum";
                        powershell.AddScript(getNetFrameworkVersion);
                        var psOutput = powershell.Invoke();
                        currentSoftware.Version = Version.Parse(psOutput.First().ToString());
                        powershell.Commands.Clear();
                    }
                }
                else if (matchingSoftware.Count == 1)
                {
                    currentSoftware = matchingSoftware.First();
                }
                else
                {
                    throw new Exception($"{matchingSoftware.Count} matches were found for {desiredSoftware.Key}");
                }
                currentSoftware.WingetId = desiredSoftware.Value!["WinGetID"]?.GetValue<string>();
                if (desiredSoftware.Value!["OfflineInstallerLookup"]?.GetValue<string>() != null)
                {
                    currentSoftware.OfflineInstaller = offlineInstallers.Where(x => x.Name.Contains(desiredSoftware.Value!["OfflineInstallerLookup"]?.GetValue<string>(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
                else
                {
                    currentSoftware.OfflineInstaller = offlineInstallers.Where(x => x.Name.Contains(currentSoftware.WingetId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
                currentSoftware.OfflineInstallerVersion = new Version(currentSoftware.OfflineInstaller.Name.Split("-").Where(x => Regex.IsMatch(x, @"^\d+\.(\d|\.)*\d+$")).First());
                filteredSoftware.Add(currentSoftware);
            }
            Console.WriteLine($"Success");

            // Fetch winget installer versions -----------------------------------------------------------------
            if (computerSystem.HasInternetAccess)
            {
                Console.WriteLine("\nFetching WinGet installer versions...");
                Console.WriteLine("-------------------------------------");
                foreach (var software in filteredSoftware)
                {
                    if (software.WingetId != null)
                    {
                        using (var powershell = PowerShell.Create())
                        {
                            string wingetSearchCommand = $"-Split (winget search --id {software.WingetId})[3]";
                            powershell.AddScript(wingetSearchCommand);
                            var output = powershell.Invoke();
                            software.WingetVersion = Version.Parse(output.Where(x => Regex.IsMatch(x.ToString(), @"^\d+\.(\d|\.)*\d+$"))!.Last()!.ToString());
                            powershell.Commands.Clear();
                        }
                        Console.WriteLine($"{software.Name}: {software.WingetVersion}");
                    }
                }
            }

            // Determine installation status -------------------------------------------------------------------
            Console.WriteLine("\nChecking for outdated software...");
            Console.WriteLine("-----------------------------------");
            var outdatedOfflineInstallers = new List<FileInfo>();
            var outdatedSoftware = new List<Software>();
            string? choice = string.Empty;
            foreach (var software in filteredSoftware)
            {
                if (computerSystem.HasInternetAccess && software.WingetVersion != null)
                {
                    // Determine what offline installers are outdated
                    if (software.WingetVersion > software.OfflineInstallerVersion)
                    {
                        outdatedOfflineInstallers.Add(software.OfflineInstaller);
                    }
                    // Determine what installed software is outdated and prompt for updates
                    if (software.WingetVersion > software.Version)
                    {
                        outdatedSoftware.Add(software);
                        Console.WriteLine($"A newer version of {software.Name} is available ({software.Version} => {software.WingetVersion})");
                        while (true)
                        {
                            Console.WriteLine($"Would you like to install the latest version? [Y / N]");
                            choice = Console.ReadLine().ToUpper();
                            if (choice == "Y")
                            {
                                using (var powershell = PowerShell.Create())
                                {
                                    string wingetInstallCommand = $"winget install --id {software.WingetId}";
                                    powershell.AddScript(wingetInstallCommand);
                                    Console.WriteLine("Starting package install...");
                                    var psOutput = powershell.Invoke();
                                    powershell.Commands.Clear();
                                    Console.WriteLine(psOutput.Last().ToString());
                                    break;
                                }
                            }
                            else if (choice == "N")
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (software.OfflineInstallerVersion > software.Version)
                    {
                        outdatedSoftware.Add(software);
                        Console.WriteLine($"A newer version of {software.Name} is available ({software.Version} => {software.OfflineInstallerVersion})");
                        while (true)
                        {
                            Console.WriteLine($"Would you like to install the latest version? [Y / N]");
                            choice = Console.ReadLine().ToUpper();
                            if (choice == "Y")
                            {
                                ProcessStartInfo startInfo = new ProcessStartInfo();
                                startInfo.CreateNoWindow = false;
                                startInfo.UseShellExecute = false;
                                startInfo.FileName = software.OfflineInstaller.FullName;
                                Console.WriteLine("Starting installation...");
                                using (Process installerProcess = Process.Start(startInfo))
                                {
                                    installerProcess.WaitForExit();
                                }
                                Console.WriteLine("Completed.");
                                break;
                            }
                            else if (choice == "N")
                            {
                                break;
                            }
                        }
                    }
                }
            }
            if (outdatedSoftware.Count == 0)
            {
                Console.WriteLine("All selected software is up to date.");
            }

            // Output outdated installers
            Console.WriteLine("\nThe following offline installers are outdated:");
            Console.WriteLine("----------------------------------------------");
            if (outdatedSoftware.Count == 0)
            {
                Console.WriteLine("None");
            }
            else
            {
                foreach (var installer in outdatedOfflineInstallers)
                {
                    Console.WriteLine(installer.Name);
                }
            }



            // Write instructions for each installer that appear when the installer is opened









            // Perform Configurations
            Console.WriteLine("\nPerforming Configurations...");
            Console.WriteLine("----------------------------");
            // Windows Terminal
            choice = string.Empty;
            while (true)
            {
                Console.WriteLine($"Would you like to configure Windows Terminal? [Y / N]");
                choice = Console.ReadLine()?.ToUpper(CultureInfo.InvariantCulture);
                if (choice == "Y")
                {
                    // Install Nerd Fonts
                    var mesloFonts = Directory.GetFiles(@"\\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Fonts\Meslo\").Where(x => x.Contains(".tff", StringComparison.OrdinalIgnoreCase));
                    foreach (var font in mesloFonts)
                    {
                        var fontFileInfo = new FileInfo(font);
                        File.Copy(fontFileInfo.FullName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", fontFileInfo.Name));
                        RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
                        string keyName = Path.GetFileNameWithoutExtension(fontFileInfo.Name) + " (TrueType)";
                        key.SetValue(keyName, fontFileInfo.Name);
                        key.Close();
                    }
                    var hackFonts = Directory.GetFiles(@"\\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Fonts\Hack\").Where(x => x.Contains(".tff", StringComparison.OrdinalIgnoreCase));
                    foreach (var font in hackFonts)
                    {
                        var fontFileInfo = new FileInfo(font);
                        File.Copy(fontFileInfo.FullName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", fontFileInfo.Name));
                        RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
                        string keyName = Path.GetFileNameWithoutExtension(fontFileInfo.Name) + " (TrueType)";
                        key.SetValue(keyName, fontFileInfo.Name);
                        key.Close();
                    }
                    // Configure Windows Terminal
                    string defaultTerminalSettingsFile = $@"C:\Users\{computerSystem.Username}\AppData\Local\Packages\Microsoft.WindowsTerminal_8wekyb3d8bbwe\LocalState\settings.json";
                    JsonNode defaultTerminalSettings = JsonNode.Parse(defaultTerminalSettingsFile)!;
                    List<JsonNode> terminalProfiles = defaultTerminalSettings["profiles"]!["list"]!.GetValue<List<JsonNode>>();
                    string? powerShellGuid = terminalProfiles.Where(x => x["name"]!.GetValue<string>() == "PowerShell").Select(x => x["guid"]!.GetValue<string>()).FirstOrDefault();
                    string? ubuntuGuid = terminalProfiles.Where(x => x["name"]!.GetValue<string>() == "Ubuntu").Select(x => x["guid"]!.GetValue<string>()).FirstOrDefault();
                    string? commandPromptGuid = terminalProfiles.Where(x => x["name"]!.GetValue<string>() == "Command Prompt").Select(x => x["guid"]!.GetValue<string>()).FirstOrDefault();
                    string customTerminalSettingsFile = @"\\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Configs\windows-terminal-settings.json";
                    JsonNode customTerminalSettings = JsonNode.Parse(customTerminalSettingsFile)!;
                    customTerminalSettings["defaultProfile"] = powerShellGuid;
                    JsonArray profiles = customTerminalSettings["profiles"]!["list"]!.AsArray();
                    JsonNode powerShellProfile = profiles.Where(x => x!["name"]!.GetValue<string>() == "PowerShell").FirstOrDefault()!;
                    JsonNode ubuntuProfile = profiles.Where(x => x!["name"]!.GetValue<string>() == "Ubuntu").FirstOrDefault()!;
                    JsonNode commandPromptProfile = profiles.Where(x => x!["name"]!.GetValue<string>() == "Command Prompt").FirstOrDefault()!;
                    powerShellProfile["guid"] = powerShellGuid;
                    ubuntuProfile["guid"] = ubuntuGuid;
                    commandPromptProfile["guid"] = commandPromptGuid;
                    File.WriteAllText(defaultTerminalSettingsFile, customTerminalSettings.ToJsonString());
                    break;
                }
                else if (choice == "N")
                {
                    break;
                }
            }






            // PowerShell Configurations
            choice = string.Empty;
            while (true)
            {
                Console.WriteLine($"Would you like to configure PowerShell? [Y / N]");
                choice = Console.ReadLine()?.ToUpper(CultureInfo.InvariantCulture);
                if (choice == "Y")
                {
                    using var powershell = PowerShell.Create();
                    // Set execution policy to remote signed
                    string remoteSignedCommand = "Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine";
                    powershell.AddScript(remoteSignedCommand);
                    powershell.Invoke();
                    powershell.Commands.Clear();

                    // Install posh-git
                    string poshGitCopyCommand = $@"Copy-Item -Path \\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Posh-Git\ -Destination C:\Users\{computerSystem.Username}\Documents\PowerShell\Modules\Posh-Git\ -Recurse";
                    powershell.AddScript(poshGitCopyCommand);
                    powershell.Invoke();
                    powershell.Commands.Clear();
                    // Install Oh My Posh
                    string ohMyPoshInstaller = @"\\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Oh-My-Posh\install-amd64.exe";
                    powershell.AddScript(ohMyPoshInstaller);
                    powershell.Invoke();
                    powershell.Commands.Clear();
                    // Install Terminal Icons
                    string terminalIconsCopyCommand = $@"Copy-Item -Path \\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Terminal-Icons\ -Destination C:\Users\{computerSystem.Username}\Documents\PowerShell\Modules\Terminal-Icons\ -Recurse";
                    powershell.AddScript(terminalIconsCopyCommand);
                    powershell.Invoke();
                    powershell.Commands.Clear();
                    // Install GNU Nano
                    // This may be complicated
                    // Replace profile.ps1
                    string psProfileSource = @"\\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Configs\profile.ps1";
                    string psProfileDest = $@"C:\Users\{computerSystem.Username}\Documents\PowerShell\profile.ps1";
                    if (File.Exists(psProfileDest))
                    {
                        File.Delete(psProfileDest);
                    }
                    File.Copy(psProfileSource, psProfileDest);
                    // Configure Mamba
                    string disableAutorunCommand = @"C:\Windows\System32\reg.exe DELETE ""HKCU\Software\Microsoft\Command Processor"" /v AutoRun /f";
                    powershell.AddScript("mamba init");
                    powershell.Invoke();
                    powershell.Commands.Clear();
                    if (computerSystem.HasInternetAccess)
                    {
                        powershell.AddScript("mamba upgrade --all");
                        powershell.Invoke();
                        powershell.Commands.Clear();
                    }
                    // Setup aliases

                    // Setup preconfigured environment
                }
                else if (choice == "N")
                {
                    break;
                }
            }

            // Configure VSCode
            choice = string.Empty;
            while (true)
            {
                Console.WriteLine($"Would you like to configure VSCode? [Y / N]");
                choice = Console.ReadLine()?.ToUpper(CultureInfo.InvariantCulture);
                if (choice == "Y")
                {
                    using var powershell = PowerShell.Create();
                    // Install all of the extensions
                    // This might be a heavy lift
                }
                else if (choice == "N")
                {
                    break;
                }
            }

            // Configure Git / GitHub / SSH
            choice = string.Empty;
            while (true)
            {
                Console.WriteLine($"Would you like to configure Git? [Y / N]");
                choice = Console.ReadLine()?.ToUpper(CultureInfo.InvariantCulture);
                if (choice == "Y")
                {
                    string sshDirectory = $@"C:\Users\{computerSystem.Username}\.ssh\";
                    if (!Directory.Exists(sshDirectory))
                    {
                        Directory.CreateDirectory(sshDirectory);
                    }
                    using var powershell = PowerShell.Create();
                    // First generate password protected key
                    // Copy public key to desktop so the user can register it with github later
                    // Copy in a deploy key
                    // Create the config file with the personal key and the deploy key
                    // Look into HTTP GCM and how it might be used to manage git users on public machines
                }
                else if (choice == "N")
                {
                    break;
                }
            }

            // Clone all repos
            if (!Directory.Exists(@"C:\BroadInstitute\"))
            {
                Directory.CreateDirectory(@"C:\BroadInstitute\");
            }
            if (!Directory.Exists(@"C:\BroadInstitute\GPAuto\"))
            {
                Directory.CreateDirectory(@"C:\BroadInstitute\GPAuto\");
            }

            // Configure Hamilton Star Environment
            choice = string.Empty;
            while (true)
            {
                Console.WriteLine($"Would you like to configure the GPAuto Hamilton Star Environment? [Y / N]");
                choice = Console.ReadLine()?.ToUpper(CultureInfo.InvariantCulture);
                if (choice == "Y")
                {
                    //StarSetup.Main(computerSystem);
                }
                else if (choice == "N")
                {
                    break;
                }
            }

            // Configure Dynamic Devices Lynx Environment
            choice = string.Empty;
            while (true)
            {
                Console.WriteLine($"Would you like to configure the GPAuto Dynamic Devices Lynx Environment? [Y / N]");
                choice = Console.ReadLine()?.ToUpper(CultureInfo.InvariantCulture);
                if (choice == "Y")
                {
                    //LynxSetup.Main(computerSystem);
                }
                else if (choice == "N")
                {
                    break;
                }
            }

            // Configure Agilent Bravo Environment
            choice = string.Empty;
            while (true)
            {
                Console.WriteLine($"Would you like to configure the GPAuto Agilent Bravo Environment? [Y / N]");
                choice = Console.ReadLine()?.ToUpper(CultureInfo.InvariantCulture);
                if (choice == "Y")
                {
                    //BravoSetup.Main(computerSystem);
                }
                else if (choice == "N")
                {
                    break;
                }
            }
        }
    }
}

