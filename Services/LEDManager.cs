using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Independent_Reader_GUI.Exceptions;

namespace Independent_Reader_GUI.Services
{
    internal class LEDManager
    {
        private LED Cy5;
        private LED FAM;
        private LED HEX;
        private LED Atto;
        private LED Alexa;
        private LED Cy5p5;
        // Command Queue to feed in Commands
        private ConcurrentQueue<LEDCommand> commandQueue = new ConcurrentQueue<LEDCommand>();
        // Command Queue cancellation token
        private CancellationTokenSource commandQueueCancellationTokenSource = new CancellationTokenSource();

        public LEDManager(LED Cy5, LED FAM, LED HEX, LED Atto, LED Alexa, LED Cy5p5)
        {
            this.Cy5 = Cy5;
            this.FAM = FAM;
            this.HEX = HEX;
            this.Atto = Atto;
            this.Alexa = Alexa;
            this.Cy5p5 = Cy5p5;
        }

        public async Task InitializeLEDParameters()
        {
            // Check the connection of the LED board (only check one since they all share the same board)
            await Cy5.CheckConnectionAsync();
            if (Cy5.Connected)
            {
                FAM.connected = true;
                HEX.connected = true;
                Atto.connected = true;
                Alexa.connected = true;
                Cy5p5.connected = true;
            }
        }

        public LED GetLEDFromName(string name)
        {
            if (name == Cy5.Name) return Cy5;
            else if (name ==  FAM.Name) return FAM;
            else if (name == HEX.Name) return HEX;
            else if (name == Atto.Name) return Atto;
            else if (name == Alexa.Name) return Alexa;
            else if (name == Cy5p5.Name) return Cy5p5;
            string message = $"{name} LED is not Managed by the LEDManager";
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw new LEDNotFoundException(message);
        }

        /// <summary>
        /// Enqueue commands to the LED Manager
        /// </summary>
        /// <param name="command"></param>
        public void EnqueueCommand(LEDCommand command)
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

        public async Task ProcessCommandQueue()
        {
            while (!commandQueueCancellationTokenSource.Token.IsCancellationRequested)
            {
                if (commandQueue.TryDequeue(out LEDCommand command))
                {
                    switch (command.Type)
                    {
                        case LEDCommand.CommandType.CheckConnectionAsync:
                            await command.LED.CheckConnectionAsync();
                            break;
                        case LEDCommand.CommandType.GetVersionAsync:
                            await command.LED.GetVersionAsync();
                            break;
                        case LEDCommand.CommandType.On:
                            await command.LED.On(int.Parse(command.Parameter.ToString()));
                            break;
                        case LEDCommand.CommandType.Off:
                            await command.LED.Off();
                            break;
                    }
                }
                await Task.Delay(50);
            }
        }
    }
}
