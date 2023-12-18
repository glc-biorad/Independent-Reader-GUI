using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class TECManager
    {
        private TEC TECA;
        private TEC TECB;
        private TEC TECC;
        private TEC TECD;

        public TECManager(TEC tecA, TEC tecB, TEC tevC, TEC tecD)
        {
            TECA = tecA;
            TECB = tecB;
            TECC = tevC;
            TECD = tecD;
        }
    }
}
