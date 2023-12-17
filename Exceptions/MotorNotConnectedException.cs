using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Exceptions
{
    internal class MotorNotConnectedException : Exception
    {
        private const string message = "Motor not connected";

        public MotorNotConnectedException() : base(message) { }
    }
}
