using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Environmate
{
    public class Software
    {
        //".NET Framework": {
        //    "Lookup": "Microsoft .NET Framework 4.8 SDK",
        //  "WinGetID": "Microsoft.DotNet.Framework.DeveloperPack_4"
        //},
        // Implement installers for these as well
        // Microsoft Office
        // GNU Nano
        // Microsoft Edge
        // Google Chrome
        // Logseq
        // VMWare Workstation Player OR Virtual Box
        // Docker
        //new[] { @"VMware Workstation Player", @"VMware Player", "64", @"VMware.WorkstationPlayer", @"VMware.WorkstationPlayer" }
        //"Mambaforge": {
        //    "RegistryKey": "Mambaforge",
        //      "InstallLocation": "64",
        //      "OfflineInstaller": "CondaForge.Mambaforge",
        //      "WinGet": "CondaForge.Mambaforge"
        //    },
        //"Oh My Posh": {
        //    "RegistryKey": "Oh My Posh",
        //      "InstallLocation": "User",
        //      "OfflineInstaller": "JanDeDobbeleer.OhMyPosh",
        //      "WinGet": "JanDeDobbeleer.OhMyPosh"
        //    },
        // Fields
        private string _name;
        private string _publisher;
        private Version _version;
        private string _type;
        private string _scope;
        private long _size;
        private RegistryKey _registryKey;
        private string _installLocation;
        private DateTime _installDate;
        private string? _aboutLink;
        private string? _helpLink;
        private string? _updateLink;
        private string? _githubLink;
        private string? _wingetId;
        private Version? _wingetVersion;
        private FileInfo? _offlineInstaller;
        private Version? _offlineInstallerVersion;

        // Constructors
        public Software() { }
        public Software(RegistryKey registryKey)
        {
            if (registryKey == null)
            {
                throw new ArgumentNullException();
            }
            _registryKey = registryKey;
            _name = GetName(registryKey);
            _installLocation = GetInstallLocation(registryKey);
            _publisher = GetPublisher(registryKey);
            _version = GetVersion(registryKey);
            _type = registryKey.View == RegistryView.Registry32 ? "32-bit" : "64-bit";
            _scope = GetScope(registryKey);
            _size = GetInstallSize(registryKey);
            _installDate = GetInstallDate(registryKey);
            _aboutLink = GetAboutLink(registryKey);
            _helpLink = GetHelpLink(registryKey);
            _updateLink = GetUpdateLink(registryKey);
            //Console.WriteLine($"\nName: {Name}");
            //Console.WriteLine($"Install Location: {InstallLocation}");
            //Console.WriteLine($"Publisher: {Publisher}");
            //Console.WriteLine($"Version: {Version}");
            //Console.WriteLine($"Type: {Type}");
            //Console.WriteLine($"Scope: {Scope}");
            //Console.WriteLine($"Size: {Size}");
            //Console.WriteLine($"Install Date: {InstallDate}");
            //Console.WriteLine($"About Link: {AboutLink}");
            //Console.WriteLine($"Help Link: {HelpLink}");
            //Console.WriteLine($"Update Link: {UpdateLink}");
        }

        // Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Publisher
        {
            get { return _publisher; }
        }
        public Version Version
        {
            get { return _version; }
            set { _version = value; }
        }
        public string Type
        {
            get { return _type; }
        }
        public string Scope
        {
            get { return _scope; }
        }
        public string Size
        {
            get
            {
                if (_size.ToString().Count() <= 7)
                {
                    return $"{Math.Round(double.Parse(_size.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) / 1000, 2, MidpointRounding.AwayFromZero)} KB";
                }
                else if (_size.ToString().Count() <= 10)
                {
                    return $"{Math.Round(double.Parse(_size.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) / 1000000, 2, MidpointRounding.AwayFromZero)} MB";
                }
                else
                {
                    return $"{Math.Round(double.Parse(_size.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) / 1000000000, 2, MidpointRounding.AwayFromZero)} GB";
                }

            }
        }
        public RegistryKey RegistryKey
        {
            get { return _registryKey; }
        }
        public string InstallLocation
        {
            get { return _installLocation; }
        }
        public DateTime InstallDate
        {
            get { return _installDate; }
        }
        public string AboutLink
        {
            get { return _aboutLink ?? "N/A"; }
        }
        public string HelpLink
        {
            get { return _helpLink ?? "N/A"; }
        }
        public string GithubLink
        {
            get { return _githubLink ?? "N/A"; }
        }
        public string UpdateLink
        {
            get { return _updateLink ?? "N/A"; }
        }
        public string WingetId
        {
            get { return _wingetId; }
            set { _wingetId = value; }
        }
        public Version WingetVersion
        {
            get { return _wingetVersion; }
            set { _wingetVersion = value;}
        }
        public FileInfo OfflineInstaller
        {
            get { return _offlineInstaller; }
            set { _offlineInstaller = value; }
        }
        public Version OfflineInstallerVersion
        {
            get { return _offlineInstallerVersion; }
            set { _offlineInstallerVersion = value; }
        }

        // Methods
        private string GetName(RegistryKey registryKey)
        {
            string name;
            try
            {
                name = registryKey.GetValue("PackageID")!.ToString()!;
            }
            catch (NullReferenceException)
            {
                name = registryKey.GetValue("DisplayName")!.ToString()!;
            }
            return name;
        }

        private string GetInstallLocation(RegistryKey registryKey)
        {
            string installLocation;
            try
            {
                installLocation = registryKey.GetValue("InstallLocation")!.ToString()!;
                if (installLocation.EndsWith(".exe"))
                {
                    installLocation = Path.GetDirectoryName(installLocation);
                }
            }
            catch (NullReferenceException)
            {
                try
                {
                    string displayIcon = registryKey.GetValue("DisplayIcon")!.ToString()!;
                    installLocation = Path.GetDirectoryName(displayIcon.Replace("\"", "", StringComparison.OrdinalIgnoreCase));
                }
                catch (NullReferenceException)
                {
                    try
                    {
                        installLocation = registryKey.GetValue("PackageRootFolder")!.ToString()!;
                    }
                    catch (NullReferenceException)
                    {
                        throw new Exception("Needs a fourth method");
                    }
                }
            }
            if (!installLocation.EndsWith(Path.DirectorySeparatorChar))
            {
                installLocation += Path.DirectorySeparatorChar;
            }
            return installLocation;
        }

        private string GetPublisher(RegistryKey registryKey)
        {
            string publisher;
            try
            {
                publisher = registryKey.GetValue("Publisher")!.ToString()!;
            }
            catch (NullReferenceException)
            {
                try
                {
                    string installDir = new DirectoryInfo(_installLocation).Name;
                    publisher = installDir.Split('.')[0];
                }
                catch (Exception error)
                {
                    if (error is ArgumentException || error is IndexOutOfRangeException)
                    {
                        if (_name.StartsWith("@{", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                publisher = _name.Split('{')[1].Split('.')[0];
                            }
                            catch (IndexOutOfRangeException)
                            {
                                throw;
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            if (publisher.Contains("Windows", StringComparison.OrdinalIgnoreCase) || publisher.Contains("Microsoft", StringComparison.OrdinalIgnoreCase))
            {
                publisher = "Microsoft";
            }
            return publisher;
        }

        private Version GetVersion(RegistryKey registryKey)
        {
            Version version;
            try
            {
                version = new Version(registryKey.GetValue("DisplayVersion")!.ToString()!.Replace('-', '.').Split(".g")[0]);
            }
            catch (NullReferenceException)
            {
                try
                {
                    string installDir = new DirectoryInfo(_installLocation).Name;
                    version = new Version(installDir.Split('_')[1]);
                }
                catch (Exception error)
                {
                    if (error is ArgumentException || error is IndexOutOfRangeException)
                    {
                        try
                        {
                            version = new Version(_name.Split('_')[1]);
                        }
                        catch (Exception)
                        {
                            version = new Version();
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return version;
        }

        private string GetScope(RegistryKey registryKey)
        {
            if (registryKey.Name.Contains(@"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages", StringComparison.OrdinalIgnoreCase))
            {
                return "UWP";
            }
            else if (registryKey.Name.Contains("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase))
            {
                return "User";
            }
            else
            {
                return "Local Machine";
            }
        }

        private long GetInstallSize(RegistryKey registryKey)
        {
            long installSize;
            try
            {
                installSize = long.Parse(registryKey.GetValue("EstimatedSize")!.ToString(), CultureInfo.InvariantCulture);
            }
            catch (NullReferenceException)
            {
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(_installLocation);
                    installSize = directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Needs a third method");
                }
            }
            return installSize;
        }

        private DateTime GetInstallDate(RegistryKey registryKey)
        {
            DateTime installDate;
            try
            {
                installDate = DateTime.ParseExact(registryKey.GetValue("InstallDate")!.ToString()!, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (NullReferenceException)
            {
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(_installLocation);
                    installDate = directoryInfo.CreationTime;
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Needs a third method");
                }
            }
            return installDate;
        }

        private string? GetAboutLink(RegistryKey registryKey)
        {
            string? aboutLink;
            aboutLink = registryKey.GetValue("URLInfoAbout")?.ToString();
            if (string.IsNullOrEmpty(aboutLink))
            {
                aboutLink = null;
            }
            return aboutLink;
        }

        private string? GetHelpLink(RegistryKey registryKey)
        {
            string? helpLink;
            helpLink = registryKey.GetValue("HelpLink")?.ToString();
            if (string.IsNullOrEmpty(helpLink))
            {
                helpLink = null;
            }
            return helpLink;
        }

        private string? GetUpdateLink(RegistryKey registryKey)
        {
            string? updateLink;
            updateLink = registryKey.GetValue("URLUpdateInfo")?.ToString();
            if (string.IsNullOrEmpty(updateLink))
            {
                updateLink = null;
            }
            return updateLink;
        }
    }
}
