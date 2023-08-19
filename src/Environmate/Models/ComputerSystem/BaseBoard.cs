using System;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;

namespace Environmate
{
    public class BaseBoard
    {
        // Fields
        private string _status;
        private string _manufacturer;
        private string? _model;
        private bool _poweredOn;
        private string _serialNumber;
        private string _version;
        private bool _hotSwappable;
        private bool _removable;
        private bool _replaceable;
        private bool _hostingBoard;
        private bool _requiresDaughterBoard;
        private string _product;

        // Constructors
        public BaseBoard() { }
        public BaseBoard(CimInstance cimInstance)
        {
            if (cimInstance.CimClass.ToString().Split(':').Last() != "Win32_BaseBoard")
            {
                throw new ArgumentException($"A BaseBoard instance was attempted to be created with an incompatible CimInstance: {cimInstance.CimClass.ToString().Split(':').Last()}");
            }
            CimKeyedCollection<CimProperty> baseBoardProperties = cimInstance.CimInstanceProperties;
            _status = baseBoardProperties["Status"]!.Value.ToString()!;
            _manufacturer = baseBoardProperties["Manufacturer"]!.Value.ToString()!;
            _model = baseBoardProperties["Model"]?.Value?.ToString() ?? null;
            _poweredOn = bool.Parse(baseBoardProperties["PoweredOn"]!.Value.ToString()!);
            _serialNumber = baseBoardProperties["SerialNumber"]!.Value.ToString()!;
            _version = baseBoardProperties["Version"]!.Value.ToString()!;
            _hotSwappable = bool.Parse(baseBoardProperties["HotSwappable"]!.Value.ToString()!);
            _removable = bool.Parse(baseBoardProperties["Removable"]!.Value.ToString()!);
            _replaceable = bool.Parse(baseBoardProperties["Replaceable"]!.Value.ToString()!);
            _hostingBoard = bool.Parse(baseBoardProperties["HostingBoard"]!.Value.ToString()!);
            _requiresDaughterBoard = bool.Parse(baseBoardProperties["RequiresDaughterBoard"]!.Value.ToString()!);
            _product = baseBoardProperties["Product"]!.Value.ToString()!;
        }

        // Properties
        public string Status
        {
            get { return _status; }
        }
        public string Manufacturer
        {
            get { return _manufacturer; }
        }
        public string? Model
        {
            get { return _model; }
        }
        public bool PoweredOn
        {
            get { return _poweredOn; }
        }
        public string SerialNumber
        {
            get { return _serialNumber; }
        }
        public string Version
        {
            get { return _version; }
        }
        public bool HotSwappable
        {
            get { return _hotSwappable; }
        }
        public bool Removable
        {
            get { return _removable; }
        }
        public bool Replaceable
        {
            get { return _replaceable; }
        }
        public bool HostingBoard
        {
            get { return _hostingBoard; }
        }
        public bool RequiresDaughterBoard
        {
            get { return _requiresDaughterBoard; }
        }
        public string Product
        {
            get { return _product; }
        }

        // Methods
        // N/A
    }
}
