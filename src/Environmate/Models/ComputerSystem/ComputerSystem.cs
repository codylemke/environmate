using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Management.Infrastructure;
using System.Globalization;
using Microsoft.Management.Infrastructure.Generic;
using System.Reflection;

namespace Environmate
{
    public class ComputerSystem
    {
        // Fields
        private string _name;
        private string _manufacturer;
        private string _systemFamily;
        private string _systemSKUNumber;
        private string _model;
        private int _numberOfPhysicalProcessors;
        private int _numberOfLogicalProcessors;
        private long _totalPhysicalMemory;
        private string _domain;
        private string _workgroup;
        private string _username;
        private bool _hasInternetAccess;
        private string _ipAddress;
        
        // Components
        private OperatingSystem _operatingSystem;
        private BaseBoard _baseBoard;
        private BIOS _bios;
        private List<Processor> _processors = new List<Processor>();
        private List<PhysicalMemory> _physicalMemory = new List<PhysicalMemory>();
        private List<CacheMemory> _cacheMemory = new List<CacheMemory>();
        private List<DiskDrive> _diskDrives = new List<DiskDrive>();
        private List<LogicalDisk> _logicalDisks = new List<LogicalDisk>();
        private List<VideoController> _videoControllers = new List<VideoController>();
        private List<Fan> _fans = new List<Fan>();
        private List<Battery> _batteries = new List<Battery>();
        private List<TemperatureSensor> _temperatureSensors = new List<TemperatureSensor>();
        private InstalledSoftware _installedSoftware;

        // Constructors
        public ComputerSystem() { }
        public ComputerSystem(string computerName)
        {
            Initialize(computerName);
        }

        // Properties
        public string Name
        {
            get { return _name; }
        }
        public string Manufacturer
        {
            get { return _manufacturer; }
        }
        public string SystemFamily
        {
            get { return _systemFamily; }
        }
        public string SystemSKUNumber
        {
            get { return _systemSKUNumber; }
        }
        public string Model
        {
            get { return _model; }
        }
        public int NumberOfPhysicalProcessors
        {
            get { return _numberOfPhysicalProcessors; }
        }
        public int NumberOfLogicalProcessors
        {
            get { return _numberOfLogicalProcessors; }
        }
        public long TotalPhysicalMemory
        {
            get { return _totalPhysicalMemory; }
        }
        public string TotalPhysicalMemoryHR
        {
            get { return $"{Math.Round(_totalPhysicalMemory / (double)1073741824, 2, MidpointRounding.AwayFromZero)} GB"; }
        }
        public string Domain
        {
            get { return _domain; }
        }
        public string Workgroup
        {
            get { return _workgroup; }
        }
        public string Username
        {
            get { return _username; }
        }
        public bool HasInternetAccess
        {
            get { return _hasInternetAccess; }
        }
        public string IpAddress
        {
            get { return _ipAddress; }
        }
        // Components
        public OperatingSystem OperatingSystem
        {
            get { return _operatingSystem; }
        }
        public BaseBoard BaseBoard
        {
            get { return _baseBoard; }
        }
        public BIOS BIOS
        {
            get { return _bios; }
        }
        public ReadOnlyCollection<Processor> Processors
        {
            get { return new ReadOnlyCollection<Processor>(_processors); }
        }
        public ReadOnlyCollection<PhysicalMemory> PhysicalMemory
        {
            get { return new ReadOnlyCollection<PhysicalMemory>(_physicalMemory); }
        }
        public ReadOnlyCollection<CacheMemory> CacheMemory
        {
            get { return new ReadOnlyCollection<CacheMemory>(_cacheMemory); }
        }
        public ReadOnlyCollection<DiskDrive> DiskDrives
        {
            get { return new ReadOnlyCollection<DiskDrive>(_diskDrives); }
        }
        public ReadOnlyCollection<LogicalDisk> LogicalDisks
        {
            get { return new ReadOnlyCollection<LogicalDisk>(_logicalDisks); }
        }
        public ReadOnlyCollection<VideoController> VideoControllers
        {
            get { return new ReadOnlyCollection<VideoController>(_videoControllers); }
        }
        public ReadOnlyCollection<Fan> Fans
        {
            get { return new ReadOnlyCollection<Fan>(_fans); }
        }
        public ReadOnlyCollection<Battery> Batteries
        {
            get { return new ReadOnlyCollection<Battery>(_batteries); }
        }
        public ReadOnlyCollection<TemperatureSensor> TemperatureSensors
        {
            get { return new ReadOnlyCollection<TemperatureSensor>(_temperatureSensors); }
        }
        public ReadOnlyCollection<Software> InstalledSoftware
        {
            get { return new ReadOnlyCollection<Software>(_installedSoftware.Software); }
        }

