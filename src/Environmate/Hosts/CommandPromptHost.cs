using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Environmate.Apps
{
    public class CommandPromptHost : IDisposable
    {
        // Fields
        private Process _commandPrompt;
        private bool _elevated;

        // Constructors
        public CommandPromptHost() : this(false) { }
        public CommandPromptHost(bool asAdmin)
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
            _commandPrompt = new Process
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
            _commandPrompt.Start();
        }

        // Properties
        public bool IsElevated
        {
            get => _elevated;
        }

        // Methods
        public (string output, string errors) Run(string command)
        {
            _commandPrompt.StandardInput.WriteLine(command);
            string output = _commandPrompt.StandardOutput.ReadToEnd();
            string errors = _commandPrompt.StandardError.ReadToEnd();
            _commandPrompt.WaitForExit();
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
                _commandPrompt.Close();
                _commandPrompt.Dispose();
            }
        }
    }
}
