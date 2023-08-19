using System;
using System.Globalization;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class CacheMemory
    {
        // Fields
        private string _status;
        private string _deviceId;
        private int _blockSize;
        private int _numberOfBlocks;
        private string _purpose;
        private long _installedSize;
        private int _location;
        private int _maxCacheSize;

        // Constructors
        public CacheMemory() { }
        public CacheMemory(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_CacheMemory")
            {
                throw new ArgumentException($"A Memory instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> cacheMemoryProperties = cimInstance.CimInstanceProperties;
            _status = cacheMemoryProperties["Status"]!.Value.ToString()!;
            _deviceId = cacheMemoryProperties["DeviceID"]!.Value.ToString()!;
            _blockSize = int.Parse(cacheMemoryProperties["BlockSize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfBlocks = int.Parse(cacheMemoryProperties["NumberOfBlocks"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _purpose = cacheMemoryProperties["Purpose"]!.Value.ToString()!;
            _installedSize = long.Parse(cacheMemoryProperties["InstalledSize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _location = int.Parse(cacheMemoryProperties["Location"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _maxCacheSize = int.Parse(cacheMemoryProperties["MaxCacheSize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
        }

        // Properties
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
        public int BlockSize
        {
            get { return _blockSize; }
            set { _blockSize = value; }
        }
        public int NumberOfBlocks
        {
            get { return _numberOfBlocks; }
            set { _numberOfBlocks = value; }
        }
        public string Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        public long InstalledSize
        {
            get { return _installedSize; }
            set { _installedSize = value; }
        }
        public int Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public int MaxCacheSize
        {
            get { return _maxCacheSize; }
            set { _maxCacheSize = value; }
        }

        // Methods
        // N/A
    }
}
