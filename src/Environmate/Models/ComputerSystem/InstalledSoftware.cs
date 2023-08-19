using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Environmate
{
    public class InstalledSoftware
    {
        // Fields
        private List<KeyValuePair<string, string>> _appPaths = new List<KeyValuePair<string, string>>();
        private List<RegistryKey> _x86UninstallKeys;
        private List<RegistryKey> _x64UninstallKeys;
        private List<RegistryKey> _userUninstallKeys;
        private List<RegistryKey> _appxUninstallKeys;
        private List<Software> _x86InstalledSoftware = new List<Software>();
        private List<Software> _x64InstalledSoftware = new List<Software>();
        private List<Software> _userInstalledSoftware = new List<Software>();
        private List<Software> _appxInstalledSoftware = new List<Software>();
        private List<Software> _software = new List<Software>();

        // Constructors
        public InstalledSoftware()
        {
            // Get uninstall registry keys
            using RegistryKey HKLM32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            using RegistryKey HKLM64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            string appPathsKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\";
            string uninstallRegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
            string appxPackagesRegistryKeyPath = @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages\";
            using RegistryKey appPathsKey32 = HKLM32.OpenSubKey(appPathsKeyPath);
            using RegistryKey appPathsKey64 = HKLM64.OpenSubKey(appPathsKeyPath);
            using RegistryKey softwareUninstallKey32 = HKLM32.OpenSubKey(uninstallRegistryKeyPath) ?? throw new ItemNotFoundException($"The 32 bit registry key \"{uninstallRegistryKeyPath}\" could not be found.");
            using RegistryKey softwareUninstallKey64 = HKLM64.OpenSubKey(uninstallRegistryKeyPath) ?? throw new ItemNotFoundException($"The 64 bit registry key \"{uninstallRegistryKeyPath}\" could not be found.");
            using RegistryKey softwareUninstallKeyUser = Registry.CurrentUser.OpenSubKey(uninstallRegistryKeyPath) ?? throw new ItemNotFoundException($"The current user registry key \"{uninstallRegistryKeyPath}\" could not be found.");
            using RegistryKey softwareUninstallKeyAppx = Registry.CurrentUser.OpenSubKey(appxPackagesRegistryKeyPath) ?? throw new ItemNotFoundException($"The registry key \"{appxPackagesRegistryKeyPath}\" could not be found.");
            _x86UninstallKeys = GetUninstallKeys(softwareUninstallKey32);
            _x64UninstallKeys = GetUninstallKeys(softwareUninstallKey64);
            _userUninstallKeys = GetUninstallKeys(softwareUninstallKeyUser);
            _appxUninstallKeys = GetUninstallKeys(softwareUninstallKeyAppx);
            // Generate installed software list
            Console.Write("Gathering installed x86 Software...");
            foreach (var uninstallKey in _x86UninstallKeys)
            {
                if (!string.IsNullOrEmpty(uninstallKey.GetValue("DisplayName")?.ToString()) && !string.IsNullOrEmpty(uninstallKey.GetValue("UninstallString")?.ToString()))
                {
                    if (uninstallKey.GetValue("DisplayName")!.ToString() == "Software Operation Panel") continue;
                    if (uninstallKey.GetValue("DisplayName")!.ToString() == "FUJITSU Scanner USB HotFix") continue;
                    try
                    {
                        int systemComponent = (int)uninstallKey.GetValue("SystemComponent");
                        if (systemComponent != 1)
                        {
                            _x86InstalledSoftware.Add(new Software(uninstallKey));
                        }
                    }
                    catch (NullReferenceException)
                    {
                        _x86InstalledSoftware.Add(new Software(uninstallKey));
                    }
                }
            }
            Console.Write($"     {_x86InstalledSoftware.Count} found.\n");
            Console.Write("Gathering installed x64 software...");
            foreach (var uninstallKey in _x64UninstallKeys)
            {
                if (uninstallKey.GetValue("DisplayName") != null && uninstallKey.GetValue("UninstallString") != null && uninstallKey.GetValue("DisplayName")!.ToString()! != "GDR 1050 for SQL Server 2022 (KB5021522) (64-bit)")
                {
                    try
                    {
                        int systemComponent = (int)uninstallKey.GetValue("SystemComponent");
                        if (systemComponent != 1)
                        {
                            _x64InstalledSoftware.Add(new Software(uninstallKey));
                        }
                    }
                    catch (NullReferenceException)
                    {
                        _x64InstalledSoftware.Add(new Software(uninstallKey));
                    }
                }
            }
            Console.Write($"     {_x64InstalledSoftware.Count} found.\n");
            Console.Write("Gathering installed user software...");
            foreach (var uninstallKey in _userUninstallKeys)
            {
                if (uninstallKey.GetValue("DisplayName") != null && uninstallKey.GetValue("UninstallString") != null)
                {
                    try
                    {
                        int systemComponent = (int)uninstallKey.GetValue("SystemComponent");
                        if (systemComponent != 1)
                        {
                            _userInstalledSoftware.Add(new Software(uninstallKey));
                        }
                    }
                    catch (NullReferenceException)
                    {
                        _userInstalledSoftware.Add(new Software(uninstallKey));
                    }
                }
            }
            Console.Write($"     {_userInstalledSoftware.Count} found.\n");
            Console.Write("Gathering installed UWP software...");
            foreach (var uninstallKey in _appxUninstallKeys)
            {
                if (uninstallKey.GetValue("DisplayName") != null && uninstallKey.GetValue("PackageID") != null)
                {
                    _appxInstalledSoftware.Add(new Software(uninstallKey));
                }
            }
            Console.Write($"     {_appxInstalledSoftware.Count} found.\n");
            _software.AddRange(_x64InstalledSoftware);
            _software.AddRange(_x86InstalledSoftware);
            _software.AddRange(_userInstalledSoftware);
            _software.AddRange(_appxInstalledSoftware);
            Console.WriteLine($"{_software.Count} total installed software found.");
        }

        // Properties
        public List<Software> Software
        {
            get { return _software; }
        }
        public List<RegistryKey> x86UninstallKeys
        {
            get { return _x86UninstallKeys; }
            set { _x86UninstallKeys = value; }
        }
        public List<RegistryKey> x64UninstallKeys
        {
            get { return _x64UninstallKeys; }
            set { _x64UninstallKeys = value; }
        }
        public List<RegistryKey> UserUninstallKeys
        {
            get { return _userUninstallKeys; }
            set { _userUninstallKeys = value; }
        }
        public List<RegistryKey> AppxUninstallKeys
        {
            get { return _appxUninstallKeys; }
            set { _appxUninstallKeys = value; }
        }

        // Methods
        List<RegistryKey> GetUninstallKeys(RegistryKey uninstallRegistryKey)
        {
            var uninstallKeys = new List<RegistryKey>();
            foreach (string subkeyName in uninstallRegistryKey.GetSubKeyNames())
            {
                RegistryKey subKey = uninstallRegistryKey.OpenSubKey(subkeyName)!;
                uninstallKeys.Add(subKey);
            }
            return uninstallKeys;
        }
    }
}
