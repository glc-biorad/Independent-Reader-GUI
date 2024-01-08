using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage the Motor instances
    /// </summary>
    internal class MotorsManager
    {
        private List<Motor> motors = new List<Motor>();
        ConcurrentQueue<MotorCommand> commandQueue = new ConcurrentQueue<MotorCommand>();
        ConcurrentQueue<MotorCommand> priorityCommandQueue = new ConcurrentQueue<MotorCommand>();
        private CancellationTokenSource commandQueueCancellationTokenSource = new CancellationTokenSource();

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

        public async Task InitializeMotorParameters()
        {
            foreach (Motor motor in motors)
            {
                MotorCommand checkConnectionCommand = new MotorCommand() { Type = MotorCommand.CommandType.CheckConnectionAsync, Motor = motor };
                EnqueuePriorityCommand(checkConnectionCommand);
                Debug.WriteLine($"Motor Connection: {motor.Connected}");
            }
        }

        /// <summary>
        /// Enqueue commands to the Motor Manager
        /// </summary>
        /// <param name="command"></param>
        public void EnqueueCommand(MotorCommand command)
        {
            commandQueue.Enqueue(command);
        }

        /// <summary>
        /// Enqueue priority commands to the Motor Manager
        /// </summary>
        /// <param name="command"></param>
        public void EnqueuePriorityCommand(MotorCommand command)
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
                    if (commandQueue.TryDequeue(out MotorCommand command))
                    {
                        try
                        {
                            switch (command.Type)
                            {
                                case MotorCommand.CommandType.MoveAsync:
                                    await command.Motor.MoveAsync(command.PositionParameter, command.SpeedParameter);
                                    break;
                                case MotorCommand.CommandType.MoveRelativeAsync:
                                    await command.Motor.MoveRelativeAsync(command.DistanceParameter, command.SpeedParameter);
                                    break;
                                case MotorCommand.CommandType.HomeAsync:
                                    await command.Motor.HomeAsync();
                                    break;
                                case MotorCommand.CommandType.GetVersionAsync:
                                    await command.Motor.GetVersionAsync();
                                    break;
                                case MotorCommand.CommandType.GetPositionAsync:
                                    await command.Motor.GetPositionAsync();
                                    break;
                                case MotorCommand.CommandType.CheckConnectionAsync:
                                    await command.Motor.CheckConnectionAsync();
                                    break;
                                case MotorCommand.CommandType.CheckIfHomingComplete:
                                    await command.Motor.CheckIfHomingComplete();
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Unable to handle {command.Motor.Name} priority command due to {ex.Message}");
                            // Note: Might need to requeue this command as a priority command
                            EnqueuePriorityCommand(command);
                        }
                    }
                    await Task.Delay(200);
                }
                else
                {
                    if (priorityCommandQueue.TryDequeue(out MotorCommand command))
                    {
                        try
                        {
                            switch (command.Type)
                            {
                                case MotorCommand.CommandType.MoveAsync:
                                    await command.Motor.MoveAsync(command.PositionParameter, command.SpeedParameter);
                                    break;
                                case MotorCommand.CommandType.MoveRelativeAsync:
                                    await command.Motor.MoveRelativeAsync(command.DistanceParameter, command.SpeedParameter);
                                    break;
                                case MotorCommand.CommandType.HomeAsync:
                                    await command.Motor.HomeAsync();
                                    break;
                                case MotorCommand.CommandType.GetVersionAsync:
                                    await command.Motor.GetVersionAsync();
                                    break;
                                case MotorCommand.CommandType.GetPositionAsync:
                                    await command.Motor.GetPositionAsync();
                                    break;
                                case MotorCommand.CommandType.CheckConnectionAsync:
                                    await command.Motor.CheckConnectionAsync();
                                    break;
                                case MotorCommand.CommandType.CheckIfHomingComplete:
                                    await command.Motor.CheckIfHomingComplete();
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Unable to handle {command.Motor.Name} priority command due to {ex.Message}");
                            // Note: Might need to requeue this command as a priority command
                            EnqueuePriorityCommand(command);
                        }
                    }
                    await Task.Delay(200);
                }
            }
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

        public void HandleFastTickEvent(DataGridViewManager dataGridViewManager, DataGridView homeMotorsDataGridView, DataGridView controlMotorsDataGridView)
        {
            foreach (Motor motor in motors)
            {
                HandleMotorFastTickEvent(motor, dataGridViewManager, homeMotorsDataGridView, controlMotorsDataGridView);
            }
        }

        private void HandleMotorFastTickEvent(Motor motor, DataGridViewManager dataGridViewManager, DataGridView homeMotorsDataGridView, DataGridView controlMotorsDataGridView)
        {
            //
            // Setup and Send the Motor Commands
            //
            MotorCommand checkConnectionCommand = new MotorCommand() { Type = MotorCommand.CommandType.CheckConnectionAsync, Motor = motor };
            EnqueueCommand(checkConnectionCommand);
            MotorCommand getPositionCommand = new MotorCommand() { Type = MotorCommand.CommandType.GetPositionAsync, Motor = motor };
            EnqueueCommand(getPositionCommand);
            //
            // Set the DataGridView values for the Motor
            //
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeMotorsDataGridView, motor.Connected, "Connected", "Not Connected", motor.Name, "State");
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlMotorsDataGridView, motor.Connected, "Connected", "Not Connected", motor.Name, "State");
            if (int.TryParse(motor.Position.ToString(), out _))
            {
                int pos = motor.Position;
                try
                {
                    pos = Math.Abs(motor.Position);
                }
                catch (System.OverflowException ex)
                {
                    Debug.WriteLine($"Unable to get the absolute position value ({motor.Position}) for {motor.Name}");
                    pos = motor.Position;
                }
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Position (\u03BCS)", motor.Name, pos.ToString());
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlMotorsDataGridView, "Position (\u03BCS)", motor.Name, pos.ToString());
            }
        }
    }
}
