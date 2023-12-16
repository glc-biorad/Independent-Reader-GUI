using Independent_Reader_GUI.Forms;
using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Services;
using Independent_Reader_GUI.Utilities;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using System.ComponentModel;

namespace Independent_Reader_GUI
{
    public partial class independentReaderForm : Form
    {
        // Private attributes
        private string defaultProtocolDirectory = "C:\\Users\\u112958\\Source\\Repos\\Independent-Reader-GUI\\ThermocyclingProtocols";
        private ThermocyclingProtocol protocol = new ThermocyclingProtocol();
        private ThermocyclingProtocolPlotManager plotManager = new ThermocyclingProtocolPlotManager();
        private ThermocyclingProtocolManager protocolManager = new ThermocyclingProtocolManager();

        /// <summary>
        /// Initialization of the Form
        /// </summary>
        public independentReaderForm()
        {
            InitializeComponent();

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

            // Wire up mouse down events
            thermocyclingPlotView.MouseDown += (s, e) => thermocyclingPlotView_MouseDown(s, e);

            // Subscribe to the load event of this form to set defaults on form loading
            this.Load += new EventHandler(this.Form_Load);
        }

        /// <summary>
        /// Add default data to the Home Tab Motors Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeMotorsDefaultData()
        {
            MotorData homeMotorsX = new MotorData
            {
                Name = "x",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "300000",
                Home = "?"
            };
            MotorData homeMotorsY = new MotorData
            {
                Name = "y",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "400000",
                Home = "?"
            };
            MotorData homeMotorsZ = new MotorData
            {
                Name = "z",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "300000",
                Home = "?"
            };
            MotorData homeMotorsFilterWheel = new MotorData
            {
                Name = "Filter Wheel",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "100000",
                Home = "?"
            };
            MotorData homeMotorsClampA = new MotorData
            {
                Name = "Clamp A",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "80000",
                Home = "?"
            };
            MotorData homeMotorsClampB = new MotorData
            {
                Name = "Clamp B",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "80000",
                Home = "?"
            };
            MotorData homeMotorsClampC = new MotorData
            {
                Name = "Clamp C",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "80000",
                Home = "?"
            };
            MotorData homeMotorsClampD = new MotorData
            {
                Name = "Clamp D",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "80000",
                Home = "?"
            };
            MotorData homeMotorsTrayAB = new MotorData
            {
                Name = "Tray AB",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "200000",
                Home = "?"
            };
            MotorData homeMotorsTrayCD = new MotorData
            {
                Name = "Tray CD",
                IO = "?",
                State = "Not Connected",
                Position = "?",
                Speed = "200000",
                Home = "?"
            };
            homeMotorsDataGridView.Rows.Add(homeMotorsX.Name, homeMotorsX.IO, homeMotorsX.State, homeMotorsX.Position, homeMotorsX.Speed, homeMotorsX.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsY.Name, homeMotorsY.IO, homeMotorsY.State, homeMotorsY.Position, homeMotorsY.Speed, homeMotorsY.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsZ.Name, homeMotorsZ.IO, homeMotorsZ.State, homeMotorsZ.Position, homeMotorsZ.Speed, homeMotorsZ.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsFilterWheel.Name, homeMotorsFilterWheel.IO, homeMotorsFilterWheel.State, homeMotorsFilterWheel.Position, homeMotorsFilterWheel.Speed, homeMotorsFilterWheel.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsClampA.Name, homeMotorsClampA.IO, homeMotorsClampA.State, homeMotorsClampA.Position, homeMotorsClampA.Speed, homeMotorsClampA.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsClampB.Name, homeMotorsClampB.IO, homeMotorsClampB.State, homeMotorsClampB.Position, homeMotorsClampB.Speed, homeMotorsClampB.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsClampC.Name, homeMotorsClampC.IO, homeMotorsClampC.State, homeMotorsClampC.Position, homeMotorsClampC.Speed, homeMotorsClampC.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsClampD.Name, homeMotorsClampD.IO, homeMotorsClampD.State, homeMotorsClampD.Position, homeMotorsClampD.Speed, homeMotorsClampD.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsTrayAB.Name, homeMotorsTrayAB.IO, homeMotorsTrayAB.State, homeMotorsTrayAB.Position, homeMotorsTrayAB.Speed, homeMotorsTrayAB.Home);
            homeMotorsDataGridView.Rows.Add(homeMotorsTrayCD.Name, homeMotorsTrayCD.IO, homeMotorsTrayCD.State, homeMotorsTrayCD.Position, homeMotorsTrayCD.Speed, homeMotorsTrayCD.Home);
        }

