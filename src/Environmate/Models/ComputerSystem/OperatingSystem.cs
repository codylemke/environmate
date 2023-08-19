using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;
using System;
using System.Globalization;
using System.Linq;

namespace Environmate
{
    public class OperatingSystem
    {
        // Fields
        private string _name;
        private DateTime _installDate;
        private string _status;
        private bool _distributed;
        private long _freePhysicalMemory;
        private long _freeSpaceInPagingFiles;
        private long _freeVirtualMemory;
        private DateTime _lastBootUpTime;
        private DateTime _localDateTime;
        private long _maxNumberOfProcesses;
        private long _maxProcessMemorySize;
        private long _numberOfProcesses;
        private int _numberOfUsers;
        private long _sizeStoredInPagingFiles;
        private long _totalVirtualMemorySize;
        private long _totalVisibleMemorySize;
        private Version _version;
        private string _bootDevice;
        private int _buildNumber;
        private int _countryCode;
        private int _encryptionLevel;
        private int _foregroundApplicationBoost;
        private int _locale;
        private string _manufacturer;
        private string _architecture;
        private bool _portableOperatingSystem;
        private bool _primary;
        private string _serialNumber;
        private string _systemDevice;
        private string _systemDirectory;
        private string _systemDrive;
        private string _windowsDirectory;

