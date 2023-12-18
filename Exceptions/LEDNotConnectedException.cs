using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Exceptions
{
    internal class LEDNotConnectedException : Exception
    {
        public LEDNotConnectedException() 
            : base() { }
        public LEDNotConnectedException(string message) 
            : base(message) { }

        public LEDNotConnectedException(string message, Exception innerException) 
            : base(message, innerException) { }

        protected LEDNotConnectedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
