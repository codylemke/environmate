using System.Globalization;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class LogicalDisk
    {
        // Fields
        private string _name;
        private string _volumeName;
        private string _fileSystem;
        private long _size;
        private long _freeSpace;
        private bool _compressed;
        private bool _supportsFileBasedCompression;
        private string _volumeSerialNumber;

        // Constructors
        public LogicalDisk() { }
        public LogicalDisk(CimInstance cimInstance)
        {
            //if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_LogicalDisk")
            //{
            //    throw new ArgumentException($"A LogicalDisk instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            //}
            CimKeyedCollection<CimProperty> logicalDiskProperties = cimInstance.CimInstanceProperties;
            _name = logicalDiskProperties["Name"]!.Value.ToString()!;
            _volumeName = logicalDiskProperties["VolumeName"]!.Value.ToString()!;
            _fileSystem = logicalDiskProperties["FileSystem"]!.Value.ToString()!;
            _size = long.Parse(logicalDiskProperties["Size"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _freeSpace = long.Parse(logicalDiskProperties["FreeSpace"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _compressed = bool.Parse(logicalDiskProperties["Compressed"]!.Value.ToString()!);
            _supportsFileBasedCompression = bool.Parse(logicalDiskProperties["SupportsFileBasedCompression"]!.Value.ToString()!);
            _volumeSerialNumber = logicalDiskProperties["VolumeSerialNumber"]!.Value.ToString()!;
        }

        // Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string VolumeName
        {
            get { return _volumeName; }
            set { _volumeName = value; }
        }
        public string FileSystem
        {
            get { return _fileSystem; }
            set { _fileSystem = value; }
        }
        public long Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public string SizeHR
        {
            get { return ComputerSystem.GetSizeHR(_size); }
        }
        public long FreeSpace
        {
            get { return _freeSpace; }
            set { _freeSpace = value; }
        }
        public string FreeSpaceHR
        {
            get { return ComputerSystem.GetSizeHR(_freeSpace); }
        }
        public bool Compressed
        {
            get { return _compressed; }
            set { _compressed = value; }
        }
        public bool SupportsFileBasedCompression
        {
            get { return _supportsFileBasedCompression; }
            set { _supportsFileBasedCompression = value; }
        }
        private string VolumeSerialNumber
        {
            get { return _volumeSerialNumber; }
            set { _volumeSerialNumber = value; }
        }

        // Methods
        // N/A
    }
}