        // Constructors
        public OperatingSystem() { }
        public OperatingSystem(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_OperatingSystem")
            {
                throw new ArgumentException($"An OperatingSystem instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> operatingSystemProperties = cimInstance.CimInstanceProperties;
            _name = operatingSystemProperties["Caption"]!.Value.ToString()!;
            _installDate = DateTime.Parse(operatingSystemProperties["InstallDate"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _status = operatingSystemProperties["Status"]!.Value.ToString()!;
            _distributed = bool.Parse(operatingSystemProperties["Distributed"]!.Value.ToString()!);
            _freePhysicalMemory = long.Parse(operatingSystemProperties["FreePhysicalMemory"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _freeSpaceInPagingFiles = long.Parse(operatingSystemProperties["FreeSpaceInPagingFiles"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _freeVirtualMemory = long.Parse(operatingSystemProperties["FreeVirtualMemory"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _lastBootUpTime = DateTime.Parse(operatingSystemProperties["LastBootUpTime"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _localDateTime = DateTime.Parse(operatingSystemProperties["LocalDateTime"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _maxNumberOfProcesses = long.Parse(operatingSystemProperties["MaxNumberOfProcesses"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _maxProcessMemorySize = long.Parse(operatingSystemProperties["MaxProcessMemorySize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfProcesses = long.Parse(operatingSystemProperties["NumberOfProcesses"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfUsers = int.Parse(operatingSystemProperties["NumberOfUsers"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _sizeStoredInPagingFiles = long.Parse(operatingSystemProperties["SizeStoredInPagingFiles"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _totalVirtualMemorySize = long.Parse(operatingSystemProperties["TotalVirtualMemorySize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _totalVisibleMemorySize = long.Parse(operatingSystemProperties["TotalVisibleMemorySize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _version = Version.Parse(operatingSystemProperties["Version"]!.Value.ToString()!);
            _bootDevice = operatingSystemProperties["BootDevice"]!.Value.ToString()!;
            _buildNumber = int.Parse(operatingSystemProperties["BuildNumber"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _countryCode = int.Parse(operatingSystemProperties["CountryCode"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _encryptionLevel = int.Parse(operatingSystemProperties["EncryptionLevel"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _foregroundApplicationBoost = int.Parse(operatingSystemProperties["ForegroundApplicationBoost"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _locale = int.Parse(operatingSystemProperties["Locale"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _manufacturer = operatingSystemProperties["Manufacturer"]!.Value.ToString()!;
            _architecture = operatingSystemProperties["OSArchitecture"]!.Value.ToString()!;
            _portableOperatingSystem = bool.Parse(operatingSystemProperties["PortableOperatingSystem"]!.Value.ToString()!);
            _primary = bool.Parse(operatingSystemProperties["Primary"]!.Value.ToString()!);
            _serialNumber = operatingSystemProperties["SerialNumber"]!.Value.ToString()!;
            _systemDevice = operatingSystemProperties["SystemDevice"]!.Value.ToString()!;
            _systemDirectory = operatingSystemProperties["SystemDirectory"]!.Value.ToString()!;
            _systemDrive = operatingSystemProperties["SystemDrive"]!.Value.ToString()!;
            _windowsDirectory = operatingSystemProperties["WindowsDirectory"]!.Value.ToString()!;
        }

        // Properties
        public string Name
        {
            get { return _name; }
        }
        public DateTime InstallDate
        {
            get { return _installDate; }
        }
        public string Status
        {
            get { return _status; }
        }
        public bool Distributed
        {
            get { return _distributed; }
        }
        public long FreePhysicalMemory
        {
            get { return _freePhysicalMemory; }
        }
        public string FreePhysicalMemoryHR
        {
            get { return ComputerSystem.GetSizeHR(_freePhysicalMemory); }
        }
        public long FreeSpaceInPagingFiles
        {
            get { return _freeSpaceInPagingFiles; }
        }
        public string FreeSpaceInPagingFilesHR
        {
            get { return ComputerSystem.GetSizeHR(_freeSpaceInPagingFiles); }
        }
        public long FreeVirtualMemory
        {
            get { return _freeVirtualMemory; }
        }
        public string FreeVirtualMemoryHR
        {
            get { return ComputerSystem.GetSizeHR(_freeVirtualMemory); }
        }
        public DateTime LastBootUpTime
        {
            get { return _lastBootUpTime; }
        }
        public DateTime LocalDateTime
        {
            get { return _localDateTime; }
        }
        public long MaxNumberOfProcesses
        {
            get { return _maxNumberOfProcesses; }
        }
        public long MaxProcessMemorySize
        {
            get { return _maxProcessMemorySize; }
        }
        public string MaxProcessMemorySizeHR
        {
            get { return ComputerSystem.GetSizeHR(_maxProcessMemorySize); }
        }
        public long NumberOfProcesses
        {
            get { return _numberOfProcesses; }
        }
        public int NumberOfUsers
        {
            get { return _numberOfUsers; }
        }
        public long SizeStoredInPagingFiles
        {
            get { return _sizeStoredInPagingFiles; }
        }
        public string SizeStoredInPagingFilesHR
        {
            get { return ComputerSystem.GetSizeHR(_sizeStoredInPagingFiles); }
        }
        public long TotalVirtualMemorySize
        {
            get { return _totalVirtualMemorySize; }
        }
        public string TotalVirtualMemorySizeHR
        {
            get { return ComputerSystem.GetSizeHR(_totalVirtualMemorySize); }
        }
        public long TotalVisibleMemorySize
        {
            get { return _totalVisibleMemorySize; }
        }
        public string TotalVisibleMemorySizeHR
        {
            get { return ComputerSystem.GetSizeHR(_totalVisibleMemorySize); }
        }
        public Version Version
        {
            get { return _version; }
        }
        public string BootDevice
        {
            get { return _bootDevice; }
        }
        public int BuildNumber
        {
            get { return _buildNumber; }
        }
        public int CountryCode
        {
            get { return _countryCode; }
        }
        public int EncryptionLevel
        {
            get { return _encryptionLevel; }
        }
        public int ForegroundApplicationBoost
        {
            get { return _foregroundApplicationBoost; }
        }
        public int Locale
        {
            get { return _locale; }
        }
        public string Manufacturer
        {
            get { return _manufacturer; }
        }
        public string Architecture
        {
            get { return _architecture; }
        }
        public bool PortableOperatingSystem
        {
            get { return _portableOperatingSystem; }
        }
        public bool Primary
        {
            get { return _primary; }
        }
        public string SerialNumber
        {
            get { return _serialNumber; }
        }
        public string SystemDevice
        {
            get { return _systemDevice; }
        }
        public string SystemDirectory
        {
            get { return _systemDirectory; }
        }
        public string SystemDrive
        {
            get { return _systemDrive; }
        }
        public string WindowsDirectory
        {
            get { return _windowsDirectory; }
        }

        // Methods
        // N/A
    }
}
