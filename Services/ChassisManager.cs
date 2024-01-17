using Independent_Reader_GUI.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class ChassisManager
    {
        Chassis chassis;
        ConcurrentQueue<ChassisCommand> commandQueue = new ConcurrentQueue<ChassisCommand>();
        private CancellationTokenSource commandQueueCancellationTokenSource = new CancellationTokenSource();
        private int msDelay = 5;

        public ChassisManager(Chassis chassis)
        {
            this.chassis = chassis;
        }

        /// <summary>
        /// Enqueue commands to the Chassis Manager
        /// </summary>
        /// <param name="command"></param>
        public void EnqueueCommand(ChassisCommand command)
        {
            commandQueue.Enqueue(command);
        }

        /// <summary>
        /// Cancel the CommandQueue Processing
        /// </summary>
        public void CloseCommandQueueProcessing()
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
                if (commandQueue.TryDequeue(out ChassisCommand command))
                {
                    try
                    {
                        switch (command.Type)
                        {
                            case ChassisCommand.CommandType.SetPowerRelayState:
                                await command.Chassis.SetPowerRelayStatus(command.ID, command.RelayState);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Unable to handle chassis command due to {ex.Message}");
                        // Note: Might need to requeue this command
                        EnqueueCommand(command);
                    }
                }
                await Task.Delay(msDelay);
            }
        }
    }
}
