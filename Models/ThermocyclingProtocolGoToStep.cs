using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolGoToStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolGoToStep(int stepNumber, int cycleCount)
        {
            this.TypeName = ThermocyclingProtocolStepType.GoTo;
            this.StepNumber = stepNumber;
            this.CycleCount = cycleCount;
        }
    }
}
