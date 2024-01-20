using Independent_Reader_GUI.Models.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol
{
    internal class ThermocyclingProtocol
    {
        private string name = string.Empty;

        public List<ThermocyclingProtocolStep> Steps = new List<ThermocyclingProtocolStep>();

        public string Name
        {
            get { return name; }
        }

        public void SetName(string protocolName)
        {
            name = protocolName;
        }

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

        /// <summary>
        /// Get the ThermocyclingProtocolSteps between two step indeces, including the start and end index steps
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<ThermocyclingProtocolStep> GetStepsBetween(int startIndex, int endIndex)
        {
            List<ThermocyclingProtocolStep> betweenSteps = new List<ThermocyclingProtocolStep>();
            foreach (var step in Steps)
            {
                if (step.Index >= startIndex && step.Index <= endIndex)
                {
                    betweenSteps.Add(step);
                }
            }
            return betweenSteps;
        }

        public double GetTimeInSeconds()
        {
            double time;
            double timeInSeconds = 0;
            string timeUnits;
            string stepName;
            foreach (var step in Steps)
            {
                stepName = step.TypeName;
                if (stepName.Equals(ThermocyclingProtocolStepType.Set))
                {
                    time = step.Time;
                    timeUnits = step.TimeUnits;
                    if (timeUnits != null)
                    {
                        if (timeUnits.Equals(TimeUnit.Minutes))
                        {
                            timeInSeconds += time * 60;
                        }
                        else if (timeUnits.Equals(TimeUnit.Hours))
                        {
                            timeInSeconds += time * Math.Pow(60, 2);
                        }
                        else
                        {
                            timeInSeconds += time;
                        }
                    }
                }
                else if (stepName.Equals(ThermocyclingProtocolStepType.GoTo))
                {
                    double gotoTimeInSeconds = 0.0;
                    int gotoStepNumber = step.StepNumber;
                    int cycleCount = step.CycleCount;
                    int index = step.Index;
                    var gotoSteps = GetStepsBetween(gotoStepNumber, index - 1);
                    foreach (var gotoStep in gotoSteps)
                    {
                        time = gotoStep.Time;
                        timeUnits = gotoStep.TimeUnits;
                        if (timeUnits != null)
                        {
                            if (timeUnits.Equals(TimeUnit.Minutes))
                            {
                                gotoTimeInSeconds += time * 60;
                            }
                            else if (timeUnits.Equals(TimeUnit.Hours))
                            {
                                gotoTimeInSeconds += time * Math.Pow(60, 2);
                            }
                            else
                            {
                                gotoTimeInSeconds += time;
                            }
                        }
                    }
                    timeInSeconds += gotoTimeInSeconds * cycleCount;
                }
                else
                {
                    timeInSeconds += 0;
                }
            }
            return timeInSeconds;
        }
    }
}
