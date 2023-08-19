using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environmate.Apps
{
    public class PythonHost : IDisposable
    {
        // Fields
        private Process _pythonShell;
        private bool _elevated;

        // Constructors
        public PythonHost() : this(false) { }
        public PythonHost(bool asAdmin)
        {
            string verb;
            if (asAdmin)
            {
                verb = "runas";
                _elevated = true;
            }
            else
            {
                verb = "open";
                _elevated = false;
            }
            _pythonShell = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\WINDOWS\system32\cmd.exe",
                    Verb = verb,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            _pythonShell.Start();
        }

        // Properties
        public bool IsElevated
        {
            get => _elevated;
        }

        // Methods
        public (string output, string errors) Run(string command)
        {
            _pythonShell.StandardInput.WriteLine(command);
            string output = _pythonShell.StandardOutput.ReadToEnd();
            string errors = _pythonShell.StandardError.ReadToEnd();
            _pythonShell.WaitForExit();
            return (output, errors);
        }

        public async Task RunAsync(string command)
        {
            await Task.Run(() => Run(command)).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pythonShell.Close();
                _pythonShell.Dispose();
            }
        }
    }
}
