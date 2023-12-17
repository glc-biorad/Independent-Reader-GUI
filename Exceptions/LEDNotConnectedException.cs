using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Exceptions
{
    internal class LEDNotConnectedException : Exception
    {
        private const string message = "LED not connected";
        public LEDNotConnectedException() : base(message) { }
    }
}
