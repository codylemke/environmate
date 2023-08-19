using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environmate.Hosts
{
    public class HostManager
    {
        // Fields
        private bool _elevated;
        // Need a way of managing processes. Maybe list or dictionary of them for each type of host? Or a `TerminalProcess` object for each then put them in a list?

        // Constructors
        public HostManager() : this(false) { }
        public HostManager(bool elevated)
        {
            _elevated = elevated;
        }

        // Properties
        public bool IsElevated => _elevated;

        // Methods

    }
}
