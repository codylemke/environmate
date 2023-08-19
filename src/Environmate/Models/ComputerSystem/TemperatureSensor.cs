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
    public class TemperatureSensor
    {
        // Fields
        private string _description;
        private string _name;
        private string _status;
        private string _deviceId;
        private int _accuracy;
        private int _maxReadable;
        private int _minReadable;
        private int _resolution;
        private int _tolerance;

        // Constructors
        public TemperatureSensor() { }
        public TemperatureSensor(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_TemperatureProbe")
            {
                throw new ArgumentException($"A TemperatureSensor instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> temperatureSensorProperties = cimInstance.CimInstanceProperties;
            _description = temperatureSensorProperties["Description"]!.Value.ToString()!;
            _name = temperatureSensorProperties["Name"]!.Value.ToString()!;
            _status = temperatureSensorProperties["Status"]!.Value.ToString()!;
            _deviceId = temperatureSensorProperties["DeviceId"]!.Value.ToString()!;
            _accuracy = int.Parse(temperatureSensorProperties["Accuracy"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _maxReadable = int.Parse(temperatureSensorProperties["MaxReadable"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _minReadable = int.Parse(temperatureSensorProperties["MinReadable"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _resolution = int.Parse(temperatureSensorProperties["Resolution"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _tolerance = int.Parse(temperatureSensorProperties["Tolerance"]!.Value.ToString()!, CultureInfo.InvariantCulture);
        }

        // Properties
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
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
        public int Accuracy
        {
            get { return _accuracy; }
            set { _accuracy = value; }
        }
        public int MaxReadable
        {
            get { return _maxReadable; }
            set { _maxReadable = value; }
        }
        public int MinReadable
        {
            get { return _minReadable; }
            set { _minReadable = value; }
        }
        public int Resolution
        {
            get { return _resolution; }
            set { _resolution = value; }
        }
        public int Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
        }

        // Methods
        // N/A
    }
}
