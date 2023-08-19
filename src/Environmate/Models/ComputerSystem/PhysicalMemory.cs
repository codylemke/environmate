
using System;
using System.Globalization;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class PhysicalMemory
    {
        // Fields
        private string _tag;
        private long _capacity;
        private int _dataWidth;
        private int _speed;
        private int _configuredClockSpeed;
        private int _configuredVoltage;

        // Constructors
        public PhysicalMemory() { }
        public PhysicalMemory(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_PhysicalMemory")
            {
                throw new ArgumentException($"A PhysicalMemory instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> memoryProperties = cimInstance.CimInstanceProperties;
            _tag = memoryProperties["Tag"]!.Value.ToString()!;
            _capacity = long.Parse(memoryProperties["Capacity"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _dataWidth = int.Parse(memoryProperties["DataWidth"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _speed = int.Parse(memoryProperties["Speed"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _configuredClockSpeed = int.Parse(memoryProperties["ConfiguredClockSpeed"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _configuredVoltage = int.Parse(memoryProperties["ConfiguredVoltage"]!.Value.ToString()!, CultureInfo.InvariantCulture);
        }

        // Properties
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        public long Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }
        public string CapacityHR
        {
            get { return ComputerSystem.GetSizeHR(_capacity); }
        }
        public int DataWidth
        {
            get { return _dataWidth; }
            set { _dataWidth = value; }
        }
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public int ConfiguredClockSpeed
        {
            get { return _configuredClockSpeed; }
            set { _configuredClockSpeed = value; }
        }
        public int ConfiguredVoltage
        {
            get { return _configuredVoltage; }
            set { _configuredVoltage = value; }
        }

        // Methods
        // N/A
    }
}
