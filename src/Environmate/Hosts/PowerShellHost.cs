using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Environmate.Apps
{
    public class PowerShellHost : IDisposable
    {
        // Fields
        private PowerShell _powerShell;
        private bool _elevated;

        // Constructors
        public PowerShellHost() : this(false) { }
        
        public PowerShellHost(bool asAdmin)
        {
            _powerShell = PowerShell.Create();
            _elevated = false;
        }

        // Properties
        public bool IsElevated
        {
            get => _elevated;
        }

        // Methods
        public Collection<object> Run(string command)
        {
            _powerShell.AddScript(command, false);
            Collection<PSObject> powerShellOutput = _powerShell.Invoke();
            Collection<object> outputObjects = new Collection<object>(powerShellOutput.Select(x => x.BaseObject).ToArray());
            _powerShell.Commands.Clear();
            return outputObjects;
        }

        public async Task RunAsync(string command)
        {
            await Task.Run(() => Run(command)).ConfigureAwait(false);
        }

        public void Restart()
        {
            _powerShell.Dispose();
            _powerShell = PowerShell.Create();
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
                _powerShell.Stop();
                _powerShell.Dispose();
            }
        }
    }
}
