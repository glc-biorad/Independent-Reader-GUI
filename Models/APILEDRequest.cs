using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class APILEDRequest
    {
        private int id { get; set; }
        private double? value { get; set; }

        public APILEDRequest(int id, double? value = null)
        {
            this.id = id;
            if (value != null)
            {
                this.value = value;
            }
        }
    }
}
