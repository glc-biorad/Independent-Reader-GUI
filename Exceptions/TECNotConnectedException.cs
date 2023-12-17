using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Exceptions
{
    internal class TECNotConnectedException : Exception
    {
        private const string message = "TEC not connected";

        public TECNotConnectedException() : base(message) { }
    }
}
