using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol
{
    internal class ThermocyclingProtocolGoToStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolGoToStep(int stepNumber, int cycleCount)
        {
            TypeName = ThermocyclingProtocolStepType.GoTo;
            StepNumber = stepNumber;
            CycleCount = cycleCount;
        }
    }
}
