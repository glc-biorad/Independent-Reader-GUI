using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
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
        private List<CancellationTokenSource> tecCancellationTokenSources = new List<CancellationTokenSource>();
        private CancellationTokenSource tecARunCancellationTokenSource;
        private CancellationTokenSource tecBRunCancellationTokenSource;
        private CancellationTokenSource tecCRunCancellationTokenSource;
        private CancellationTokenSource tecDRunCancellationTokenSource;
        private bool tecAProtocolRunning;
        private bool tecBProtocolRunning;
        private bool tecCProtocolRunning;
        private bool tecDProtocolRunning;

        public TECManager(TEC tecA, TEC tecB, TEC tecC, TEC tecD)
        {
            TECA = tecA;
            TECB = tecB;
            TECC = tecC;
            TECD = tecD;
            tecCancellationTokenSources.Add(tecARunCancellationTokenSource);
            tecCancellationTokenSources.Add(tecBRunCancellationTokenSource);
            tecCancellationTokenSources.Add(tecCRunCancellationTokenSource);
            tecCancellationTokenSources.Add(tecDRunCancellationTokenSource);
        }

        public async Task Run(ThermocyclingProtocol protocol, TEC tec, DataGridView dataGridView, CancellationToken cancellationToken)
        {
            DataGridViewManager dataGridViewManager = new DataGridViewManager();
            try
            {
                // Set the protocol name in the data grid view for this tec
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol", protocol.Name);
                // TODO: Set the cell for the Protocol Running to MediumSeaGreen
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol Running", "Yes");
                // TODO: Set the tec*ProtocolRunning bool variable to true
                while (!cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    foreach (var step in protocol.Steps)
                    {
                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Step", step.StepNumber.ToString());
                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Target Temp (\u00B0C)", step.Temperature.ToString());
                        await Task.Delay(5000, cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show($"Run has been cancelled on {tec.Name}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async Task HandleRunClickEvent(object sender, EventArgs e, DataGridView dataGridView, ThermocyclingProtocol protocol)
        {
            // Get the Name of the clicked Run button to get the TEC to use
            if (sender.GetType() == typeof(Button))
            {
                Button? button = sender as Button;
                TEC? tec = (button.Name == "thermocyclingTECARunButton") ? TECA
                    : (button.Name == "thermocyclingTECBRunButton") ? TECB
                    : (button.Name == "thermocyclingTECCRunButton") ? TECC
                    : (button.Name == "thermocyclingTECDRunButton") ? TECD
                    : null;
                // Obtain the correct cancellation token source
                int _cts_idx = (tec.Name == "TEC A") ? 0
                    : (tec.Name == "TEC B") ? 1
                    : (tec.Name == "TEC C") ? 2
                    : (tec.Name == "TEC D") ? 3
                    : -1;
                // Cancel any existing operations
                if (_cts_idx != -1 && tecCancellationTokenSources[_cts_idx] != null)
                {
                    tecCancellationTokenSources[_cts_idx].Cancel();
                    tecCancellationTokenSources[_cts_idx].Dispose();
                }
                tecCancellationTokenSources[_cts_idx] = new CancellationTokenSource();
                // Start the run on the TEC with a cancelation token
                await Run(protocol, tec, dataGridView, tecCancellationTokenSources[_cts_idx].Token);
            }
        }

        public void HandleKillClickEvent(object sender, EventArgs e, DataGridView dataGridView)
        {
            DataGridViewManager dataGridViewManager = new DataGridViewManager();
            // Get the Name of the clicked Run button to get the TEC to use
            if (sender.GetType() == typeof(Button))
            {
                Button? button = sender as Button;
                int _cts_idx = (button.Name == "thermocyclingTECAKillButton") ? 0
                    : (button.Name == "thermocyclingTECBKillButton") ? 1
                    : (button.Name == "thermocyclingTECCKillButton") ? 2
                    : (button.Name == "thermocyclingTECDKillButton") ? 3
                    : -1;
                // Cancel
                if (tecCancellationTokenSources[_cts_idx] != null)
                {
                    tecCancellationTokenSources[_cts_idx].Cancel();
                    // TODO: Set the Protocol Running cell background color to Yellow for a certain amount of time or indefinitely
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, "TEC A", "Protocol Running", "No");
                    // TODO: Set the tec*ProtocolRunning bool variable to false
                }
            }
        }
    }
}
