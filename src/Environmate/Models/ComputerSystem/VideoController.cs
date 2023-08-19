using System;
using System.Globalization;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class VideoController
    {
        // Fields
        private string _name;
        private string _status;
        private string _deviceId;
        private int _maxRefreshRate;
        private int _minRefreshRate;
        private string _videoProcessor;
        private long _adapterRam;
        private DateTime _driverDate;
        private Version _driverVersion;
        private int _bitsPerPixel;
        private int _horizontalResolution;
        private long _numberOfColors;
        private string _infFilename;
        private int _refreshRate;
        private int _verticalResolution;
        private int _scanMode;

        // Constructors
        public VideoController() { }
        public VideoController(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_VideoController")
            {
                throw new ArgumentException($"A VideoController instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> videoControllerProperties = cimInstance.CimInstanceProperties;
            _status = videoControllerProperties["Status"]!.Value.ToString()!;
            _name = videoControllerProperties["Name"]!.Value.ToString()!;
            _deviceId = videoControllerProperties["DeviceID"]!.Value.ToString()!;
            _maxRefreshRate = int.Parse(videoControllerProperties["MaxRefreshRate"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _minRefreshRate = int.Parse(videoControllerProperties["MinRefreshRate"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _videoProcessor = videoControllerProperties["VideoProcessor"]!.Value.ToString()!;
            _adapterRam = long.Parse(videoControllerProperties["AdapterRAM"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _driverDate = DateTime.Parse(videoControllerProperties["DriverDate"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _driverVersion = Version.Parse(videoControllerProperties["DriverVersion"]!.Value.ToString()!);
            _bitsPerPixel = int.Parse(videoControllerProperties["CurrentBitsPerPixel"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _horizontalResolution = int.Parse(videoControllerProperties["CurrentHorizontalResolution"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _numberOfColors = long.Parse(videoControllerProperties["CurrentNumberOfColors"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _infFilename = videoControllerProperties["InfFilename"]!.Value.ToString()!;
            _refreshRate = int.Parse(videoControllerProperties["CurrentRefreshRate"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _verticalResolution = int.Parse(videoControllerProperties["CurrentVerticalResolution"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _scanMode = int.Parse(videoControllerProperties["CurrentScanMode"]!.Value.ToString()!, CultureInfo.InvariantCulture);
        }

        // Properties
        public string Name
        {
            get { return _name; }
        }
        public string Status
        {
            get { return _status; }
        }
        public string DeviceId
        {
            get { return _deviceId; }
        }
        public int MaxRefreshRate
        {
            get { return _maxRefreshRate; }
        }
        public int MinRefreshRate
        {
            get { return _minRefreshRate; }
        }
        public string VideoProcessor
        {
            get { return _videoProcessor; }
        }
        public long AdapterRam
        {
            get { return _adapterRam; }
        }
        public string AdapterRamHR
        {
            get { return ComputerSystem.GetSizeHR(_adapterRam); }
        }
        public DateTime DriverDate
        {
            get { return _driverDate; }
        }
        public Version DriverVersion
        {
            get { return _driverVersion; }
        }
        public int BitsPerPixel
        {
            get { return _bitsPerPixel; }
        }
        public int HorizontalResolution
        {
            get { return _horizontalResolution; }
        }
        public long NumberOfColors
        {
            get { return _numberOfColors; }
        }
        public string InfFilename
        {
            get { return _infFilename; }
        }
        public int RefreshRate
        {
            get { return _refreshRate; }
        }
        public int VerticalResolution
        {
            get { return _verticalResolution; }
        }
        public int ScanMode
        {
            get { return _scanMode; }
        }

        // Methods
        // N/A
    }
}
