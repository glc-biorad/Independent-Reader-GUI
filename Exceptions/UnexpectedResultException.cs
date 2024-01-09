using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Exceptions
{
    internal class UnexpectedResultException : Exception
    {
        public UnexpectedResultException()
            : base() { }
        public UnexpectedResultException(string message)
            : base(message) { }

        public UnexpectedResultException(string message, Exception innerException)
            : base(message, innerException) { }

        protected UnexpectedResultException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
