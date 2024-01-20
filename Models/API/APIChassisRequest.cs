using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.API
{
    internal class APIChassisRequest
    {
        public int id { get; set; }
        public string value { get; set; }

        public APIChassisRequest(int id, string value = null)
        {
            this.id = id;
            this.value = value;
        }
    }
}
