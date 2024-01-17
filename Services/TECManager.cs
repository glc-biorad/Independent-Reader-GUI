using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Resources;
using SpinnakerNET.GenApi;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using OxyPlot;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage the TEC instances
    /// </summary>
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
        private ConcurrentQueue<TECCommand> commandQueue = new ConcurrentQueue<TECCommand>();
        private ConcurrentQueue<TECCommand> priorityCommandQueue = new ConcurrentQueue<TECCommand>();
        private CancellationTokenSource commandQueueCancellationTokenSource = new CancellationTokenSource();
        private int msDelay = 5;
        private double tempReachedCutoff = 0.5;
        private TECPlotManager tecPlotManager = new TECPlotManager();

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

        /// <summary>
        /// Enqueue commands to the TEC Manager
        /// </summary>
        /// <param name="command"></param>
        public void EnqueueCommand(TECCommand command)
        {
            commandQueue.Enqueue(command);
        }

        /// <summary>
        /// Enqueue priority commands to the TEC Manager
        /// </summary>
        /// <param name="command"></param>
        public void EnqueuePriorityCommand(TECCommand command)
        {
            priorityCommandQueue.Enqueue(command);
        }

        /// <summary>
        /// Cancel the CommandQueue Processing
        /// </summary>
        public void CloseCommandQueueProcessing()
        {
            commandQueueCancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Cancel the Priority CommandQueue Processing
        /// </summary>
        public void ClosePriorityCommandQueueProcessing()
        {
            commandQueueCancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Process the command queue
        /// </summary>
        /// <returns></returns>
        public async Task ProcessCommandQueues()
        {
            while (!commandQueueCancellationTokenSource.Token.IsCancellationRequested)
            {
                if (priorityCommandQueue.Count == 0) 
                {
                    if (commandQueue.TryDequeue(out TECCommand command))
                    {
                        try
                        {
                            switch (command.Type)
                            {
                                case TECCommand.CommandType.CheckConnectionAsync:
                                    await command.TEC.CheckConnectionAsync();
                                    break;
                                case TECCommand.CommandType.GetObjectTemperature:
                                    await command.TEC.GetObjectTemperature();
                                    break;
                                case TECCommand.CommandType.SetObjectTemperature:
                                    await command.TEC.SetObjectTemperature(double.Parse(command.Parameter.ToString()));
                                    break;
                                case TECCommand.CommandType.GetSinkTemperature:
                                    await command.TEC.GetSinkTemperature();
                                    break;
                                case TECCommand.CommandType.GetTargetObjectTemperature:
                                    await command.TEC.GetTargetObjectTemperature();
                                    break;
                                case TECCommand.CommandType.GetActualOutputCurrent:
                                    await command.TEC.GetActualOutputCurrent();
                                    break;
                                case TECCommand.CommandType.GetActualOutputVoltage:
                                    await command.TEC.GetActualOutputVoltage();
                                    break;
                                case TECCommand.CommandType.GetRelativeCoolingPower:
                                    await command.TEC.GetRelativeCoolingPower();
                                    break;
                                case TECCommand.CommandType.GetActualFanSpeed:
                                    await command.TEC.GetActualFanSpeed();
                                    break;
                                case TECCommand.CommandType.GetCurrentErrorThreshold:
                                    await command.TEC.GetCurrentErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetVoltageErrorThreshold:
                                    await command.TEC.GetVoltageErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetObjectUpperErrorThreshold:
                                    await command.TEC.GetObjectUpperErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetObjectLowerErrorThreshold:
                                    await command.TEC.GetObjectLowerErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetSinkUpperErrorThreshold:
                                    await command.TEC.GetSinkUpperErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetSinkLowerErrorThreshold:
                                    await command.TEC.GetSinkLowerErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetTemperatureIsStable:
                                    await command.TEC.GetTemperatureIsStable();
                                    break;
                                case TECCommand.CommandType.GetTemperatureControl:
                                    await command.TEC.GetTemperatureControl();
                                    break;
                                case TECCommand.CommandType.ResetAsync:
                                    await command.TEC.ResetAsync();
                                    break;
                                case TECCommand.CommandType.GetFanControl:
                                    await command.TEC.GetFanControl();
                                    break;
                                case TECCommand.CommandType.SetFanControl:
                                    await command.TEC.SetFanControl(command.Parameter.ToString());
                                    break;
                                case TECCommand.CommandType.GetFanTargetTemperature:
                                    await command.TEC.GetFanTargetTemperature();
                                    break;
                                case TECCommand.CommandType.SetFanTargetTemperature:
                                    await command.TEC.SetFanTargetTemperature(double.Parse(command.Parameter.ToString()));
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Unable to handle {command.TEC.Name} command due to {ex.Message}");
                            // Note: Might need to requeue this command as a priority command
                            EnqueuePriorityCommand(command);
                        }
                    }
                    await Task.Delay(msDelay);
                }
                else
                {
                    if (priorityCommandQueue.TryDequeue(out TECCommand command))
                    {
                        try
                        {
                            switch (command.Type)
                            {
                                case TECCommand.CommandType.CheckConnectionAsync:
                                    await command.TEC.CheckConnectionAsync();
                                    break;
                                case TECCommand.CommandType.GetObjectTemperature:
                                    await command.TEC.GetObjectTemperature();
                                    break;
                                case TECCommand.CommandType.SetObjectTemperature:
                                    await command.TEC.SetObjectTemperature(double.Parse(command.Parameter.ToString()));
                                    break;
                                case TECCommand.CommandType.GetSinkTemperature:
                                    await command.TEC.GetSinkTemperature();
                                    break;
                                case TECCommand.CommandType.GetTargetObjectTemperature:
                                    await command.TEC.GetTargetObjectTemperature();
                                    break;
                                case TECCommand.CommandType.GetActualOutputCurrent:
                                    await command.TEC.GetActualOutputCurrent();
                                    break;
                                case TECCommand.CommandType.GetActualOutputVoltage:
                                    await command.TEC.GetActualOutputVoltage();
                                    break;
                                case TECCommand.CommandType.GetRelativeCoolingPower:
                                    await command.TEC.GetRelativeCoolingPower();
                                    break;
                                case TECCommand.CommandType.GetActualFanSpeed:
                                    await command.TEC.GetActualFanSpeed();
                                    break;
                                case TECCommand.CommandType.GetCurrentErrorThreshold:
                                    await command.TEC.GetCurrentErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetVoltageErrorThreshold:
                                    await command.TEC.GetVoltageErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetObjectUpperErrorThreshold:
                                    await command.TEC.GetObjectUpperErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetObjectLowerErrorThreshold:
                                    await command.TEC.GetObjectLowerErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetSinkUpperErrorThreshold:
                                    await command.TEC.GetSinkUpperErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetSinkLowerErrorThreshold:
                                    await command.TEC.GetSinkLowerErrorThreshold();
                                    break;
                                case TECCommand.CommandType.GetTemperatureIsStable:
                                    await command.TEC.GetTemperatureIsStable();
                                    break;
                                case TECCommand.CommandType.GetTemperatureControl:
                                    await command.TEC.GetTemperatureControl();
                                    break;
                                case TECCommand.CommandType.ResetAsync:
                                    await command.TEC.ResetAsync();
                                    break;
                                case TECCommand.CommandType.GetFanControl:
                                    await command.TEC.GetFanControl();
                                    break;
                                case TECCommand.CommandType.SetFanControl:
                                    await command.TEC.SetFanControl(command.Parameter.ToString());
                                    break;
                                case TECCommand.CommandType.GetFanTargetTemperature:
                                    await command.TEC.GetFanTargetTemperature();
                                    break;
                                case TECCommand.CommandType.SetFanTargetTemperature:
                                    await command.TEC.SetFanTargetTemperature(double.Parse(command.Parameter.ToString()));
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Unable to handle {command.TEC.Name} priority command due to {ex.Message}");
                            // Note: Might need to requeue this command as a priority command
                            EnqueuePriorityCommand(command);
                        }
                    }
                    await Task.Delay(msDelay);
                }
            }
        }

        public async Task InitializeTECParameters()
        {
            // Check the connection of each TEC
            TECCommand tecACheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = TECA };
            EnqueuePriorityCommand(tecACheckConnectionCommand);
            TECCommand tecBCheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = TECB };
            EnqueuePriorityCommand(tecBCheckConnectionCommand);
            TECCommand tecCCheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = TECC };
            EnqueuePriorityCommand(tecCCheckConnectionCommand);
            TECCommand tecDCheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = TECD };
            EnqueuePriorityCommand(tecDCheckConnectionCommand);

            // Obtain all internal parameters
            if (TECA.Connected)
            {
                TECCommand tecAGetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectTemperature, TEC = TECA };
                EnqueuePriorityCommand(tecAGetObjectTemperatureCommand);
                TECCommand tecAGetSinkTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkTemperature, TEC = TECA };
                EnqueuePriorityCommand(tecAGetSinkTemperatureCommand);
                TECCommand tecAGetTargetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetTargetObjectTemperature, TEC = TECA };
                EnqueuePriorityCommand(tecAGetTargetObjectTemperatureCommand);
                TECCommand tecAGetCurrentErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetCurrentErrorThreshold, TEC = TECA };
                EnqueuePriorityCommand(tecAGetCurrentErrorThresholdCommand);
                TECCommand tecAGetVoltageErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetVoltageErrorThreshold, TEC = TECA };
                EnqueuePriorityCommand(tecAGetVoltageErrorThresholdCommand);
                TECCommand tecAGetObjectUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectUpperErrorThreshold, TEC = TECA };
                EnqueuePriorityCommand(tecAGetObjectUpperErrorThresholdCommand);
                TECCommand tecAGetObjectLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectLowerErrorThreshold, TEC = TECA };
                EnqueuePriorityCommand(tecAGetObjectLowerErrorThresholdCommand);
                TECCommand tecAGetSinktUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkUpperErrorThreshold, TEC = TECA };
                EnqueuePriorityCommand(tecAGetSinktUpperErrorThresholdCommand);
                TECCommand tecAGetSinkLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkLowerErrorThreshold, TEC = TECA };
                EnqueuePriorityCommand(tecAGetSinkLowerErrorThresholdCommand);
                TECCommand tecAGetTemperatureIsStableCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureIsStable, TEC = TECA };
                EnqueuePriorityCommand(tecAGetTemperatureIsStableCommand);
                TECCommand tecAGetTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureControl, TEC = TECA };
                EnqueuePriorityCommand(tecAGetTemperatureControlCommand);
                TECCommand tecAGetFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECA };
                EnqueuePriorityCommand(tecAGetFanControlCommand);
                TECCommand tecAGetFanTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECA };
                EnqueuePriorityCommand(tecAGetFanTemperatureCommand);
                TECCommand getFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECA };
                EnqueuePriorityCommand(getFanControlCommand);
                TECCommand getTargetFanTempCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECA };
                EnqueuePriorityCommand(getTargetFanTempCommand);
            }
            if (TECB.Connected)
            {
                TECCommand tecBGetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectTemperature, TEC = TECB };
                EnqueuePriorityCommand(tecBGetObjectTemperatureCommand);
                TECCommand tecBGetSinkTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkTemperature, TEC = TECB };
                EnqueuePriorityCommand(tecBGetSinkTemperatureCommand);
                TECCommand tecBGetTargetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetTargetObjectTemperature, TEC = TECB };
                EnqueuePriorityCommand(tecBGetTargetObjectTemperatureCommand);
                TECCommand tecBGetCurrentErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetCurrentErrorThreshold, TEC = TECB };
                EnqueuePriorityCommand(tecBGetCurrentErrorThresholdCommand);
                TECCommand tecBGetVoltageErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetVoltageErrorThreshold, TEC = TECB };
                EnqueuePriorityCommand(tecBGetVoltageErrorThresholdCommand);
                TECCommand tecBGetObjectUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectUpperErrorThreshold, TEC = TECB };
                EnqueuePriorityCommand(tecBGetObjectUpperErrorThresholdCommand);
                TECCommand tecBGetObjectLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectLowerErrorThreshold, TEC = TECB };
                EnqueuePriorityCommand(tecBGetObjectLowerErrorThresholdCommand);
                TECCommand tecBGetSinktUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkUpperErrorThreshold, TEC = TECB };
                EnqueuePriorityCommand(tecBGetSinktUpperErrorThresholdCommand);
                TECCommand tecBGetSinkLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkLowerErrorThreshold, TEC = TECB };
                EnqueuePriorityCommand(tecBGetSinkLowerErrorThresholdCommand);
                TECCommand tecBGetTemperatureIsStableCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureIsStable, TEC = TECB };
                EnqueuePriorityCommand(tecBGetTemperatureIsStableCommand);
                TECCommand tecBGetTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureControl, TEC = TECB };
                EnqueuePriorityCommand(tecBGetTemperatureControlCommand);
                TECCommand tecBGetFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECB };
                EnqueuePriorityCommand(tecBGetFanControlCommand);
                TECCommand tecBGetFanTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECB };
                EnqueuePriorityCommand(tecBGetFanTemperatureCommand);
                TECCommand getFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECB };
                EnqueuePriorityCommand(getFanControlCommand);
                TECCommand getTargetFanTempCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECB };
                EnqueuePriorityCommand(getTargetFanTempCommand);
            }
            if (TECC.Connected)
            {
                TECCommand tecCGetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectTemperature, TEC = TECC };
                EnqueuePriorityCommand(tecCGetObjectTemperatureCommand);
                TECCommand tecCGetSinkTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkTemperature, TEC = TECC };
                EnqueuePriorityCommand(tecCGetSinkTemperatureCommand);
                TECCommand tecCGetTargetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetTargetObjectTemperature, TEC = TECC };
                EnqueuePriorityCommand(tecCGetTargetObjectTemperatureCommand);
                TECCommand tecCGetCurrentErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetCurrentErrorThreshold, TEC = TECC };
                EnqueuePriorityCommand(tecCGetCurrentErrorThresholdCommand);
                TECCommand tecCGetVoltageErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetVoltageErrorThreshold, TEC = TECC };
                EnqueuePriorityCommand(tecCGetVoltageErrorThresholdCommand);
                TECCommand tecCGetObjectUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectUpperErrorThreshold, TEC = TECC };
                EnqueuePriorityCommand(tecCGetObjectUpperErrorThresholdCommand);
                TECCommand tecCGetObjectLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectLowerErrorThreshold, TEC = TECC };
                EnqueuePriorityCommand(tecCGetObjectLowerErrorThresholdCommand);
                TECCommand tecCGetSinktUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkUpperErrorThreshold, TEC = TECC };
                EnqueuePriorityCommand(tecCGetSinktUpperErrorThresholdCommand);
                TECCommand tecCGetSinkLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkLowerErrorThreshold, TEC = TECC };
                EnqueuePriorityCommand(tecCGetSinkLowerErrorThresholdCommand);
                TECCommand tecCGetTemperatureIsStableCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureIsStable, TEC = TECC };
                EnqueuePriorityCommand(tecCGetTemperatureIsStableCommand);
                TECCommand tecCGetTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureControl, TEC = TECB };
                EnqueuePriorityCommand(tecCGetTemperatureControlCommand);
                TECCommand tecCGetFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECC };
                EnqueuePriorityCommand(tecCGetFanControlCommand);
                TECCommand tecCGetFanTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECB };
                EnqueuePriorityCommand(tecCGetFanTemperatureCommand);
                TECCommand getFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECB };
                EnqueuePriorityCommand(getFanControlCommand);
                TECCommand getTargetFanTempCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECB };
                EnqueuePriorityCommand(getTargetFanTempCommand);
            }
            if (TECD.Connected)
            {
                TECCommand tecDGetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectTemperature, TEC = TECD };
                EnqueuePriorityCommand(tecDGetObjectTemperatureCommand);
                TECCommand tecDGetSinkTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkTemperature, TEC = TECD };
                EnqueuePriorityCommand(tecDGetSinkTemperatureCommand);
                TECCommand tecDGetTargetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetTargetObjectTemperature, TEC = TECD };
                EnqueuePriorityCommand(tecDGetTargetObjectTemperatureCommand);
                TECCommand tecDGetCurrentErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetCurrentErrorThreshold, TEC = TECD };
                EnqueuePriorityCommand(tecDGetCurrentErrorThresholdCommand);
                TECCommand tecDGetVoltageErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetVoltageErrorThreshold, TEC = TECD };
                EnqueuePriorityCommand(tecDGetVoltageErrorThresholdCommand);
                TECCommand tecDGetObjectUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectUpperErrorThreshold, TEC = TECD };
                EnqueuePriorityCommand(tecDGetObjectUpperErrorThresholdCommand);
                TECCommand tecDGetObjectLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectLowerErrorThreshold, TEC = TECD };
                EnqueuePriorityCommand(tecDGetObjectLowerErrorThresholdCommand);
                TECCommand tecDGetSinktUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkUpperErrorThreshold, TEC = TECD };
                EnqueuePriorityCommand(tecDGetSinktUpperErrorThresholdCommand);
                TECCommand tecDGetSinkLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkLowerErrorThreshold, TEC = TECD };
                EnqueuePriorityCommand(tecDGetSinkLowerErrorThresholdCommand);
                TECCommand tecDGgetTemperatureIsStableCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureIsStable, TEC = TECD };
                EnqueuePriorityCommand(tecDGgetTemperatureIsStableCommand);
                TECCommand tecDGetTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.GetTemperatureControl, TEC = TECB };
                EnqueuePriorityCommand(tecDGetTemperatureControlCommand);
                TECCommand tecDGetFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECD };
                EnqueuePriorityCommand(tecDGetFanControlCommand);
                TECCommand tecDGetFanTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECB };
                EnqueuePriorityCommand(tecDGetFanTemperatureCommand);
                TECCommand getFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = TECB };
                EnqueuePriorityCommand(getFanControlCommand);
                TECCommand getTargetFanTempCommand = new TECCommand { Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = TECB };
                EnqueuePriorityCommand(getTargetFanTempCommand);
            }
        }

        /// <summary>
        /// Run Task for a TEC controlled by the TEC Manager
        /// </summary>
        /// <param name="protocol">protocol to be run on the TEC</param>
        /// <param name="tec">tec to run the prtocol</param>
        /// <param name="dataGridView">DataGridView to be updated, showing protocol status</param>
        /// <param name="configuration">Configuration for the instrument</param>
        /// <param name="cancellationToken">Cancellation token for killing the run</param>
        /// <returns></returns>
        public async Task Run(ThermocyclingProtocol protocol, TEC tec, DataGridView dataGridView, Configuration configuration, CancellationToken cancellationToken)
        {
            DataGridViewManager dataGridViewManager = new DataGridViewManager();
            DateTime startDateTime = DateTime.Now;
            tec.RunningProtocol = true;
            try
            {
                // Set the protocol name in the data grid view for this tec
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol", protocol.Name);
                // Get the amount of time the protocol is estimated to take
                int estimatedProtocolTimeInSeconds = int.Parse(protocol.GetTimeInSeconds().ToString());
                TimeSpan estimatedProtocolTimeSpan = TimeSpan.FromSeconds(estimatedProtocolTimeInSeconds);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Estimated Time Left", estimatedProtocolTimeSpan.ToString(@"hh\:mm\:ss"));
                // TODO: Set the cell for the Protocol Running to MediumSeaGreen
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol Running", "Yes");
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "IO", "Running");
                // TODO: Set the tec*ProtocolRunning bool variable to true
                while (!cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    int stepIndex = 1;
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Cycle Number", "NA");
                    foreach (var step in protocol.Steps)
                    {
                        // Get the step type
                        string stepType = step.TypeName;
                        // Set the step number
                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Step", (stepIndex).ToString());
                        stepIndex++;
                        if (stepType == ThermocyclingProtocolStepType.Set)
                        {
                            // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                            TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tec, Parameter = step.Temperature };
                            EnqueuePriorityCommand(setObjectTemperatureCommand);
                            // TODO: Ensure that the temperature is actually changed
                            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Target Temp (\u00B0C)", step.Temperature.ToString());
                            // Get the step time and units
                            int stepTime = int.Parse(step.Time.ToString());
                            string stepTimeUnits = step.TimeUnits;
                            TimeUnit timeUnit = new TimeUnit();
                            int stepTimeInMilliseconds = timeUnit.ConvertTimeToMilliseconds(stepTime, stepTimeUnits);
                            await Task.Delay(stepTimeInMilliseconds, cancellationToken);
                        }
                        else if (stepType == ThermocyclingProtocolStepType.Hold)
                        {
                            // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                            TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tec, Parameter = step.Temperature };
                            EnqueuePriorityCommand(setObjectTemperatureCommand);
                            // TODO: Ensure that the temperature is actually changed
                            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Target Temp (\u00B0C)", step.Temperature.ToString());
                        }
                        else if (stepType == ThermocyclingProtocolStepType.GoTo)
                        {
                            // Get the step number to go to
                            int stepNumber = step.StepNumber;
                            // Get the cycle count
                            int cycleCount = step.CycleCount;
                            // Get the steps between this GoTo step and the Step Number the GoTo step needs
                            List<ThermocyclingProtocolStep> cycleSteps = protocol.GetStepsBetween(stepNumber, step.Index-1);
                            // Cycle 
                            for (int i = 0; i < cycleCount; i++)
                            {
                                foreach (ThermocyclingProtocolStep cycleStep in cycleSteps)
                                {
                                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Cycle Number", $"{i+1}/{cycleCount}");
                                    if (cycleStep.TypeName == ThermocyclingProtocolStepType.Set)
                                    {
                                        // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                                        TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tec, Parameter = cycleStep.Temperature };
                                        EnqueuePriorityCommand(setObjectTemperatureCommand);
                                        // TODO: Ensure that the temperature is actually changed
                                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Target Temp (\u00B0C)", cycleStep.Temperature.ToString());
                                        // Get the step time and units
                                        int stepTime = int.Parse(cycleStep.Time.ToString());
                                        string stepTimeUnits = cycleStep.TimeUnits;
                                        TimeUnit timeUnit = new TimeUnit();
                                        int stepTimeInMilliseconds = timeUnit.ConvertTimeToMilliseconds(stepTime, stepTimeUnits);
                                        await Task.Delay(stepTimeInMilliseconds, cancellationToken);
                                    }
                                }
                            }
                        } 
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol Running", "No");
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "IO", "Cancelled");
                MessageBox.Show($"Run has been cancelled on {tec.Name}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tec.RunningProtocol = false;
                // TODO: Generate a report
                return;
            }
            // Set the Running Protocol parameter for the TEC
            tec.RunningProtocol = false;
            // TODO: Generate a report
            DateTime endDateTime = DateTime.Now;
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol Running", "No");
            ReportManager reportManager = new ReportManager(configuration.ReportsDataPath 
                + $"{tec.Name.Replace(" ","")}_TC_Report.pdf");
            await reportManager.AddHeaderLogoAsync(configuration.ReportLogoDataPath, 50, 50);
            await reportManager.AddMainTitleAsync($"Thermocycling Run on {tec.Name}");
            await reportManager.AddParagraphTextAsync($"This is a report automatically generated after a run on {tec.Name}." +
                $" The contents of this report include when the run started and ended, which protocol was used, plots for the expected temperature, actual temperature (where actual here" +
                $" means the temperature measured by the internal temperature sensor for {tec.Name}), the sink temperature, and when the fans were on for {tec.Name}. Extra information" +
                $" regarding the cartridge used, the pressure applied to said cartridge, the elastomer/bergquist used if at all, images before/during/after, and positions for the clamp" +
                $" and tray are not included within this report.");
            await reportManager.AddSubSectionAsync("Protocol Status", "This section includes the protocol statuses for all TECs at the end of this run.");
            await reportManager.AddTableAsync(dataGridView);
            // Create the data plot for the TEC Actual and Target Object Temperatures
            await reportManager.AddPlotAsync(tec.ObjectTemperatures, tec.TargetTemperatures, $"{tec.Name} Object and Target Temperatures", "T (\u00B0C)", "Object Temp", "Target Temp");
            // Create the data plot for the TEC Sink Temperature
            await reportManager.AddPlotAsync(tec.SinkTemperatures, $"{tec.Name} Sink Temperatures", "T (\u00B0C)");
            // Create the data plot for the TEC Fan Speeds
            await reportManager.AddPlotAsync(tec.FanSpeeds, $"{tec.Name} Fan Speeds", "T (rpm)");
            await reportManager.CloseAsync();
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "IO", "Complete");
        }

        /// <summary>
        /// Handles the Run Click Event on the Thermocycling Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="dataGridView"></param>
        /// <param name="protocol"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public async Task HandleRunClickEvent(object sender, EventArgs e, DataGridView dataGridView, ThermocyclingProtocol protocol, Configuration configuration)
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
                await Run(protocol, tec, dataGridView, configuration, tecCancellationTokenSources[_cts_idx].Token);
            }
        }

        /// <summary>
        /// Handles the Click of the Kill button on the Thermocycling tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="dataGridView"></param>
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
                string tecName = "TEC " + button.Name[16].ToString();
                // Cancel
                if (tecCancellationTokenSources[_cts_idx] != null)
                {
                    tecCancellationTokenSources[_cts_idx].Cancel();
                    // Set the Running Protocol Parameter for the TEC
                    TEC tec = GetTECFromName(tecName);
                    tec.RunningProtocol = false;
                    // TODO: Set the Protocol Running cell background color to Yellow for a certain amount of time or indefinitely
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tecName, "Protocol Running", "No");
                    // TODO: Generate a report based on what occured
                }
            }
        }

        /// <summary>
        /// Get the TEC instance from the name
        /// </summary>
        /// <param name="name">Name of the TEC</param>
        /// <returns></returns>
        public TEC? GetTECFromName(string name)
        {
            if (TECA.Name == name)
            {
                return TECA;
            }
            else if (TECB.Name == name)
            {
                return TECB;
            }
            else if (TECC.Name == name)
            {
                return TECC;
            }
            else if (TECD.Name == name)
            {
                return TECD;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Handle the Fast Tick Events (attributes to be checked and updated each Fast Tick)
        /// </summary>
        /// <param name="dataGridViewManager"></param>
        /// <param name="homeTECsDataGridView"></param>
        /// <param name="controlTECsDataGridView"></param>
        public void HandleFastTickEvent(DataGridViewManager dataGridViewManager, DataGridView homeTECsDataGridView, DataGridView controlTECsDataGridView, DataGridView thermocyclingProtocolStatusesDataGridView)
        {
            // TECA
            if (TECA.Connected)
            {
                HandleTECFastTickEvent(TECA, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView);
            }
            // TECB
            if (TECB.Connected)
            {
                HandleTECFastTickEvent(TECB, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView);
            }
            // TECC
            if (TECC.Connected)
            {
                HandleTECFastTickEvent(TECC, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView);
            }
            // TECD
            if (TECD.Connected)
            {
                HandleTECFastTickEvent(TECD, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView);
            }
        }

        /// <summary>
        /// Handle a single TEC's Fast Tick Event
        /// </summary>
        /// <param name="tec"></param>
        /// <param name="dataGridViewManager"></param>
        /// <param name="homeTECsDataGridView"></param>
        /// <param name="controlTECsDataGridView"></param>
        private void HandleTECFastTickEvent(TEC tec, DataGridViewManager dataGridViewManager, DataGridView homeTECsDataGridView, DataGridView controlTECsDataGridView, DataGridView thermocyclingProtocolStatusesDataGridView)
        {
            //
            // Get the current time
            //
            DateTime now = DateTime.Now;
            //
            // Setup and Send TEC Commands
            //
            // Setup the CheckConnection command and add it to the TECsManager Queue
            TECCommand checkConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = tec };
            EnqueueCommand(checkConnectionCommand);
            // Setup the GetObjectTemperature command and add it to the TECsManager Queue
            TECCommand getObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectTemperature, TEC = tec };
            EnqueueCommand(getObjectTemperatureCommand);
            // Setup the GetSinkTemperature command and add it to the TECsManager Queue
            TECCommand getSinkTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkTemperature, TEC = tec };
            EnqueueCommand(getSinkTemperatureCommand);
            // Setup the GetTargetObjectTemperature command and add it to the TECsManager Queue
            TECCommand getTargetObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.GetTargetObjectTemperature, TEC = tec };
            EnqueueCommand(getTargetObjectTemperatureCommand);
            // Setup the GetActualFanSpeed command and add it to the TECsManager Queue
            TECCommand getActualFanSpeedCommand = new TECCommand { Type = TECCommand.CommandType.GetActualFanSpeed, TEC = tec };
            EnqueueCommand(getActualFanSpeedCommand);
            // Setup the GetActualOutputCurrent command and add it to the TECsManager Queue
            TECCommand getActualOutputCurrentCommand = new TECCommand { Type = TECCommand.CommandType.GetActualOutputCurrent, TEC = tec };
            EnqueueCommand(getActualOutputCurrentCommand);
            // Setup the GetActualOutputVoltage command and add it to the TECsManager Queue
            TECCommand getActualOutputVoltageCommand = new TECCommand { Type = TECCommand.CommandType.GetActualOutputVoltage, TEC = tec };
            EnqueueCommand(getActualOutputVoltageCommand);
            // Setup the Temperature enabled command and add it to the TECsManager Queue
            TECCommand checkTemperatureControl = new TECCommand { Type = TECCommand.CommandType.GetTemperatureControl, TEC = tec };
            EnqueueCommand(checkTemperatureControl);
            // Setup the Fan Enabled command and add it to the TECsManager Queue
            TECCommand getFanControlCommand = new TECCommand { Type = TECCommand.CommandType.GetFanControl, TEC = tec };
            EnqueueCommand(getFanControlCommand);
            // Setup the Fan Target Temperature command and add it to the TECsManager Queue
            TECCommand getTargetFanTempCommand = new TECCommand {  Type = TECCommand.CommandType.GetFanTargetTemperature, TEC = tec };
            EnqueueCommand(getTargetFanTempCommand);
            // Setup Command to get the Current Error Threshold and Voltage Error Threshold
            TECCommand getCurrentErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetCurrentErrorThreshold, TEC = tec };
            EnqueueCommand(getCurrentErrorThresholdCommand);
            TECCommand getVoltageErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetVoltageErrorThreshold, TEC = tec };
            EnqueueCommand(getVoltageErrorThresholdCommand);
            // Setup Commands for the Object and Sink upper and lower thresholds
            TECCommand getObjectUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectUpperErrorThreshold, TEC = tec };
            EnqueueCommand(getObjectUpperErrorThresholdCommand);
            TECCommand getObjectLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetObjectLowerErrorThreshold, TEC = tec };
            EnqueueCommand(getObjectLowerErrorThresholdCommand);
            TECCommand getSinkUpperErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkUpperErrorThreshold, TEC = tec };
            EnqueueCommand(getSinkUpperErrorThresholdCommand);
            TECCommand getSinkLowerErrorThresholdCommand = new TECCommand { Type = TECCommand.CommandType.GetSinkLowerErrorThreshold, TEC = tec };
            EnqueueCommand(getSinkLowerErrorThresholdCommand);
            //
            // Set the DataGridValues for the TEC
            //
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeTECsDataGridView, tec.Connected, "Connected", "Not Connected", "State", tec.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlTECsDataGridView, tec.Connected, "Connected", "Not Connected", "State", tec.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(thermocyclingProtocolStatusesDataGridView, tec.Connected, "Connected", "Not Connected", "State", tec.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeTECsDataGridView, tec.TemperatureControlled == "On", "On", "Off", "Temp Enabled", tec.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlTECsDataGridView, tec.TemperatureControlled == "On", "On", "Off", "Temp Enabled", tec.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeTECsDataGridView, tec.FanControlled == "Enabled", "Enabled", "Disabled", "Fan Control", tec.Name);
            if (tec.FanControlChanged())
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlTECsDataGridView, tec.FanControlled == "Enabled", "Enabled", "Disabled", "Fan Control", tec.Name);
            }
            if (double.TryParse(tec.ActualObjectTemperature, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name, 
                    "Actual Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualObjectTemperature)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Actual Object Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualObjectTemperature)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualObjectTemperature)));
                // Store the Actual Object Temperature
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.ActualObjectTemperature));
                tec.ObjectTemperatures.Add(tuple);
            }
            if (double.TryParse(tec.ActualSinkTemperature, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name, 
                    "Sink Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualSinkTemperature)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Sink Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualSinkTemperature)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Sink Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualSinkTemperature)));
                // Store the Actual Sink Temperature
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.ActualSinkTemperature));
                tec.SinkTemperatures.Add(tuple);
            }
            if (double.TryParse(tec.TargetObjectTemperature, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Target Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.TargetObjectTemperature)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Target Object Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.TargetObjectTemperature)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Target Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.TargetObjectTemperature)));
                // Store the Target Object Temperature
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.TargetObjectTemperature));
                tec.TargetTemperatures.Add(tuple);
            }
            if (double.TryParse(tec.ActualFanSpeed, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Fan RPM", String.Format("{0:0.0000}", double.Parse(tec.ActualFanSpeed)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Actual Fan Speed (rpm)", String.Format("{0:0.0000}", double.Parse(tec.ActualFanSpeed)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Fan RPM", String.Format("{0:0.0000}", double.Parse(tec.ActualFanSpeed)));
                // Store the Fan Speeds
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.ActualFanSpeed));
                tec.FanSpeeds.Add(tuple);
            }
            if (double.TryParse(tec.ActualOutputCurrent, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Current (A)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputCurrent)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Current (A)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputCurrent)));
            }
            if (double.TryParse(tec.ActualOutputVoltage, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Voltage (V)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputVoltage)));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Voltage (V)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputVoltage)));
            }         
            if (double.TryParse(tec.ActualObjectTemperature, out double actualTemp) && double.TryParse(tec.TargetObjectTemperature, out double targetTemp))
            {
                if (Math.Abs(tec.PreviousObjectTemperature - double.Parse(tec.ActualObjectTemperature)) < tempReachedCutoff)
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                        "IO", "Idle");
                    dataGridViewManager.SetCellBackColorByColumnAndRowNames(homeTECsDataGridView, Color.White, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                        "IO", "Idle");
                    dataGridViewManager.SetCellBackColorByColumnAndRowNames(controlTECsDataGridView, Color.White, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                        "IO", "Idle");
                    dataGridViewManager.SetCellBackColorByColumnAndRowNames(thermocyclingProtocolStatusesDataGridView, Color.White, tec.Name, "IO");
                }
                else
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                        "IO", "Ramping");
                    dataGridViewManager.SetCellBackColorByColumnAndRowNames(homeTECsDataGridView, Color.LightGoldenrodYellow, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                        "IO", "Ramping");
                    dataGridViewManager.SetCellBackColorByColumnAndRowNames(controlTECsDataGridView, Color.LightGoldenrodYellow, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                        "IO", "Ramping");
                    dataGridViewManager.SetCellBackColorByColumnAndRowNames(thermocyclingProtocolStatusesDataGridView, Color.LightGoldenrodYellow, tec.Name, "IO");
                }
            }
            if (double.TryParse(tec.CurrentErrorThreshold, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Max Current (A)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.CurrentErrorThreshold))));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Max Current (A)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.CurrentErrorThreshold))));
            }
            if (double.TryParse(tec.VoltageErrorThreshold, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Max Voltage (V)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.VoltageErrorThreshold))));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Max Voltage (V)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.VoltageErrorThreshold))));
            }
            if (double.TryParse(tec.FanTargetTemperature, out double fanTargetTemp))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name, 
                    "Fan On Temp (\u00B0C)", String.Format("{0:0.0000}", fanTargetTemp));
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Fan On Temp (\u00B0C)", String.Format("{0:0.0000}", fanTargetTemp));
            }
            if (double.TryParse(tec.ObjectUpperErrorThreshold, out double objectUpperErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Object Upper Error Threshold (\u00B0C)", String.Format("{0:0.0000}", objectUpperErrorThreshold));
            }
            if (double.TryParse(tec.ObjectLowerErrorThreshold, out double objectLowerErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Object Lower Error Threshold (\u00B0C)", String.Format("{0:0.0000}", objectLowerErrorThreshold));
            }
            if (double.TryParse(tec.SinkUpperErrorThreshold, out double sinkUpperErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Sink Upper Error Threshold (\u00B0C)", String.Format("{0:0.0000}", sinkUpperErrorThreshold));
            }
            if (double.TryParse(tec.SinkLowerErrorThreshold, out double sinkLowerErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Sink Lower Error Threshold (\u00B0C)", String.Format("{0:0.0000}", sinkLowerErrorThreshold));
            }
        }
    }
}
