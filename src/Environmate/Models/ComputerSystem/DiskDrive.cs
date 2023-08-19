using System;
using System.Globalization;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class DiskDrive
    {
        // Fields
        private string _deviceId;
        private string _model;
        private string _manufacturer;
        private string _firmwareRevision;
        private string _interfaceType;
        private string _mediaType;
        private bool _mediaLoaded;
        private string _serialNumber;
        private string _status;
        private long _totalHeads;
        private long _totalCylinders;
        private long _totalTracks;
        private long _totalSectors;
        private long _size;
        private int _tracksPerCylinder;
        private int _sectorsPerTrack;
        private string _bytesPerSector;
        private int _partitions;

        // Constructors
        public DiskDrive() { }
        public DiskDrive(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_DiskDrive")
            {
                throw new ArgumentException($"A DiskDrive instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> cimInstanceProperties = cimInstance.CimInstanceProperties;
            _deviceId = cimInstanceProperties["DeviceID"]!.Value.ToString()!;
            _model = cimInstanceProperties["Model"]!.Value.ToString()!;
            _manufacturer = cimInstanceProperties["Manufacturer"]!.Value.ToString()!;
            _firmwareRevision = cimInstanceProperties["FirmwareRevision"]!.Value.ToString()!;
            _interfaceType = cimInstanceProperties["InterfaceType"]!.Value.ToString()!;
            _mediaType = cimInstanceProperties["MediaType"]!.Value.ToString()!;
            _mediaLoaded = bool.Parse(cimInstanceProperties["MediaLoaded"]!.Value.ToString()!);
            _serialNumber = cimInstanceProperties["SerialNumber"]!.Value.ToString()!;
            _status = cimInstanceProperties["Status"]!.Value.ToString()!;
            _totalHeads = long.Parse(cimInstanceProperties["TotalHeads"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _totalCylinders = long.Parse(cimInstanceProperties["TotalCylinders"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _totalTracks = long.Parse(cimInstanceProperties["TotalTracks"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _totalSectors = long.Parse(cimInstanceProperties["TotalSectors"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _size = long.Parse(cimInstanceProperties["Size"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _tracksPerCylinder = int.Parse(cimInstanceProperties["TracksPerCylinder"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _sectorsPerTrack = int.Parse(cimInstanceProperties["SectorsPerTrack"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _bytesPerSector = cimInstanceProperties["BytesPerSector"]!.Value.ToString()!;
            _partitions = int.Parse(cimInstanceProperties["Partitions"]!.Value.ToString()!, CultureInfo.InvariantCulture);
        }

        // Properties
        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }
        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; }
        }
        public string FirmwareRevision
        {
            get { return _firmwareRevision; }
            set { _firmwareRevision = value; }
        }
        public string InterfaceType
        {
            get { return _interfaceType; }
            set { _interfaceType = value; }
        }
        public string MediaType
        {
            get { return _mediaType; }
            set { _mediaType = value; }
        }
        public bool MediaLoaded
        {
            get { return _mediaLoaded; }
            set { _mediaLoaded = value; }
        }
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public long TotalHeads
        {
            get { return _totalHeads; }
            set { _totalHeads = value; }
        }
        public long TotalCylinders
        {
            get { return _totalCylinders; }
            set { _totalCylinders = value; }
        }
        public long TotalTracks
        {
            get { return _totalTracks; }
            set { _totalTracks = value; }
        }
        public long TotalSectors
        {
            get { return _totalSectors; }
            set { _totalSectors = value; }
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
        public int TracksPerCylinder
        {
            get { return _tracksPerCylinder; }
            set { _tracksPerCylinder = value; }
        }
        public int SectorsPerTrack
        {
            get { return _sectorsPerTrack; }
            set { _sectorsPerTrack = value; }
        }
        public string BytesPerSector
        {
            get { return _bytesPerSector; }
            set { _bytesPerSector = value; }
        }
        public int Partitions
        {
            get { return _partitions; }
            set { _partitions = value; }
        }

        // Methods
        // N/A
    }
}
