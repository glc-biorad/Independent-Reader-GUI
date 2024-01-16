using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class APITECRequest
    {
        private string name { get; set; }
        private double? value { get; set; }

        private string? strValue { get; set; }

        public APITECRequest(string name, double? value = null, string? strValue = null)
        {
            this.name = name;
            if (value != null)
            {
                this.value = value;
            }
            if (strValue != null)
            {
                this.strValue = strValue;
            }
        }
    }
}
