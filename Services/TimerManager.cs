using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage a Timer
    /// </summary>
    internal class TimerManager
    {
        private int interval;
        private System.Windows.Forms.Timer timer;

        public TimerManager(int interval, EventHandler tickEventHandler)
        {
            timer = new System.Windows.Forms.Timer();
            this.interval = interval;
            timer.Interval = interval;
            timer.Tick += tickEventHandler;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public int Interval
        {
            get { return this.interval; }
        }
    }
}