        /// <summary>
        /// Add default data to the Home Tab LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddHomeCameraDefaultData()
        {
            CameraData homeCamera = new CameraData
            {
                State = "Not Connected",
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
            runExperimentDataGridView.Rows.Add("Start Time (HH:mm:ss)", runExperimentData.StartDateTime.ToString("HH:mm:ss"));
            runExperimentDataGridView.Rows.Add("Start Date (MM/dd/YYYY)", runExperimentData.StartDateTime.ToString("MM/dd/yyyy"));
            runExperimentDataGridView.Rows.Add("Projected End Time (HH:mm:ss)", runExperimentData.EndDateTime.ToString("HH:mm:ss"));
            runExperimentDataGridView.Rows.Add("Projected End Date (MM/dd/YYYY)", runExperimentData.EndDateTime.ToString("MM/dd/yyyy"));
            runExperimentDataGridView.Rows.Add("Heater", runExperimentData.Heater);
            runExperimentDataGridView.Rows.Add("Partition Type", runExperimentData.PartitionType);
            runExperimentDataGridView.Rows.Add("Cartridge");
            runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.CartridgeComboBoxCell;
            runExperimentDataGridView.Rows.Add("Cartridge Length (mm)", runExperimentData.CartridgeLength);
            runExperimentDataGridView.Rows.Add("Cartridge Width (mm)", runExperimentData.CartridgeWidth);
            runExperimentDataGridView.Rows.Add("Cartridge Height (mm)", runExperimentData.CartridgeHeight);
            runExperimentDataGridView.Rows.Add("Clamp Position (\u03BCs)", runExperimentData.ClampPosition);
            runExperimentDataGridView.Rows.Add("Tray Position (\u03BCs)", runExperimentData.TrayPosition);
            runExperimentDataGridView.Rows.Add("Glass Offset (mm)", runExperimentData.GlassOffset);
            runExperimentDataGridView.Rows.Add("Elastomer", runExperimentData.Elastomer);
            runExperimentDataGridView.Rows.Add("Elastomer Thickness (mm)", runExperimentData.ElastomerThickness);
            runExperimentDataGridView.Rows.Add("Bergquist");
            runExperimentDataGridView.Rows[runExperimentDataGridView.Rows.Count - 1].Cells[1] = runExperimentData.BergquistComboBoxCell;
            runExperimentDataGridView.Rows.Add("Bergquist Thickness (mm)", runExperimentData.BergquistThickness);
            runExperimentDataGridView.Rows.Add("Surface Area (mm x mm)", runExperimentData.SurfaceArea);
            runExperimentDataGridView.Rows.Add("Pressure (KPa)", runExperimentData.Pressure);
        }

        /// <summary>
        /// Add default data to the Run Imaging Setup Data Grid View upon loading the Form
        /// </summary>
        private void AddRunImagingSetupDefaultData()
        {
            ImagingSetupData runImagingSetupData = new ImagingSetupData();
            runImagingSetupDataGridView.Rows.Add("Image Before", runImagingSetupData.ImageBefore);
            runImagingSetupDataGridView.Rows.Add("Image During (FOV)", runImagingSetupData.ImageDuringFOV);
            runImagingSetupDataGridView.Rows.Add("Image During Frequency (FOV)", runImagingSetupData.ImageDuringFrequencyFOV);
            runImagingSetupDataGridView.Rows.Add("Image During (Assay)", runImagingSetupData.ImageDuringAssay);
            runImagingSetupDataGridView.Rows.Add("Image During Frequency (Assay)", runImagingSetupData.ImageDuringFrequencyAssay);
            runImagingSetupDataGridView.Rows.Add("Image During (Sample)", runImagingSetupData.ImageDuringFOV);
            runImagingSetupDataGridView.Rows.Add("Image During Frequency (Sample)", runImagingSetupData.ImageDuringFrequencySample);
            runImagingSetupDataGridView.Rows.Add("Image After", runImagingSetupData.ImageAfter);
            runImagingSetupDataGridView.Rows.Add("S3 Bucket Path", runImagingSetupData.S3BucketPath);
            runImagingSetupDataGridView.Rows.Add("Local Path", runImagingSetupData.LocalPath);
            runImagingSetupDataGridView.Rows.Add("Image in Cy5", runImagingSetupData.ImageInCy5);
            runImagingSetupDataGridView.Rows.Add("Image in FAM", runImagingSetupData.ImageInFAM);
            runImagingSetupDataGridView.Rows.Add("Image in HEX", runImagingSetupData.ImageInHEX);
            runImagingSetupDataGridView.Rows.Add("Image in Atto", runImagingSetupData.ImageInAtto);
            runImagingSetupDataGridView.Rows.Add("Image in Alexa", runImagingSetupData.ImageInAlexa);
            runImagingSetupDataGridView.Rows.Add("Image in Cy5.5", runImagingSetupData.ImageInCy5p5);
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
            controlMotorsDataGridView.Rows.Add("x", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("y", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("z", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Filter Wheel", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp A", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp B", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp C", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Clamp D", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Tray AB", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
            controlMotorsDataGridView.Rows.Add("Tray CD", controlMotorData.IO, controlMotorData.State, controlMotorData.Position, controlMotorData.Speed, controlMotorData.Home);
        }

        /// <summary>
        /// Add default data to the Control LEDs Data Grid View upon loading the Form
        /// </summary>
        private void AddControlLEDsDefaultData()
        {
            LEDsData controlLEDsData = new LEDsData();
            controlLEDsDataGridView.Rows.Add("State", "N", "N", "N", "N", "N", "N");
            controlLEDsDataGridView.Rows.Add("IO", controlLEDsData.ValueCy5, controlLEDsData.ValueFAM, controlLEDsData.ValueHEX, controlLEDsData.ValueAtto, controlLEDsData.ValueAlexa, controlLEDsData.ValueCy5p5);
            controlLEDsDataGridView.Rows.Add("Intensity (%)", controlLEDsData.ValueCy5, controlLEDsData.ValueFAM, controlLEDsData.ValueHEX, controlLEDsData.ValueAtto, controlLEDsData.ValueAlexa, controlLEDsData.ValueCy5p5);
            controlLEDsDataGridView.Rows.Add("Exposure (ms)", controlLEDsData.ValueCy5, controlLEDsData.ValueFAM, controlLEDsData.ValueHEX, controlLEDsData.ValueAtto, controlLEDsData.ValueAlexa, controlLEDsData.ValueCy5p5);
        }

        /// <summary>
        /// Add default data to the Control TECs Data Grid View upon loading the Form
        /// </summary>
        private void AddControlTECsDefaultData()
        {
            TECsData controlTECsData = new TECsData();
            controlTECsDataGridView.Rows.Add("State", "Not Connected", "Not Connected", "Not Connected", "Not Connected");
            controlTECsDataGridView.Rows.Add("IO", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Actual Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Target Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Temp Enabled", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Sink Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Sink Temp Max (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Fan RPM", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Fan on Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
            controlTECsDataGridView.Rows.Add("Fan off Temp (\u00B0C)", controlTECsData.ValueTECA, controlTECsData.ValueTECB, controlTECsData.ValueTECC, controlTECsData.ValueTECD);
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
            //var model = new PlotModel { Title = "Thermocycling Protocol" };
            //var series = new LineSeries();
            // TODO: Replace this default plot with data pulled from a protocol json file
            #region 
            // Plot the protocol
            //series.Points.Add(new DataPoint(0, 20));
            //series.Points.Add(new DataPoint(10, 20));
            //series.Points.Add(new DataPoint(10, 20));
            //series.Points.Add(new DataPoint(20, 20));
            //model.Series.Add(series);
            //var tempAnnotation = new TextAnnotation { TextPosition = new DataPoint(5, 20), Text = "20\u00B0C" };
            //var stepAnnotation = new TextAnnotation { TextPosition = new DataPoint(5, 15), Text = "Step 1" };
            //model.Annotations.Add(tempAnnotation);
            //model.Annotations.Add(stepAnnotation);
            #endregion
            // Assign the model to the plot view
            //thermocyclingPlotView.Model = model;   
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
            imagingLEDsDataGridView.Rows.Add("IO");
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[1] = ledsData.IOCy5ComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[2] = ledsData.IOFAMComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[3] = ledsData.IOHEXComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[4] = ledsData.IOAttoComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[5] = ledsData.IOAlexaComboBoxCell;
            imagingLEDsDataGridView.Rows[imagingLEDsDataGridView.Rows.Count - 1].Cells[6] = ledsData.IOCy5p5ComboBoxCell;
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var cellValue = e.Value as string;
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
            foreach (DataGridViewRow row in runImagingSetupDataGridView.Rows)
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
        }

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
        private void controlMoveButton_Click(object sender, EventArgs e)
        {
            // TODO: Replace this region of code with a reusable Utility class for checking motor connectivity
            #region To be replaced with a new Utility class for checking if a motor is connected
            int columnIndex = -1;
            string motorState = string.Empty;
            DataGridViewColumnSearcher ColumnSearcher = new DataGridViewColumnSearcher();
            // Get the motor to be moved
            string motorName = controlMotorsComboBox.Text;
            // Get the state of the motor
            if (motorName != string.Empty)
            {
                foreach (DataGridViewRow row in controlMotorsDataGridView.Rows)
                {
                    if (row.Cells[0].Value.ToString() == motorName)
                    {
                        columnIndex = ColumnSearcher.GetColumnIndexFromHeaderText("State", controlMotorsDataGridView);
                        motorState = row.Cells[columnIndex].Value.ToString();
                    }
                }
            }
            else
            {
                return;
            }
            // Move if possible else warn the user
            if (motorState == "Not Connected")
            {
                MessageBox.Show("Motor is not connected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            #endregion
            else
            {
                MessageBox.Show("Code not written yet to move motors.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Click event for the Control Tab Home motor button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlHomeButton_Click(object sender, EventArgs e)
        {
            // TODO: Replace this region of code with a reusable Utility class for checking motor connectivity
            #region To be replaced with a new Utility class for checking if a motor is connected
            int columnIndex = -1;
            string motorState = string.Empty;
            DataGridViewColumnSearcher ColumnSearcher = new DataGridViewColumnSearcher();
            // Get the motor to be homed
            string motorName = controlMotorsComboBox.Text;
            // Get the state of the motor
            if (motorName != string.Empty)
            {
                foreach (DataGridViewRow row in controlMotorsDataGridView.Rows)
                {
                    if (row.Cells[0].Value.ToString() == motorName)
                    {
                        columnIndex = ColumnSearcher.GetColumnIndexFromHeaderText("State", controlMotorsDataGridView);
                        motorState = row.Cells[columnIndex].Value.ToString();
                    }
                }
            }
            else
            {
                return;
            }
            // Home if possible else warn the user
            if (motorState == "Not Connected")
            {
                MessageBox.Show("Motor is not connected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            #endregion
            else
            {
                MessageBox.Show("Code not written yet to home motors.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // FIXME: Currently 
            FLIRCameraService cameraService = new FLIRCameraService();
            cameraService.Connect();
            cameraService.Disconnect();
        }
    }
}