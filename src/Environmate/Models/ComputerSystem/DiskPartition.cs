using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environmate
{
    public class DiskPartition
    {
        // Fields
        private string _deviceId;
        private int _blockSize;
        private int _numberOfBlocks;
        private bool _bootable;
        private bool _primaryPartition;
        private bool _bootPartition;
        private int _index;
        private long _size;
        private long _startingOffset;
        private string _type;

        // Constructors
        public DiskPartition() { }
        public DiskPartition(CimInstance cimInstance) 
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_DiskPartition")
            {
                throw new ArgumentException($"A DiskPartition instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> diskPartitionProperties = cimInstance.CimInstanceProperties;
            _deviceId = diskPartitionProperties["DeviceID"]!.Value.ToString()!;
            _blockSize = int.Parse(diskPartitionProperties["BlockSize"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfBlocks = int.Parse(diskPartitionProperties["NumberOfBlocks"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _bootable = bool.Parse(diskPartitionProperties["Bootable"]!.Value.ToString()!);
            _primaryPartition = bool.Parse(diskPartitionProperties["PrimaryPartition"]!.Value.ToString()!);
            _bootPartition = bool.Parse(diskPartitionProperties["BootPartition"]!.Value.ToString()!);
            _index = int.Parse(diskPartitionProperties["Index"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _size = long.Parse(diskPartitionProperties["Size"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _startingOffset = long.Parse(diskPartitionProperties["StartingOffset"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _type = diskPartitionProperties["Type"]!.Value.ToString()!;
        }

        // Properties
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
        public bool Bootable
        {
            get { return _bootable; }
            set { _bootable = value; }
        }
        public bool PrimaryPartition
        {
            get { return _primaryPartition; }
            set { _primaryPartition = value; }
        }
        public bool BootPartition
        {
            get { return _bootPartition; }
            set { _bootPartition = value; }
        }
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public long Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public long StartingOffset
        {
            get { return _startingOffset; }
            set { _startingOffset = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        // Methods
        // N/A
    }
}
