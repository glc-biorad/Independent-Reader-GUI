using Independent_Reader_GUI.Exceptions;
using Independent_Reader_GUI.Forms;
using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Resources;
using Independent_Reader_GUI.Services;
using Independent_Reader_GUI.Utilities;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using SpinnakerNET;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

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
        private ScanningOptions scanningOptions;
        private ScanningOption defaultScanningOption;
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
            tecA = new TEC(id: configuration.TECAAddress, name: "TEC A", apiManager: apiManager);
            tecB = new TEC(id: configuration.TECBAddress, name: "TEC B", apiManager: apiManager);
            tecC = new TEC(id: configuration.TECCAddress, name: "TEC C", apiManager: apiManager);
            tecD = new TEC(id: configuration.TECDAddress, name: "TEC D", apiManager: apiManager);
            // NOTE: This section of code slows down initialization of the GUI
            Task.Run(async () => await tecA.CheckConnectionAsync());//.Wait();
            Task.Run(async () => await tecB.CheckConnectionAsync());//.Wait();
            Task.Run(async () => await tecC.CheckConnectionAsync());//.Wait();
            Task.Run(async () => await tecD.CheckConnectionAsync());//.Wait();

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
            // NOTE: This section of code slows down initialization of the GUI
            try
            {
                Task.Run(async () => await xMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await yMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await zMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await filterWheelMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await trayABMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await trayCDMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await clampAMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await clampBMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await clampCMotor.CheckConnectionAsync());//.Wait();
                Task.Run(async () => await clampDMotor.CheckConnectionAsync());//.Wait();
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
            scanningOptions = new ScanningOptions(configuration, configureImageScanningDataGridView);
            defaultScanningOption = scanningOptions.GetScanningOptionData("A", configuration.DefaultCartridge, configuration.DefaultGlassOffset, configuration.DefaultElastomer, configuration.DefaultBergquist);

            // Obtain default data paths
            defaultProtocolDirectory = configuration.ThermocyclingProtocolsDataPath;

            // Add default data
            AddHomeMotorsDefaultData();
            AddHomeCameraDefaultData();
            AddHomeLEDsDefaultData();
            AddHomeTECsDefaultData();
            AddRunExperimentDefaultData();
            AddRunImagingSetupDefaultData();
            AddRunSamplesMetaDefaultData();
            AddRunAssaysMetaDefaultData();
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
            this.controlTECsDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.controlTECsDataGridView_CellFormatting);
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

            // Subscribe to CellClick events
            this.controlMotorsDataGridView.CellClick += new DataGridViewCellEventHandler(controlMotorsDataGridView_CellClick);

            // Subscribe to value changed events
            this.runExperimentDataGridView.CellValueChanged += new DataGridViewCellEventHandler(runExperimentDataGridView_CellValueChanged);
            this.controlLEDsDataGridView.CellValueChanged += new DataGridViewCellEventHandler(controlLEDsDataGridView_CellValueChanged);

            // Wire up mouse down events
            //thermocyclingPlotView.MouseDown += (s, e) => thermocyclingPlotView_MouseDown(s, e);

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
            TECsData homeTECsData = new TECsData();
            homeTECsDataGridView.Rows.Add(homeTECsState.PropertyName, homeTECsState.ValueTECA, homeTECsState.ValueTECB, homeTECsState.ValueTECC, homeTECsState.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsIO.PropertyName, homeTECsIO.ValueTECA, homeTECsIO.ValueTECB, homeTECsIO.ValueTECC, homeTECsIO.ValueTECD);
            homeTECsDataGridView.Rows.Add("Version", homeTECsData.ValueTECA, homeTECsData.ValueTECB, homeTECsData.ValueTECC, homeTECsData.ValueTECD);
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
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(AddRunExperimentDefaultData));
            }
            else
            {
                ExperimentData runExperimentData = new ExperimentData();
                runExperimentDataGridView.Rows.Add("Experiment Name", runExperimentData.Name);
                runExperimentDataGridView.Rows.Add("Protocol");
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.ProtocolComboBoxCell;
                runExperimentDataGridView.Rows.Add("Start Time (HH:mm:ss)", runExperimentData.StartDateTime.ToString("HH:mm:ss"));
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Start Date (MM/dd/YYYY)", runExperimentData.StartDateTime.ToString("MM/dd/yyyy"));
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Projected End Time (HH:mm:ss)", runExperimentData.EndDateTime.ToString("HH:mm:ss"));
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Projected End Date (MM/dd/YYYY)", runExperimentData.EndDateTime.ToString("MM/dd/yyyy"));
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Heater", runExperimentData.Heater);
                runExperimentDataGridView.Rows.Add("Partition Type");
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.PartitionTypeComboBoxCell;
                runExperimentDataGridView.Rows.Add("Cartridge");
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.CartridgeComboBoxCell;
                runExperimentDataGridView.Rows.Add("Cartridge Length (mm)", cartridgeOptions.GetCartridgeFromName(runExperimentData.CartridgeComboBoxCell.Value.ToString()).Length);
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Cartridge Width (mm)", cartridgeOptions.GetCartridgeFromName(runExperimentData.CartridgeComboBoxCell.Value.ToString()).Width);
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Cartridge Height (mm)", cartridgeOptions.GetCartridgeFromName(runExperimentData.CartridgeComboBoxCell.Value.ToString()).Height);
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Clamp Position (\u03BCs)", runExperimentData.ClampPosition);
                runExperimentDataGridView.Rows.Add("Tray Position (\u03BCs)", runExperimentData.TrayPosition);
                runExperimentDataGridView.Rows.Add("Glass Offset (mm)", runExperimentData.GlassOffset);
                runExperimentDataGridView.Rows.Add("Elastomer");
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.ElastomerComboBoxCell;
                runExperimentDataGridView.Rows.Add("Elastomer Thickness (mm)", elastomerOptions.GetElastomerFromName(configuration.DefaultElastomer).Thickness);
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Bergquist");
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.BergquistComboBoxCell;
                runExperimentDataGridView.Rows.Add("Bergquist Thickness (mm)", bergquistOptions.GetBergquistFromName(configuration.DefaultBergquist).Thickness);
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
                runExperimentDataGridView.Rows.Add("Contact Surface Area (mm x mm)", runExperimentData.ContactSurfaceArea);
                runExperimentDataGridView.Rows.Add("Pressure (KPa)", runExperimentData.Pressure);
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].ReadOnly = true;
            }
        }

        private void AddRunSamplesMetaDefaultData()
        {
            Cartridge cartridge = new Cartridge();
            cartridge = cartridgeOptions.GetCartridgeFromName(configuration.DefaultCartridge);
            // Setup Sample DataGridView
            int numberOfSampleChambers = cartridge.NumberofSampleChambers;
            for (int i = 0; i < numberOfSampleChambers; i++)
            {
                runSampleMetaDataGridView.Rows.Add($"{i + 1}", $"Sample {i + 1}");
            }
        }

        private void AddRunAssaysMetaDefaultData()
        {
            Cartridge cartridge = new Cartridge();
            cartridge = cartridgeOptions.GetCartridgeFromName(configuration.DefaultCartridge);
            // Setup Assays Meta DataGridView
            int numberOfAssayChambers = cartridge.NumberofAssayChambers;
            for (int i = 0; i < numberOfAssayChambers; i++)
            {
                runAssayMetaDataGridView.Rows.Add($"{i + 1}", $"Assay {i + 1}");
            }
        }

        /// <summary>
        /// Add default data to the Run Imaging Setup Data Grid View upon loading the Form
        /// </summary>
        private void AddRunImagingSetupDefaultData()
        {
            ImagingSetupData runImagingSetupData = new ImagingSetupData();
            runImagingSetupDataGridView.Rows.Add("X0 (\u03BCS)", defaultScanningOption.X0);
            runImagingSetupDataGridView.Rows.Add("Y0 (\u03BCS)", defaultScanningOption.Y0);
            runImagingSetupDataGridView.Rows.Add("Z0 (\u03BCS)", defaultScanningOption.Z0);
            runImagingSetupDataGridView.Rows.Add("FOV dX (\u03BCS)", defaultScanningOption.FOVdX);
            runImagingSetupDataGridView.Rows.Add("dY (\u03BCS)", defaultScanningOption.dY);
            runImagingSetupDataGridView.Rows.Add("Rotational Offset (\u00B0)", defaultScanningOption.RotationalOffset);
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
            runImagingSetupDataGridView.Rows.Add("Cy5 Exposure (\u03BCs)", runImagingSetupData.ExposureCy5);
            runImagingSetupDataGridView.Rows.Add("FAM Exposure (\u03BCs)", runImagingSetupData.ExposureFAM);
            runImagingSetupDataGridView.Rows.Add("HEX Exposure (\u03BCs)", runImagingSetupData.ExposureHEX);
            runImagingSetupDataGridView.Rows.Add("Atto Exposure (\u03BCs)", runImagingSetupData.ExposureAtto);
            runImagingSetupDataGridView.Rows.Add("Alexa Exposure (\u03BCs)", runImagingSetupData.ExposureAlexa);
            runImagingSetupDataGridView.Rows.Add("Cy5.5 Exposure (\u03BCs)", runImagingSetupData.ExposureCy5p5);
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
            DataGridViewComboBoxCell dataGridViewComboBoxCellTempEnabledA = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellTempEnabledB = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellTempEnabledC = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellTempEnabledD = new DataGridViewComboBoxCell();
            dataGridViewComboBoxCellTempEnabledA.Items.AddRange(new object[] { "Yes", "No" });
            dataGridViewComboBoxCellTempEnabledB.Items.AddRange(new object[] { "Yes", "No" });
            dataGridViewComboBoxCellTempEnabledC.Items.AddRange(new object[] { "Yes", "No" });
            dataGridViewComboBoxCellTempEnabledD.Items.AddRange(new object[] { "Yes", "No" });
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledA = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledB = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledC = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledD = new DataGridViewComboBoxCell();
            dataGridViewComboBoxCellFanEnabledA.Items.AddRange(new object[] { "Yes", "No" });
            dataGridViewComboBoxCellFanEnabledB.Items.AddRange(new object[] { "Yes", "No" });
            dataGridViewComboBoxCellFanEnabledC.Items.AddRange(new object[] { "Yes", "No" });
            dataGridViewComboBoxCellFanEnabledD.Items.AddRange(new object[] { "Yes", "No" });
            controlTECsDataGridView.Rows.Add("State", "Not Connected", "Not Connected", "Not Connected", "Not Connected");
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("IO", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            // NOTE: Pull the Firmware version from the TECs on Form loading, leave these defaults here. 
            controlTECsDataGridView.Rows.Add("Version", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("New Object Temp (\u00B0C)");
            controlTECsDataGridView.Rows.Add("Actual Object Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Target Object Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Temp Enabled");
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[1] = dataGridViewComboBoxCellTempEnabledA;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[2] = dataGridViewComboBoxCellTempEnabledB;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[3] = dataGridViewComboBoxCellTempEnabledC;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[4] = dataGridViewComboBoxCellTempEnabledD;
            controlTECsDataGridView.Rows.Add("Object Upper Error Threshold (\u00B0C)",
                configuration.TECAObjectUpperErrorThreshold,
                configuration.TECBObjectUpperErrorThreshold,
                configuration.TECCObjectUpperErrorThreshold,
                configuration.TECDObjectUpperErrorThreshold);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Object Lower Error Threshold (\u00B0C)",
                configuration.TECAObjectLowerErrorThreshold,
                configuration.TECBObjectLowerErrorThreshold,
                configuration.TECCObjectLowerErrorThreshold,
                configuration.TECDObjectLowerErrorThreshold);
            controlTECsDataGridView.Rows.Add("Sink Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
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
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Fan on Temp (\u00B0C)",
                configuration.TECAFanOnTemp,
                configuration.TECBFanOnTemp,
                configuration.TECCFanOnTemp,
                configuration.TECDFanOnTemp);
            controlTECsDataGridView.Rows.Add("Fan Control Enabled");
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[1] = dataGridViewComboBoxCellFanEnabledA;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[2] = dataGridViewComboBoxCellFanEnabledB;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[3] = dataGridViewComboBoxCellFanEnabledC;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[4] = dataGridViewComboBoxCellFanEnabledD;
            controlTECsDataGridView.Rows.Add("Current (A)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Max Current (A)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Voltage (V)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
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
            // FIXME: Use the configuration file to set this default value for the heater
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1].Value = "A";
            imagingScanParametersDataGridView.Rows.Add("Elastomer");
            scanParameterData.Elastomer = elastomerOptions.GetOptionNamesComboBoxCell();
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1] = scanParameterData.Elastomer;
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1].Value = configuration.DefaultElastomer;
            imagingScanParametersDataGridView.Rows.Add("Bergquist");
            scanParameterData.Bergquist = bergquistOptions.GetOptionNamesComboBoxCell();
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1] = scanParameterData.Bergquist;
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1].Value = configuration.DefaultBergquist;
            imagingScanParametersDataGridView.Rows.Add("Cartridge");
            scanParameterData.Cartridge = cartridgeOptions.GetOptionNamesComboBoxCell();
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1] = scanParameterData.Cartridge;
            imagingScanParametersDataGridView.Rows[imagingScanParametersDataGridView.Rows.Count - 1].Cells[1].Value = configuration.DefaultCartridge;
            imagingScanParametersDataGridView.Rows.Add("Glass Offset (mm)", configuration.DefaultGlassOffset);
            imagingScanParametersDataGridView.Rows.Add("x0 (\u03BCS)", defaultScanningOption.X0);
            imagingScanParametersDataGridView.Rows.Add("y0 (\u03BCS)", defaultScanningOption.Y0);
            imagingScanParametersDataGridView.Rows.Add("z0 (\u03BCS)", defaultScanningOption.Z0);
            imagingScanParametersDataGridView.Rows.Add("FOV dX (\u03BCS)", defaultScanningOption.FOVdX);
            imagingScanParametersDataGridView.Rows.Add("dY (\u03BCS)", defaultScanningOption.dY);
            imagingScanParametersDataGridView.Rows.Add("Rotational Offset (\u00B0)", defaultScanningOption.RotationalOffset);
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

            // Setup the Glass Offset ComboBox
            foreach (var glassOffsetOption in scanningOptions.GlassOffsets)
            {
                configureGlassOffsetComboBox.Items.Add(glassOffsetOption.ToString());
            }
            configureGlassOffsetComboBox.SelectedItem = configuration.DefaultGlassOffset;
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

        private void controlMotorsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // If input is being added to either the position or speed for a motor get the name of the motor
            if (dataGridViewManager.GetColumnNameFromIndex(controlMotorsDataGridView, e.ColumnIndex) == "Position (\u03BCS)" ||
                dataGridViewManager.GetColumnNameFromIndex(controlMotorsDataGridView, e.ColumnIndex) == "Speed (\u03BCS/s)")
            {
                // Update the selected item in the Motor ComboBox on the Control tab
                string name = controlMotorsDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                controlMotorsComboBox.SelectedItem = name;
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
                    // Setup Sample and Assays
                    int numberOfSampleChambers = cartridge.NumberofSampleChambers;
                    runSampleMetaDataGridView.Rows.Clear();
                    for (int i = 0; i < numberOfSampleChambers; i++)
                    {
                        runSampleMetaDataGridView.Rows.Add($"{i + 1}", $"Sample {i + 1}");
                    }
                    int numberOfAssayChambers = cartridge.NumberofAssayChambers;
                    runAssayMetaDataGridView.Rows.Clear();
                    for (int i = 0; i < numberOfAssayChambers; i++)
                    {
                        runAssayMetaDataGridView.Rows.Add($"{i + 1}", $"Assay {i + 1}");
                    }
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
                    // Setup Sample and Assays
                    int numberOfSampleChambers = cartridge.NumberofSampleChambers;
                    runSampleMetaDataGridView.Rows.Clear();
                    for (int i = 0; i < numberOfSampleChambers; i++)
                    {
                        runSampleMetaDataGridView.Rows.Add($"{i + 1}", $"Sample {i+1}");
                    }
                    int numberOfAssayChambers = cartridge.NumberofAssayChambers;
                    runAssayMetaDataGridView.Rows.Clear();
                    for (int i = 0; i< numberOfAssayChambers; i++)
                    {
                        runAssayMetaDataGridView.Rows.Add($"{i + 1}", $"Assay {i+1}");
                    }
                }
            }
        }

        /// <summary>
        /// Form loading event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_Load(object sender, EventArgs e)
        {
            // Change the default cell selection for data grid views on form loading
            homeMotorsDataGridView.ClearSelection();
            homeTECsDataGridView.ClearSelection();
            homeCameraDataGridView.ClearSelection();
            homeLEDsDataGridView.ClearSelection();
            runExperimentDataGridView.CurrentCell = runExperimentDataGridView.Rows[0].Cells[1];
            runImagingSetupDataGridView.CurrentCell = runImagingSetupDataGridView.Rows[0].Cells[1];

            // Obtain single use data
            SingleUseDataLoad();

            // Load in Configure Combo Boxes
            LoadConfigureComboBoxInitialData();
        }

        private async void SingleUseDataLoad()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(SingleUseDataLoad));
            }
            else
            {
                string xMotorVersion = await xMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "x", xMotorVersion);
                string yMotorVersion = await yMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "y", yMotorVersion);
                string zMotorVersion = await zMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "z", zMotorVersion);
                string filterWheelMotorVersion = await filterWheelMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Filter Wheel", filterWheelMotorVersion);
                string trayABMotorVersion = await trayABMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Tray AB", trayABMotorVersion);
                string trayCDMotorVersion = await trayCDMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Tray CD", trayCDMotorVersion);
                string tecAFirmwareVersion = await tecA.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC A", "Version", tecAFirmwareVersion);
                string tecBFirmwareVersion = await tecB.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC B", "Version", tecBFirmwareVersion);
                string tecCFirmwareVersion = await tecC.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC C", "Version", tecCFirmwareVersion);
                string tecDFirmwareVersion = await tecD.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC D", "Version", tecDFirmwareVersion);
                string clampAMotorVersion = await clampAMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp A", clampAMotorVersion);
                string clampBMotorVersion = await clampBMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp B", clampBMotorVersion);
                string clampCMotorVersion = await clampCMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp C", clampCMotorVersion);
                string clampDMotorVersion = await clampDMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp D", clampDMotorVersion);
            }
        }

        private void LoadConfigureComboBoxInitialData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadConfigureComboBoxInitialData));
            }
            else
            {
                List<Cartridge> cartridgeOptionsList = cartridgeOptions.Options;
                foreach (var option in cartridgeOptionsList)
                {
                    configureCartridgeComboBox.Items.Add(option.Name);
                }
                configureCartridgeComboBox.SelectedItem = cartridgeOptionsList.First().Name;
                List<Elastomer> elastomerOptionsList = elastomerOptions.Options;
                foreach (var option in elastomerOptionsList)
                {
                    configureElastomerComboBox.Items.Add(option.Name);
                }
                configureElastomerComboBox.SelectedItem = elastomerOptionsList.First().Name;
                List<Bergquist> bergquistOptionsList = bergquistOptions.Options;
                foreach (var option in bergquistOptionsList)
                {
                    configureBergquistComboBox.Items.Add(option.Name);
                }
                configureBergquistComboBox.SelectedItem = bergquistOptionsList.First().Name;
            }
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
            if (motor != null)
            {
                if (motor.Connected)
                {
                    await motor.HomeAsync();
                }
                else
                {
                    MessageBox.Show("Motor is not connected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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
                string stepTimeUnits = addStepForm.StepTimeUnits;
                int index = protocol.Count;
                ThermocyclingProtocolStep step = new ThermocyclingProtocolStep();
                step.Temperature = stepTemperature;
                step.TypeName = stepTypeName;
                step.Time = stepTime;
                step.TimeUnits = stepTimeUnits;
                step.Index = index;
                protocol.AddStep(step);
                plotManager.AddStep(step);
            }
        }

        private void thermocyclingAddGoToButton_Click(object sender, EventArgs e)
        {
            // Open a ThermocyclingProtocolAddGoToForm window
            ThermocyclingProtocolAddGotoForm addGoToForm = new ThermocyclingProtocolAddGotoForm(plotManager);
            try
            {
                if (addGoToForm.ShowDialog() == DialogResult.OK)
                {
                    int stepNumber = addGoToForm.StepNumber;
                    int cycleCount = addGoToForm.CycleCount;
                    ThermocyclingProtocolGoToStep step = new ThermocyclingProtocolGoToStep(stepNumber, cycleCount);
                    step.Index = protocol.Count;
                    protocol.AddStep(step);
                    plotManager.AddStep(step);
                }
            }
            catch (ObjectDisposedException ex)
            {
                return;
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
            //MessageBox.Show($"{e.X}, {e.Y}");
            // Invoke the plot manager's mouse down handler
            // FIXME: Clicking on the Plot View causes the GUI to crash, this needs to be resolved
            // Create an OxyPlot mouse event args
            var oxyArgs = new OxyMouseDownEventArgs
            {
                Position = screenPoint,
                ChangedButton = OxyMouseButton.Left // assumes a left mouse click
            };
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
            if (plotManager.StepCount > 0)
            {
                ThermocyclingProtocolEditStepForm editStepForm = new ThermocyclingProtocolEditStepForm(plotManager);
                if (editStepForm.ShowDialog() == DialogResult.OK)
                {
                    int stepNumber = editStepForm.StepNumber;
                    double stepTemperature = editStepForm.StepTemperature;
                    if (editStepForm.StepTime.ToString() == "\u221E")
                    {
                        ThermocyclingProtocolHoldStep editedStep = new ThermocyclingProtocolHoldStep(stepTemperature);
                        protocol.EditStep(stepNumber - 1, editedStep);
                        return;
                    }
                    else
                    {
                        double stepTime = editStepForm.StepTime;
                        string stepTimeUnits = editStepForm.StepTimeUnits;
                        ThermocyclingProtocolSetStep editedStep = new ThermocyclingProtocolSetStep(temperature: stepTemperature, time: stepTime, timeUnits: stepTimeUnits);
                        protocol.EditStep(stepNumber - 1, editedStep);
                        return;
                    }
                }
            }       
        }

        private void thermocyclingLoadProtocolButton_Click(object sender, EventArgs e)
        {
            FileSearcher fileSearcher = new FileSearcher();
            // FIXME: take the initial directory default value from a config file 
            var filePath = fileSearcher.GetLoadFilePath(initialDirectory: defaultProtocolDirectory);
            if (filePath != null)
            {
                // FIXME: ClearPlot dos not behave as expected, x axis does not reset to 0
                plotManager.ClearPlot();
                protocol = protocolManager.LoadProtocol(filePath);
                plotManager.PlotProtocol(protocol);
                // Set the "Plotted Protocol Name"
                thermocyclingProtocolNameTextBox.Text = protocol.Name;
            }
        }

        private void thermocyclingSaveProtocolButton_Click(object sender, EventArgs e)
        {
            string protocolName = thermocyclingProtocolNameTextBox.Text;
            if (protocolName == string.Empty)
            {
                MessageBox.Show("Cannot save this protocol, it must be given a name in the 'Plotted Protocol' text box.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            protocol.SetName(protocolName);
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
            DateTime projectedEndTime = DateTime.Now;
            // Get Start Time Row
            DataGridViewRow r = new DataGridViewRow();
            // Get the Start Time row
            r = dataGridViewManager.GetRowFromName(runExperimentDataGridView, "Start Time (HH:mm:ss)");
            r.Cells[1].Value = DateTime.Now.ToString("HH:mm:ss");
            // Get the Start Date row
            r = dataGridViewManager.GetRowFromName(runExperimentDataGridView, "Start Date (MM/dd/YYYY)");
            r.Cells[1].Value = DateTime.Now.ToString("MM/dd/yyyy");
            // Get the Protocol Name 
            string protocolName = dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Protocol", runExperimentDataGridView);
            // Load in the protocol 
            ThermocyclingProtocol selectedProtocol = protocolManager.GetProtocolFromName(protocolName);
            // Calculate the Protocol Time in seconds based on the Protocol Name
            protocolTimeInSeconds = selectedProtocol.GetTimeInSeconds();
            projectedEndTime = projectedEndTime.AddSeconds(protocolTimeInSeconds);
            // Determine if imaging before and/or after
            bool imageBefore = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image Before", runImagingSetupDataGridView) == "Yes") ? true : false; 
            bool imageAfter = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image After", runImagingSetupDataGridView) == "Yes") ? true : false;
            // Determine the number of samples
            int numberOfSamplesToImage = 0;
            // FIXME: The number of samples does not change
            foreach (DataGridViewRow row in runSampleMetaDataGridView.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    numberOfSamplesToImage++;
                }
            }
            // Determine the number of assays
            int numberOfAssaysToImage = 0;
            // FIXME: The number of assays does not change
            foreach (DataGridViewRow row in runAssayMetaDataGridView.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    numberOfAssaysToImage++;
                }
            }
            // Determine if the user is imaging in any of the channels
            int exposureTime = 0;
            TimeUnit timeUnitResource = new TimeUnit();
            bool imageInCy5 = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image in Cy5", runImagingSetupDataGridView) == "Yes") ? true : false;
            if (imageInCy5)
            {
                if (int.TryParse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Cy5 Exposure (μs)", runImagingSetupDataGridView), out _))
                {
                    exposureTime = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Cy5 Exposure (\u03BCs)", runImagingSetupDataGridView));
                }
                else
                {
                    exposureTime = 0;
                }
                projectedEndTime = projectedEndTime.AddSeconds(timeUnitResource.ConvertMicrosecondsToSeconds(exposureTime));
            }
            bool imageInFAM = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image in FAM", runImagingSetupDataGridView) == "Yes") ? true : false;
            if (imageInFAM)
            {
                if (int.TryParse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "FAM Exposure (μs)", runImagingSetupDataGridView), out _))
                {
                    exposureTime = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "FAM Exposure (\u03BCs)", runImagingSetupDataGridView));
                }
                else
                {
                    exposureTime = 0;
                }
                projectedEndTime = projectedEndTime.AddSeconds(timeUnitResource.ConvertMicrosecondsToSeconds(exposureTime));
            }
            bool imageInHEX = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image in HEX", runImagingSetupDataGridView) == "Yes") ? true : false;
            if (imageInHEX)
            {
                if (int.TryParse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "HEX Exposure (μs)", runImagingSetupDataGridView), out _))
                {
                    exposureTime = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "HEX Exposure (\u03BCs)", runImagingSetupDataGridView));
                }
                else
                {
                    exposureTime = 0;
                }
                projectedEndTime = projectedEndTime.AddSeconds(timeUnitResource.ConvertMicrosecondsToSeconds(exposureTime));
            }
            bool imageInAtto = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image in Atto", runImagingSetupDataGridView).ToString() == "Yes") ? true : false;
            if (imageInAtto)
            {
                if (int.TryParse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Atto Exposure (μs)", runImagingSetupDataGridView), out _))
                {
                    exposureTime = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Atto Exposure (\u03BCs)", runImagingSetupDataGridView));
                }
                else
                {
                    exposureTime = 0;
                }
                projectedEndTime = projectedEndTime.AddSeconds(timeUnitResource.ConvertMicrosecondsToSeconds(exposureTime));
            }
            bool imageInAlexa = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image in Alexa", runImagingSetupDataGridView) == "Yes") ? true : false;
            if (imageInAlexa)
            {
                if (int.TryParse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Alexa Exposure (μs)", runImagingSetupDataGridView), out _))
                {
                    exposureTime = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Alexa Exposure (\u03BCs)", runImagingSetupDataGridView));
                }
                else
                {
                    exposureTime = 0;
                }
                projectedEndTime = projectedEndTime.AddSeconds(timeUnitResource.ConvertMicrosecondsToSeconds(exposureTime));
            }
            bool imageInCy5p5 = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Image in Cy5.5", runImagingSetupDataGridView) == "Yes") ? true : false;
            if (imageInCy5p5)
            {
                if (int.TryParse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Cy5.5 Exposure (μs)", runImagingSetupDataGridView), out _))
                {
                    exposureTime = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Cy5.5 Exposure (\u03BCs)", runImagingSetupDataGridView));
                }
                else
                {
                    exposureTime = 0;
                }
                projectedEndTime = projectedEndTime.AddSeconds(timeUnitResource.ConvertMicrosecondsToSeconds(exposureTime));
            }
            // Calculate the additional time from imaging (before and/or after) based on the number of samples and assays
            int timeInSecondsToImage = numberOfSamplesToImage * numberOfAssaysToImage * configuration.EstimateAssayCaptureTimeSeconds;
            if (imageBefore)
            {
                projectedEndTime = projectedEndTime.AddSeconds(timeInSecondsToImage);
            }
            if (imageAfter)
            {
                projectedEndTime = projectedEndTime.AddSeconds(timeInSecondsToImage);
            }
            // Get the End Time row
            r = dataGridViewManager.GetRowFromName(runExperimentDataGridView, "Projected End Time (HH:mm:ss)");
            r.Cells[1].Value = projectedEndTime.ToString("HH:mm:ss");
            // Get the ENd Date row
            r = dataGridViewManager.GetRowFromName(runExperimentDataGridView, "Projected End Date (MM/dd/YYYY)");
            r.Cells[1].Value = projectedEndTime.ToString("MM/dd/yyyy");
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
                double temp;
                var cellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName("TEC A", "New Object Temp (\u00B0C)", controlTECsDataGridView);
                if (!double.TryParse(cellValue, out _))
                {
                    MessageBox.Show("New Object Temp (\u00B0C) must be a number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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
            await reportManager.AddSubSectionAsync("Experiment Data",
                "Experiment data includes all information regarding the experimental setup on the instrument."
                + " This includes the experiment name, the instrument's configuration, dPCR cartridge information, as well as any excess materials used in the run.");
            await reportManager.AddTableAsync(runExperimentDataGridView);
            await reportManager.AddSubSectionAsync("Cartridge Layout",
                "Cartridge layout summarizes information regarding the sample and assay loading in the cartridge. This includes the names for each sample loaded"
                + " as well as for each assay loaded. An empty cell is an indication of a sample or assay section that was not loaded for this run.");
            await reportManager.AddSubSectionAsync("Imaging Setup",
                "Imaging setup includes all information regarding the imaging performed before, during, and after the run.");
            await reportManager.AddTableAsync(runImagingSetupDataGridView);
            await reportManager.AddSubSectionAsync("Initial Conditions and Constants",
                "These are the initial conditions determined at the onset of the run, covering a range of values that implicitly or explicitly affect the run's outcome."
                + " The initial conditions include information pertaining to the TECs initial object and sink temperature which could potentially affect ramp rates for the"
                + " entire run. Other initial conditions are as influential, but may slightly extend the projected run time, such as error states on the TEC which"
                + " require a reset.");
            await reportManager.AddSubSectionAsync("Versions",
                "Versions include all version pertaining to the Firmware loaded on each submodule and module board (including the Firmware loaded on the"
                + " TEC boards). Software versions are also included, these are version numbers for the Image Analysis Model used in post-processing, the"
                + " version of the Autofocus model, GUI, Backend, and API.");
            await reportManager.CloseAsync();
            // TODO: Email the user that the run is complete and a copy of the report
        }

        private async void logoutButton_Click(object sender, EventArgs e)
        {
            var p = await xMotor.GetPositionAsync();
            MessageBox.Show(p.ToString());
        }

        /// <summary>
        /// Cell Validating handler for the ControlTECsDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Ensure that the Actual Object Temp is a valid number
            string? rowName = dataGridViewManager.GetRowNameFromIndex(controlTECsDataGridView, e.RowIndex);
            if (rowName != null)
            {
                if (rowName == "New Object Temp (\u00B0C)" && e.ColumnIndex > 0)
                {
                    var cellValue = controlTECsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (cellValue != null)
                    {
                        // Format the cell based on the value
                        if (!double.TryParse(cellValue.ToString(), out _))
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Black;
                        }
                    }
                }
                else if (rowName == "Temp Enabled" && e.ColumnIndex > 0)
                {
                    var cellValue = controlTECsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (cellValue != null)
                    {
                        // Format the cell based on the value
                        // FIXME: This does not work (the ComboBox does not change color)
                        e.CellStyle.BackColor = Color.Yellow;
                        //dataGridViewManager.ChangeCellStyleBackgroundBasedOnOption(dataGridViewComboBoxCellTempEnabledA, Color.White, Color.Yellow, "Yes", "No");
                    }
                }
            }
        }

        private void configureCartridgeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the cartridge options to get the most update to date results
            cartridgeOptions.Update();
            // FIXME: This needs to be resolved, this region of code causes issues (Stackoverflow)
            // Reload in the data to the comboboxcell
            //configureCartridgeComboBox.Items.Clear();
            //foreach (var option in cartridgeOptions.Options)
            //{
            //    configureCartridgeComboBox.Items.Add(option.Name);
            //}
            //configureCartridgeComboBox.SelectedItem = cartridgeOptions.Options.First().Name;
            // Get the Cartridge Name
            string cartridgeName = configureCartridgeComboBox.SelectedItem.ToString();
            // Get the Cartridge instance from the name
            Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(cartridgeName);
            // Fill in the DataGridView for the cartridge on the Configure Tab
            configureCartridgesDataGridView.Rows.Clear();
            configureCartridgesDataGridView.Rows.Add();
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: configureCartridgesDataGridView, cellValue: cartridge.Name, rowIndex: 0, columnName: "Name");
            // FIXME: Make the Partition Type column cell a DataGridViewComboBox
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: configureCartridgesDataGridView, cellValue: cartridge.PartitionType, rowIndex: 0, columnName: "Partition Type");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: configureCartridgesDataGridView, cellValue: cartridge.NumberofSampleChambers.ToString(), rowIndex: 0, columnName: "Number of Samples");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: configureCartridgesDataGridView, cellValue: cartridge.NumberofAssayChambers.ToString(), rowIndex: 0, columnName: "Number of Assays");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: configureCartridgesDataGridView, cellValue: cartridge.Length.ToString(), rowIndex: 0, columnName: "Length (mm)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: configureCartridgesDataGridView, cellValue: cartridge.Width.ToString(), rowIndex: 0, columnName: "Width (mm)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: configureCartridgesDataGridView, cellValue: cartridge.Height.ToString(), rowIndex: 0, columnName: "Height (mm)");
        }

        private void configureSaveNewCartridgeButton_Click(object sender, EventArgs e)
        {
            // Ensure all cells are filled with appropriate inputs
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Name");
            string? partitionType = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Partition Type");
            int numberOfSampleChambers;
            if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Number of Samples"), out numberOfSampleChambers))
            {
                MessageBox.Show("Cannot have a non-integer value for the number of samples in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            int numberOfAssayChambers;
            if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Number of Assays"), out numberOfAssayChambers))
            {
                MessageBox.Show("Cannot have a non-integer value for the number of assays in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            double length;
            if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Length (mm)"), out length))
            {
                MessageBox.Show("Cannot have a non-real value for the length of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            double width;
            if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Width (mm)"), out width))
            {
                MessageBox.Show("Cannot have a non-real value for the width of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            double height;
            if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Height (mm)"), out height))
            {
                MessageBox.Show("Cannot have a non-real value for the height of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            if (name == string.Empty)
            {
                MessageBox.Show("A cartridge must have a name in order to save it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            if (partitionType == string.Empty)
            {
                MessageBox.Show("A cartridge must have a valid partition type in order to save it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            if (name == string.Empty)
            {
                MessageBox.Show("A cartridge must have a name", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            if (cartridgeOptions.NameInOptions(name))
            {
                MessageBox.Show($"A cartridge named {name} already exists, choose a unique name.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            // TODO: Make sure that the Cartridge name does not already exist and is unique
            // Add this cartridge to the Cartridges XML data file
            Cartridge cartridge = new Cartridge();
            cartridge.Name = name;
            cartridge.PartitionType = partitionType;
            cartridge.NumberofSampleChambers = numberOfSampleChambers;
            cartridge.NumberofAssayChambers = numberOfAssayChambers;
            cartridge.Length = length;
            cartridge.Width = width;
            cartridge.Height = height;
            cartridgeOptions.SaveNewCartridge(cartridge);
        }

        private void configureDeleteCartridgeButton_Click(object sender, EventArgs e)
        {
            // Get the name of cartridge to be deleted
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Name");
            if (name != null)
            {
                // Delete the cartridge from the XML data
                cartridgeOptions.DeleteCartridgeByName(name);
            }
        }

        private void configureUpdateCartridgeDataButton_Click(object sender, EventArgs e)
        {
            // Get the name of the cartridge to be updated
            string? oldName = configureCartridgeComboBox.SelectedItem.ToString();
            if (oldName != null)
            {
                string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Name");
                string? partitionType = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Partition Type");
                int numberOfSampleChambers;
                if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Number of Samples"), out numberOfSampleChambers))
                {
                    MessageBox.Show("Cannot have a non-integer value for the number of samples in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                int numberOfAssayChambers;
                if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Number of Assays"), out numberOfAssayChambers))
                {
                    MessageBox.Show("Cannot have a non-integer value for the number of assays in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                double length;
                if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Length (mm)"), out length))
                {
                    MessageBox.Show("Cannot have a non-real value for the length of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                double width;
                if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Width (mm)"), out width))
                {
                    MessageBox.Show("Cannot have a non-real value for the width of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                double height;
                if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: configureCartridgesDataGridView, rowIndex: 0, columnName: "Height (mm)"), out height))
                {
                    MessageBox.Show("Cannot have a non-real value for the height of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                if (name == string.Empty)
                {
                    MessageBox.Show("A cartridge must have a name in order to save it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                if (partitionType == string.Empty)
                {
                    MessageBox.Show("A cartridge must have a valid partition type in order to save it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                if (name == string.Empty)
                {
                    MessageBox.Show("A cartridge must have a name", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                if (!cartridgeOptions.NameInOptions(oldName))
                {
                    MessageBox.Show($"A cartridge named {oldName} does not exists,.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                // TODO: Make sure that the Cartridge name does not already exist and is unique
                // Add this cartridge to the Cartridges XML data file
                Cartridge cartridge = new Cartridge();
                cartridge.Name = name;
                cartridge.PartitionType = partitionType;
                cartridge.NumberofSampleChambers = numberOfSampleChambers;
                cartridge.NumberofAssayChambers = numberOfAssayChambers;
                cartridge.Length = length;
                cartridge.Width = width;
                cartridge.Height = height;
                // Update the cartridge from the XML data
                cartridgeOptions.UpdateCartridge(name, cartridge);
            }
        }

        private void configureElastomerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the elastomer options to get the most up to date options
            elastomerOptions.Update();
            // TODO: update the combo box options
            // Get the elastomer from the name
            string? name = configureElastomerComboBox.SelectedItem.ToString();
            if (name != string.Empty)
            {
                Elastomer? elastomer = elastomerOptions.GetElastomerFromName(name);
                if (elastomer != null)
                {
                    configureElastomerDataGridView.Rows.Clear();
                    configureElastomerDataGridView.Rows.Add();
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(configureElastomerDataGridView, elastomer.Name, 0, "Name");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(configureElastomerDataGridView, elastomer.Thickness.ToString(), 0, "Thickness (mm)");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(configureElastomerDataGridView, elastomer.ThermalCoefficient.ToString(), 0, "Thermal Coefficient (W/m K)");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(configureElastomerDataGridView, elastomer.ShoreHardness.ToString(), 0, "Shore Hardness");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(configureElastomerDataGridView, elastomer.Mold.ToString(), 0, "Mold");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(configureElastomerDataGridView, elastomer.Film.ToString(), 0, "Film");
                }
            }
        }

        private void configureSaveNewElastomerButton_Click(object sender, EventArgs e)
        {
            // Update the elastomer options to get the most up to date options
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(configureElastomerDataGridView, "Name", 0);
            string? thickness = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(configureElastomerDataGridView, "Thickness (mm)", 0);
            string? thermalCoefficient = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(configureElastomerDataGridView, "Thermal Coefficient (W/m K)", 0);
            string? shoreHardness = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(configureElastomerDataGridView, "Shore Hardness", 0);
            string? mold = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(configureElastomerDataGridView, "Mold", 0);
            string? film = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(configureElastomerDataGridView, "Film", 0);
            string?[] properties = { name, thickness, thermalCoefficient, shoreHardness, mold, film };
            if (properties.Contains(string.Empty) || properties.Contains(null))
            {
                MessageBox.Show("An elastomer property is missing, save failed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Elastomer elastomer = new Elastomer();
                if (elastomerOptions.NameInOptions(name))
                {
                    MessageBox.Show($"{name} already exists, choose a different name, save failed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                else
                {
                    elastomer.Name = name;
                }
                if (double.TryParse(thickness, out _))
                {
                    elastomer.Thickness = double.Parse(thickness);
                }
                else
                {
                    MessageBox.Show("Thickness must be a real value, save failed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                if (double.TryParse(thermalCoefficient, out _))
                {
                    elastomer.ThermalCoefficient = double.Parse(thermalCoefficient);
                }
                else
                {
                    MessageBox.Show("Thermal Coefficient must be a real value, save failed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                if (double.TryParse(shoreHardness, out _))
                {
                    elastomer.ShoreHardness = double.Parse(shoreHardness);
                }
                else
                {
                    MessageBox.Show("Shore Hardness must be a real value, save failed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                elastomer.Film = film;
                elastomer.Mold = mold;
                // Add the new elastomer
                elastomerOptions.SaveNewElastomer(elastomer);
            }
        }

        private void configureDeleteElastomerButton_Click(object sender, EventArgs e)
        {
            // Get the name of the elastomer to be deleted
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(configureElastomerDataGridView, "Name", 0);
            if (name != null)
            {
                // Delete the elastomer from the XML data
                elastomerOptions.DeleteElastomerByName(name);
            }
        }

        private void configureUpdateElastomerData_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to update the Elastomer data has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void configureBergquistDataButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to update the Bergquist data has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void configureSaveNewBergquistButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to save a new Bergquist has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void configureDeleteBergquistButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to delete a Bergquist has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void configureGlassOffsetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the Configure tab's Image Scanning data grid view
            configureImageScanningDataGridView.Rows.Clear();
            // Make sure value is valid
            if (double.TryParse(configureGlassOffsetComboBox.SelectedItem.ToString(), out _))
            {
                scanningOptions.ReadInScanningData(dataGridView: configureImageScanningDataGridView,
                    cartridgeName: configureCartridgeComboBox.SelectedItem.ToString(), glassOffset: double.Parse(configureGlassOffsetComboBox.SelectedItem.ToString()),
                    elastomerName: configureElastomerComboBox.SelectedItem.ToString(), configureBergquistComboBox.SelectedItem.ToString());
            }
        }
    }
}