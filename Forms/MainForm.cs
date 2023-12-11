using Independent_Reader_GUI.Models;
using System.ComponentModel;

namespace Independent_Reader_GUI
{
    public partial class independentReaderForm : Form
    {
        public independentReaderForm()
        {
            InitializeComponent();

            // Add default data
            AddHomeMotorsDefaultData();
            AddHomeCameraDefaultData();
            AddHomeLEDsDefaultData();
            AddHomeTECsDefaultData();
        }

        /// <summary>
        /// Add default data to the Home Tab Motors Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeMotorsDefaultData()
        {
            MotorData homeMotorsX = new MotorData 
            {
                Name = "x",
                IO = "Idle",
                State = "Connected",
                Steps = "0",
                Speed = "300000",
                Home = "Homed"
            };
            MotorData homeMotorsY = new MotorData 
            {
                Name = "y",
                IO = "Idle",
                State = "Connected",
                Steps = "0",
                Speed = "400000",
                Home = "Homed"
            };
            MotorData homeMotorsZ = new MotorData
            {
                Name = "z",
                IO = "Moving",
                State = "Connected",
                Steps = "?",
                Speed = "300000",
                Home = "Not Homed"
            };
            MotorData homeMotorsFilterWheel = new MotorData
            {
                Name = "Filter Wheel",
                IO = "Idle",
                State = "Connected",
                Steps = "21000",
                Speed = "100000",
                Home = "Homed"
            };
            MotorData homeMotorsClampA = new MotorData
            {
                Name = "Clamp A",
                IO = "?",
                State = "Not Connected",
                Steps = "?",
                Speed = "80000",
                Home = "?"
            };
            MotorData homeMotorsClampB = new MotorData
            {
                Name = "Clamp B",
                IO = "Idle",
                State = "Connected",
                Steps = "0",
                Speed = "80000",
                Home = "Homed"
            };
            MotorData homeMotorsClampC = new MotorData
            {
                Name = "Clamp C",
                IO = "Idle",
                State = "Connected",
                Steps = "350000",
                Speed = "80000",
                Home = "Homed"
            };
            MotorData homeMotorsClampD = new MotorData
            {
                Name = "Clamp D",
                IO = "Idle",
                State = "Connected",
                Steps = "450000",
                Speed = "80000",
                Home = "Homed"
            };
            MotorData homeMotorsTrayAB = new MotorData
            {
                Name = "Tray AB",
                IO = "Idle",
                State = "Connected",
                Steps = "0",
                Speed = "200000",
                Home = "Homed"
            };
            MotorData homeMotorsTrayCD = new MotorData
            {
                Name = "Tray CD",
                IO = "Idle",
                State = "Connected",
                Steps = "790000",
                Speed = "200000",
                Home = "Homed"
            };
            homeMotorsDataGridView.Rows.Add(homeMotorsX.Name, homeMotorsX.IO, homeMotorsX.State, homeMotorsX.Steps, homeMotorsX.Speed, homeMotorsX.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsY.Name, homeMotorsY.IO, homeMotorsY.State, homeMotorsY.Steps, homeMotorsY.Speed, homeMotorsY.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsZ.Name, homeMotorsZ.IO, homeMotorsZ.State, homeMotorsZ.Steps, homeMotorsZ.Speed, homeMotorsZ.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsFilterWheel.Name, homeMotorsFilterWheel.IO, homeMotorsFilterWheel.State, homeMotorsFilterWheel.Steps, homeMotorsFilterWheel.Speed, homeMotorsFilterWheel.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsClampA.Name, homeMotorsClampA.IO, homeMotorsClampA.State, homeMotorsClampA.Steps, homeMotorsClampA.Speed, homeMotorsClampA.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsClampB.Name, homeMotorsClampB.IO, homeMotorsClampB.State, homeMotorsClampB.Steps, homeMotorsClampB.Speed, homeMotorsClampB.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsClampC.Name, homeMotorsClampC.IO, homeMotorsClampC.State, homeMotorsClampC.Steps, homeMotorsClampC.Speed, homeMotorsClampC.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsClampD.Name, homeMotorsClampD.IO, homeMotorsClampD.State, homeMotorsClampD.Steps, homeMotorsClampD.Speed, homeMotorsClampD.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsTrayAB.Name, homeMotorsTrayAB.IO, homeMotorsTrayAB.State, homeMotorsTrayAB.Steps, homeMotorsTrayAB.Speed, homeMotorsTrayAB.Home);           
            homeMotorsDataGridView.Rows.Add(homeMotorsTrayCD.Name, homeMotorsTrayCD.IO, homeMotorsTrayCD.State, homeMotorsTrayCD.Steps, homeMotorsTrayCD.Speed, homeMotorsTrayCD.Home);           
        }

