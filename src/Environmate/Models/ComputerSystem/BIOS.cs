using System;
using System.Globalization;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class BIOS
    {
        // Fields
        private Version _version;
        private string _manufacturer;
        private string _serialNumber;
        private bool _primaryBios;
        private DateTime _releaseDate;
        private string _status;

        // Constructors
        public BIOS() { }
        public BIOS(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_BIOS")
            {
                throw new ArgumentException($"A BIOSElement instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> biosProperties = cimInstance.CimInstanceProperties;
            _version = Version.Parse(biosProperties["SMBIOSBIOSVersion"]!.Value.ToString()!);
            _manufacturer = biosProperties["Manufacturer"]!.Value.ToString()!;
            _serialNumber = biosProperties["SerialNumber"]!.Value.ToString()!;
            _primaryBios = bool.Parse(biosProperties["PrimaryBIOS"]!.Value.ToString()!);
            _releaseDate = DateTime.Parse(biosProperties["ReleaseDate"]!.Value.ToString()!, CultureInfo.InvariantCulture);
            _status = biosProperties["Status"]!.Value.ToString()!;
        }

        // Properties
        public Version Version
        {
            get { return _version; }
        }
        public string Manufacturer
        {
            get { return _manufacturer; }
        }
        public string SerialNumber
        {
            get { return _serialNumber; }
        }
        public bool PrimaryBios
        {
            get { return _primaryBios; }
        }
        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
        }
        public string Status
        {
            get { return _status; }
        }

        // Methods
        // N/A
    }
}
