using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class MotorsManager
    {
        private List<Motor> motors = new List<Motor>();

        public MotorsManager(Motor x, Motor y, Motor z, Motor filterWheel, Motor trayAB, Motor trayCD, Motor clampA, Motor clampB, Motor clampC, Motor clampD) 
        {
            motors.Add(x);
            motors.Add(y);
            motors.Add(z);
            motors.Add(filterWheel);
            motors.Add(trayAB);
            motors.Add(trayCD);
            motors.Add(clampA);
            motors.Add(clampB);
            motors.Add(clampC);
            motors.Add(clampD);
        }

        public Motor? GetMotorByName(string name)
        {
            foreach (Motor motor in motors)
            {
                if (motor.Name.Equals(name))
                {
                    return motor;
                }
            }
            return null;
        }
    }
}
