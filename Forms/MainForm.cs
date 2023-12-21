using Independent_Reader_GUI.Exceptions;
using Independent_Reader_GUI.Forms;
using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Resources;
using Independent_Reader_GUI.Services;
using Independent_Reader_GUI.Utilities;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using System.ComponentModel;
using System.Reflection;

namespace Independent_Reader_GUI
{
    public partial class independentReaderForm : Form
    {
        // Private attributes
        private Configuration configuration = new Configuration();
        private readonly APIManager apiManager = new APIManager();
        private DataGridViewManager dataGridViewManager = new DataGridViewManager();
        private TimerManager runExperimentDataTimerManager;
        private TimerManager controlTabTimerManager;
        private string defaultProtocolDirectory;
        private ThermocyclingProtocol protocol = new ThermocyclingProtocol();
        private ThermocyclingProtocolPlotManager plotManager = new ThermocyclingProtocolPlotManager();
        private ThermocyclingProtocolManager protocolManager;
        private TECManager tecManager;
        private MotorsManager motorManager;
        private CartridgeOptions cartridgeOptions;
        private ElastomerOptions elastomerOptions;
        private BergquistOptions bergquistOptions;
        private FLIRCameraManager cameraManager = new FLIRCameraManager();
        private string runExperimentProtocolName = string.Empty;
        private double runExperimentProtocolTime = 0.0;
        private TEC tecA;
        private TEC tecB;
        private TEC tecC;
        private TEC tecD;
        private Motor xMotor;
        private Motor yMotor;
        private Motor zMotor;
        private Motor filterWheelMotor;
        private Motor trayABMotor;
        private Motor trayCDMotor;
        private Motor clampAMotor;
        private Motor clampBMotor;
        private Motor clampCMotor;
        private Motor clampDMotor;

