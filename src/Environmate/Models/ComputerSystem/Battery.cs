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
    public class Battery
    {
        // Fields
        private int _chemistry;
        private int? _designCapacity;
        private int _designVoltage;
        private string _name;
        private string _status;
        private string _deviceId;
        private int _estimatedChargeRemaining;
        private long _estimatedRunTime;

        // Constructors
        public Battery() { }
        public Battery(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_Battery")
            {
                throw new ArgumentException($"A Battery instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> batteryProperties = cimInstance.CimInstanceProperties;
            _chemistry = int.Parse(batteryProperties["Chemistry"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            //_designCapacity = int.Parse(batteryProperties["DesignCapacity"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _designVoltage = int.Parse(batteryProperties["DesignVoltage"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _name = batteryProperties["Name"]!.Value.ToString()!;
            _status = batteryProperties["Status"]!.Value.ToString()!;
            _deviceId = batteryProperties["DeviceID"]!.Value.ToString()!;
            _estimatedChargeRemaining = int.Parse(batteryProperties["EstimatedChargeRemaining"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _estimatedRunTime = long.Parse(batteryProperties["EstimatedRunTime"]!.Value.ToString()!, CultureInfo.InvariantCulture);
        }

        // Properties
        public int Chemistry
        {
            get { return _chemistry; }
            set { _chemistry = value; }
        }
        public int? DesignCapacity
        {
            get { return _designCapacity; }
            set { _designCapacity = value; }
        }
        public int DesignVoltage
        {
            get { return _designVoltage; }
            set { _designVoltage = value; }
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
        public string DeviceID
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
        public int EstimatedChargeRemaining
        {
            get { return _estimatedChargeRemaining; }
            set { _estimatedChargeRemaining = value; }
        }
        public long EstimatedRunTime
        {
            get { return _estimatedRunTime; }
            set { _estimatedRunTime = value; }
        }

        // Methods
        // N/A
    }
}
