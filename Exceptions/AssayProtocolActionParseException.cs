using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Exceptions
{
    internal class AssayProtocolActionParseException : Exception
    {
        public AssayProtocolActionParseException()
           : base() { }

        public AssayProtocolActionParseException(string actionText, List<string> typeOptions, Type AssayProtocolAction)
            : base($"Error: Assay Protocol Action ({AssayProtocolAction.GetType()}) Type ({string.Join(", ", typeOptions)}) not found in Action Text ({actionText})") { }

        public AssayProtocolActionParseException(string message, Exception innerException)
            : base(message, innerException) { }

        protected AssayProtocolActionParseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
