using System;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class Fan
    {
        // Fields
        private string _name;
        private string _status;
        private string _deviceId;
        private bool _activeCooling;

        // Constructors
        public Fan() { }
        public Fan(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_Fan")
            {
                throw new ArgumentException($"A Fan instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> fanProperties = cimInstance.CimInstanceProperties;
            _name = fanProperties["Name"]!.Value.ToString()!;
            _status = fanProperties["Status"]!.Value.ToString()!;
            _deviceId = fanProperties["DeviceID"]!.Value.ToString()!;
            _activeCooling = bool.Parse(fanProperties["ActiveCooling"]!.Value.ToString()!);
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
        public bool ActiveCooling
        {
            get { return _activeCooling; }
            set { _activeCooling = value; }
        }

        // Methods
        // N/A
    }
}
