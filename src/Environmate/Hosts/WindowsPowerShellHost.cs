using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Environmate.Apps
{
    public class WindowsPowerShellHost : IDisposable
    {
        // Fields
        private Process _windowsPowerShell;
        private bool _elevated;

        // Constructors
        public WindowsPowerShellHost() : this(false) { }

        public WindowsPowerShellHost(bool asAdmin)
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
            _windowsPowerShell = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
                    Verb = verb,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            _windowsPowerShell.Start();
        }

        // Properties
        public bool IsElevated
        {
            get => _elevated;
        }

        // Methods
        public (string output, string errors) Run(string command)
        {
            _windowsPowerShell.StandardInput.WriteLine(command);
            string output = _windowsPowerShell.StandardOutput.ReadToEnd();
            string errors = _windowsPowerShell.StandardError.ReadToEnd();
            _windowsPowerShell.WaitForExit();
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
                _windowsPowerShell.Close();
                _windowsPowerShell.Dispose();
            }
        }
    }
}
