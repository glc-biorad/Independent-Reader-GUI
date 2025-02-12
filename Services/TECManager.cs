﻿using Independent_Reader_GUI.Models;
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
using Independent_Reader_GUI.Models.Consumables;
using Independent_Reader_GUI.Models.Hardware.TEC;
using Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol;
using Independent_Reader_GUI.Models.Units;
using System.Windows.Forms;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage the TEC instances
    /// </summary>
    internal class TECManager
    {
        public TEC TECA;
        public TEC TECB;
        public TEC TECC;
        public TEC TECD;
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
        private Configuration configuration;

        public TECManager(TEC tecA, TEC tecB, TEC tecC, TEC tecD, Configuration configuration)
        {
            TECA = tecA;
            TECB = tecB;
            TECC = tecC;
            TECD = tecD;
            tecCancellationTokenSources.Add(tecARunCancellationTokenSource);
            tecCancellationTokenSources.Add(tecBRunCancellationTokenSource);
            tecCancellationTokenSources.Add(tecCRunCancellationTokenSource);
            tecCancellationTokenSources.Add(tecDRunCancellationTokenSource);
            this.configuration = configuration;
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
                                case TECCommand.CommandType.GetDeviceStatus:
                                    await command.TEC.GetDeviceStatus();
                                    break;
                                case TECCommand.CommandType.GetErrorNumber:
                                    await command.TEC.GetErrorNumber();
                                    break;
                                case TECCommand.CommandType.GetErrorDescription:
                                    await command.TEC.GetErrorDescription();
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
                                case TECCommand.CommandType.GetDeviceStatus:
                                    await command.TEC.GetDeviceStatus();
                                    break;
                                case TECCommand.CommandType.GetErrorNumber:
                                    await command.TEC.GetErrorNumber();
                                    break;
                                case TECCommand.CommandType.GetErrorDescription:
                                    await command.TEC.GetErrorDescription();
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
        /// Turn off TEC Fans based on an outcome
        /// </summary>
        /// <param name="tecAOutcome"></param>
        /// <param name="tecBOutcome"></param>
        /// <param name="tecDOutcome"></param>
        /// <param name="tecDOutcome"></param>
        public void TurnOffFansBasedOnOutcome(bool tecAOutcome, bool tecBOutcome, bool tecCOutcome, bool tecDOutcome)
        {
            if (tecAOutcome)
            {
                TECCommand command = new TECCommand { Type = TECCommand.CommandType.SetFanControl, TEC = TECA, Parameter = "Disabled" };
                EnqueuePriorityCommand(command);
            }
            if (tecBOutcome)
            {
                TECCommand command = new TECCommand { Type = TECCommand.CommandType.SetFanControl, TEC = TECB, Parameter = "Disabled" };
                EnqueuePriorityCommand(command);
            }
            if (tecCOutcome)
            {
                TECCommand command = new TECCommand { Type = TECCommand.CommandType.SetFanControl, TEC = TECC, Parameter = "Disabled" };
                EnqueuePriorityCommand(command);
            }
            if (tecDOutcome)
            {
                TECCommand command = new TECCommand { Type = TECCommand.CommandType.SetFanControl, TEC = TECD, Parameter = "Disabled" };
                EnqueuePriorityCommand(command);
            }
        }

        public void HandleError(TEC tec)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("hh:mm:ss tt");
            string date = now.ToString("MM/dd/yyyy");
            Debug.WriteLine($"{tec.Name} was found in an error state {tec.ErrorMessage} at {time} on {date}, attempting to resolve and this will be logged.");
            // Attempt to reset the device N times
            // TODO: Repalce 5 with a configurable value from the XML
            for (int i = 0; i < 5; i++)
            {
                TECCommand resetCommand = new TECCommand { Type = TECCommand.CommandType.ResetAsync, TEC = tec };
                EnqueuePriorityCommand(resetCommand);
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
        public async Task Run(ThermocyclingProtocol protocol, TEC tec, DataGridView dataGridView, 
            Configuration configuration, Cartridge cartridge, Elastomer elastomer, Bergquist bergquist, CancellationToken cancellationToken)
        {
            DataGridViewManager dataGridViewManager = new DataGridViewManager();
            DateTime startDateTime = DateTime.Now;
            tec.RunningProtocol = true;
            try
            {
                // Set the protocol name in the data grid view for this tec
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol", protocol.Name, Color.White);
                // Get the amount of time the protocol is estimated to take
                int estimatedProtocolTimeInSeconds = int.Parse(protocol.GetTimeInSeconds().ToString());
                TimeSpan estimatedProtocolTimeSpan = TimeSpan.FromSeconds(estimatedProtocolTimeInSeconds);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Estimated Time Left", estimatedProtocolTimeSpan.ToString(@"hh\:mm\:ss"), Color.White);
                // TODO: Set the cell for the Protocol Running to MediumSeaGreen
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol Running", "Yes", Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "IO", "Running", Color.White);
                // TODO: Set the tec*ProtocolRunning bool variable to true
                while (!cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    int stepIndex = 1;
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Cycle Number", "NA", Color.White);
                    foreach (var step in protocol.Steps)
                    {
                        // Get the step type
                        string stepType = step.TypeName;
                        // Set the step number
                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Step", (stepIndex).ToString(), Color.White);
                        stepIndex++;
                        if (stepType == ThermocyclingProtocolStepType.Set)
                        {
                            // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                            TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tec, Parameter = step.Temperature };
                            EnqueuePriorityCommand(setObjectTemperatureCommand);
                            // TODO: Ensure that the temperature is actually changed
                            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Target Temp (\u00B0C)", step.Temperature.ToString(), Color.White);
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
                            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Target Temp (\u00B0C)", step.Temperature.ToString(), Color.White);
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
                                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Cycle Number", $"{i+1}/{cycleCount}", Color.White);
                                    if (cycleStep.TypeName == ThermocyclingProtocolStepType.Set)
                                    {
                                        // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                                        TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tec, Parameter = cycleStep.Temperature };
                                        EnqueuePriorityCommand(setObjectTemperatureCommand);
                                        // TODO: Ensure that the temperature is actually changed
                                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Target Temp (\u00B0C)", cycleStep.Temperature.ToString(), Color.White);
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
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol Running", "No", Color.MediumSeaGreen);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "IO", "Cancelled", Color.Yellow);
                MessageBox.Show($"Run has been cancelled on {tec.Name}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tec.RunningProtocol = false;
                // TODO: Generate a report
                return;
            }
            // Set the Running Protocol parameter for the TEC
            tec.RunningProtocol = false;
            // TODO: Generate a report
            DateTime endDateTime = DateTime.Now;
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "Protocol Running", "No", Color.White);
            await GenerateCompletedRunReport(tec, protocol, cartridge, elastomer, bergquist, dataGridView);
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tec.Name, "IO", "Complete", Color.MediumSeaGreen);
        }

        private async Task GenerateCompletedRunReport(TEC tec, ThermocyclingProtocol protocol, Cartridge cartridge, Elastomer elastomer, Bergquist bergquist, DataGridView dataGridView)
        {
            ReportManager reportManager = new ReportManager(configuration.ReportsDataPath
                + $"{tec.Name.Replace(" ", "")}_TC_Report.pdf");
            await reportManager.AddHeaderLogoAsync(configuration.ReportLogoDataPath, 50, 50);
            await reportManager.AddMainTitleAsync($"Thermocycling Run on {tec.Name} (COMPLETED)");
            await reportManager.AddParagraphTextAsync($"This is a report automatically generated after a completed run on {tec.Name}." +
                $" The contents of this report include when the run started and ended, which protocol was used, plots for the expected temperature, actual temperature (where actual here" +
                $" means the temperature measured by the internal temperature sensor for {tec.Name}), the sink temperature, and when the fans were on for {tec.Name}. Extra information" +
                $" regarding the cartridge used, the pressure applied to said cartridge, the elastomer/bergquist used if at all, images before/during/after, and positions for the clamp" +
                $" and tray are not included within this report.");
            await reportManager.AddSubSectionAsync("Run Data", "This section includes the initial conditions and run parameters.");
            // Setup the run data table
            List<string> runDataColumnHeaders = new List<string>
            {
                "",
                "Value",
            };
            List<string> protocolNameRow = new List<string> { "Protocol", protocol.Name };
            List<string> estimateRunTimeRow = new List<string> { "Estimated Run Time", TimeSpan.FromSeconds(int.Parse(protocol.GetTimeInSeconds().ToString())).ToString(@"hh\:mm\:ss") };
            List<string> startDateRow = new List<string> { "Date", DateTime.Now.ToString("MM/dd/yyyy") };
            List<string> startTimeRow = new List<string> { "Date", DateTime.Now.ToString("hh:mm:ss tt") };
            List<string> cartridgeRow = new List<string> { "Cartridge", cartridge.Name };
            List<string> elastomerRow = new List<string> { "Elastomer", elastomer.Name };
            List<string> bergquistRow = new List<string> { "Bergquist", bergquist.Name };
            List<string> initialSinkTempRow = new List<string> { "Initial Sink Temp (\u00B0C)", "" };
            List<string> initialObjectTempRow = new List<string> { "Initial Object Temp (\u00B0C)", "" };
            List<List<string>> rows = new List<List<string>>
            {
                protocolNameRow, estimateRunTimeRow, startDateRow, startTimeRow, cartridgeRow, elastomerRow, bergquistRow, initialSinkTempRow, initialObjectTempRow
            };
            await reportManager.AddTableAsync(runDataColumnHeaders, rows);
            await reportManager.AddSubSectionAsync("Protocol Status", "This section includes the protocol statuses for all TECs at the end of this run.");
            await reportManager.AddTableAsync(dataGridView);
            // Create the data plot for the TEC Actual and Target Object Temperatures
            await reportManager.AddPlotAsync(tec.ObjectTemperatures, tec.TargetTemperatures, $"{tec.Name} Object and Target Temperatures", "T (\u00B0C)", "Object Temp", "Target Temp");
            // Create the data plot for the TEC Sink Temperature
            await reportManager.AddPlotAsync(tec.SinkTemperatures, $"{tec.Name} Sink Temperatures", "T (\u00B0C)");
            // Create the data plot for the TEC Fan Speeds
            await reportManager.AddPlotAsync(tec.FanSpeeds, $"{tec.Name} Fan Speeds", "T (rpm)");
            await reportManager.CloseAsync();
        }

        private async Task GenerateKilledRunReport(TEC tec)
        {
            ReportManager reportManager = new ReportManager(configuration.ReportsDataPath
                + $"{tec.Name.Replace(" ", "")}_TC_Report.pdf");
            await reportManager.AddHeaderLogoAsync(configuration.ReportLogoDataPath, 50, 50);
            await reportManager.AddMainTitleAsync($"Thermocycling Run on {tec.Name} (KILLED)");
            await reportManager.AddParagraphTextAsync($"This is a report automatically generated after killing the run on {tec.Name}." +
                $" The contents of this report include when the run started and ended, which protocol was used, plots for the expected temperature, actual temperature (where actual here" +
                $" means the temperature measured by the internal temperature sensor for {tec.Name}), the sink temperature, and when the fans were on for {tec.Name}. Extra information" +
                $" regarding the cartridge used, the pressure applied to said cartridge, the elastomer/bergquist used if at all, images before/during/after, and positions for the clamp" +
                $" and tray are not included within this report.");
            // Create the data plot for the TEC Actual and Target Object Temperatures
            await reportManager.AddPlotAsync(tec.ObjectTemperatures, tec.TargetTemperatures, $"{tec.Name} Object and Target Temperatures", "T (\u00B0C)", "Object Temp", "Target Temp");
            // Create the data plot for the TEC Sink Temperature
            await reportManager.AddPlotAsync(tec.SinkTemperatures, $"{tec.Name} Sink Temperatures", "T (\u00B0C)");
            // Create the data plot for the TEC Fan Speeds
            await reportManager.AddPlotAsync(tec.FanSpeeds, $"{tec.Name} Fan Speeds", "T (rpm)");
            await reportManager.CloseAsync();
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
        public async Task HandleRunClickEvent(object sender, EventArgs e, DataGridView dataGridView, 
            ThermocyclingProtocol protocol, Configuration configuration, Cartridge cartridge, Elastomer elastomer, Bergquist bergquist)
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
                await Run(protocol, tec, dataGridView, configuration, cartridge, elastomer, bergquist, tecCancellationTokenSources[_cts_idx].Token);
            }
        }

        /// <summary>
        /// Handles the Click of the Kill button on the Thermocycling tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="dataGridView"></param>
        public async void HandleKillClickEvent(object sender, EventArgs e, DataGridView dataGridView)
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
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(dataGridView, tecName, "Protocol Running", "Cancelled", Color.Yellow);
                    // TODO: Generate a report based on what occured
                    await GenerateKilledRunReport(tec);
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
        public void HandleFastTickEvent(DataGridViewManager 
            dataGridViewManager, DataGridView homeTECsDataGridView, 
            DataGridView controlTECsDataGridView, DataGridView thermocyclingProtocolStatusesDataGridView, DataGridView mainLogErrorDataGridView)
        {            
            // TECA
            if (TECA.Connected)
            {
                HandleTECFastTickEvent(TECA, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
            else
            {
                HandleTECFastTickEventOnDisconnect(TECD, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
            // TECB
            if (TECB.Connected)
            {
                HandleTECFastTickEvent(TECB, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
            else
            {
                HandleTECFastTickEventOnDisconnect(TECD, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
            // TECC
            if (TECC.Connected)
            {
                HandleTECFastTickEvent(TECC, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
            else
            {
                HandleTECFastTickEventOnDisconnect(TECD, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
            // TECD
            if (TECD.Connected)
            {
                HandleTECFastTickEvent(TECD, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
            else
            {
                HandleTECFastTickEventOnDisconnect(TECD, dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorDataGridView);
            }
        }

        /// <summary>
        /// Handle the Fast Tick Event for a TEC when it is not connected
        /// </summary>
        /// <param name="tec"></param>
        /// <param name="dataGridViewManager"></param>
        /// <param name="homeTECsDataGridView"></param>
        /// <param name="controlTECsDataGridView"></param>
        /// <param name="thermocyclingProtocolStatusesDataGridView"></param>
        private void HandleTECFastTickEventOnDisconnect(TEC tec, 
            DataGridViewManager dataGridViewManager, DataGridView homeTECsDataGridView, 
            DataGridView controlTECsDataGridView, DataGridView thermocyclingProtocolStatusesDataGridView, DataGridView mainLogErrorDataGridView)
        {
            // Setup the CheckConnection command and add it to the TECsManager Queue
            TECCommand checkConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = tec };
            EnqueueCommand(checkConnectionCommand);
            // Fill the DataGridViews
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeTECsDataGridView, tec.Connected, "Connected", "Not Connected", "State", tec.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlTECsDataGridView, tec.Connected, "Connected", "Not Connected", "State", tec.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(thermocyclingProtocolStatusesDataGridView, tec.Connected, "Connected", "Not Connected", "State", tec.Name);
        }

        /// <summary>
        /// Handle a single TEC's Fast Tick Event
        /// </summary>
        /// <param name="tec"></param>
        /// <param name="dataGridViewManager"></param>
        /// <param name="homeTECsDataGridView"></param>
        /// <param name="controlTECsDataGridView"></param>
        private void HandleTECFastTickEvent(TEC tec, 
            DataGridViewManager dataGridViewManager, DataGridView homeTECsDataGridView, 
            DataGridView controlTECsDataGridView, DataGridView thermocyclingProtocolStatusesDataGridView, DataGridView mainLogErrorDataGridView)
        {
            //
            // Get the current time
            //
            DateTime now = DateTime.Now;
            //
            //
            // Check the Device Status
            //
            TECCommand deviceStatusCommand = new TECCommand { Type = TECCommand.CommandType.GetDeviceStatus, TEC = tec };
            EnqueuePriorityCommand(deviceStatusCommand);
            if (tec.DeviceStatus == "Error")
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                        "IO", "Error", Color.Red);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "IO", "Error", Color.Red);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "IO", "Error", Color.Red);
                HandleError(tec);
            }
            // 
            // Get Error Descriptions and log if in an error state
            //
            if (tec.InErrorState)
            {
                TECCommand getErrorDescriptionCommand = new TECCommand { Type = TECCommand.CommandType.GetErrorDescription, TEC = tec };
                EnqueuePriorityCommand(getErrorDescriptionCommand);
                TECCommand getErrorNumberCommand = new TECCommand { Type = TECCommand.CommandType.GetErrorNumber, TEC = tec };
                EnqueuePriorityCommand(getErrorNumberCommand);
            }
            // Log any non-error descriptions
            if (tec.ErrorDescription != "No Error" && tec.ErrorNumber != 0)
            {
                mainLogErrorDataGridView.Rows.Add(tec.ParentModule, tec.Name, $"{tec.ErrorNumber}: {tec.ErrorDescription}", now.ToString("hh:mm:ss tt"), now.ToString("MM/dd/yyyy"), "glc-biorad");
                Debug.WriteLine($"Error: {tec.ErrorNumber}: {tec.ErrorDescription}");
            }
            // Setup and Send TEC Commands
            //
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
                    "Actual Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualObjectTemperature)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Actual Object Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualObjectTemperature)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualObjectTemperature)), Color.White);
                // Store the Actual Object Temperature
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.ActualObjectTemperature));
                tec.ObjectTemperatures.Add(tuple);
            }
            if (double.TryParse(tec.ActualSinkTemperature, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name, 
                    "Sink Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualSinkTemperature)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Sink Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualSinkTemperature)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Sink Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.ActualSinkTemperature)), Color.White);
                // Warn the user if the Sink Value is getting too close to the upper or lower error thresholds
                if (double.TryParse(tec.ActualSinkTemperature, out double sinkTemp) && double.TryParse(tec.SinkUpperErrorThreshold, out double sueThreshold))
                {
                    if (sinkTemp >= sueThreshold - 20)
                    {
                        dataGridViewManager.SetCellBackColorByColumnAndRowNames(controlTECsDataGridView, Color.Yellow, tec.Name, "Sink Temp (\u00B0C)");
                        //dataGridViewManager.SetCellBackColorByColumnAndRowNamesBasedOnColor(controlTECsDataGridView, Color.Yellow, Color.LightGoldenrodYellow, tec.Name, "Sink Temp (\u00B0C)");
                    }
                    else
                    {
                        dataGridViewManager.SetCellBackColorByColumnAndRowNames(controlTECsDataGridView, Color.White, tec.Name, "Sink Temp (\u00B0C)");
                    }
                }               
                // Store the Actual Sink Temperature
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.ActualSinkTemperature));
                tec.SinkTemperatures.Add(tuple);
            }
            if (double.TryParse(tec.TargetObjectTemperature, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Target Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.TargetObjectTemperature)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Target Object Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.TargetObjectTemperature)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Target Temp (\u00B0C)", String.Format("{0:0.0000}", double.Parse(tec.TargetObjectTemperature)), Color.White);
                // Store the Target Object Temperature
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.TargetObjectTemperature));
                tec.TargetTemperatures.Add(tuple);
            }
            if (double.TryParse(tec.ActualFanSpeed, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Fan RPM", String.Format("{0:0.0000}", double.Parse(tec.ActualFanSpeed)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Actual Fan Speed (rpm)", String.Format("{0:0.0000}", double.Parse(tec.ActualFanSpeed)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                    "Fan RPM", String.Format("{0:0.0000}", double.Parse(tec.ActualFanSpeed)), Color.White);
                // Store the Fan Speeds
                Tuple<DateTime, double> tuple = new Tuple<DateTime, double>(now, double.Parse(tec.ActualFanSpeed));
                tec.FanSpeeds.Add(tuple);
            }
            if (double.TryParse(tec.ActualOutputCurrent, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Current (A)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputCurrent)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Current (A)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputCurrent)), Color.White);
            }
            if (double.TryParse(tec.ActualOutputVoltage, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Voltage (V)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputVoltage)), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Voltage (V)", String.Format("{0:0.0000}", -1 * double.Parse(tec.ActualOutputVoltage)), Color.White);
            }         
            if (double.TryParse(tec.ActualObjectTemperature, out double actualTemp) && double.TryParse(tec.TargetObjectTemperature, out double targetTemp))
            {
                if (Math.Abs(tec.PreviousObjectTemperature - double.Parse(tec.ActualObjectTemperature)) < tempReachedCutoff)
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                        "IO", "Idle", Color.White);
                    //dataGridViewManager.SetCellBackColorByColumnAndRowNames(homeTECsDataGridView, Color.White, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                        "IO", "Idle", Color.White);
                    //dataGridViewManager.SetCellBackColorByColumnAndRowNames(controlTECsDataGridView, Color.White, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                        "IO", "Idle", Color.White);
                    //dataGridViewManager.SetCellBackColorByColumnAndRowNames(thermocyclingProtocolStatusesDataGridView, Color.White, tec.Name, "IO");
                }
                else
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                        "IO", "Ramping", Color.LightGoldenrodYellow);
                    //dataGridViewManager.SetCellBackColorByColumnAndRowNames(homeTECsDataGridView, Color.LightGoldenrodYellow, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                        "IO", "Ramping", Color.LightGoldenrodYellow);
                    //dataGridViewManager.SetCellBackColorByColumnAndRowNames(controlTECsDataGridView, Color.LightGoldenrodYellow, tec.Name, "IO");
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView, tec.Name,
                        "IO", "Ramping", Color.LightGoldenrodYellow);
                    //dataGridViewManager.SetCellBackColorByColumnAndRowNames(thermocyclingProtocolStatusesDataGridView, Color.LightGoldenrodYellow, tec.Name, "IO");
                }
            }
            if (double.TryParse(tec.CurrentErrorThreshold, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Max Current (A)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.CurrentErrorThreshold))), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Max Current (A)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.CurrentErrorThreshold))), Color.White);
            }
            if (double.TryParse(tec.VoltageErrorThreshold, out _))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Max Voltage (V)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.VoltageErrorThreshold))), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Max Voltage (V)", String.Format("{0:0.0000}", Math.Abs(double.Parse(tec.VoltageErrorThreshold))), Color.White);
            }
            if (double.TryParse(tec.FanTargetTemperature, out double fanTargetTemp))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name, 
                    "Fan On Temp (\u00B0C)", String.Format("{0:0.0000}", fanTargetTemp), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Fan On Temp (\u00B0C)", String.Format("{0:0.0000}", fanTargetTemp), Color.White);
            }
            if (double.TryParse(tec.ObjectUpperErrorThreshold, out double objectUpperErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Object Upper Error Threshold (\u00B0C)", String.Format("{0:0.0000}", objectUpperErrorThreshold), Color.White);
            }
            if (double.TryParse(tec.ObjectLowerErrorThreshold, out double objectLowerErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Object Lower Error Threshold (\u00B0C)", String.Format("{0:0.0000}", objectLowerErrorThreshold), Color.White);
            }
            if (double.TryParse(tec.SinkUpperErrorThreshold, out double sinkUpperErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Sink Upper Error Threshold (\u00B0C)", String.Format("{0:0.0000}", sinkUpperErrorThreshold), Color.White);
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, tec.Name,
                    "Sink Temp Max (\u00B0C)", String.Format("{0:0.0000}", sinkUpperErrorThreshold), Color.White);
            }
            if (double.TryParse(tec.SinkLowerErrorThreshold, out double sinkLowerErrorThreshold))
            {
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlTECsDataGridView, tec.Name,
                    "Sink Lower Error Threshold (\u00B0C)", String.Format("{0:0.0000}", sinkLowerErrorThreshold), Color.White);
            }
        }
    }
}
