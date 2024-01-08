using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class APIMotorRequest
    {
        public int id {  get; set; }
        public int? postion { get; set; }
        public int? distance {  get; set; }
        public int? velocity { get; set; }

        public APIMotorRequest(int id, int? postion = null, int? velocity = null, int? distance = null)
        {
            this.id = id;
            if (postion != null)
            {
                // TODO: Make it so that the GUI will only take positive step values or convert so it doesnt need to be done here
                if (postion < 0)
                {
                    this.postion = -1 * postion;
                }
                else
                {
                    this.postion = postion;
                }
            }
            this.velocity = velocity;
            this.distance = distance;
        }
    }
}