        // Methods
        public void Initialize(string? computerName = null)
        {
            CimSession cimSession;
            if (computerName == null)
            {
                cimSession = CimSession.Create(_name);
            }
            else
            {
                cimSession = CimSession.Create(computerName);
            }
            if (!cimSession.TestConnection())
            {
                throw new CimException("CimSession connection was not successfully created");
            }
            // Retrieve compter system information
            List<CimInstance> computerSystems = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_ComputerSystem").ToList();
            if (computerSystems.Count != 1)
            {
                throw new CimException($"{computerSystems.Count} computer systems were found. Only 1 is acceptable.");
            }
            CimKeyedCollection<CimProperty> computerSystemProperties = computerSystems.First().CimInstanceProperties;
            _name = computerSystemProperties["Name"]!.Value.ToString()!;
            _manufacturer = computerSystemProperties["Manufacturer"]!.Value.ToString()!;
            _systemFamily = computerSystemProperties["SystemFamily"]!.Value.ToString()!;
            _systemSKUNumber = computerSystemProperties["SystemSKUNumber"]!.Value.ToString()!;
            _model = computerSystemProperties["Model"]!.Value.ToString()!;
            _numberOfPhysicalProcessors = int.Parse(computerSystemProperties["NumberOfProcessors"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfLogicalProcessors = int.Parse(computerSystemProperties["NumberOfLogicalProcessors"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _totalPhysicalMemory = long.Parse(computerSystemProperties["TotalPhysicalMemory"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _domain = computerSystemProperties["UserName"]!.Value.ToString()!.Split('\\')[0];
            _workgroup = computerSystemProperties["Workgroup"]!.Value.ToString()!;
            _username = computerSystemProperties["UserName"]!.Value.ToString()!.Split('\\')[1];

            // Determine if the machine has internet access.
            using Ping googlePing = new Ping();
            string host = "google.com";
            int timeout = 1000;
            PingReply pingReply = googlePing.Send(host, timeout);
            if (pingReply.Status == IPStatus.Success)
            {
                _hasInternetAccess = true;
            }
            else
            {
                _hasInternetAccess = false;
            }
            // Determine the IP address.
            _ipAddress = Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily.ToString() == "InterNetwork").First().ToString();

            // Operating System
            List<CimInstance> operatingSystems = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_OperatingSystem").ToList();
            if (operatingSystems.Count != 1)
            {
                throw new CimException($"{operatingSystems.Count} operating systems were found. Only 1 is acceptable.");
            }
            _operatingSystem = new OperatingSystem(operatingSystems.First());

            // BaseBoards
            List<CimInstance> baseBoards = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM Win32_BaseBoard").ToList();
            if (baseBoards.Count != 1)
            {
                throw new CimException($"{baseBoards.Count} operating systems were found. Only 1 is acceptable.");
            }
            _baseBoard = new BaseBoard(baseBoards.First());

            // BIOS
            List<CimInstance> bios = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM Win32_BIOS").ToList();
            if (bios.Count != 1)
            {
                throw new CimException($"{bios.Count} bios elements were found. Only 1 is acceptable.");
            }
            _bios = new BIOS(bios.First());

            // Processors
            List<CimInstance> processors = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_Processor").ToList();
            processors.ForEach(x => _processors.Add(new Processor(x)));

            // PhysicalMemory
            List<CimInstance> physicalMemory = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_PhysicalMemory").ToList();
            physicalMemory.ForEach(x => _physicalMemory.Add(new PhysicalMemory(x)));

            // CacheMemory
            List<CimInstance> cacheMemory = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_Memory").ToList();
            cacheMemory.ForEach(x => _cacheMemory.Add(new CacheMemory(x)));

            // DiskDrives
            List<CimInstance> diskDrives = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_DiskDrive").ToList();
            diskDrives.ForEach(x => _diskDrives.Add(new DiskDrive(x)));

            // DiskPartitions
            List<CimInstance> diskPartitions = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM Win32_DiskPartition").ToList();
            

            // LogicalDisks
            List<CimInstance> logicalDisks = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_LogicalDisk").ToList();
            logicalDisks.Where(x => x.CimClass.ToString().Split(':').Last() == "Win32_LogicalDisk").ToList().ForEach(x => _logicalDisks.Add(new LogicalDisk(x)));

            // VideoControllers
            List<CimInstance> videoControllers = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_VideoController").ToList();
            videoControllers.ForEach(x => _videoControllers.Add(new VideoController(x)));

            // Fans
            List<CimInstance> fans = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_Fan").ToList();
            fans.ForEach(x => _fans.Add(new Fan(x)));

            // TemperatureSensors
            List<CimInstance> temperatureSensors = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_TemperatureSensor").ToList();
            temperatureSensors.ForEach(x => _temperatureSensors.Add(new TemperatureSensor(x)));

            // Batteries
            List<CimInstance> batteries = cimSession.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM CIM_Battery").ToList();
            batteries.Where(x => x.CimClass.ToString().Split(':').Last() == "Win32_Battery").ToList().ForEach(x => _batteries.Add(new Battery(x)));

            // TO ADD*
            // PowerSupply (CIM_PowerSupply)
            // Serial Controller (CIM_SerialController)
            // USBDevice (CIM_USBDevice)
            // USBController (CIM_USBController)
            // Printer (CIM_Printer)
            // Network Adapter (CIM_NetworkAdapter)

            foreach (CimProperty property in diskPartitions[2].CimInstanceProperties)
            {
                if (property.Value != null)
                {
                    Console.WriteLine($"{property.Name}: {property.Value}");
                }
            }
            Console.WriteLine();

            //foreach (PropertyInfo property in _videoControllers[0].GetType().GetProperties())
            //{
            //    Console.WriteLine($"{property.Name}: {property.GetValue(_videoControllers[0])}");
            //}
            //Console.WriteLine();
        }

        public void FetchInstalledSoftware()
        {
            _installedSoftware = new InstalledSoftware();
        }

        public static string GetSizeHR(long bytes)
        {
            double gigabytes = bytes / (double)1073741824;
            if (gigabytes > 1)
            {
                return $"{Math.Round(gigabytes, 2, MidpointRounding.AwayFromZero)} GB";
            }
            double megabytes = gigabytes * 1024;
            if (megabytes > 1)
            {
                return $"{Math.Round(megabytes, 2, MidpointRounding.AwayFromZero)} MB";
            }
            double kilobytes = megabytes * 1024;
            return $"{Math.Round(kilobytes, 2, MidpointRounding.AwayFromZero)} KB";
        }
    }
}