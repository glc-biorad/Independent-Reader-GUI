using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Exceptions
{
    internal class LEDNotFoundException : Exception
    {
        public LEDNotFoundException()
            : base() { }
        public LEDNotFoundException(string message)
            : base(message) { }

        public LEDNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected LEDNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
