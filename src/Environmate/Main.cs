using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Environmate
{
    internal class MainSetup
    {
        private static void Main(string[] args)
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
                throw new ArgumentException($"Software file at \"{softwareJson}\" could not be found.");
            }
            JsonNode desiredSoftwares = JsonObject.Parse(File.ReadAllText(softwareJson));
            JsonObject generalDesiredSoftwares = desiredSoftwares["General"].AsObject();
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
                string lookup = desiredSoftware.Value["Lookup"]?.GetValue<string>();
                if (lookup == null)
                {
                    matchingSoftware = computerSystem.InstalledSoftware.Where(x => x.Name.Contains(desiredSoftware.Key)).ToList();
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
                currentSoftware.WingetId = desiredSoftware.Value["WinGetID"]?.GetValue<string>();
                if (desiredSoftware.Value["OfflineInstallerLookup"]?.GetValue<string>() != null)
                {
                    currentSoftware.OfflineInstaller = offlineInstallers.Where(x => x.Name.Contains(desiredSoftware.Value["OfflineInstallerLookup"]?.GetValue<string>())).FirstOrDefault();
                }
                else
                {
                    currentSoftware.OfflineInstaller = offlineInstallers.Where(x => x.Name.Contains(currentSoftware.WingetId)).FirstOrDefault();
                }
                currentSoftware.OfflineInstallerVersion = new Version(currentSoftware.OfflineInstaller.Name.Split('-').Where(x => Regex.IsMatch(x, @"^\d+\.(\d|\.)*\d+$")).First());
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
                            software.WingetVersion = Version.Parse(output.Where(x => Regex.IsMatch(x.ToString(), @"^\d+\.(\d|\.)*\d+$")).Last().ToString());
                            powershell.Commands.Clear();
                        }
                        Console.WriteLine($"{software.Name}: {software.WingetVersion}");
                    }
                }
            }
        }
    }
}
