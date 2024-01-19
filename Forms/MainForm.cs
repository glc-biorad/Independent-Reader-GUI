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
using System.Diagnostics;
using System.Globalization;
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
        private ScanManager scanManager;
        private EmailManager emailManager;
        private TimerManager runExperimentDataTimerManager;
        private TimerManager controlTabTimerManager;
        private TimerManager mainFormFastTimerManager;
        private TimerManager mainFormSlowTimerManager;
        private TimerManager thermocyclingStatusTimerManager;
        private PictureBoxManager pictureBoxManager;
        private string defaultThermocyclingProtocolDirectory;
        private string defaultAssayProtocolDirectory;
        private ThermocyclingProtocol protocol = new ThermocyclingProtocol();
        private ThermocyclingProtocolPlotManager plotManager = new ThermocyclingProtocolPlotManager();
        private ThermocyclingProtocolManager protocolManager;
        private AssayProtocolManager assayProtocolManager;
        private TECManager tecManager;
        private MotorsManager motorManager;
        private ChassisManager chassisManager;
        private CartridgeOptions cartridgeOptions;
        private ElastomerOptions elastomerOptions;
        private BergquistOptions bergquistOptions;
        private ScanningOptions scanningOptions;
        private ScanningOption defaultScanningOption;
        private FLIRCameraManager cameraManager;
        private string runExperimentProtocolName = string.Empty;
        private double runExperimentProtocolTime = 0.0;
        private TEC tecA;
        private TEC tecB;
        private TEC tecC;
        private TEC tecD;
        private Chassis chassis;
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
        private LED Cy5;
        private LED FAM;
        private LED HEX;
        private LED Atto;
        private LED Alexa;
        private LED Cy5p5;
        private LEDManager ledManager;
        private ClampPressureCalculator clampPressureCalculator = new ClampPressureCalculator();

        /// <summary>
        /// Initialization of the Form
        /// </summary>
        public independentReaderForm()
        {
            InitializeComponent();

            // TODO: Replace with a .Connect() method
            Task.Run(async () => await apiManager.CheckAPIConnection()).Wait();
            if (!apiManager.Connected)
            {
                homeAPIConnectedRadioButton.BackColor = Color.Red;
            }
            // Connect to the FLIR Camera
            cameraManager = new FLIRCameraManager(imagingPictureBox);
            cameraManager.Connect();
            // Connect to the TECs
            tecA = new TEC(id: configuration.TECAAddress, name: "TEC A", apiManager: apiManager, configuration: configuration);
            tecB = new TEC(id: configuration.TECBAddress, name: "TEC B", apiManager: apiManager, configuration: configuration);
            tecC = new TEC(id: configuration.TECCAddress, name: "TEC C", apiManager: apiManager, configuration: configuration);
            tecD = new TEC(id: configuration.TECDAddress, name: "TEC D", apiManager: apiManager, configuration: configuration);
            // Setup the TEC Manager
            tecManager = new TECManager(tecA, tecB, tecC, tecD);
            // Intialize the TEC Parameters
            Task.Run(async () => await tecManager.InitializeTECParameters()).Wait();
            // Start the TEC Manager Command Queue Processing
            Task.Run(async () => await tecManager.ProcessCommandQueues());
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
            motorManager = new MotorsManager(xMotor, yMotor, zMotor, filterWheelMotor, trayABMotor, trayCDMotor, clampAMotor, clampBMotor, clampCMotor, clampDMotor);
            Task.Run(async () => await motorManager.InitializeMotorParameters()).Wait();
            Task.Run(async () => await motorManager.ProcessCommandQueues());

            //
            // Setup the LEDs, their Manager, and start processing the LED command queue
            //
            Cy5 = new LED("Cy5", configuration, apiManager);
            FAM = new LED("FAM", configuration, apiManager);
            HEX = new LED("HEX", configuration, apiManager);
            Atto = new LED("Atto", configuration, apiManager);
            Alexa = new LED("Alexa", configuration, apiManager);
            Cy5p5 = new LED("Cy5.5", configuration, apiManager);
            ledManager = new LEDManager(Cy5, FAM, HEX, Atto, Alexa, Cy5p5);
            Task.Run(async () => await ledManager.InitializeLEDParameters()).Wait();
            Task.Run(async () => await ledManager.ProcessCommandQueue());

            //
            // Setup the Chassis and it's Manager
            //
            chassis = new Chassis(apiManager);
            chassisManager = new ChassisManager(chassis);
            Task.Run(async () => await chassisManager.ProcessCommandQueues());

            // Initialize
            protocolManager = new ThermocyclingProtocolManager(configuration);
            assayProtocolManager = new AssayProtocolManager();
            pictureBoxManager = new PictureBoxManager(configuration);
            cartridgeOptions = new CartridgeOptions(configuration);
            elastomerOptions = new ElastomerOptions(configuration);
            bergquistOptions = new BergquistOptions(configuration);
            scanningOptions = new ScanningOptions(configuration, consumablesImageScanningDataGridView);
            defaultScanningOption = scanningOptions.GetScanningOptionData("A", configuration.DefaultCartridge, configuration.DefaultGlassOffset, configuration.DefaultElastomer, configuration.DefaultBergquist);
            scanManager = new ScanManager(configuration, motorManager, cameraManager, ledManager, tecManager);
            emailManager = new EmailManager();

            // Obtain default data paths
            defaultThermocyclingProtocolDirectory = configuration.ThermocyclingProtocolsDataPath;
            defaultAssayProtocolDirectory = configuration.AssayProtocolsDataPath;

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
            AddMainHomeDefaultData();

            // Add default plots
            AddThermocyclingDefaultPlot();

            // Initialize the timer managers after data is added to it
            runExperimentDataTimerManager = new TimerManager(interval: configuration.RunDataTimerInterval, tickEventHandler: runExperimentDataGridView_TickEventHandler);
            runExperimentDataTimerManager.Start();
            controlTabTimerManager = new TimerManager(interval: configuration.ControlTabTimerInterval, tickEventHandler: controlTab_TickEventHandler);
            controlTabTimerManager.Start();
            thermocyclingStatusTimerManager = new TimerManager(interval: configuration.ThermocyclingTabTimerInterval, tickEventHandler: thermocyclingTab_TickEventHandler);
            thermocyclingStatusTimerManager.Start();
            mainFormFastTimerManager = new TimerManager(interval: configuration.MainFormFastTimerInterval, tickEventHandler: mainFormFast_TickEventHandler);
            mainFormFastTimerManager.Start();
            mainFormSlowTimerManager = new TimerManager(interval: configuration.MainFormSlowTimerInterval, tickEventHandler: mainFormSlow_TickEventHandler);
            mainFormSlowTimerManager.Start();

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
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeLEDsDataGridView, Cy5.Connected, "C", "N", "State", Cy5.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeLEDsDataGridView, FAM.Connected, "C", "N", "State", FAM.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeLEDsDataGridView, HEX.Connected, "C", "N", "State", HEX.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeLEDsDataGridView, Atto.Connected, "C", "N", "State", Atto.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeLEDsDataGridView, Alexa.Connected, "C", "N", "State", Alexa.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(homeLEDsDataGridView, Cy5p5.Connected, "C", "N", "State", Cy5p5.Name);
            // Control Tab
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Cy5.Connected, "C", "N", "State", Cy5.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, FAM.Connected, "C", "N", "State", FAM.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, HEX.Connected, "C", "N", "State", HEX.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Atto.Connected, "C", "N", "State", Atto.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Alexa.Connected, "C", "N", "State", Alexa.Name);
            dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Cy5p5.Connected, "C", "N", "State", Cy5p5.Name);
            #endregion

            // Subscribe to CellClick events
            this.controlMotorsDataGridView.CellClick += new DataGridViewCellEventHandler(controlMotorsDataGridView_CellClick);

            // Subscribe to value changed events
            this.runExperimentDataGridView.CellValueChanged += new DataGridViewCellEventHandler(runExperimentDataGridView_CellValueChanged);
            this.controlLEDsDataGridView.CellValueChanged += new DataGridViewCellEventHandler(controlLEDsDataGridView_CellValueChanged);
            this.imagingScanParametersDataGridView.CellValueChanged += new DataGridViewCellEventHandler(imagingScanParametersDataGridView_CellValueChanged);
            this.mainHomePowerRelaysDataGridView.CellValueChanged += new DataGridViewCellEventHandler(mainHomePowerRelaysDataGridView_CellValueChanged);

            // Subscribe to the load event of this form to set defaults on form loading
            this.Load += new EventHandler(this.Form_Load);

            // Subscribe to the form closing event handler
            this.FormClosing += new FormClosingEventHandler(IndependentReaderGUI_FormClosing);
        }

        private void AddMainHomeDefaultData()
        {
            //
            // Setup the Motors data
            //

            //
            // Setup the TECs data
            //

            // 
            // Setup the Camera data
            //

            //
            // Setup the LEDs data
            //

            //
            // Setup the Valves data
            //

            //
            // Setup the Power Relays data
            //
            mainHomePowerRelaysDataGridView.Rows.Add(1, "TEC Fans", false);
            mainHomePowerRelaysDataGridView.Rows.Add(2, "TEC A", false);
            mainHomePowerRelaysDataGridView.Rows.Add(3, "TEC B", false);
            mainHomePowerRelaysDataGridView.Rows.Add(4, "TEC C", false);
            mainHomePowerRelaysDataGridView.Rows.Add(5, "TEC D", false);
            mainHomePowerRelaysDataGridView.Rows.Add(6, "Motor", false);
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
            homeLEDsDataGridView.Rows.Add("State", Cy5.GetState(), FAM.GetState(), HEX.GetState(), Atto.GetState(), Alexa.GetState(), Cy5p5.GetState());
            homeLEDsDataGridView.Rows.Add("IO", Cy5.IO, FAM.IO, HEX.IO, Atto.IO, Alexa.IO, Cy5p5.IO);
            homeLEDsDataGridView.Rows.Add("Exposure (\u03BCs)", configuration.Cy5Exposure, configuration.FAMExposure, configuration.HEXExposure,
                configuration.AttoExposure, configuration.AlexaExposure, configuration.Cy5p5Exposure);
            homeLEDsDataGridView.Rows.Add("Intensity (%)", Cy5.Intensity, FAM.Intensity, HEX.Intensity, Atto.Intensity, Alexa.Intensity, Cy5p5.Intensity);
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
            homeTECsDataGridView.Rows.Add("Fan Control", "Disabled", "Disabled", "Disabled", "Disabled");
            homeTECsDataGridView.Rows.Add("Fan On Temp (\u00B0C)", "?", "?", "?", "?");
            homeTECsDataGridView.Rows.Add(homeTECsCurrent.PropertyName, homeTECsCurrent.ValueTECA, homeTECsCurrent.ValueTECB, homeTECsCurrent.ValueTECC, homeTECsCurrent.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsMaxCurrent.PropertyName, tecA.CurrentErrorThreshold, tecB.CurrentErrorThreshold, tecC.CurrentErrorThreshold, tecD.CurrentErrorThreshold);
            homeTECsDataGridView.Rows.Add(homeTECsVoltage.PropertyName, homeTECsVoltage.ValueTECA, homeTECsVoltage.ValueTECB, homeTECsVoltage.ValueTECC, homeTECsVoltage.ValueTECD);
            homeTECsDataGridView.Rows.Add(homeTECsMaxVoltage.PropertyName, tecA.VoltageErrorThreshold, tecB.VoltageErrorThreshold, tecC.VoltageErrorThreshold, tecD.VoltageErrorThreshold);
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
                runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1].Value = configuration.DefaultThermocyclingProtocolName;
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
            runImagingSetupDataGridView.Rows.Add("Sample dX (\u03BCS)", defaultScanningOption.SampledX);
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
            controlMotorsDataGridView.Rows.Add("x", xMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.xMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("y", yMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.yMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("z", zMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.zMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Filter Wheel", filterWheelMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.FilterWheelMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Tray AB", trayABMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.TrayABMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Tray CD", trayCDMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.TrayCDMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp A", clampAMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampAMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp B", clampBMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampBMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp C", clampCMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampCMotorDefaultSpeed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp D", clampDMotor.Version, controlMotorData.State, controlMotorData.Position, configuration.ClampDMotorDefaultSpeed, controlMotorData.Home);
        }

        /// <summary>
        /// Add default data to the Control LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddControlLEDsDefaultData()
        {
            LEDsData controlLEDsData = new LEDsData();
            controlLEDsDataGridView.Rows.Add("State", Cy5.GetState(), FAM.GetState(), HEX.GetState(), Atto.GetState(), Alexa.GetState(), Cy5p5.GetState());
            controlLEDsDataGridView.Rows.Add("IO");
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[1].Value = Cy5.Intensity > 0 ? "On" : "Off";
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[2].Value = FAM.Intensity > 0 ? "On" : "Off";
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[3].Value = HEX.Intensity > 0 ? "On" : "Off";
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[4].Value = Atto.Intensity > 0 ? "On" : "Off";
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[5].Value = Alexa.Intensity > 0 ? "On" : "Off";
            controlLEDsDataGridView.Rows[controlLEDsDataGridView.Rows.Count - 1].Cells[6].Value = Cy5p5.Intensity > 0 ? "On" : "Off";
            controlLEDsDataGridView.Rows.Add("Intensity (%)",
                Cy5.Intensity,
                FAM.Intensity,
                HEX.Intensity,
                Atto.Intensity,
                Alexa.Intensity,
                Cy5p5.Intensity);
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
            dataGridViewComboBoxCellTempEnabledA.Items.AddRange(new object[] { "On", "Off" });
            dataGridViewComboBoxCellTempEnabledB.Items.AddRange(new object[] { "On", "Off" });
            dataGridViewComboBoxCellTempEnabledC.Items.AddRange(new object[] { "On", "Off" });
            dataGridViewComboBoxCellTempEnabledD.Items.AddRange(new object[] { "On", "Off" });
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledA = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledB = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledC = new DataGridViewComboBoxCell();
            DataGridViewComboBoxCell dataGridViewComboBoxCellFanEnabledD = new DataGridViewComboBoxCell();
            dataGridViewComboBoxCellFanEnabledA.Items.AddRange(new object[] { "Enabled", "Disabled" });
            dataGridViewComboBoxCellFanEnabledB.Items.AddRange(new object[] { "Enabled", "Disabled" });
            dataGridViewComboBoxCellFanEnabledC.Items.AddRange(new object[] { "Enabled", "Disabled" });
            dataGridViewComboBoxCellFanEnabledD.Items.AddRange(new object[] { "Enabled", "Disabled" });
            controlTECsDataGridView.Rows.Add("State", "Not Connected", "Not Connected", "Not Connected", "Not Connected");
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("IO", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            // NOTE: Pull the Firmware version from the TECs on Form loading, leave these defaults here. 
            controlTECsDataGridView.Rows.Add("Version", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("New Object Temp (\u00B0C)");
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[1].Value = tecA.TargetObjectTemperature;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[2].Value = tecB.TargetObjectTemperature;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[3].Value = tecC.TargetObjectTemperature;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[4].Value = tecD.TargetObjectTemperature;
            controlTECsDataGridView.Rows.Add("Actual Object Temp (\u00B0C)", tecA.ActualObjectTemperature, tecB.ActualObjectTemperature, tecC.ActualObjectTemperature, tecD.ActualObjectTemperature);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Target Object Temp (\u00B0C)", tecA.TargetObjectTemperature, tecB.TargetObjectTemperature, tecC.TargetObjectTemperature, tecD.TargetObjectTemperature);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Temp Enabled");
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[1] = dataGridViewComboBoxCellTempEnabledA;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[1].Value = tecA.TemperatureControlled;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[2] = dataGridViewComboBoxCellTempEnabledB;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[2].Value = tecB.TemperatureControlled; ;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[3] = dataGridViewComboBoxCellTempEnabledC;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[3].Value = tecC.TemperatureControlled;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[4] = dataGridViewComboBoxCellTempEnabledD;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[4].Value = tecD.TemperatureControlled;
            controlTECsDataGridView.Rows.Add("Object Upper Error Threshold (\u00B0C)",
                tecA.ObjectUpperErrorThreshold,
                tecB.ObjectUpperErrorThreshold,
                tecC.ObjectUpperErrorThreshold,
                tecD.ObjectUpperErrorThreshold);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Object Lower Error Threshold (\u00B0C)",
                tecA.ObjectLowerErrorThreshold,
                tecB.ObjectLowerErrorThreshold,
                tecC.ObjectLowerErrorThreshold,
                tecD.ObjectLowerErrorThreshold);
            controlTECsDataGridView.Rows.Add("Sink Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Sink Upper Error Threshold (°C)",
                tecA.SinkUpperErrorThreshold,
                tecB.SinkUpperErrorThreshold,
                tecC.SinkUpperErrorThreshold,
                tecD.SinkUpperErrorThreshold);
            controlTECsDataGridView.Rows.Add("Sink Lower Error Threshold (°C)",
                tecA.SinkLowerErrorThreshold,
                tecB.SinkLowerErrorThreshold,
                tecC.SinkLowerErrorThreshold,
                tecD.SinkLowerErrorThreshold);
            controlTECsDataGridView.Rows.Add("Actual Fan Speed (rpm)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Fan On Temp (\u00B0C)",
                configuration.TECAFanOnTemp,
                configuration.TECBFanOnTemp,
                configuration.TECCFanOnTemp,
                configuration.TECDFanOnTemp);
            controlTECsDataGridView.Rows.Add("Fan Control");
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[1] = dataGridViewComboBoxCellFanEnabledA;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[1].Value = tecA.FanControlled;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[2] = dataGridViewComboBoxCellFanEnabledB;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[2].Value = tecB.FanControlled;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[3] = dataGridViewComboBoxCellFanEnabledC;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[3].Value = tecC.FanControlled;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[4] = dataGridViewComboBoxCellFanEnabledD;
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].Cells[4].Value = tecD.FanControlled;
            controlTECsDataGridView.Rows.Add("Current (A)", tecA.ActualOutputCurrent, tecB.ActualOutputCurrent, tecC.ActualOutputCurrent, tecD.ActualOutputCurrent);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Max Current (A)", tecA.CurrentErrorThreshold, tecB.CurrentErrorThreshold, tecC.CurrentErrorThreshold, tecD.CurrentErrorThreshold);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Voltage (V)", tecA.ActualOutputVoltage, tecB.ActualOutputVoltage, tecC.ActualOutputVoltage, tecD.ActualOutputVoltage);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
            controlTECsDataGridView.Rows.Add("Max Voltage (V)", tecA.VoltageErrorThreshold, tecB.VoltageErrorThreshold, tecC.VoltageErrorThreshold, tecD.VoltageErrorThreshold);
            controlTECsDataGridView.Rows[controlTECsDataGridView.Rows.Count - 1].ReadOnly = true;
        }

        /// <summary>
        /// Add default data to the Thermocycling Tab's Protocol Statuses Data Grid View upon loading of the Form
        /// </summary>
        private void AddThermocyclingProtocolStatusesDefaultData()
        {
            // Add the Default Tray and Clamp Positions
            thermocyclingTrayPositionTextBox.Text = configuration.TrayABClosedValue.ToString();
            thermocyclingClampPositionTextBox.Text = configuration.ClampALoweredValue.ToString();
            // Set the Pressure from the default settings
            //double glassOffset = double.Parse(thermocyclingGlassOffsetComboBox.Text.ToString());
            double glassOffset = configuration.DefaultGlassOffset;
            Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(configuration.DefaultCartridge);
            Elastomer elastomer = elastomerOptions.GetElastomerFromName(configuration.DefaultElastomer);
            Bergquist bergquist = bergquistOptions.GetBergquistFromName(configuration.DefaultBergquist);
            double clampPositionMM = configuration.ClampALoweredValue * configuration.MicrostepToMillimeterConversion;
            double A = cartridge.Width * cartridge.Length * 0.00155; // mm^2 to in^2
            thermocyclingPressureTextBox.Text = clampPressureCalculator.ComputePressureKPa(0.23, glassOffset, cartridge, elastomer, bergquist, clampPositionMM, A).ToString();
            // Setup the DataGridView
            TECsData thermocyclingProtocolStatusesData = new TECsData();
            thermocyclingProtocolStatusesDataGridView.Rows.Add("State", "Not Connected", "Not Connected", "Not Connected", "Not Connected");
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Protocol", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Protocol Running", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Estimated Time Left", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("IO", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Step", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
            thermocyclingProtocolStatusesDataGridView.Rows.Add("Cycle Number", thermocyclingProtocolStatusesData.ValueTECA, thermocyclingProtocolStatusesData.ValueTECB, thermocyclingProtocolStatusesData.ValueTECC, thermocyclingProtocolStatusesData.ValueTECD);
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
            // Add the Default Dx, Dy, and Dz values
            imagingDxTextBox.Text = configuration.DefaultDx.ToString();
            imagingDyTextBox.Text = configuration.DefaultDy.ToString();
            imagingDzTextBox.Text = configuration.DefaultDz.ToString();
            ScanParameterData scanParameterData = new ScanParameterData();
            imagingScanParametersDataGridView.Rows.Add("Experiment", "");
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
            imagingScanParametersDataGridView.Rows.Add("Sample dX (\u03BCS)", defaultScanningOption.SampledX);
            imagingScanParametersDataGridView.Rows.Add("dY (\u03BCS)", defaultScanningOption.dY);
            imagingScanParametersDataGridView.Rows.Add("Rotational Offset (\u00B0)", defaultScanningOption.RotationalOffset);
        }

        private void AddImagingLEDsDefaultData()
        {
            LEDsData ledsData = new LEDsData();
            imagingLEDsDataGridView.Rows.Add("Use");
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[1] = ledsData.UseBrightFieldComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[1].Value = "Yes";
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[2] = ledsData.UseCy5ComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[2].Value = "No";
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[3] = ledsData.UseFAMComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[3].Value = "No";
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[4] = ledsData.UseHEXComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[4].Value = "No";
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[5] = ledsData.UseAttoComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[5].Value = "No";
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[6] = ledsData.UseAlexaComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[6].Value = "No";
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[7] = ledsData.UseCy5p5ComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[7].Value = "No";
            imagingLEDsDataGridView.Rows.Add("Intensity (%)",
                configuration.HEXIntensity, // Bright Field
                configuration.Cy5Intensity, configuration.FAMIntensity, configuration.HEXIntensity, configuration.AttoIntensity, configuration.AlexaIntensity, configuration.Cy5p5Intensity);
            imagingLEDsDataGridView.Rows.Add("Exposure (\u03BCs)",
                5000, // Bright Field
                configuration.Cy5Exposure, configuration.FAMExposure, configuration.HEXExposure, configuration.AttoExposure, configuration.AlexaExposure, configuration.Cy5p5Exposure);
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
                else if (cellValue.Equals("Ramping"))
                {
                    e.CellStyle.BackColor = Color.LightGoldenrodYellow;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (cellValue.Equals("Error"))
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                }
                //else
                //{
                //    e.CellStyle.BackColor = homeMotorsDataGridView.DefaultCellStyle.BackColor;
                //    e.CellStyle.ForeColor = homeMotorsDataGridView.DefaultCellStyle.ForeColor;
                //}

                // TODO: Change Cell Style based on Values
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
            foreach (var glassOffsetOption in scanningOptions.GetGlassOffsets())
            {
                consumablesGlassOffsetComboBox.Items.Add(glassOffsetOption.ToString());
            }
            consumablesGlassOffsetComboBox.SelectedItem = configuration.DefaultGlassOffset;

            // Setup the Cartridge, Elastomer, and Bergquist ComboBoxes on the Reader - Thermocycling Tab
            thermocyclingCartridgeComboBox.DataSource = cartridgeOptions.Names();
            thermocyclingCartridgeComboBox.Text = configuration.DefaultCartridge;
            thermocyclingElastomerComboBox.DataSource = elastomerOptions.Names();
            thermocyclingElastomerComboBox.Text = configuration.DefaultElastomer;
            thermocyclingBergquistComboBox.DataSource = bergquistOptions.Names();
            thermocyclingBergquistComboBox.Text = configuration.DefaultBergquist;
            thermocyclingGlassOffsetComboBox.DataSource = scanningOptions.GetGlassOffsets();
            thermocyclingGlassOffsetComboBox.Text = configuration.DefaultGlassOffset.ToString();
        }

        private async void controlLEDsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the selected property
            var propertySelected = controlLEDsDataGridView.Rows[e.RowIndex].Cells[0].Value;
            // Check if the intensity value changed
            if (propertySelected.Equals("Intensity (%)"))
            {
                // Turn on or off the LED
                string ledName = controlLEDsDataGridView.Columns[e.ColumnIndex].HeaderText;
                LED led = ledManager.GetLEDFromName(ledName);
                try
                {
                    int intensity = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(ledName, "Intensity (%)", controlLEDsDataGridView));
                    if (intensity == 0)
                    {
                        LEDCommand ledOffCommand = new LEDCommand() { Type = LEDCommand.CommandType.Off, LED = led };
                        ledManager.EnqueuePriorityCommand(ledOffCommand);
                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlLEDsDataGridView, led.Name, "IO", "Off", Color.White);
                    }
                    else
                    {
                        LEDCommand ledOnCommand = new LEDCommand() { Type = LEDCommand.CommandType.On, Intensity = intensity, LED = led };
                        ledManager.EnqueuePriorityCommand(ledOnCommand);
                        dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(controlLEDsDataGridView, led.Name, "IO", "On", Color.White);
                    }
                }
                catch (LEDNotConnectedException ex)
                {
                    //controlLEDsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Off";
                    return;
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

        public void mainHomePowerRelaysDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the Power Relay ID of the row which had a change
            int id = int.Parse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(mainHomePowerRelaysDataGridView, "ID", e.RowIndex));
            // Setup the Command to set the Power Relay state
            string state = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(mainHomePowerRelaysDataGridView, "IO", e.RowIndex).ToString().ToLower();
            ChassisCommand chassisCommand = new ChassisCommand { Type = ChassisCommand.CommandType.SetPowerRelayState, Chassis = chassis, ID = id - 1, RelayState = state };
            chassisManager.EnqueueCommand(chassisCommand);
        }

        public void imagingScanParametersDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the Heater, Elastomer, Bergquist, Cartridge, and Glass Offset to determine the Scan Parameters
            string heaterName = dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Heater", imagingScanParametersDataGridView);
            string elastomerName = dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Elastomer", imagingScanParametersDataGridView);
            string bergquistName = dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Bergquist", imagingScanParametersDataGridView);
            string cartridgeName = dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Cartridge", imagingScanParametersDataGridView);
            double glassOffset = double.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Glass Offset (mm)", imagingScanParametersDataGridView));
            // Get the Scan parameters based on this combination
            ScanningOption scanParameters = scanningOptions.GetScanningOptionData(heaterName, cartridgeName, glassOffset, elastomerName, bergquistName);
            // Set the Values in the DataGridView base on the results
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(imagingScanParametersDataGridView, "Value", "x0 (\u03BCS)", scanParameters.X0.ToString(), Color.White);
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(imagingScanParametersDataGridView, "Value", "y0 (\u03BCS)", scanParameters.Y0.ToString(), Color.White);
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(imagingScanParametersDataGridView, "Value", "z0 (\u03BCS)", scanParameters.Z0.ToString(), Color.White);
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(imagingScanParametersDataGridView, "Value", "FOV dX (\u03BCS)", scanParameters.FOVdX.ToString(), Color.White);
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(imagingScanParametersDataGridView, "Value", "Sample dX (\u03BCS)", scanParameters.SampledX.ToString(), Color.White);
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(imagingScanParametersDataGridView, "Value", "dY (\u03BCS)", scanParameters.dY.ToString(), Color.White);
            dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(imagingScanParametersDataGridView, "Value", "Rotational Offset (\u00B0)", scanParameters.RotationalOffset.ToString(), Color.White);
            // Get the row we are changing
            DataGridViewRow row = imagingScanParametersDataGridView.Rows[e.RowIndex];
            // TODO: Update this to only trigger if the cartridge changes (will trigger if I reselect the same cartridge?)
            if (row.Cells[0].Value == "Cartridge")
            {
                // Set the Sample and Assay name rows but first clear the previous ones if there are any
                dataGridViewManager.RemoveRowsByNameWhichContains(imagingScanParametersDataGridView, "Name");
                Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(cartridgeName);
                int numberOfSamples = cartridge.NumberofSampleChambers;
                int numberOfAssays = cartridge.NumberofAssayChambers;
                for (int i = 0; i < numberOfSamples; i++)
                {
                    imagingScanParametersDataGridView.Rows.Add($"Sample {i + 1} Name", $"Sample {i + 1}");
                }
                for (int i = 0; i < numberOfAssays; i++)
                {
                    imagingScanParametersDataGridView.Rows.Add($"Assay {i + 1} Name", $"Assay {i + 1}");
                }
            }
            else if (imagingScanParametersDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Contains("(\u03BCS)"))
            {
                dataGridViewManager.SetCellForeColorByColumnAndRowIndexBasedOnOutcome(dataGridView: imagingScanParametersDataGridView,
                    outcome: int.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out _),
                    successColor: Color.Black, failColor: Color.Red, columnIndex: e.ColumnIndex, rowIndex: e.RowIndex);
            }
            else if (imagingScanParametersDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Contains("(\u00B0)"))
            {
                dataGridViewManager.SetCellForeColorByColumnAndRowIndexBasedOnOutcome(dataGridView: imagingScanParametersDataGridView,
                    outcome: int.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out _),
                    successColor: Color.Black, failColor: Color.Red, columnIndex: e.ColumnIndex, rowIndex: e.RowIndex);
            }
        }

        private void runExperimentDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // All cells of interest that will change are in column 1
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
                        runSampleMetaDataGridView.Rows.Add($"{i + 1}", $"Sample {i + 1}");
                    }
                    int numberOfAssayChambers = cartridge.NumberofAssayChambers;
                    runAssayMetaDataGridView.Rows.Clear();
                    for (int i = 0; i < numberOfAssayChambers; i++)
                    {
                        runAssayMetaDataGridView.Rows.Add($"{i + 1}", $"Assay {i + 1}");
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

            // Load in consumables Combo Boxes
            LoadconsumablesComboBoxInitialData();

            // Start streaming
            await cameraManager.StartStreamAsync();
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
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "x", xMotorVersion, Color.White);
                string yMotorVersion = await yMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "y", yMotorVersion, Color.White);
                string zMotorVersion = await zMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "z", zMotorVersion, Color.White);
                string filterWheelMotorVersion = await filterWheelMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Filter Wheel", filterWheelMotorVersion, Color.White);
                string trayABMotorVersion = await trayABMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Tray AB", trayABMotorVersion, Color.White);
                string trayCDMotorVersion = await trayCDMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Tray CD", trayCDMotorVersion, Color.White);
                string tecAFirmwareVersion = await tecA.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC A", "Version", tecAFirmwareVersion, Color.White);
                string tecBFirmwareVersion = await tecB.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC B", "Version", tecBFirmwareVersion, Color.White);
                string tecCFirmwareVersion = await tecC.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC C", "Version", tecCFirmwareVersion, Color.White);
                string tecDFirmwareVersion = await tecD.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeTECsDataGridView, "TEC D", "Version", tecDFirmwareVersion, Color.White);
                string clampAMotorVersion = await clampAMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp A", clampAMotorVersion, Color.White);
                string clampBMotorVersion = await clampBMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp B", clampBMotorVersion, Color.White);
                string clampCMotorVersion = await clampCMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp C", clampCMotorVersion, Color.White);
                string clampDMotorVersion = await clampDMotor.GetVersionAsync();
                dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(homeMotorsDataGridView, "Version", "Clamp D", clampDMotorVersion, Color.White);
            }
        }

        private void LoadconsumablesComboBoxInitialData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadconsumablesComboBoxInitialData));
            }
            else
            {
                List<Cartridge> cartridgeOptionsList = cartridgeOptions.Options;
                foreach (var option in cartridgeOptionsList)
                {
                    consumablesCartridgeComboBox.Items.Add(option.Name);
                }
                consumablesCartridgeComboBox.SelectedItem = cartridgeOptionsList.First().Name;
                List<Elastomer> elastomerOptionsList = elastomerOptions.Options;
                foreach (var option in elastomerOptionsList)
                {
                    consumablesElastomerComboBox.Items.Add(option.Name);
                }
                consumablesElastomerComboBox.SelectedItem = elastomerOptionsList.First().Name;
                List<Bergquist> bergquistOptionsList = bergquistOptions.Options;
                foreach (var option in bergquistOptionsList)
                {
                    consumablesBergquistComboBox.Items.Add(option.Name);
                }
                consumablesBergquistComboBox.SelectedItem = bergquistOptionsList.First().Name;
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
            var filePath = fileSearcher.GetLoadFilePath(initialDirectory: defaultThermocyclingProtocolDirectory);
            if (filePath != null)
            {
                // FIXME: ClearPlot dos not behave as expected, x axis does not reset to 0
                plotManager.ClearPlot();
                protocol = protocolManager.LoadProtocol(filePath);
                plotManager.PlotProtocol(protocol);
                // Set the "Plotted Protocol Name"
                thermocyclingProtocolNameTextBox.Text = protocol.Name;
                // Set the "Estimated Time"
                TimeSpan estimatedTimeSpan = TimeSpan.FromSeconds(int.Parse(protocol.GetTimeInSeconds().ToString()));
                thermocyclingEstimatedTimeTextBox.Text = estimatedTimeSpan.ToString(@"hh\:mm\:ss");
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
            var filePath = fileSearcher.GetSaveFilePath(initialDirectory: defaultThermocyclingProtocolDirectory);
            if (filePath != null)
            {
                protocolManager.SaveProtocol(protocol, filePath, userLabel.Text, DateTime.Now.ToString("MM/dd/yyyy"));
            }
        }

        // TODO: This function should not exist, the FastTick handling will handle everything on the Control Tab
        public async void controlTab_TickEventHandler(object sender, EventArgs e)
        {
            // FIXME: If I turn off the instrument and turn it back on the instrument does not show being connected again 
            // FIXME: Could be that I need to check if each motor is connected here at the onset
            // Check if the APIManger is connect (if not API server could be down)
            if (apiManager != null)
            {
                // TODO: Remove these actions from this function
                // Check the LEDs
                dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Cy5.Connected, "C", "N", "State", Cy5.Name);
                dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, FAM.Connected, "C", "N", "State", FAM.Name);
                dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, HEX.Connected, "C", "N", "State", HEX.Name);
                dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Atto.Connected, "C", "N", "State", Atto.Name);
                dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Alexa.Connected, "C", "N", "State", Alexa.Name);
                dataGridViewManager.SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(controlLEDsDataGridView, Cy5p5.Connected, "C", "N", "State", Cy5p5.Name);
            }
        }

        public async void mainFormFast_TickEventHandler(object sender, EventArgs e)
        {
            // Check the Camera is connected every fast tick
            dataGridViewManager.SetTextBoxCellStringValueByIndicesBasedOnOutcome(homeCameraDataGridView, cameraManager.Connected, "Connected", "Not Connected", 0, 0);
            // Check the Motors data every fast tick
            motorManager.HandleFastTickEvent(dataGridViewManager, homeMotorsDataGridView, controlMotorsDataGridView);
            // Check the TECs data every fast tick
            tecManager.HandleFastTickEvent(dataGridViewManager, homeTECsDataGridView, controlTECsDataGridView, thermocyclingProtocolStatusesDataGridView, mainLogErrorsDataGridView);
            // TODO: Check the LEDs data every fast tick
            ledManager.HandleFastTickEvent(dataGridViewManager, homeLEDsDataGridView, controlLEDsDataGridView);
            // Check the Camera
            cameraManager.HandleFastTickEvent(dataGridViewManager, homeCameraDataGridView);
        }

        public async void mainFormSlow_TickEventHandler(object sender, EventArgs e)
        {
            // Check the TECs are connected still
            TECCommand tecACheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = tecA };
            tecManager.EnqueueCommand(tecACheckConnectionCommand);
            TECCommand tecBCheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = tecB };
            tecManager.EnqueueCommand(tecACheckConnectionCommand);
            TECCommand tecCCheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = tecC };
            tecManager.EnqueueCommand(tecACheckConnectionCommand);
            TECCommand tecDCheckConnectionCommand = new TECCommand { Type = TECCommand.CommandType.CheckConnectionAsync, TEC = tecD };
            tecManager.EnqueueCommand(tecACheckConnectionCommand);
            // Check the LEDs are connected still
            await ledManager.CheckLEDBoardConnectionAsync();
        }

        public async void thermocyclingTab_TickEventHandler(object sender, EventArgs e)
        {
            TimeSpan dt = TimeSpan.FromSeconds(configuration.ThermocyclingTabTimerInterval / 1000); // time tick in milliseconds
            // Reduce all time left on a protocol running for each TEC by dt * 1000;
            var tecATimeLeftCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecA.Name, "Estimated Time Left", thermocyclingProtocolStatusesDataGridView);
            var tecBTimeLeftCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecB.Name, "Estimated Time Left", thermocyclingProtocolStatusesDataGridView);
            var tecCTimeLeftCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecC.Name, "Estimated Time Left", thermocyclingProtocolStatusesDataGridView);
            var tecDTimeLeftCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecD.Name, "Estimated Time Left", thermocyclingProtocolStatusesDataGridView);
            // TEC A
            if (TimeSpan.TryParseExact(tecATimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out TimeSpan timeSpan))
            {
                if (TimeSpan.ParseExact(tecATimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture) > TimeSpan.Zero)
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView,
                        tecA.Name, "Estimated Time Left", (TimeSpan.Parse(tecATimeLeftCellValue) - dt).ToString(@"hh\:mm\:ss"), Color.White);
                }
            }
            // TEC B
            if (TimeSpan.TryParseExact(tecBTimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out timeSpan))
            {
                if (TimeSpan.ParseExact(tecBTimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture) > TimeSpan.Zero)
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView,
                        tecB.Name, "Estimated Time Left", (TimeSpan.Parse(tecBTimeLeftCellValue) - dt).ToString(@"hh\:mm\:ss"), Color.White);
                }
            }
            // TEC C
            if (TimeSpan.TryParseExact(tecCTimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out timeSpan))
            {
                if (TimeSpan.ParseExact(tecCTimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture) > TimeSpan.Zero)
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView,
                        tecC.Name, "Estimated Time Left", (TimeSpan.Parse(tecCTimeLeftCellValue) - dt).ToString(@"hh\:mm\:ss"), Color.White);
                }
            }
            // TEC D
            if (TimeSpan.TryParseExact(tecDTimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out timeSpan))
            {
                if (TimeSpan.ParseExact(tecDTimeLeftCellValue, @"hh\:mm\:ss", CultureInfo.InvariantCulture) > TimeSpan.Zero)
                {
                    dataGridViewManager.SetTextBoxCellStringValueByColumnandRowNames(thermocyclingProtocolStatusesDataGridView,
                        tecD.Name, "Estimated Time Left", (TimeSpan.Parse(tecDTimeLeftCellValue) - dt).ToString(@"hh\:mm\:ss"), Color.White);
                }
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
            // Stop the TimerManagers
            runExperimentDataTimerManager.Stop();
            controlTabTimerManager.Stop();
            thermocyclingStatusTimerManager.Stop();
            mainFormFastTimerManager.Stop();
            mainFormSlowTimerManager.Stop();
            // Stop the Processing of Command Queues for the Manager services
            tecManager.CloseCommandQueueProcessing();
            tecManager.ClosePriorityCommandQueueProcessing();
            ledManager.CloseCommandQueueProcessing();
            motorManager.CloseCommandQueueProcessing();
            motorManager.ClosePriorityCommandQueueProcessing();
            chassisManager.CloseCommandQueueProcessing();
            // Disconnect the Camera
            cameraManager.StopStreamAsync().Wait();
            cameraManager.Disconnect();
            // Clean up the API Manager service
            apiManager.Disponse();
        }

        /// <summary>
        /// Click event handler for clicking the Capture Image button on the Imaging tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void imagingCaptureImageButton_Click(object sender, EventArgs e)
        {
            // Save the current image in the Imaging Picture Box to a file
            await pictureBoxManager.SaveImageWithSaveFileDialog(imagingPictureBox);
        }

        /// <summary>
        /// Click event handler for the setting the temperature of TEC A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECAUpdateButton_Click(object sender, EventArgs e)
        {
            if (tecA.RunningProtocol)
            {
                MessageBox.Show("Unable to update TEC A, a protocol is currently being run on it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Check if the temperature or fan control was set
                var tempControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecA.Name, "Temp Enabled", controlTECsDataGridView);
                TECCommand setTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.SetTemperatureControl, Parameter = tempControlCellValue, TEC = tecA };
                tecManager.EnqueuePriorityCommand(setTemperatureControlCommand);
                var fanControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecA.Name, "Fan Control", controlTECsDataGridView);
                TECCommand setFanControlCommand = new TECCommand { Type = TECCommand.CommandType.SetFanControl, Parameter = fanControlCellValue, TEC = tecA };
                tecManager.EnqueuePriorityCommand(setFanControlCommand);
                // Check if the target temperature is set
                double temp;
                var cellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecA.Name, "New Object Temp (\u00B0C)", controlTECsDataGridView);
                if (!double.TryParse(cellValue, out _))
                {
                    MessageBox.Show("New Object Temp (\u00B0C) must be a number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    temp = double.Parse(cellValue);
                    // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                    TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tecA, Parameter = temp };
                    tecManager.EnqueuePriorityCommand(setObjectTemperatureCommand);
                }
            }
        }

        /// <summary>
        /// Click event handler for the setting the temperature of TEC B
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECBUpdateButton_Click(object sender, EventArgs e)
        {
            if (tecB.Connected)
            {
                // TODO: Implement code to handle setting the temperature of the TEC
                if (tecB.RunningProtocol)
                {
                    MessageBox.Show("Unable to update TEC B, a protocol is currently being run on it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Check if the temperature or fan control was set
                    var tempControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecB.Name, "Temp Enabled", controlTECsDataGridView);
                    TECCommand setTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.SetTemperatureControl, Parameter = tempControlCellValue, TEC = tecB };
                    tecManager.EnqueuePriorityCommand(setTemperatureControlCommand);
                    var fanControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecB.Name, "Fan Control", controlTECsDataGridView);
                    TECCommand setFanControlCommand = new TECCommand { Type = TECCommand.CommandType.SetFanControl, Parameter = fanControlCellValue, TEC = tecB };
                    tecManager.EnqueuePriorityCommand(setFanControlCommand);
                    // Check if the target temperature is set
                    double temp;
                    var cellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecB.Name, "New Object Temp (\u00B0C)", controlTECsDataGridView);
                    if (!double.TryParse(cellValue, out _))
                    {
                        MessageBox.Show("New Object Temp (\u00B0C) must be a number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        temp = double.Parse(cellValue);
                        // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                        TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tecB, Parameter = temp };
                        tecManager.EnqueuePriorityCommand(setObjectTemperatureCommand);
                    }
                }
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
        private void controlTECCUpdateButton_Click(object sender, EventArgs e)
        {
            if (tecC.Connected)
            {
                // TODO: Implement code to handle setting the temperature of the TEC
                if (tecC.RunningProtocol)
                {
                    MessageBox.Show("Unable to update TEC C, a protocol is currently being run on it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Check if the temperature or fan control was set
                    var tempControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecC.Name, "Temp Enabled", controlTECsDataGridView);
                    TECCommand setTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.SetTemperatureControl, Parameter = tempControlCellValue, TEC = tecC };
                    tecManager.EnqueuePriorityCommand(setTemperatureControlCommand);
                    var fanControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecC.Name, "Fan Control", controlTECsDataGridView);
                    TECCommand setFanControlCommand = new TECCommand { Type = TECCommand.CommandType.SetFanControl, Parameter = fanControlCellValue, TEC = tecC };
                    tecManager.EnqueuePriorityCommand(setFanControlCommand);
                    // Check if the target temperature is set
                    double temp;
                    var cellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecC.Name, "New Object Temp (\u00B0C)", controlTECsDataGridView);
                    if (!double.TryParse(cellValue, out _))
                    {
                        MessageBox.Show("New Object Temp (\u00B0C) must be a number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        temp = double.Parse(cellValue);
                        // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                        TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tecC, Parameter = temp };
                        tecManager.EnqueuePriorityCommand(setObjectTemperatureCommand);
                    }
                }
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
        private void controlTECDUpdateButton_Click(object sender, EventArgs e)
        {
            if (tecD.Connected)
            {
                // TODO: Implement code to handle setting the temperature of the TEC
                if (tecD.RunningProtocol)
                {
                    MessageBox.Show("Unable to update TEC D, a protocol is currently being run on it", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Check if the temperature or fan control was set
                    var tempControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecD.Name, "Temp Enabled", controlTECsDataGridView);
                    TECCommand setTemperatureControlCommand = new TECCommand { Type = TECCommand.CommandType.SetTemperatureControl, Parameter = tempControlCellValue, TEC = tecD };
                    tecManager.EnqueuePriorityCommand(setTemperatureControlCommand);
                    var fanControlCellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecD.Name, "Fan Control", controlTECsDataGridView);
                    TECCommand setFanControlCommand = new TECCommand { Type = TECCommand.CommandType.SetFanControl, Parameter = fanControlCellValue, TEC = tecD };
                    tecManager.EnqueuePriorityCommand(setFanControlCommand);
                    // Check if the target temperature is set
                    double temp;
                    var cellValue = dataGridViewManager.GetColumnCellValueByColumnAndRowName(tecD.Name, "New Object Temp (\u00B0C)", controlTECsDataGridView);
                    if (!double.TryParse(cellValue, out _))
                    {
                        MessageBox.Show("New Object Temp (\u00B0C) must be a number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        temp = double.Parse(cellValue);
                        // Setup the SetObjectTemperature command and add it to the TECsManager Queue
                        TECCommand setObjectTemperatureCommand = new TECCommand { Type = TECCommand.CommandType.SetObjectTemperature, TEC = tecD, Parameter = temp };
                        tecManager.EnqueuePriorityCommand(setObjectTemperatureCommand);
                    }
                }
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
                // Setup the Reset command and add it to the queue
                TECCommand resetCommand = new TECCommand { Type = TECCommand.CommandType.ResetAsync, TEC = tecA };
                tecManager.EnqueuePriorityCommand(resetCommand);
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
                // Setup the Reset command and add it to the queue
                TECCommand resetCommand = new TECCommand { Type = TECCommand.CommandType.ResetAsync, TEC = tecA };
                tecManager.EnqueuePriorityCommand(resetCommand);
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
                // Setup the Reset command and add it to the queue
                TECCommand resetCommand = new TECCommand { Type = TECCommand.CommandType.ResetAsync, TEC = tecA };
                tecManager.EnqueuePriorityCommand(resetCommand);
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
                // Setup the Reset command and add it to the queue
                TECCommand resetCommand = new TECCommand { Type = TECCommand.CommandType.ResetAsync, TEC = tecA };
                tecManager.EnqueuePriorityCommand(resetCommand);
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

        private void consumablesCartridgeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the cartridge options to get the most update to date results
            cartridgeOptions.Update();
            // FIXME: This needs to be resolved, this region of code causes issues (Stackoverflow)
            // Reload in the data to the comboboxcell
            //consumablesCartridgeComboBox.Items.Clear();
            //foreach (var option in cartridgeOptions.Options)
            //{
            //    consumablesCartridgeComboBox.Items.Add(option.Name);
            //}
            //consumablesCartridgeComboBox.SelectedItem = cartridgeOptions.Options.First().Name;
            // Get the Cartridge Name
            string cartridgeName = consumablesCartridgeComboBox.SelectedItem.ToString();
            // Get the Cartridge instance from the name
            Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(cartridgeName);
            // Fill in the DataGridView for the cartridge on the consumables Tab
            consumablesCartridgesDataGridView.Rows.Clear();
            consumablesCartridgesDataGridView.Rows.Add();
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: consumablesCartridgesDataGridView, cellValue: cartridge.Name, rowIndex: 0, columnName: "Name");
            // FIXME: Make the Partition Type column cell a DataGridViewComboBox
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: consumablesCartridgesDataGridView, cellValue: cartridge.PartitionType, rowIndex: 0, columnName: "Partition Type");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: consumablesCartridgesDataGridView, cellValue: cartridge.NumberofSampleChambers.ToString(), rowIndex: 0, columnName: "Number of Samples");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: consumablesCartridgesDataGridView, cellValue: cartridge.NumberofAssayChambers.ToString(), rowIndex: 0, columnName: "Number of Assays");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: consumablesCartridgesDataGridView, cellValue: cartridge.Length.ToString(), rowIndex: 0, columnName: "Length (mm)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: consumablesCartridgesDataGridView, cellValue: cartridge.Width.ToString(), rowIndex: 0, columnName: "Width (mm)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView: consumablesCartridgesDataGridView, cellValue: cartridge.Height.ToString(), rowIndex: 0, columnName: "Height (mm)");
        }

        private void consumablesSaveNewCartridgeButton_Click(object sender, EventArgs e)
        {
            // Ensure all cells are filled with appropriate inputs
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Name");
            string? partitionType = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Partition Type");
            int numberOfSampleChambers;
            if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Number of Samples"), out numberOfSampleChambers))
            {
                MessageBox.Show("Cannot have a non-integer value for the number of samples in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            int numberOfAssayChambers;
            if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Number of Assays"), out numberOfAssayChambers))
            {
                MessageBox.Show("Cannot have a non-integer value for the number of assays in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            double length;
            if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Length (mm)"), out length))
            {
                MessageBox.Show("Cannot have a non-real value for the length of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            double width;
            if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Width (mm)"), out width))
            {
                MessageBox.Show("Cannot have a non-real value for the width of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            double height;
            if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Height (mm)"), out height))
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

        private void consumablesDeleteCartridgeButton_Click(object sender, EventArgs e)
        {
            // Get the name of cartridge to be deleted
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Name");
            if (name != null)
            {
                // Delete the cartridge from the XML data
                cartridgeOptions.DeleteCartridgeByName(name);
            }
        }

        private void consumablesUpdateImageScanningButton_Click(object sender, EventArgs e)
        {
            // Get the name of the Cartridge, Elastomer, Bergquist and the Glass Offset
            string cartridgeName = consumablesCartridgeComboBox.SelectedItem.ToString();
            string elastomerName = consumablesElastomerComboBox.SelectedItem.ToString();
            string bergquistName = consumablesBergquistComboBox.SelectedItem.ToString();
            double glassOffset = double.Parse(consumablesGlassOffsetComboBox.Text.ToString());
            // TODO: Update this to use the Scanning Data Model class to upload or write
            scanningOptions.WriteInScanningData(dataGridViewManager, consumablesImageScanningDataGridView, cartridgeName, glassOffset, elastomerName, bergquistName);
        }

        private void consumablesUpdateCartridgeDataButton_Click(object sender, EventArgs e)
        {
            // Get the name of the cartridge to be updated
            string? oldName = consumablesCartridgeComboBox.SelectedItem.ToString();
            if (oldName != null)
            {
                string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Name");
                string? partitionType = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Partition Type");
                int numberOfSampleChambers;
                if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Number of Samples"), out numberOfSampleChambers))
                {
                    MessageBox.Show("Cannot have a non-integer value for the number of samples in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                int numberOfAssayChambers;
                if (!int.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Number of Assays"), out numberOfAssayChambers))
                {
                    MessageBox.Show("Cannot have a non-integer value for the number of assays in a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                double length;
                if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Length (mm)"), out length))
                {
                    MessageBox.Show("Cannot have a non-real value for the length of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                double width;
                if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Width (mm)"), out width))
                {
                    MessageBox.Show("Cannot have a non-real value for the width of a cartridge.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                }
                double height;
                if (!double.TryParse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView: consumablesCartridgesDataGridView, rowIndex: 0, columnName: "Height (mm)"), out height))
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

        private void consumablesElastomerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the elastomer options to get the most up to date options
            elastomerOptions.Update();
            // TODO: update the combo box options
            // Get the elastomer from the name
            string? name = consumablesElastomerComboBox.SelectedItem.ToString();
            if (name != string.Empty)
            {
                Elastomer? elastomer = elastomerOptions.GetElastomerFromName(name);
                if (elastomer != null)
                {
                    consumablesElastomerDataGridView.Rows.Clear();
                    consumablesElastomerDataGridView.Rows.Add();
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(consumablesElastomerDataGridView, elastomer.Name, 0, "Name");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(consumablesElastomerDataGridView, elastomer.Thickness.ToString(), 0, "Thickness (mm)");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(consumablesElastomerDataGridView, elastomer.ThermalCoefficient.ToString(), 0, "Thermal Coefficient (W/m K)");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(consumablesElastomerDataGridView, elastomer.ShoreHardness.ToString(), 0, "Shore Hardness");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(consumablesElastomerDataGridView, elastomer.Mold.ToString(), 0, "Mold");
                    dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(consumablesElastomerDataGridView, elastomer.Film.ToString(), 0, "Film");
                }
            }
        }

        private void consumablesSaveNewElastomerButton_Click(object sender, EventArgs e)
        {
            // Update the elastomer options to get the most up to date options
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(consumablesElastomerDataGridView, "Name", 0);
            string? thickness = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(consumablesElastomerDataGridView, "Thickness (mm)", 0);
            string? thermalCoefficient = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(consumablesElastomerDataGridView, "Thermal Coefficient (W/m K)", 0);
            string? shoreHardness = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(consumablesElastomerDataGridView, "Shore Hardness", 0);
            string? mold = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(consumablesElastomerDataGridView, "Mold", 0);
            string? film = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(consumablesElastomerDataGridView, "Film", 0);
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

        private void consumablesDeleteElastomerButton_Click(object sender, EventArgs e)
        {
            // Get the name of the elastomer to be deleted
            string? name = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(consumablesElastomerDataGridView, "Name", 0);
            if (name != null)
            {
                // Delete the elastomer from the XML data
                elastomerOptions.DeleteElastomerByName(name);
            }
        }

        private void consumablesUpdateElastomerData_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to update the Elastomer data has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void consumablesBergquistDataButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to update the Bergquist data has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void consumablesSaveNewBergquistButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to save a new Bergquist has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void consumablesDeleteBergquistButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code to delete a Bergquist has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void consumablesGlassOffsetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the consumables tab's Image Scanning data grid view
            consumablesImageScanningDataGridView.Rows.Clear();
            // Make sure value is valid
            if (double.TryParse(consumablesGlassOffsetComboBox.SelectedItem.ToString(), out _))
            {
                scanningOptions.ReadInScanningData(dataGridView: consumablesImageScanningDataGridView,
                    cartridgeName: consumablesCartridgeComboBox.SelectedItem.ToString(), glassOffset: double.Parse(consumablesGlassOffsetComboBox.SelectedItem.ToString()),
                    elastomerName: consumablesElastomerComboBox.SelectedItem.ToString(), consumablesBergquistComboBox.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// Handle key down events for the RunSampleMetaDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runSampleMetaDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(runSampleMetaDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the RunImagingSetupDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runImagingSetupDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(runImagingSetupDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the RunExperimentDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runExperimentDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(runExperimentDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the RunAssayMetaDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runAssayMetaDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(runAssayMetaDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the ControlMotorsDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlMotorsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(controlMotorsDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the ControlTECsDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTECsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(controlTECsDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the ImagingScanParametersDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imagingScanParametersDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(imagingScanParametersDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events consumablesCartridgeDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void consumablesCartridgesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(consumablesCartridgesDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the consumablesElastomerDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void consumablesElastomerDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(consumablesElastomerDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the consumablesBergquistsDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void consumablesBergquistsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(consumablesBergquistsDataGridView);
            }
        }

        /// <summary>
        /// Handle key down events for the consumablesImageScanningDataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void consumablesImageScanningDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dataGridViewManager.ClearSelectedCellsNotInEditMode(consumablesImageScanningDataGridView);
            }
        }

        /// <summary>
        /// Handles the click event for the ThermocyclingTECARunButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void thermocyclingTECARunButton_Click(object sender, EventArgs e)
        {
            // Get the protocol name and protocol
            string? protocolName = thermocyclingProtocolNameTextBox.Text;
            ThermocyclingProtocol? protocol = protocolManager.GetProtocolFromName(protocolName);
            bool protocolNameExists = protocolManager.ProtocolNameExists(protocolName);
            // Get the selected Cartridge, Elastomer, and Bergquist used for the run
            Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(thermocyclingCartridgeComboBox.Text);
            Elastomer elastomer = elastomerOptions.GetElastomerFromName(thermocyclingElastomerComboBox.Text);
            Bergquist bergquist = bergquistOptions.GetBergquistFromName(thermocyclingBergquistComboBox.Text);
            // Handle the click event
            if (protocol != null && protocolNameExists)
            {
                await tecManager.HandleRunClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView, protocol, configuration, cartridge, elastomer, bergquist);
            }
            else
            {
                MessageBox.Show($"Unable to run protocol, a protocol by the name '{protocolName}' could not be found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void thermocyclingTECAKillButton_Click(object sender, EventArgs e)
        {
            tecManager.HandleKillClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView);
        }

        private async void thermocyclingTECBRunButton_Click(object sender, EventArgs e)
        {
            // Get the protocol name and protocol
            string? protocolName = thermocyclingProtocolNameTextBox.Text;
            ThermocyclingProtocol? protocol = protocolManager.GetProtocolFromName(protocolName);
            bool protocolNameExists = protocolManager.ProtocolNameExists(protocolName);
            // Get the selected Cartridge, Elastomer, and Bergquist used for the run
            Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(thermocyclingCartridgeComboBox.Text);
            Elastomer elastomer = elastomerOptions.GetElastomerFromName(thermocyclingElastomerComboBox.Text);
            Bergquist bergquist = bergquistOptions.GetBergquistFromName(thermocyclingBergquistComboBox.Text);
            // Handle the click event
            if (protocol != null && protocolNameExists)
            {
                await tecManager.HandleRunClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView, protocol, configuration, cartridge, elastomer, bergquist);
            }
            else
            {
                MessageBox.Show($"Unable to run protocol, a protocol by the name '{protocolName}' could not be found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void thermocyclingTECBKillButton_Click(object sender, EventArgs e)
        {
            tecManager.HandleKillClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView);
        }

        private async void thermocyclingTECCRunButton_Click(object sender, EventArgs e)
        {
            // Get the protocol name and protocol
            string? protocolName = thermocyclingProtocolNameTextBox.Text;
            ThermocyclingProtocol? protocol = protocolManager.GetProtocolFromName(protocolName);
            bool protocolNameExists = protocolManager.ProtocolNameExists(protocolName);
            // Get the selected Cartridge, Elastomer, and Bergquist used for the run
            Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(thermocyclingCartridgeComboBox.Text);
            Elastomer elastomer = elastomerOptions.GetElastomerFromName(thermocyclingElastomerComboBox.Text);
            Bergquist bergquist = bergquistOptions.GetBergquistFromName(thermocyclingBergquistComboBox.Text);
            // Handle the click event
            if (protocol != null && protocolNameExists)
            {
                await tecManager.HandleRunClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView, protocol, configuration, cartridge, elastomer, bergquist);
            }
            else
            {
                MessageBox.Show($"Unable to run protocol, a protocol by the name '{protocolName}' could not be found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void thermocyclingTECCKillButton_Click(object sender, EventArgs e)
        {
            tecManager.HandleKillClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView);
        }

        private async void thermocyclingTECDRunButton_Click(object sender, EventArgs e)
        {
            // Get the protocol name and protocol
            string? protocolName = thermocyclingProtocolNameTextBox.Text;
            ThermocyclingProtocol? protocol = protocolManager.GetProtocolFromName(protocolName);
            bool protocolNameExists = protocolManager.ProtocolNameExists(protocolName);
            // Get the selected Cartridge, Elastomer, and Bergquist used for the run
            Cartridge cartridge = cartridgeOptions.GetCartridgeFromName(thermocyclingCartridgeComboBox.Text);
            Elastomer elastomer = elastomerOptions.GetElastomerFromName(thermocyclingElastomerComboBox.Text);
            Bergquist bergquist = bergquistOptions.GetBergquistFromName(thermocyclingBergquistComboBox.Text);
            // Handle the click event
            if (protocol != null && protocolNameExists)
            {
                await tecManager.HandleRunClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView, protocol, configuration, cartridge, elastomer, bergquist);
            }
            else
            {
                MessageBox.Show($"Unable to run protocol, a protocol by the name '{protocolName}' could not be found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void thermocyclingTECDKillButton_Click(object sender, EventArgs e)
        {
            tecManager.HandleKillClickEvent(sender, e, thermocyclingProtocolStatusesDataGridView);
        }

        private void imagingBrightFieldButton_Click(object sender, EventArgs e)
        {
            // Set the position of the filter wheel to the FAM Filter position
            MotorCommand moveFilterWheelCommand = new MotorCommand
            {
                Type = MotorCommand.CommandType.MoveAsync,
                Motor = filterWheelMotor,
                PositionParameter = configuration.FAMFilterWheelPosition,
                SpeedParameter = configuration.FilterWheelMotorDefaultSpeed
            };
            motorManager.EnqueuePriorityCommand(moveFilterWheelCommand);
            // TODO: Turn off the other LEDs
            // Turn on the HEX LED
            LEDCommand turnOnLEDCommand = new LEDCommand { Type = LEDCommand.CommandType.On, Intensity = configuration.HEXIntensity, LED = HEX };
            ledManager.EnqueueCommand(turnOnLEDCommand);
        }

        private void tabControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 
            // Imaging Tab
            //
            if (this.readerTabControl.SelectedTab == imagingTabPage)
            {
                if (e.KeyCode == Keys.Up)
                {
                    if (int.TryParse(imagingDyTextBox.Text, out int value))
                    {
                        MotorCommand upCommand = new MotorCommand
                        {
                            Type = MotorCommand.CommandType.MoveRelativeAsync,
                            Motor = yMotor,
                            DistanceParameter = -value,
                            SpeedParameter = configuration.xMotorDefaultSpeed
                        };
                        motorManager.EnqueuePriorityCommand(upCommand);
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (int.TryParse(imagingDyTextBox.Text, out int value))
                    {
                        MotorCommand downCommand = new MotorCommand
                        {
                            Type = MotorCommand.CommandType.MoveRelativeAsync,
                            Motor = yMotor,
                            DistanceParameter = value,
                            SpeedParameter = configuration.xMotorDefaultSpeed
                        };
                        motorManager.EnqueuePriorityCommand(downCommand);
                    }
                }
                else if (e.KeyCode == Keys.Left)
                {
                    if (int.TryParse(imagingDxTextBox.Text, out int value))
                    {
                        MotorCommand leftCommand = new MotorCommand
                        {
                            Type = MotorCommand.CommandType.MoveRelativeAsync,
                            Motor = xMotor,
                            DistanceParameter = -value,
                            SpeedParameter = configuration.xMotorDefaultSpeed
                        };
                        motorManager.EnqueuePriorityCommand(leftCommand);
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (int.TryParse(imagingDxTextBox.Text, out int value))
                    {
                        MotorCommand rightCommand = new MotorCommand
                        {
                            Type = MotorCommand.CommandType.MoveRelativeAsync,
                            Motor = xMotor,
                            DistanceParameter = value,
                            SpeedParameter = configuration.xMotorDefaultSpeed
                        };
                        motorManager.EnqueuePriorityCommand(rightCommand);
                    }
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    if (int.TryParse(imagingDzTextBox.Text, out int value))
                    {
                        MotorCommand pageDownCommand = new MotorCommand
                        {
                            Type = MotorCommand.CommandType.MoveRelativeAsync,
                            Motor = zMotor,
                            DistanceParameter = value,
                            SpeedParameter = configuration.xMotorDefaultSpeed
                        };
                        motorManager.EnqueuePriorityCommand(pageDownCommand);
                    }
                }
                else if (e.KeyCode == Keys.PageUp)
                {
                    if (int.TryParse(imagingDzTextBox.Text, out int value))
                    {
                        MotorCommand pageUpCommand = new MotorCommand
                        {
                            Type = MotorCommand.CommandType.MoveRelativeAsync,
                            Motor = zMotor,
                            DistanceParameter = -value,
                            SpeedParameter = configuration.xMotorDefaultSpeed
                        };
                        motorManager.EnqueuePriorityCommand(pageUpCommand);
                    }
                }
            }
        }

        private void consumablesGlassOffsetComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (double.TryParse(consumablesGlassOffsetComboBox.Text, out double value))
            {
                consumablesGlassOffsetComboBox.ForeColor = Color.Black;
            }
            else
            {
                consumablesGlassOffsetComboBox.ForeColor = Color.Red;
            }
        }

        private async void imagingScanButton_Click(object sender, EventArgs e)
        {
            // Get the scanning parameters
            string experimentName = dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Experiment", imagingScanParametersDataGridView);
            string heaterLetter = dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Heater", imagingScanParametersDataGridView);
            int x0 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "x0 (\u03BCS)", imagingScanParametersDataGridView));
            int y0 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "y0 (\u03BCS)", imagingScanParametersDataGridView));
            int z0 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "z0 (\u03BCS)", imagingScanParametersDataGridView));
            int fovdx = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "FOV dX (\u03BCS)", imagingScanParametersDataGridView));
            int sampledx = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Sample dX (\u03BCS)", imagingScanParametersDataGridView));
            int dy = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "dY (\u03BCS)", imagingScanParametersDataGridView));
            double rotationalOffset = double.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Value", "Rotational Offset (\u00B0)", imagingScanParametersDataGridView));
            // Determine the Channels to image in with their associated parameters (intensity, exposure)
            bool imageBrightField = (dataGridViewManager.GetColumnCellValueByColumnAndRowName("Bright Field", "Use", imagingLEDsDataGridView) == "Yes") ? true : false;
            int intensityBrightField = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Bright Field", "Intensity (%)", imagingLEDsDataGridView));
            int exposureBrightField = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName("Bright Field", "Exposure (\u03BCs)", imagingLEDsDataGridView));
            bool imageCy5 = (dataGridViewManager.GetColumnCellValueByColumnAndRowName(Cy5.Name, "Use", imagingLEDsDataGridView) == "Yes") ? true : false;
            int intensityCy5 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Cy5.Name, "Intensity (%)", imagingLEDsDataGridView));
            int exposureCy5 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Cy5.Name, "Exposure (\u03BCs)", imagingLEDsDataGridView));
            bool imageFAM = (dataGridViewManager.GetColumnCellValueByColumnAndRowName(FAM.Name, "Use", imagingLEDsDataGridView) == "Yes") ? true : false;
            int intensityFAM = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(FAM.Name, "Intensity (%)", imagingLEDsDataGridView));
            int exposureFAM = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(FAM.Name, "Exposure (\u03BCs)", imagingLEDsDataGridView));
            bool imageHEX = (dataGridViewManager.GetColumnCellValueByColumnAndRowName(HEX.Name, "Use", imagingLEDsDataGridView) == "Yes") ? true : false;
            int intensityHEX = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(HEX.Name, "Intensity (%)", imagingLEDsDataGridView));
            int exposureHEX = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(HEX.Name, "Exposure (\u03BCs)", imagingLEDsDataGridView));
            bool imageAtto = (dataGridViewManager.GetColumnCellValueByColumnAndRowName(Atto.Name, "Use", imagingLEDsDataGridView) == "Yes") ? true : false;
            int intensityAtto = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Atto.Name, "Intensity (%)", imagingLEDsDataGridView));
            int exposureAtto = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Atto.Name, "Exposure (\u03BCs)", imagingLEDsDataGridView));
            bool imageAlexa = (dataGridViewManager.GetColumnCellValueByColumnAndRowName(Alexa.Name, "Use", imagingLEDsDataGridView) == "Yes") ? true : false;
            int intensityAlexa = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Alexa.Name, "Intensity (%)", imagingLEDsDataGridView));
            int exposureAlexa = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Alexa.Name, "Exposure (\u03BCs)", imagingLEDsDataGridView));
            bool imageCy5p5 = (dataGridViewManager.GetColumnCellValueByColumnAndRowName(Cy5p5.Name, "Use", imagingLEDsDataGridView) == "Yes") ? true : false;
            int intensityCy5p5 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Cy5p5.Name, "Intensity (%)", imagingLEDsDataGridView));
            int exposureCy5p5 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnAndRowName(Cy5p5.Name, "Exposure (\u03BCs)", imagingLEDsDataGridView));
            // Determine which parts of the chip to image
            Samples sampleNames = new Samples();
            Assays assayNames = new Assays();
            List<DataGridViewRow> sampleAndAssayRows = dataGridViewManager.GetRowsByNameWhichContain(imagingScanParametersDataGridView, "Name");
            foreach (DataGridViewRow row in sampleAndAssayRows)
            {
                if (row.Cells[0].Value.ToString().Contains("Sample"))
                {
                    if (row.Cells[1].Value == null)
                    {
                        sampleNames.Add("");
                    }
                    else
                    {
                        try
                        {
                            sampleNames.Add(row.Cells[1].Value.ToString());
                        }
                        catch (Exception ex)
                        {
                            sampleNames.Add("");
                        }
                    }
                }
                else if (row.Cells[0].Value.ToString().Contains("Assay"))
                {
                    if (row.Cells[1].Value == null)
                    {
                        assayNames.Add("");
                    }
                    else
                    {
                        try
                        {
                            assayNames.Add(row.Cells[1].Value.ToString());
                        }
                        catch (Exception ex)
                        {
                            assayNames.Add("");
                        }
                    }
                }
            }
            // Setup the Scan Parameters to use
            ScanParameters scanParameters = new ScanParameters(
                experimentName: experimentName,
                heaterLetter: heaterLetter,
                x0: x0, y0: y0, z0: z0, fOVdX: fovdx, sampledX: sampledx, dY: dy, rotationalOffset: rotationalOffset,
                imageBrightField: imageBrightField, brightFieldIntensity: intensityBrightField, brightFieldExposure: exposureBrightField,
                imageCy5: imageCy5, cy5Intensity: intensityCy5, cy5Exposure: exposureCy5,
                imageFAM, intensityFAM, exposureFAM,
                imageHEX, intensityHEX, exposureHEX,
                imageAtto, intensityAtto, exposureAtto,
                imageAlexa, intensityAlexa, exposureAlexa,
                imageCy5p5, intensityCy5p5, exposureCy5p5,
                sampleNames, assayNames);
            // Scan
            await scanManager.Scan(scanParameters);
        }

        private void imagingKillButon_Click(object sender, EventArgs e)
        {
            emailManager.SendEmail("gabriel.lopez.candales@gmail.com");
        }

        private async void imagingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Stop Streaming as long as we are not mid scan
                if (!cameraManager.Scanning)
                {
                    //pictureBoxManager.lineBitmap = cameraManager.CurrentImage;
                    // Stop streaming
                    await cameraManager.StopStreamAsync();
                    imagingPictureBox.Image = cameraManager.CurrentImage;
                    // Initializet linebitmap
                    pictureBoxManager.InitializeLineBitmap(imagingPictureBox);
                    pictureBoxManager.MouseDrawing = true;
                    pictureBoxManager.SetMouseStartPoint(e.Location);
                }

            }
        }

        private void imagingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBoxManager.MouseDrawing)
            {
                pictureBoxManager.SetMouseEndPoint(e.Location);
                pictureBoxManager.DrawTemporaryLine(imagingPictureBox);
            }
        }

        private async void imagingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBoxManager.MouseDrawing)
            {
                pictureBoxManager.MouseDrawing = false;
                pictureBoxManager.DrawLine(imagingPictureBox);
                // Start the stream again
                await cameraManager.StartStreamAsync();
            }
        }

        private void thermocyclingRemoveStepButton_Click(object sender, EventArgs e)
        {
            // Get the selected step
            // Remove the selected step
            MessageBox.Show("Removing a step has not been implemented yet.");
        }

        private void apeProtocolEditorLoadProtocolButton_Click(object sender, EventArgs e)
        {
            // Open a Load File window
            FileSearcher fileSearcher = new FileSearcher();
            var filePath = fileSearcher.GetLoadFilePath(initialDirectory: defaultAssayProtocolDirectory);
            if (filePath != null)
            {

                // Load in the protocol
                AssayProtocol assayProtocol = assayProtocolManager.LoadProtocol(filePath);
                // Set the Protocol Name Text Box
                apeProtocolEditorProtocolNameTextBox.Text = assayProtocol.Name;
                // Set the "Estimated Time"
                //TimeSpan estimatedTimeSpan = TimeSpan.FromSeconds(int.Parse(protocol.GetTimeInSeconds().ToString()));
                apeProtocolEditorEstimateTimeTextBox.Text = "2:01:53";
                // Load in the Actions to the DataGridView
                foreach (var action in assayProtocol.Actions)
                {
                    apeProtocolEditorProtocolDataGridView.Rows.Add(action.ActionText);
                }
            }
        }

        private void imagingAutofocusButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unable to Autofocus, this has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}