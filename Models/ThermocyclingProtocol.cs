using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocol
    {
        public List<ThermocyclingProtocolStep> Steps = new List<ThermocyclingProtocolStep>();

        /// <summary>
        /// Add a step to the thermocycling protocol
        /// </summary>
        /// <param name="step">ThermocyclingProtocolStep to be added to the protocol</param>
        public void AddStep(ThermocyclingProtocolStep step)
        {
            Steps.Add(step);
        }

        /// <summary>
        /// Remove a step by the step index
        /// </summary>
        /// <param name="stepIndex">Index of the step within the protocol</param>
        public void RemoveStep(int stepIndex)
        {
            Steps.RemoveAt(stepIndex);
        }

        /// <summary>
        /// Edit a step in the protocol with the step index 
        /// </summary>
        /// <param name="stepIndex">Step index for the step to be edited</param>
        /// <param name="step">Step to replace the old step</param>
        public void EditStep(int stepIndex, ThermocyclingProtocolStep step)
        {
            Steps[stepIndex] = step;
        }

        /// <summary>
        /// Return the number of steps in the protocol
        /// </summary>
        public int Count
        {
            get { return Steps.Count; }
        }
    }
}