        /// <summary>
        /// Add default data to the Home Tab LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeCameraDefaultData()
        {
            CameraData homeCamera = new CameraData
            {
                State = "Connected",
                Exposure = "600000",
                Temp = "3400"
            };
            homeCameraDataGridView.Rows.Add(homeCamera.State, homeCamera.Exposure, homeCamera.Temp);
        }

        /// <summary>
        /// Add default data to the Home Tab LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeLEDsDefaultData()
        {
            LEDsData homeLEDsState = new LEDsData
            {
                PropertyName = "State",
                ValueCy5 = "C",
                ValueFAM = "C",
                ValueHEX = "C",
                ValueAtto = "C",
                ValueAlexa = "C",
                ValueCy5p5 = "N",
            };
            LEDsData homeLEDsIO = new LEDsData
            {
                PropertyName = "IO",
                ValueCy5 = "Off",
                ValueFAM = "On",
                ValueHEX = "Off",
                ValueAtto = "Off",
                ValueAlexa = "Off",
                ValueCy5p5 = "Off",
            };
            LEDsData homeLEDsID = new LEDsData
            {
                PropertyName = "ID",
                ValueCy5 = "3",
                ValueFAM = "1",
                ValueHEX = "5",
                ValueAtto = "2",
                ValueAlexa = "4",
                ValueCy5p5 = "6",
            };
            LEDsData homeLEDsIntensity = new LEDsData
            {
                PropertyName = "Intensity (%)",
                ValueCy5 = "20",
                ValueFAM = "20",
                ValueHEX = "20",
                ValueAtto = "20",
                ValueAlexa = "20",
                ValueCy5p5 = "20",
            };
            homeLEDsDataGridView.Rows.Add(homeLEDsState.PropertyName, homeLEDsState.ValueCy5, homeLEDsState.ValueFAM, homeLEDsState.ValueHEX, homeLEDsState.ValueAtto, homeLEDsState.ValueAlexa, homeLEDsState.ValueCy5p5);
            homeLEDsDataGridView.Rows.Add(homeLEDsIO.PropertyName, homeLEDsIO.ValueCy5, homeLEDsIO.ValueFAM, homeLEDsIO.ValueHEX, homeLEDsIO.ValueAtto, homeLEDsIO.ValueAlexa, homeLEDsIO.ValueCy5p5);
            homeLEDsDataGridView.Rows.Add(homeLEDsID.PropertyName, homeLEDsID.ValueCy5, homeLEDsID.ValueFAM, homeLEDsID.ValueHEX, homeLEDsID.ValueAtto, homeLEDsID.ValueAlexa, homeLEDsID.ValueCy5p5);
            homeLEDsDataGridView.Rows.Add(homeLEDsIntensity.PropertyName, homeLEDsIntensity.ValueCy5, homeLEDsIntensity.ValueFAM, homeLEDsIntensity.ValueHEX, homeLEDsIntensity.ValueAtto, homeLEDsIntensity.ValueAlexa, homeLEDsIntensity.ValueCy5p5);
        }

