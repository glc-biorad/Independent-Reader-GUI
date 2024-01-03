using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage the Motor instances
    /// </summary>
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

        public async Task HomeImagerAsync()
        {
            // TODO: Home the Filter Wheel
            await motors[3].HomeAsync();
            // TODO: Home z
            await motors[2].HomeAsync();
            // TODO: Home x and y
            var homeXMotorTask = motors[0].HomeAsync();
            var homeYMotorTask = motors[1].HomeAsync();
            await Task.WhenAll(homeXMotorTask, homeYMotorTask);
        }

        public async Task HomeTrayABSafelyAsync()
        {
            // TODO: Warn the user Clamp A and Clamp B will be homed
            // TODO: Home Clamp A and Clamp B
            var homeClampAMotorTask = motors[6].HomeAsync();
            var homeClampBMotorTask = motors[7].HomeAsync();
            await Task.WhenAll(homeClampAMotorTask, homeClampBMotorTask);
            // TODO: Home Tray AB
            await motors[4].HomeAsync();
        }

        public async Task HomeTrayCDSafelyAsync()
        {
            // TODO: Warn the user Clamp C and Clamp D will be homed
            // TODO: Home Clamp C and Clamp D
            var homeClampCMotorTask = motors[8].HomeAsync();
            var homeClampDMotorTask = motors[9].HomeAsync();
            await Task.WhenAll(homeClampCMotorTask, homeClampDMotorTask);
            // TODO: Home Tray CD
            await motors[5].HomeAsync();
        }

        /// <summary>
        /// Get the Motor instance from it's name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
