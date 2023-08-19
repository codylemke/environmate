using System;
using System.Linq;
using System.Globalization;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class Processor
    {
        // Fields
        private string _name;
        private string _status;
        private string _deviceId;
        private int _dataWidth;
        private string _role;
        private int _clockSpeed;
        private int _maxClockSpeed;
        private int _l2CacheSize;
        private int _l3CacheSize;
        private string _manufacturer;
        private int _numberOfCores;
        private int _numberOfEnabledCore;
        private int _numberOfLogicalProcessors;
        private int _threadCount;
        private string _socketDesignation;
        private bool _virtualizationFirmwareEnabled;
        private bool _vmMonitorModeExtensions;

        // Constructors
        public Processor() { }
        public Processor(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_Processor")
            {
                throw new ArgumentException($"A Processor instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> processorProperties = cimInstance.CimInstanceProperties;
            _name = processorProperties["Name"]!.Value.ToString()!;
            _status = processorProperties["Status"]!.Value.ToString()!;
            _deviceId = processorProperties["DeviceID"]!.Value.ToString()!;
            _dataWidth = int.Parse(processorProperties["DataWidth"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _role = processorProperties["Role"]!.Value.ToString()!;
            _clockSpeed = int.Parse(processorProperties["CurrentClockSpeed"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _maxClockSpeed = int.Parse(processorProperties["MaxClockSpeed"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _l2CacheSize = int.Parse(processorProperties["L2CacheSize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _l3CacheSize = int.Parse(processorProperties["L3CacheSize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _manufacturer = processorProperties["Manufacturer"]!.Value.ToString()!;
            _numberOfCores = int.Parse(processorProperties["NumberOfCores"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfEnabledCore = int.Parse(processorProperties["NumberOfEnabledCore"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfLogicalProcessors = int.Parse(processorProperties["NumberOfLogicalProcessors"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _threadCount = int.Parse(processorProperties["ThreadCount"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _socketDesignation = processorProperties["SocketDesignation"]!.Value.ToString()!;
            _virtualizationFirmwareEnabled = bool.Parse(processorProperties["VirtualizationFirmwareEnabled"]!.Value.ToString()!);
            _vmMonitorModeExtensions = bool.Parse(processorProperties["VMMonitorModeExtensions"]!.Value.ToString()!);
        }

        // Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
        public int DataWidth
        {
            get { return _dataWidth; }
            set { _dataWidth = value; }
        }
        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public int ClockSpeed
        {
            get { return _clockSpeed; }
            set { _clockSpeed = value; }
        }
        public int MaxClockSpeed
        {
            get { return _maxClockSpeed; }
            set { _maxClockSpeed = value; }
        }
        public int L2CacheSize
        {
            get { return _l2CacheSize; }
            set { _l2CacheSize = value; }
        }
        public int L3CacheSize
        {
            get { return _l3CacheSize; }
            set { _l3CacheSize = value; }
        }
        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; }
        }
        public int NumberOfCores
        {
            get { return _numberOfCores; }
            set { _numberOfCores = value; }
        }
        public int NumberOfEnabledCore
        {
            get { return _numberOfEnabledCore; }
            set { _numberOfEnabledCore = value; }
        }
        public int NumberOfLogicalProcessors
        {
            get { return _numberOfLogicalProcessors; }
            set { _numberOfLogicalProcessors = value; }
        }
        public int ThreadCount
        {
            get { return _threadCount; }
            set { _threadCount = value; }
        }
        public string SocketDesignation
        {
            get { return _socketDesignation; }
            set { _socketDesignation = value; }
        }
        public bool VirtualizationFirmwareEnabled
        {
            get { return _virtualizationFirmwareEnabled; }
            set { _virtualizationFirmwareEnabled = value; }
        }
        public bool VMMonitorModeExtensions
        {
            get { return _vmMonitorModeExtensions; }
            set { _vmMonitorModeExtensions = value; }
        }

        // Methods
        // N/A
    }
}