        /// <summary>
        /// Initialization of the Form
        /// </summary>
        public independentReaderForm()
        {
            InitializeComponent();

            // Check if the FastAPI server is potentially down
            if (apiManager == null)
            {
                MessageBox.Show("The API could be down or the connection to the Chassis Board could be lost", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // Connect to the FLIR Camera
            cameraManager.Connect();
            // Connect to the TECs
            tecA = new TEC(id: configuration.TECAAddress, name: "TEC A");
            tecB = new TEC(id: configuration.TECBAddress, name: "TEC B");
            tecC = new TEC(id: configuration.TECCAddress, name: "TEC C");
            tecD = new TEC(id: configuration.TECDAddress, name: "TEC D");
            tecManager = new TECManager(tecA, tecB, tecC, tecD);
            // Connect to the Motors
            xMotor = new Motor(address: configuration.xMotorAddress, name: "x", apiManager: apiManager);
            yMotor = new Motor(address: configuration.yMotorAddress, name: "y", apiManager: apiManager);            
            zMotor = new Motor(address: configuration.zMotorAddress, name: "z", apiManager: apiManager);           
            filterWheelMotor = new Motor(address: configuration.FilterWheelMotorAddress, name: "Filter Wheel", apiManager: apiManager);            
            trayABMotor = new Motor(address: configuration.TrayABMotorAddress, name: "Tray AB", apiManager: apiManager);            
            trayCDMotor = new Motor(address: configuration.TrayCDMotorAddress, name: "Tray CD", apiManager: apiManager);            
            clampAMotor = new Motor(address: configuration.ClampAMotorAddress, name: "Clamp A", apiManager: apiManager);           
            clampBMotor = new Motor(address: configuration.ClampBMotorAddress, name: "Clamp B", apiManager: apiManager);            
            clampCMotor = new Motor(address: configuration.ClampCMotorAddress, name: "Clamp C", apiManager: apiManager);            
            clampDMotor = new Motor(address: configuration.ClampDMotorAddress, name: "Clamp D", apiManager: apiManager);
            try
            {
                Task.Run(async () => await xMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await yMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await zMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await filterWheelMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await trayABMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await trayCDMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await clampAMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await clampBMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await clampCMotor.CheckConnectionAsync()).Wait();
                Task.Run(async () => await clampDMotor.CheckConnectionAsync()).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not connect to motors");
            }
            motorManager = new MotorsManager(xMotor, yMotor, zMotor, filterWheelMotor, trayABMotor, trayCDMotor, clampAMotor, clampBMotor, clampCMotor, clampDMotor);

            // Initialize
            protocolManager = new ThermocyclingProtocolManager(configuration);
            cartridgeOptions = new CartridgeOptions(configuration);
            elastomerOptions = new ElastomerOptions(configuration);
            bergquistOptions = new BergquistOptions(configuration);

            // Obtain default data paths
            defaultProtocolDirectory = configuration.ThermocyclingProtocolsDataPath;

            // Add default data
            AddHomeMotorsDefaultData();
            AddHomeCameraDefaultData();
            AddHomeLEDsDefaultData();
            AddHomeTECsDefaultData();
            AddRunExperimentDefaultData();
            AddRunImagingSetupDefaultData();
            AddControlMotorsDefaultData();
            AddControlLEDsDefaultData();
            AddControlTECsDefaultData();
            AddThermocyclingProtocolStatusesDefaultData();
            AddImagingScanParametersDefaultData();
            AddImagingLEDsDefaultData();

            // Add default plots
            AddThermocyclingDefaultPlot();

            // Initialize the RunExperimentDataGridView timer after data is added to it
            runExperimentDataTimerManager = new TimerManager(interval: configuration.RunDataTimerInterval, tickEventHandler: runExperimentDataGridView_TickEventHandler);
            runExperimentDataTimerManager.Start();
            controlTabTimerManager = new TimerManager(interval: configuration.ControlTabTimerInterval, tickEventHandler: controlTab_TickEventHandler);
            controlTabTimerManager.Start();

            // Add formatting to the data grid views
            this.runExperimentDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.runDataGridView_CellFormatting);
            this.homeMotorsDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.homeTECsDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.homeCameraDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.homeLEDsDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.controlMotorsDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.controlTECsDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.controlLEDsDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.thermocyclingProtocolStatusesDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.homeDataGridView_CellFormatting);
            this.dataGridView_SetupComboBoxes();


            // Set initial colors for connected modules
            // TODO: Put this region of code in a function to be called every N seconds
            #region
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, xMotor.Connected, "Connected", "Not Connected", 0, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, yMotor.Connected, "Connected", "Not Connected", 1, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, zMotor.Connected, "Connected", "Not Connected", 2, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, filterWheelMotor.Connected, "Connected", "Not Connected", 3, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, trayABMotor.Connected, "Connected", "Not Connected", 4, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, trayCDMotor.Connected, "Connected", "Not Connected", 5, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, clampAMotor.Connected, "Connected", "Not Connected", 6, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, clampBMotor.Connected, "Connected", "Not Connected", 7, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, clampCMotor.Connected, "Connected", "Not Connected", 8, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeMotorsDataGridView, clampDMotor.Connected, "Connected", "Not Connected", 9, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeTECsDataGridView, tecA.Connected, "Connected", "Not Connected", 0, 1);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeTECsDataGridView, tecB.Connected, "Connected", "Not Connected", 0, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeTECsDataGridView, tecC.Connected, "Connected", "Not Connected", 0, 3);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeTECsDataGridView, tecD.Connected, "Connected", "Not Connected", 0, 4);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeCameraDataGridView, cameraManager.Connected, "Connected", "Not Connected", 0, 0);
            // Control Tab
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, xMotor.Connected, "Connected", "Not Connected", 0, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, yMotor.Connected, "Connected", "Not Connected", 1, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, zMotor.Connected, "Connected", "Not Connected", 2, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, filterWheelMotor.Connected, "Connected", "Not Connected", 3, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, trayABMotor.Connected, "Connected", "Not Connected", 4, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, trayCDMotor.Connected, "Connected", "Not Connected", 5, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampAMotor.Connected, "Connected", "Not Connected", 6, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampBMotor.Connected, "Connected", "Not Connected", 7, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampCMotor.Connected, "Connected", "Not Connected", 8, 2);
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampDMotor.Connected, "Connected", "Not Connected", 9, 2);
            #endregion

            // Subscribe to value changed events
            this.runExperimentDataGridView.CellValueChanged += new DataGridViewCellEventHandler(runExperimentDataGridView_CellValueChanged);
            this.controlLEDsDataGridView.CellValueChanged += new DataGridViewCellEventHandler(controlLEDsDataGridView_CellValueChanged);

            // Wire up mouse down events
            thermocyclingPlotView.MouseDown += (s, e) => thermocyclingPlotView_MouseDown(s, e);

            // Subscribe to the load event of this form to set defaults on form loading
            this.Load += new EventHandler(this.Form_Load);

            // Subscribe to the form closing event handler
            this.FormClosing += new FormClosingEventHandler(IndependentReaderGUI_FormClosing);
        }

        /// <summary>
        /// Add default data to the Home Tab Motors Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeMotorsDefaultData()
        {
            MotorData motorData = new MotorData();
            homeMotorsDataGridView.Rows.Add("x", motorData.Version, motorData.State, motorData.Position, configuration.xMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("y", motorData.Version, motorData.State, motorData.Position, configuration.yMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("z", motorData.Version, motorData.State, motorData.Position, configuration.zMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("Filter Wheel", motorData.Version, motorData.State, motorData.Position, configuration.FilterWheelMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("Tray AB", motorData.Version, motorData.State, motorData.Position, configuration.TrayABMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("Tray CD", motorData.Version, motorData.State, motorData.Position, configuration.TrayCDMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("Clamp A", motorData.Version, motorData.State, motorData.Position, configuration.ClampAMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("Clamp B", motorData.Version, motorData.State, motorData.Position, configuration.ClampBMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("Clamp C", motorData.Version, motorData.State, motorData.Position, configuration.ClampCMotorDefaultSpeed, motorData.Home);
            homeMotorsDataGridView.Rows.Add("Clamp D", motorData.Version, motorData.State, motorData.Position, configuration.ClampDMotorDefaultSpeed, motorData.Home);
        }

        /// <summary>
        /// Add default data to the Home Tab LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeCameraDefaultData()
        {
            // TODO: Remove the exposure and temp of the camera and update the designer so the DataGridView doesnt use them and the CameraData class
            // doesnt use them
            CameraData homeCamera = new CameraData
            {
                State = "Not Connected",
                IO = "?"
            };
            homeCameraDataGridView.Rows.Add(homeCamera.State, homeCamera.IO);
        }

        /// <summary>
        /// Add default data to the Home Tab LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeLEDsDefaultData()
        {
            LEDsData homeLEDsState = new LEDsData
            {
                PropertyName = "State",
                ValueCy5 = "N",
                ValueFAM = "N",
                ValueHEX = "N",
                ValueAtto = "N",
                ValueAlexa = "N",
                ValueCy5p5 = "N",
            };
            LEDsData homeLEDsIO = new LEDsData
            {
                PropertyName = "IO",
                ValueCy5 = "Off",
                ValueFAM = "Off",
                ValueHEX = "Off",
                ValueAtto = "Off",
                ValueAlexa = "Off",
                ValueCy5p5 = "Off",
            };
            LEDsData homeLEDsExposure = new LEDsData
            {
                PropertyName = "Exposure (ms)",
                ValueCy5 = "300000",
                ValueFAM = "600000",
                ValueHEX = "500000",
                ValueAtto = "200000",
                ValueAlexa = "400000",
                ValueCy5p5 = "600000",
            };
            LEDsData homeLEDsIntensity = new LEDsData
            {
                PropertyName = "Intensity (%)",
                ValueCy5 = "0",
                ValueFAM = "0",
                ValueHEX = "0",
                ValueAtto = "0",
                ValueAlexa = "0",
                ValueCy5p5 = "0",
            };
            homeLEDsDataGridView.Rows.Add(homeLEDsState.PropertyName, homeLEDsState.ValueCy5, homeLEDsState.ValueFAM, homeLEDsState.ValueHEX, homeLEDsState.ValueAtto, homeLEDsState.ValueAlexa, homeLEDsState.ValueCy5p5);
            homeLEDsDataGridView.Rows.Add(homeLEDsIO.PropertyName, homeLEDsIO.ValueCy5, homeLEDsIO.ValueFAM, homeLEDsIO.ValueHEX, homeLEDsIO.ValueAtto, homeLEDsIO.ValueAlexa, homeLEDsIO.ValueCy5p5);
            homeLEDsDataGridView.Rows.Add(homeLEDsExposure.PropertyName, homeLEDsExposure.ValueCy5, homeLEDsExposure.ValueFAM, homeLEDsExposure.ValueHEX, homeLEDsExposure.ValueAtto, homeLEDsExposure.ValueAlexa, homeLEDsExposure.ValueCy5p5);
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
                ValueTECA = "Not Connected",
                ValueTECB = "Not Connected",
                ValueTECC = "Not Connected",
                ValueTECD = "Not Connected",
            };
            TECsData homeTECsIO = new TECsData
            {
                PropertyName = "IO",
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
            };
            TECsData homeTECsActualTemp = new TECsData
            {
                PropertyName = "Actual Temp (\u00B0C)",
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
            };
            TECsData homeTECsTargetTemp = new TECsData
            {
                PropertyName = "Target Temp (\u00B0C)",
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
            };
            TECsData homeTECsTempEnabled = new TECsData
            {
                PropertyName = "Temp Enabled",
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
            };
            TECsData homeTECsSinkTemp = new TECsData
            {
                PropertyName = "Sink Temp (\u00B0C)",
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
            };
            TECsData homeTECsSinkTempMax = new TECsData
            {
                PropertyName = "Sink Temp Max (\u00B0C)",
                ValueTECA = "101.0",
                ValueTECB = "101.0",
                ValueTECC = "101.0",
                ValueTECD = "101.0",
            };
            TECsData homeTECsFanRPM = new TECsData
            {
                PropertyName = "Fan RPM",
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
            };
            TECsData homeTECsFanOnTemp = new TECsData
            {
                PropertyName = "Fan on Temp (\u00B0C)",
                ValueTECA = "30.0",
                ValueTECB = "30.0",
                ValueTECC = "30.0",
                ValueTECD = "30.0",
            };
            TECsData homeTECsFanOffTemp = new TECsData
            {
                PropertyName = "Fan off Temp (\u00B0C)",
                ValueTECA = "30.0",
                ValueTECB = "30.0",
                ValueTECC = "30.0",
                ValueTECD = "30.0",
            };
            TECsData homeTECsCurrent = new TECsData
            {
                PropertyName = "Current (A)",
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
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
                ValueTECA = "?",
                ValueTECB = "?",
                ValueTECC = "?",
                ValueTECD = "?",
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

        /// <summary>
        /// Add default data to the Run Experiment Data Grid View upon loading the Form
        /// </summary>
        private void AddRunExperimentDefaultData()
        {
            ExperimentData runExperimentData = new ExperimentData();
            runExperimentDataGridView.Rows.Add("Experiment Name", runExperimentData.Name);
            runExperimentDataGridView.Rows.Add("Protocol");
            runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.ProtocolComboBoxCell;
            runExperimentDataGridView.Rows.Add("Start Time (HH:mm:ss)", runExperimentData.StartDateTime.ToString("HH:mm:ss"));
            runExperimentDataGridView.Rows.Add("Start Date (MM/dd/YYYY)", runExperimentData.StartDateTime.ToString("MM/dd/yyyy"));
            runExperimentDataGridView.Rows.Add("Projected End Time (HH:mm:ss)", runExperimentData.EndDateTime.ToString("HH:mm:ss"));
            runExperimentDataGridView.Rows.Add("Projected End Date (MM/dd/YYYY)", runExperimentData.EndDateTime.ToString("MM/dd/yyyy"));
            runExperimentDataGridView.Rows.Add("Heater", runExperimentData.Heater);
            runExperimentDataGridView.Rows.Add("Partition Type");
            runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.PartitionTypeComboBoxCell;
            runExperimentDataGridView.Rows.Add("Cartridge");
            runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.CartridgeComboBoxCell;
            runExperimentDataGridView.Rows.Add("Cartridge Length (mm)", cartridgeOptions.GetCartridgeFromName(runExperimentData.CartridgeComboBoxCell.Value.ToString()).Length);
            runExperimentDataGridView.Rows.Add("Cartridge Width (mm)", cartridgeOptions.GetCartridgeFromName(runExperimentData.CartridgeComboBoxCell.Value.ToString()).Width);
            runExperimentDataGridView.Rows.Add("Cartridge Height (mm)", cartridgeOptions.GetCartridgeFromName(runExperimentData.CartridgeComboBoxCell.Value.ToString()).Height);
            runExperimentDataGridView.Rows.Add("Clamp Position (\u03BCs)", runExperimentData.ClampPosition);
            runExperimentDataGridView.Rows.Add("Tray Position (\u03BCs)", runExperimentData.TrayPosition);
            runExperimentDataGridView.Rows.Add("Glass Offset (mm)", runExperimentData.GlassOffset);
            runExperimentDataGridView.Rows.Add("Elastomer");
            runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.ElastomerComboBoxCell;
            runExperimentDataGridView.Rows.Add("Elastomer Thickness (mm)", elastomerOptions.GetElastomerFromName(configuration.DefaultElastomer).Thickness);
            runExperimentDataGridView.Rows.Add("Bergquist");
            runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.BergquistComboBoxCell;
            runExperimentDataGridView.Rows.Add("Bergquist Thickness (mm)", bergquistOptions.GetBergquistFromName(configuration.DefaultBergquist).Thickness);
            runExperimentDataGridView.Rows.Add("Surface Area (mm x mm)", runExperimentData.SurfaceArea);
            runExperimentDataGridView.Rows.Add("Pressure (KPa)", runExperimentData.Pressure);
        }

        /// <summary>
        /// Add default data to the Run Imaging Setup Data Grid View upon loading the Form
        /// </summary>
        private void AddRunImagingSetupDefaultData()
        {
            ImagingSetupData runImagingSetupData = new ImagingSetupData();
            runImagingSetupDataGridView.Rows.Add("X0 (\u03BCS)", runImagingSetupData.X0);
            runImagingSetupDataGridView.Rows.Add("Y0 (\u03BCS)", runImagingSetupData.Y0);
            runImagingSetupDataGridView.Rows.Add("Z0 (\u03BCS)", runImagingSetupData.Z0);
            runImagingSetupDataGridView.Rows.Add("FOV dX (\u03BCS)", runImagingSetupData.FOVdX);
            runImagingSetupDataGridView.Rows.Add("dY (\u03BCS)", runImagingSetupData.dY);
            runImagingSetupDataGridView.Rows.Add("Rotational Offset (\u00B0)", runImagingSetupData.RotationalOffset);
            runImagingSetupDataGridView.Rows.Add("Use Autofocus");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.UseAutofocus;
            runImagingSetupDataGridView.Rows.Add("Image Before");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageBefore;
            runImagingSetupDataGridView.Rows.Add("Image During (FOV)");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageDuringFOV;
            runImagingSetupDataGridView.Rows.Add("Image During Frequency (FOV)", runImagingSetupData.ImageDuringFrequencyFOV);
            runImagingSetupDataGridView.Rows.Add("Image During (Assay)");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageDuringAssay;
            runImagingSetupDataGridView.Rows.Add("Image During Frequency (Assay)", runImagingSetupData.ImageDuringFrequencyAssay);
            runImagingSetupDataGridView.Rows.Add("Image During (Sample)");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageDuringSample;
            runImagingSetupDataGridView.Rows.Add("Image During Frequency (Sample)", runImagingSetupData.ImageDuringFrequencySample);
            runImagingSetupDataGridView.Rows.Add("Image After");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageAfter;
            runImagingSetupDataGridView.Rows.Add("S3 Bucket Path", runImagingSetupData.S3BucketPath);
            runImagingSetupDataGridView.Rows.Add("Local Path", runImagingSetupData.LocalPath);
            runImagingSetupDataGridView.Rows.Add("Image in Cy5");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageInCy5;
            runImagingSetupDataGridView.Rows.Add("Image in FAM");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageInFAM;
            runImagingSetupDataGridView.Rows.Add("Image in HEX");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageInHEX;
            runImagingSetupDataGridView.Rows.Add("Image in Atto");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageInAtto;
            runImagingSetupDataGridView.Rows.Add("Image in Alexa");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageInAlexa;
            runImagingSetupDataGridView.Rows.Add("Image in Cy5.5");
            runImagingSetupDataGridView.Rows[runImagingSetupDataGridView.Rows.Count - 1].Cells[1] = runImagingSetupData.ImageInCy5p5;
            runImagingSetupDataGridView.Rows.Add("Cy5 Intensity (%)", runImagingSetupData.IntensityCy5);
            runImagingSetupDataGridView.Rows.Add("FAM Intensity (%)", runImagingSetupData.IntensityFAM);
            runImagingSetupDataGridView.Rows.Add("HEX Intensity (%)", runImagingSetupData.IntensityHEX);
            runImagingSetupDataGridView.Rows.Add("Atto Intensity (%)", runImagingSetupData.IntensityAtto);
            runImagingSetupDataGridView.Rows.Add("Alexa Intensity (%)", runImagingSetupData.IntensityAlexa);
            runImagingSetupDataGridView.Rows.Add("Cy5.5 Intensity (%)", runImagingSetupData.IntensityCy5p5);
            runImagingSetupDataGridView.Rows.Add("Cy5 Exposure (ms)", runImagingSetupData.ExposureCy5);
            runImagingSetupDataGridView.Rows.Add("FAM Exposure (ms)", runImagingSetupData.ExposureFAM);
            runImagingSetupDataGridView.Rows.Add("HEX Exposure (ms)", runImagingSetupData.ExposureHEX);
            runImagingSetupDataGridView.Rows.Add("Atto Exposure (ms)", runImagingSetupData.ExposureAtto);
            runImagingSetupDataGridView.Rows.Add("Alexa Exposure (ms)", runImagingSetupData.ExposureAlexa);
            runImagingSetupDataGridView.Rows.Add("Cy5.5 Exposure (ms)", runImagingSetupData.ExposureCy5p5);
        }

        /// <summary>
        /// Add default data to the Control Motors Data Grid View upon loading the Form
        /// </summary>
        private void AddControlMotorsDefaultData()
        {
            MotorData controlMotorData = new MotorData();
            controlMotorsDataGridView.Rows.Add("x", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.xMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("y", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.yMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("z", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.zMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Filter Wheel", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.FilterWheelMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Tray AB", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.TrayABMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Tray CD", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.TrayCDMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp A", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampAMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp B", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampBMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp C", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampCMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp D", controlMotorData.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampDMotorDefaultSpeed, controlMotorData.Home);
        }

        /// <summary>
        /// Add default data to the Control LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddControlLEDsDefaultData()
        {
            LEDsData controlLEDsData = new LEDsData();
            controlLEDsDataGridView.Rows.Add("State", "N", "N", "N", "N", "N", "N");
            controlLEDsDataGridView.Rows.Add("IO");
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[1] = controlLEDsData.IOCy5ComboBoxCell;
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[2] = controlLEDsData.IOFAMComboBoxCell;
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[3] = controlLEDsData.IOHEXComboBoxCell;
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[4] = controlLEDsData.IOAttoComboBoxCell;
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[5] = controlLEDsData.IOAlexaComboBoxCell;
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[6] = controlLEDsData.IOCy5p5ComboBoxCell;
            controlLEDsDataGridView.Rows.Add("Intensity (%)",
                configuration.Cy5Intensity,
                configuration.FAMIntensity,
                configuration.HEXIntensity,
                configuration.AttoIntensity,
                configuration.AttoIntensity,
                configuration.Cy5p5Intensity);
        }

        /// <summary>
        /// Add default data to the Control TECs Data Grid View upon loading the Form
        /// </summary>
        private void AddControlTECsDefaultData()
        {
            TECsData controlTECsData = new TECsData();
            controlTECsDataGridView.Rows.Add("State", "Not Connected", "Not Connected", "Not Connected", "Not Connected");
            controlTECsDataGridView.Rows.Add("IO", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("New Object Temp (\u00B0C)");
            controlTECsDataGridView.Rows.Add("Actual Object Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Target Object Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Temp Enabled", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Object Upper Error Threshold (\u00B0C)",
                configuration.TECAObjectUpperErrorThreshold,
                configuration.TECBObjectUpperErrorThreshold,
                configuration.TECCObjectUpperErrorThreshold,
                configuration.TECDObjectUpperErrorThreshold);
            controlTECsDataGridView.Rows.Add("Object Lower Error Threshold (\u00B0C)",
                configuration.TECAObjectLowerErrorThreshold,
                configuration.TECBObjectLowerErrorThreshold,
                configuration.TECCObjectLowerErrorThreshold,
                configuration.TECDObjectLowerErrorThreshold);
            controlTECsDataGridView.Rows.Add("Sink Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Sink Upper Error Threshold (°C)",
                configuration.TECASinkUpperErrorThreshold,
                configuration.TECBSinkUpperErrorThreshold,
                configuration.TECCSinkUpperErrorThreshold,
                configuration.TECDSinkUpperErrorThreshold);
            controlTECsDataGridView.Rows.Add("Sink Lower Error Threshold (°C)",
                configuration.TECASinkLowerErrorThreshold,
                configuration.TECBSinkLowerErrorThreshold,
                configuration.TECCSinkLowerErrorThreshold,
                configuration.TECDSinkLowerErrorThreshold);
            controlTECsDataGridView.Rows.Add("Actual Fan Speed (rpm)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Fan on Temp (\u00B0C)",
                configuration.TECAFanOnTemp,
                configuration.TECBFanOnTemp,
                configuration.TECCFanOnTemp,
                configuration.TECDFanOnTemp);
            controlTECsDataGridView.Rows.Add("Fan Control Enabled", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Current (A)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Max Current (A)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Voltage (V)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Max Voltage (V)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
        }

        /// <summary>
        /// Add default data to the Thermocycling Tab's Protocol Statuses Data Grid View upon loading of the Form
        /// </summary>
        private void AddThermocyclingProtocolStatusesDefaultData()
        {
            TECsData thermocyclingProtocolStatusesData = new TECsData();
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Status", "Not Connected", "Not Connected", "Not Connected", "Not Connected");
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Protocol Running", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Protocol", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Step", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Temp (\u00B0C)", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Target Temp (\u00B0C)", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Sink Temp (\u00B0C)", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Fan RPM", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
        }

        /// <summary>
        ///  Add default plot to the Thermocycling Plot View upon loading the Form
        /// </summary>
        private void AddThermocyclingDefaultPlot()
        {
            thermocyclingPlotView.Model = plotManager.GetPlotModel();
        }

        private void AddImagingScanParametersDefaultData()
        {
            ScanParameterData scanParameterData = new ScanParameterData();
            imagingScanParametersDataGridView.Rows.Add("Heater");
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1] = scanParameterData.Heater;
            imagingScanParametersDataGridView.Rows.Add("Partition Type");
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1] = scanParameterData.PartitionType;
            imagingScanParametersDataGridView.Rows.Add("Cartridge");
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1] = scanParameterData.Cartridge;
            imagingScanParametersDataGridView.Rows.Add("x0", scanParameterData.X0);
            imagingScanParametersDataGridView.Rows.Add("y0", scanParameterData.Y0);
            imagingScanParametersDataGridView.Rows.Add("z0", scanParameterData.Z0);
            imagingScanParametersDataGridView.Rows.Add("FOV dx", scanParameterData.FOVdX);
            imagingScanParametersDataGridView.Rows.Add("dy", scanParameterData.dY);
            imagingScanParametersDataGridView.Rows.Add("Rotational Offset", scanParameterData.RotationalOffset);
        }

        private void AddImagingLEDsDefaultData()
        {
            LEDsData ledsData = new LEDsData();
            imagingLEDsDataGridView.Rows.Add("Use");
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[1] = ledsData.UseCy5ComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[2] = ledsData.UseFAMComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[3] = ledsData.UseHEXComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[4] = ledsData.UseAttoComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[5] = ledsData.UseAlexaComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[6] = ledsData.UseCy5p5ComboBoxCell;
            imagingLEDsDataGridView.Rows.Add("Intensity (%)", null, null, null, null, null, null);
            imagingLEDsDataGridView.Rows.Add("Exposure (ms)", null, null, null, null, null, null);
        }

        /// <summary>
        /// Cell Formatting for the Data Grid Views upon loading the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Only add tool tips to the property column of the data grid view
            if (e.ColumnIndex == 0)
            {
                // Obtain the cell value to determine which tool tip to add to this cell
                var cellValue = runExperimentDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (cellValue != null)
                {
                    // Add Tool Tip Tex where necessary
                    DataGridViewToolTipFormatter ToolTipFormatter = new DataGridViewToolTipFormatter();
                    string toolTipText = ToolTipFormatter.GetCellValueToolTipText(cellValue.ToString());
                    runExperimentDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = toolTipText;
                }
            }
        }

        /// <summary>
        /// Cell Formatting for all Data Grid Views on the Home Tab upon loading of the main Form.
        /// After interface with each module is complete this method should be obsolete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var cellValue = e.Value as string;
            if (cellValue != null)
            {
                if (cellValue.Equals("Connected"))
                {
                    // Change Connected cells to MediumSeaGreen
                    e.CellStyle.BackColor = Color.MediumSeaGreen;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (cellValue.Equals("C"))
                {
                    // Change C cells to MediumSeaGreen
                    e.CellStyle.BackColor = Color.MediumSeaGreen;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (cellValue.Equals("Not Connected"))
                {
                    // Change Not cells to Red
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (cellValue.Equals("N"))
                {
                    // Change N cells to Red
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (cellValue.Equals("Not Homed"))
                {
                    // Change N cells to Gold
                    e.CellStyle.BackColor = Color.Gold;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = homeMotorsDataGridView.DefaultCellStyle.BackColor;
                    e.CellStyle.ForeColor = homeMotorsDataGridView.DefaultCellStyle.ForeColor;
                }
            }
        }

        /// <summary>
        /// Setup of the Combo Boxes for all Data Grid Views in the Form upon loading
        /// </summary>
        private void dataGridView_SetupComboBoxes()
        {
            // Iterate through the Data Grid Views to setup combo boxes
            foreach (DataGridViewRow row in runExperimentDataGridView.Rows)
            {
                if (!row.IsNewRow) // check to skip the new row template
                {
                    var cellValue = row.Cells[0].Value; // Obtain the cell value in the first column
                    if (cellValue != null)
                    {
                        DataGridViewComboBoxFormatter ComboBoxFormater = new DataGridViewComboBoxFormatter();
                        var comboBoxCell = ComboBoxFormater.GetCellValueComboBox(cellValue.ToString());
                        if (comboBoxCell != null)
                        {
                            row.Cells[1] = comboBoxCell;
                        }
                    }
                }
            }
            //foreach (DataGridViewRow row in runImagingSetupDataGridView.Rows)
            //{
            //    if (!row.IsNewRow) // check to skip the new row template
            //    {
            //        var cellValue = row.Cells[0].Value; // Obtain the cell value in the first column
            //        if (cellValue != null)
            //        {
            //            DataGridViewComboBoxFormatter ComboBoxFormater = new DataGridViewComboBoxFormatter();
            //            var comboBoxCell = ComboBoxFormater.GetCellValueComboBox(cellValue.ToString());
            //            if (comboBoxCell != null)
            //            {
            //                row.Cells[1] = comboBoxCell;
            //            }
            //        }
            //    }
            //}
        }

        private void controlLEDsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the IO cell values change
            var propertySelected = controlLEDsDataGridView.Rows[e.RowIndex].Cells[0].Value;
            if (propertySelected.Equals("IO"))
            {
                // Turn on or off the LED
                string ledName = controlLEDsDataGridView.Columns[e.ColumnIndex].HeaderText;
                LED led = new LED(ledName, configuration, false);
                var valueSelected = controlLEDsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (valueSelected.Equals("On"))
                {
                    try
                    {
                        led.On();
                    }
                    catch (LEDNotConnectedException ex)
                    {
                        //controlLEDsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Off";
                        return;
                    }
                }
                else if (valueSelected.Equals("Off"))
                {
                    try
                    {
                        led.Off();
                    }
                    catch (LEDNotConnectedException ex)
                    {
                        //controlLEDsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "?";
                        return;
                    }
                }
            }
        }
        private void runExperimentDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                var propertySelected = runExperimentDataGridView.Rows[e.RowIndex].Cells[0].Value;
                // Handle PartitionType changes
                if (propertySelected.Equals("Partition Type"))
                {
                    var valueSelected = runExperimentDataGridView.Rows[e.RowIndex].Cells[1].Value;
                    // Get the possible cartridge options.
                    runExperimentDataGridView.Rows[e.RowIndex + 1].Cells[1] = cartridgeOptions.GetOptionNamesComboBoxCell(valueSelected.ToString());
                    var cartridgeSelected = runExperimentDataGridView.Rows[e.RowIndex + 1].Cells[1].Value;
                    Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(cartridgeSelected.ToString());
                    // Convert to millimeters if necessary
                    // Update the TextBox texts
                    runExperimentDataGridView.Rows[e.RowIndex + 2].Cells[1].Value = cartridge.Length.ToString();
                    runExperimentDataGridView.Rows[e.RowIndex + 3].Cells[1].Value = cartridge.Width.ToString();
                    runExperimentDataGridView.Rows[e.RowIndex + 4].Cells[1].Value = cartridge.Height.ToString();
                }
                // Handle Cartridge Changes
                else if (propertySelected.Equals("Cartridge"))
                {
                    var valueSelected = runExperimentDataGridView.Rows[e.RowIndex].Cells[1].Value;
                    Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(valueSelected.ToString());
                    // Convert to millimeters if necessary
                    // Update the TextBox texts
                    runExperimentDataGridView.Rows[e.RowIndex + 1].Cells[1].Value = cartridge.Length.ToString();
                    runExperimentDataGridView.Rows[e.RowIndex + 2].Cells[1].Value = cartridge.Width.ToString();
                    runExperimentDataGridView.Rows[e.RowIndex + 3].Cells[1].Value = cartridge.Height.ToString();
                }
            }
        }

        /// <summary>
        /// Form loading event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {
            // Change the default cell selection for data grid views on form loading
            homeMotorsDataGridView.ClearSelection();
            homeTECsDataGridView.ClearSelection();
            homeCameraDataGridView.ClearSelection();
            homeLEDsDataGridView.ClearSelection();
            runExperimentDataGridView.CurrentCell = runExperimentDataGridView.Rows[0].Cells[1];
            runImagingSetupDataGridView.CurrentCell = runImagingSetupDataGridView.Rows[0].Cells[1];
        }

        /// <summary>
        /// Click event for the Run tab Add Sample button. Will only add a new sample if all other data
        /// is filled in the Sample Meta data grid view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runAddSampleButton_Click(object sender, EventArgs e)
        {
            // Add a new row only if the other rows are filled out otherwise provide a warning to the user.
            foreach (DataGridViewRow row in runSampleMetaDataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == string.Empty)
                    {
                        // Warning message 
                        MessageBox.Show("Sample Meta Data Form is missing data, complete form before adding more samples.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            object[] newRow = new object[] { string.Empty, string.Empty };
            runSampleMetaDataGridView.Rows.Add(newRow);
        }

        /// <summary>
        /// Click event for the Run tab Remove Sample button. Will delete all selected cell rows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runRemoveSampleButton_Click(object sender, EventArgs e)
        {
            DataGridViewContentDeleter ContentDeleter = new DataGridViewContentDeleter();
            ContentDeleter.DeleteSelectedRows(runSampleMetaDataGridView);
        }

        /// <summary>
        /// Click event for the Control Tab Move motor button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void controlMoveButton_Click(object sender, EventArgs e)
        {
            // Get the motor to be moved
            string motorName = controlMotorsComboBox.Text;
            Motor motor = motorManager.GetMotorByName(motorName);
            if (motor != null)
            {
                if (!motor.Connected)
                {
                    MessageBox.Show(motorName + " motor cannot move, it is not connected", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // TODO: Add this section of code back in once the Home column for the ControlMotorsDataGridView values are set
                //if (!motor.Homed)
                //{
                //    MessageBox.Show(motorName + " motor cannot move, it has not been homed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                int position = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Position (μS)", motorName, controlMotorsDataGridView));
                int velocity = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Speed (μS/s)", motorName, controlMotorsDataGridView)); //(\u00b5S)
                await motor.MoveAsync(position, velocity);
            }
        }

        /// <summary>
        /// Click event for the Control Tab Home motor button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void controlHomeButton_Click(object sender, EventArgs e)
        {
            // Get the motor to be homed
            string motorName = controlMotorsComboBox.Text;
            var motor = motorManager.GetMotorByName(motorName);
            if (motor.Connected)
            {
                await motor.HomeAsync();
            }
            else
            {
                MessageBox.Show("Motor is not connected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event for the Thermocycling Add Step button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thermocyclingAddStepButton_Click(object sender, EventArgs e)
        {
            // Open a Thermocycling Add Step Form window
            ThermocyclingProtocolAddStepForm addStepForm = new ThermocyclingProtocolAddStepForm();
            if (addStepForm.ShowDialog() == DialogResult.OK)
            {
                double stepTemperature = addStepForm.StepTemperature;
                string stepTypeName = addStepForm.StepTypeName;
                double stepTime = addStepForm.StepTime;
                int index = protocol.Count;
                ThermocyclingProtocolStep step = new ThermocyclingProtocolStep();
                step.Temperature = stepTemperature;
                step.TypeName = stepTypeName;
                step.Time = stepTime;
                step.Index = index;
                //plotManager.AddStepOld(stepTemperature, stepTime, stepTypeName);
                protocol.AddStep(step);
                plotManager.AddStep(step);
            }
        }

        /// <summary>
        /// Mouse Down event handler for clicking on the Thermocycling Plot View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thermocyclingPlotView_MouseDown(object sender, MouseEventArgs e)
        {
            // Convert Windows Forms mouse event args to OxyPlot ScreenPoint
            var screenPoint = new ScreenPoint(e.X, e.Y);
            // Create an OxyPlot mouse event args
            var oxyArgs = new OxyMouseDownEventArgs
            {
                Position = screenPoint,
                ChangedButton = OxyMouseButton.Left // assumes a left mouse click
            };

            // Invoke the plot manager's mouse down handler
            // FIXME: Clicking on the Plot View causes the GUI to crash, this needs to be resolved
            plotManager.HandleMouseDown(sender, oxyArgs);
        }

        /// <summary>
        /// Click event handler for clicking the Thermocycling Edit Step button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thermocyclingEditStepButton_Click(object sender, EventArgs e)
        {
            // Open a Thermocycling Edit Step Form window
            ThermocyclingProtocolEditStepForm editStepForm = new ThermocyclingProtocolEditStepForm(plotManager);
            if (editStepForm.ShowDialog() == DialogResult.OK)
            {
                int stepNumber = editStepForm.StepNumber;
                double stepTemperature = editStepForm.StepTemperature;
                double stepTime = editStepForm.StepTime;
            }
        }

        private void thermocyclingLoadProtocolButton_Click(object sender, EventArgs e)
        {
            FileSearcher fileSearcher = new FileSearcher();
            // FIXME: take the initial directory default value from a config file 
            var filePath = fileSearcher.GetLoadFilePath(initialDirectory: defaultProtocolDirectory);
            if (filePath != null)
            {
                // FIXME: ClearPlot dos not behave as expected
                //plotManager.ClearPlot();
                protocol = protocolManager.LoadProtocol(filePath);
                plotManager.PlotProtocol(protocol);
            }
        }

        private void thermocyclingSaveProtocolButton_Click(object sender, EventArgs e)
        {
            FileSearcher fileSearcher = new FileSearcher();
            // Determine where to save this protocol
            // FIXME: take the initial directory default value from a config file 
            var filePath = fileSearcher.GetSaveFilePath(initialDirectory: defaultProtocolDirectory);
            if (filePath != null)
            {
                protocolManager.SaveProtocol(protocol, filePath, userLabel.Text, DateTime.Now.ToString("MM/dd/yyyy"));
            }
        }

        private void imagingStreamButton_Click(object sender, EventArgs e)
        {
            if (cameraManager.Streaming)
            {
                cameraManager.StopStream();
            }
            else
            {
                cameraManager.StartStream();
            }
        }

        public async void controlTab_TickEventHandler(object sender, EventArgs e)
        {
            // FIXME: If I turn off the instrument and turn it back on the instrument does not show being connected again 
            // FIXME: Could be that I need to check if each motor is connected here at the onset
            // Check if the APIManger is connect (if not API server could be down)
            if (apiManager != null)
            {
                // Check the motors
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, xMotor.Connected, "Connected", "Not Connected", 0, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, yMotor.Connected, "Connected", "Not Connected", 1, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, zMotor.Connected, "Connected", "Not Connected", 2, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, filterWheelMotor.Connected, "Connected", "Not Connected", 3, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, trayABMotor.Connected, "Connected", "Not Connected", 4, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, trayCDMotor.Connected, "Connected", "Not Connected", 5, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampAMotor.Connected, "Connected", "Not Connected", 6, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampBMotor.Connected, "Connected", "Not Connected", 7, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampCMotor.Connected, "Connected", "Not Connected", 8, 2);
                dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(controlMotorsDataGridView, clampDMotor.Connected, "Connected", "Not Connected", 9, 2);
                // TODO: Check the TECs
                // TODO: Check the LEDs
            }
        }

        /// <summary>
        /// Tick Event Handler for the time data in the Run tabs Experiment DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runExperimentDataGridView_TickEventHandler(object sender, EventArgs e)
        {
            double projectedAdditionalTimeSeconds = 0.0;
            double protocolTimeInSeconds = 0.0;
            foreach (DataGridViewRow row in runExperimentDataGridView.Rows)
            {
                if (row.Cells[0].Value.ToString().Contains("Start Time"))
                {
                    int rowIndex = row.Index;
                    runExperimentDataGridView.Rows[rowIndex].Cells[1].Value = DateTime.Now.ToString("HH:mm:ss");
                }
                else if (row.Cells[0].Value.ToString().Contains("Start Date"))
                {
                    int rowIndex = row.Index;
                    runExperimentDataGridView.Rows[rowIndex].Cells[1].Value = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else if (row.Cells[0].Value.ToString().Contains("End Time"))
                {
                    int rowIndex = row.Index;
                    DateTime projectedEndTime = DateTime.Now;
                    foreach (DataGridViewRow imagingSetupRow in runImagingSetupDataGridView.Rows)
                    {
                        // TODO: Check the number of samples and or assays to be more precious than just the entire chip being scanned and the number of channels
                        if (imagingSetupRow.Cells[0].Value.Equals("Image Before") && imagingSetupRow.Cells[1].Value.Equals("Yes"))
                        {
                            projectedAdditionalTimeSeconds += configuration.EstimateSampleCaptureTimeSeconds * 8;
                        }
                        else if (imagingSetupRow.Cells[0].Value.Equals("Image After") && imagingSetupRow.Cells[1].Value.Equals("Yes"))
                        {
                            projectedAdditionalTimeSeconds += configuration.EstimateSampleCaptureTimeSeconds * 8;
                        }
                    }
                    foreach (DataGridViewRow runExperimentDataRow in runExperimentDataGridView.Rows)
                    {
                        if (runExperimentDataRow.Cells[0].Value.Equals("Protocol"))
                        {
                            // TODO: Get the estimated protocol time from the selected protocol
                            string protocolName = runExperimentDataRow.Cells[1].Value.ToString();
                            if (!runExperimentProtocolName.Equals(protocolName))
                            {
                                string protocolFileName = protocolName.Replace(" ", "_") + ".xml";
                                string protocolFilePath = configuration.ThermocyclingProtocolsDataPath + protocolFileName;
                                ThermocyclingProtocol protocol = protocolManager.LoadProtocol(protocolFilePath);
                                protocolTimeInSeconds = protocol.GetTimeInSeconds();
                            }
                            projectedAdditionalTimeSeconds += protocolTimeInSeconds;
                        }
                    }
                    runExperimentDataGridView.Rows[rowIndex].Cells[1].Value = DateTime.Now.AddSeconds(projectedAdditionalTimeSeconds).ToString("HH:mm:ss");
                }
                else if (row.Cells[0].Value.ToString().Contains("End Date"))
                {
                    int rowIndex = row.Index;
                    runExperimentDataGridView.Rows[rowIndex].Cells[1].Value = DateTime.Now.AddSeconds(projectedAdditionalTimeSeconds).ToString("MM/dd/yyyy");
                }
            }
        }

        /// <summary>
        /// Handles the main form closing events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndependentReaderGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            runExperimentDataTimerManager.Stop();
            controlTabTimerManager.Stop();
            cameraManager.Disconnect();
            apiManager.Disponse();
        }

        /// <summary>
        /// Click event handler for clicking the Capture Image button on the Imaging tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imagingCaptureImageButton_Click(object sender, EventArgs e)
        {
            cameraManager.CaptureImage();
        }

        /// <summary>
        /// Click event handler for the setting the temperature of TEC A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECASetTempButton_Click(object sender, EventArgs e)
        {
            if (tecA.Connected)
            {
                // TODO: Implement code to handle setting the temperature of the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecA.SetTemp();
            }
            else
            {
                MessageBox.Show("Cannot set TEC A's temperature, TEC A is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event handler for the setting the temperature of TEC B
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECBSetTempButton_Click(object sender, EventArgs e)
        {
            if (tecB.Connected)
            {
                // TODO: Implement code to handle setting the temperature of the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecB.SetTemp();
            }
            else
            {
                MessageBox.Show("Cannot set TEC B's temperature, TEC B is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event handler for the setting the temperature of TEC C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECCSetTempButton_Click(object sender, EventArgs e)
        {
            if (tecC.Connected)
            {
                // TODO: Implement code to handle setting the temperature of the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecC.SetTemp();
            }
            else
            {
                MessageBox.Show("Cannot set TEC C's temperature, TEC C is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event handler for the setting the temperature of TEC D
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECDSetTempButton_Click(object sender, EventArgs e)
        {
            if (tecD.Connected)
            {
                // TODO: Implement code to handle setting the temperature of the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecD.SetTemp();
            }
            else
            {
                MessageBox.Show("Cannot set TEC D's temperature, TEC D is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event handler for the reseting of TEC A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECAResetButton_Click(object sender, EventArgs e)
        {
            if (tecA.Connected)
            {
                // TODO: Implement code to handle resetting the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecA.Reset();
            }
            else
            {
                MessageBox.Show("Cannot reset TEC A, TEC A is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event handler for the reseting of TEC  B
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECBResetButton_Click(object sender, EventArgs e)
        {
            if (tecB.Connected)
            {
                // TODO: Implement code to handle resetting the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecB.Reset();
            }
            else
            {
                MessageBox.Show("Cannot reset TEC B, TEC B is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Click event handler for the reseting of TEC C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECCResetButton_Click(object sender, EventArgs e)
        {
            if (tecC.Connected)
            {
                // TODO: Implement code to handle resetting the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecC.Reset();
            }
            else
            {
                MessageBox.Show("Cannot reset " + tecC.Name + ", " + tecC.Name + " is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event handler for the reseting of TEC D
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECDResetButton_Click(object sender, EventArgs e)
        {
            if (tecD.Connected)
            {
                // TODO: Implement code to handle resetting the TEC
                // FIXME: So that this cannot work if a protocol is currently running on this tec
                tecD.Reset();
            }
            else
            {
                MessageBox.Show("Cannot reset " + tecD.Name + ", " + tecD.Name + " is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void runRunButton_Click(object sender, EventArgs e)
        {
            // TODO: Implement code to ensure the run will start successfully (Connected motors, clamps are out of the way, etc)
            // TODO: Implement code to close the tray for the correct heater
            // TODO: Implement code to lower the heater for the correct heater
            // TODO: Implement code to image before if necessary
            // TODO: Implement code to start the thermocycling protocol and image during if necessary
            // TODO: Implement code to image after if necessary
            // Generate the report for the run
            var experimentName = runExperimentDataGridView.Rows[0].Cells[1].Value.ToString();
            ReportManager reportManager = new ReportManager(configuration.ReportsDataPath + experimentName.Replace(" ", "_") + "_Report.pdf");
            await reportManager.AddHeaderLogoAsync(configuration.ReportLogoDataPath, 50, 50);
            await reportManager.AddMainTitleAsync(runExperimentDataGridView.Rows[0].Cells[1].Value + " dPCR Report");
            await reportManager.AddParagraphTextAsync("This report for experiment "
                + experimentName
                + " includes experiment data, setup data for imaging before, during, and/or after the run,"
                + " errors/warnings regarding the run, the expected protocol and estimated thermocycling metrology. The report also includes"
                + " any stitched images before and/or after the run. It does not include postprocessing of the images, but this report can be augmented"
                + " with postprocessing data after the fact. Initial conditions for the run can be found to trace any unexpected behavior after thermocycling."
                + " This report does not include the actual thermal profile experienced by the system but is estimated based on the materials at the heater"
                + " cartridge interface.");
            await reportManager.AddSubSectionAsync("Experiment Data", "Experiment data includes all information regarding the experimental setup on the instrument."
                + " This includes the experiment name, the instrument's configuration, dPCR cartridge information, as well as any excess materials used in the run.");
            await reportManager.AddTableAsync(runExperimentDataGridView);
            await reportManager.AddSubSectionAsync("Imaging Setup", "Imaging setup includes all information regarding the imaging performed before, during, and after the run.");
            await reportManager.AddTableAsync(runImagingSetupDataGridView);
            await reportManager.CloseAsync();
            // TODO: Email the user that the run is complete and a copy of the report
        }

        private async void logoutButton_Click(object sender, EventArgs e)
        {
            var p = await xMotor.GetPositionAsync();
            MessageBox.Show(p.ToString());
        }
    }
}