        /// <summary>
        /// Add default data to the Home Tab TECs Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeTECsDefaultData()
        {
            TECsData homeTECsState = new TECsData
            {
                PropertyName = "State",
                ValueTECA = "Connected",
                ValueTECB = "Connected",
                ValueTECC = "Connected",
                ValueTECD = "Connected",
            };
            TECsData homeTECsIO = new TECsData
            {
                PropertyName = "IO",
                ValueTECA = "Ramping Up",
                ValueTECB = "Disabled",
                ValueTECC = "Ramping Down",
                ValueTECD = "Holding",
            };
            TECsData homeTECsActualTemp = new TECsData
            {
                PropertyName = "Actual Temp (C)",
                ValueTECA = "37.8",
                ValueTECB = "24.2",
                ValueTECC = "63.5",
                ValueTECD = "28.7",
            };
            TECsData homeTECsTargetTemp = new TECsData
            {
                PropertyName = "Target Temp (C)",
                ValueTECA = "94.0",
                ValueTECB = "30.0",
                ValueTECC = "45.0",
                ValueTECD = "30.0",
            };
            TECsData homeTECsTempEnabled = new TECsData
            {
                PropertyName = "Temp Enabled",
                ValueTECA = "On",
                ValueTECB = "Off",
                ValueTECC = "On",
                ValueTECD = "On",
            };
            TECsData homeTECsSinkTemp = new TECsData
            {
                PropertyName = "Sink Temp (C)",
                ValueTECA = "42.1",
                ValueTECB = "24.9",
                ValueTECC = "88.3",
                ValueTECD = "32.9",
            };
            TECsData homeTECsSinkTempMax = new TECsData
            {
                PropertyName = "Sink Temp Max (C)",
                ValueTECA = "101.0",
                ValueTECB = "101.0",
                ValueTECC = "101.0",
                ValueTECD = "101.0",
            };
            TECsData homeTECsFanRPM = new TECsData
            {
                PropertyName = "Fan RPM",
                ValueTECA = "0",
                ValueTECB = "0",
                ValueTECC = "12000",
                ValueTECD = "0",
            };
            TECsData homeTECsFanOnTemp = new TECsData
            {
                PropertyName = "Fan on Temp (C)",
                ValueTECA = "30.0",
                ValueTECB = "30.0",
                ValueTECC = "30.0",
                ValueTECD = "30.0",
            };
            TECsData homeTECsFanOffTemp = new TECsData
            {
                PropertyName = "Fan off Temp (C)",
                ValueTECA = "30.0",
                ValueTECB = "30.0",
                ValueTECC = "30.0",
                ValueTECD = "30.0",
            };
            TECsData homeTECsCurrent = new TECsData
            {
                PropertyName = "Current (A)",
                ValueTECA = "0.1",
                ValueTECB = "0.1",
                ValueTECC = "0.1",
                ValueTECD = "0.1",
            };
            TECsData homeTECsMaxCurrent = new TECsData
            {
                PropertyName = "Max Current (A)",
                ValueTECA = "0.2",
                ValueTECB = "0.2",
                ValueTECC = "0.2",
                ValueTECD = "0.2",
            };
            TECsData homeTECsVoltage = new TECsData
            {
                PropertyName = "Voltage (V)",
                ValueTECA = "24.0",
                ValueTECB = "24.0",
                ValueTECC = "24.0",
                ValueTECD = "24.0",
            };
            TECsData homeTECsMaxVoltage = new TECsData
            {
                PropertyName = "Max Voltage (V)",
                ValueTECA = "24.0",
                ValueTECB = "24.0",
                ValueTECC = "24.0",
                ValueTECD = "24.0",
            };
            homeTECsDataGridView.Rows.Add(homeTECsState.PropertyName, homeTECsState.ValueTECA, homeTECsState.ValueTECB, homeTECsState.ValueTECC, homeTECsState.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsIO.PropertyName, homeTECsIO.ValueTECA, homeTECsIO.ValueTECB, homeTECsIO.ValueTECC, homeTECsIO.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsActualTemp.PropertyName, homeTECsActualTemp.ValueTECA, homeTECsActualTemp.ValueTECB, homeTECsActualTemp.ValueTECC, homeTECsActualTemp.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsTargetTemp.PropertyName, homeTECsTargetTemp.ValueTECA, homeTECsTargetTemp.ValueTECB, homeTECsTargetTemp.ValueTECC, homeTECsTargetTemp.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsTempEnabled.PropertyName, homeTECsTempEnabled.ValueTECA, homeTECsTempEnabled.ValueTECB, homeTECsTempEnabled.ValueTECC, homeTECsTempEnabled.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsSinkTemp.PropertyName, homeTECsSinkTemp.ValueTECA, homeTECsSinkTemp.ValueTECB, homeTECsSinkTemp.ValueTECC, homeTECsSinkTemp.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsSinkTempMax.PropertyName, homeTECsSinkTempMax.ValueTECA, homeTECsSinkTempMax.ValueTECB, homeTECsSinkTempMax.ValueTECC, homeTECsSinkTempMax.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsFanRPM.PropertyName, homeTECsFanRPM.ValueTECA, homeTECsFanRPM.ValueTECB, homeTECsFanRPM.ValueTECC, homeTECsFanRPM.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsFanOnTemp.PropertyName, homeTECsFanOnTemp.ValueTECA, homeTECsFanOnTemp.ValueTECB, homeTECsFanOnTemp.ValueTECC, homeTECsFanOnTemp.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsFanOffTemp.PropertyName, homeTECsFanOffTemp.ValueTECA, homeTECsFanOffTemp.ValueTECB, homeTECsFanOffTemp.ValueTECC, homeTECsFanOffTemp.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsCurrent.PropertyName, homeTECsCurrent.ValueTECA, homeTECsCurrent.ValueTECB, homeTECsCurrent.ValueTECC, homeTECsCurrent.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsMaxCurrent.PropertyName, homeTECsMaxCurrent.ValueTECA, homeTECsMaxCurrent.ValueTECB, homeTECsMaxCurrent.ValueTECC, homeTECsMaxCurrent.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsVoltage.PropertyName, homeTECsVoltage.ValueTECA, homeTECsVoltage.ValueTECB, homeTECsVoltage.ValueTECC, homeTECsVoltage.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsMaxVoltage.PropertyName, homeTECsMaxVoltage.ValueTECA, homeTECsMaxVoltage.ValueTECB, homeTECsMaxVoltage.ValueTECC, homeTECsMaxVoltage.ValueTECD);
        }
    }
}