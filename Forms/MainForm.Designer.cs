namespace Independent_Reader_GUI
{
    partial class independentReaderForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl = new TabControl();
            homeTabPage = new TabPage();
            homeTECsDataGridView = new DataGridView();
            homeTECsProprtyColumn = new DataGridViewTextBoxColumn();
            homeTECsTECAColumn = new DataGridViewTextBoxColumn();
            homeTECsTECBColumn = new DataGridViewTextBoxColumn();
            homeTECsTECCColumn = new DataGridViewTextBoxColumn();
            homeTECsTECDColumn = new DataGridViewTextBoxColumn();
            homeTECsLabel = new Label();
            homeLEDsDataGridView = new DataGridView();
            homeLEDsPropertiesColumn = new DataGridViewTextBoxColumn();
            homeLEDsCy5Column = new DataGridViewTextBoxColumn();
            homeLEDsFAMColumn = new DataGridViewTextBoxColumn();
            homeLEDsHEXColumn = new DataGridViewTextBoxColumn();
            homeLEDsAttoColumn = new DataGridViewTextBoxColumn();
            homeLEDsAlexaColumn = new DataGridViewTextBoxColumn();
            homeLEDsCy55Column = new DataGridViewTextBoxColumn();
            homeLEDsLabel = new Label();
            homeCameraDataGridView = new DataGridView();
            homeCameraStateColumn = new DataGridViewTextBoxColumn();
            homeCameraExposureColumn = new DataGridViewTextBoxColumn();
            homeCameraTempColumn = new DataGridViewTextBoxColumn();
            homeCameraLabel = new Label();
            homeMotorsDataGridView = new DataGridView();
            homeMotorsMotorColumn = new DataGridViewTextBoxColumn();
            homeMotorsIOColumn = new DataGridViewTextBoxColumn();
            homeMotorsStateColumn = new DataGridViewTextBoxColumn();
            homeMotorsStepsColumn = new DataGridViewTextBoxColumn();
            homeMotorsSpeedColumn = new DataGridViewTextBoxColumn();
            homeMotorsHomeColumn = new DataGridViewTextBoxColumn();
            homeMotorsLabel = new Label();
            runTabPage = new TabPage();
            runClearFormButton = new Button();
            runRemoveAssayButton = new Button();
            runAddAssayButton = new Button();
            runAssayMetaDataGridView = new DataGridView();
            runAssayMetaDataIDColumn = new DataGridViewTextBoxColumn();
            runAssayMetaDataNameColumn = new DataGridViewTextBoxColumn();
            runRemoveSampleButton = new Button();
            runAddSampleButton = new Button();
            runSampleMetaDataGridView = new DataGridView();
            runSampleMetaDataIDColumn = new DataGridViewTextBoxColumn();
            runSampleMetaDataNameColumn = new DataGridViewTextBoxColumn();
            runImagingSetupDataGridView = new DataGridView();
            runImagingSetupPropertyColumn = new DataGridViewTextBoxColumn();
            runImagingSetupValueColumn = new DataGridViewTextBoxColumn();
            runExperimentDataGridView = new DataGridView();
            runExperimentDataPropertyColumn = new DataGridViewTextBoxColumn();
            runExperimentDataValueColumn = new DataGridViewTextBoxColumn();
            runRunButton = new Button();
            runAssayMetaDataLabel = new Label();
            runSampleMetaDataLabel = new Label();
            runImagingSetupLabel = new Label();
            tunExperimentDataLabel = new Label();
            controlTabPage = new TabPage();
            controlLEDsDataGridView = new DataGridView();
            controlLEDsPropertyColumn = new DataGridViewTextBoxColumn();
            controlLEDsCy5Column = new DataGridViewTextBoxColumn();
            controlLEDsFAMColumn = new DataGridViewTextBoxColumn();
            controlLEDsHEXColumn = new DataGridViewTextBoxColumn();
            controlLEDsAttoColumn = new DataGridViewTextBoxColumn();
            controlLEDsAlexaColumn = new DataGridViewTextBoxColumn();
            controlLEDsCy5p5Column = new DataGridViewTextBoxColumn();
            controlLEDsLabel = new Label();
            controlTECDSetTempButton = new Button();
            controlTECCSetTempButton = new Button();
            controlTECBSetTempButton = new Button();
            controlTECASetTempButton = new Button();
            controlTECsDataGridView = new DataGridView();
            controlTECsPropertyColumn = new DataGridViewTextBoxColumn();
            controlTECsTECAColumn = new DataGridViewTextBoxColumn();
            controlTECsTECBColumn = new DataGridViewTextBoxColumn();
            controlTECsTECCColumn = new DataGridViewTextBoxColumn();
            controlTECsTECDColumn = new DataGridViewTextBoxColumn();
            controlTECsLabel = new Label();
            controlHomeButton = new Button();
            controlMoveButton = new Button();
            controlMotorsComboBox = new ComboBox();
            controlMotorsDataGridView = new DataGridView();
            controlMotorsMotorColumn = new DataGridViewTextBoxColumn();
            controlMotorsIOColumn = new DataGridViewTextBoxColumn();
            controlMotorsStateColumn = new DataGridViewTextBoxColumn();
            controlMotorsPositionColumn = new DataGridViewTextBoxColumn();
            controlMotorsSpeedColumn = new DataGridViewTextBoxColumn();
            controlMotorsHomeColumn = new DataGridViewTextBoxColumn();
            controlMotorsLabel = new Label();
            thermocyclingTabPage = new TabPage();
            thermocyclingAddGoToButton = new Button();
            thermocyclingTECDKillButton = new Button();
            thermocyclingTECCKillButton = new Button();
            thermocyclingTECBKillButton = new Button();
            thermocyclingTECAKillButton = new Button();
            thermocyclingTECDRunButton = new Button();
            thermocyclingTECCRunButton = new Button();
            thermocyclingTECBRunButton = new Button();
            thermocyclingTECARunButton = new Button();
            thermocyclingProtocolStatusesDataGridView = new DataGridView();
            thermocyclingProtocolStatusesPropertyColumn = new DataGridViewTextBoxColumn();
            thermocyclingProtocolStatusesTECAColumn = new DataGridViewTextBoxColumn();
            thermocyclingProtocolStatusesTECBColumn = new DataGridViewTextBoxColumn();
            thermocyclingProtocolStatusesTECCColumn = new DataGridViewTextBoxColumn();
            thermocyclingProtocolStatusesTECDColumn = new DataGridViewTextBoxColumn();
            thermocyclingProtocolStatusesLabel = new Label();
            thermocyclingLoadProtocolButton = new Button();
            thermocyclingRemoveStepButton = new Button();
            thermocyclingEditStepButton = new Button();
            thermocyclingSaveProtocolButton = new Button();
            thermocyclingAddStepButton = new Button();
            thermocyclingPlotView = new OxyPlot.WindowsForms.PlotView();
            imagingTabPage = new TabPage();
            imagingLEDsLabel = new Label();
            imagingMetaDataButton = new Button();
            imagingMetaDataLabel = new Label();
            imagingKillButon = new Button();
            imagingScanButton = new Button();
            imagingScanParametersDataGridView = new DataGridView();
            imagingScanParametersPropertyColumn = new DataGridViewTextBoxColumn();
            imagingScanParametersValueColumn = new DataGridViewTextBoxColumn();
            imagingScanParametersLabel = new Label();
            imagingDzTextBox = new TextBox();
            imagingDyTextBox = new TextBox();
            imagingDxTextBox = new TextBox();
            imagingDzLabel = new Label();
            imagingDyLabel = new Label();
            imagingDxLabel = new Label();
            imagingCaptureImageButton = new Button();
            imagingCameraViewLabel = new Label();
            imagingStreamButton = new Button();
            imagingPictureBox = new PictureBox();
            settingsTabPage = new TabPage();
            metrologyTabPage = new TabPage();
            softwareVersionLabel = new Label();
            firmwareVersionLabel = new Label();
            apiVersionLabel = new Label();
            guiVersionLabel = new Label();
            userLabel = new Label();
            logoutButton = new Button();
            resetButton = new Button();
            imagingLEDsDataGridView = new DataGridView();
            imagingLEDsPropertyColumn = new DataGridViewTextBoxColumn();
            imagingLEDsCy5Column = new DataGridViewTextBoxColumn();
            imagingLEDsFAMColumn = new DataGridViewTextBoxColumn();
            imagingLEDsHEXColumn = new DataGridViewTextBoxColumn();
            imagingLEDsAttoColumn = new DataGridViewTextBoxColumn();
            imagingLEDsAlexaColumn = new DataGridViewTextBoxColumn();
            imagingLEDsCy5p5Column = new DataGridViewTextBoxColumn();
            tabControl.SuspendLayout();
            homeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)homeTECsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)homeLEDsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)homeCameraDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)homeMotorsDataGridView).BeginInit();
            runTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)runAssayMetaDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)runSampleMetaDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)runImagingSetupDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)runExperimentDataGridView).BeginInit();
            controlTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)controlLEDsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)controlTECsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)controlMotorsDataGridView).BeginInit();
            thermocyclingTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)thermocyclingProtocolStatusesDataGridView).BeginInit();
            imagingTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imagingScanParametersDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imagingPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imagingLEDsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(homeTabPage);
            tabControl.Controls.Add(runTabPage);
            tabControl.Controls.Add(controlTabPage);
            tabControl.Controls.Add(thermocyclingTabPage);
            tabControl.Controls.Add(imagingTabPage);
            tabControl.Controls.Add(settingsTabPage);
            tabControl.Controls.Add(metrologyTabPage);
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1124, 612);
            tabControl.TabIndex = 0;
            // 
            // homeTabPage
            // 
            homeTabPage.Controls.Add(homeTECsDataGridView);
            homeTabPage.Controls.Add(homeTECsLabel);
            homeTabPage.Controls.Add(homeLEDsDataGridView);
            homeTabPage.Controls.Add(homeLEDsLabel);
            homeTabPage.Controls.Add(homeCameraDataGridView);
            homeTabPage.Controls.Add(homeCameraLabel);
            homeTabPage.Controls.Add(homeMotorsDataGridView);
            homeTabPage.Controls.Add(homeMotorsLabel);
            homeTabPage.Location = new Point(4, 24);
            homeTabPage.Name = "homeTabPage";
            homeTabPage.Padding = new Padding(3);
            homeTabPage.Size = new Size(1116, 584);
            homeTabPage.TabIndex = 0;
            homeTabPage.Text = "Home";
            homeTabPage.UseVisualStyleBackColor = true;
            // 
            // homeTECsDataGridView
            // 
            homeTECsDataGridView.AllowUserToAddRows = false;
            homeTECsDataGridView.AllowUserToDeleteRows = false;
            homeTECsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            homeTECsDataGridView.Columns.AddRange(new DataGridViewColumn[] { homeTECsProprtyColumn, homeTECsTECAColumn, homeTECsTECBColumn, homeTECsTECCColumn, homeTECsTECDColumn });
            homeTECsDataGridView.Enabled = false;
            homeTECsDataGridView.Location = new Point(549, 24);
            homeTECsDataGridView.Name = "homeTECsDataGridView";
            homeTECsDataGridView.ReadOnly = true;
            homeTECsDataGridView.RowHeadersVisible = false;
            homeTECsDataGridView.RowTemplate.Height = 25;
            homeTECsDataGridView.Size = new Size(545, 402);
            homeTECsDataGridView.TabIndex = 7;
            // 
            // homeTECsProprtyColumn
            // 
            homeTECsProprtyColumn.HeaderText = "";
            homeTECsProprtyColumn.Name = "homeTECsProprtyColumn";
            homeTECsProprtyColumn.ReadOnly = true;
            homeTECsProprtyColumn.Width = 140;
            // 
            // homeTECsTECAColumn
            // 
            homeTECsTECAColumn.HeaderText = "TEC A";
            homeTECsTECAColumn.Name = "homeTECsTECAColumn";
            homeTECsTECAColumn.ReadOnly = true;
            // 
            // homeTECsTECBColumn
            // 
            homeTECsTECBColumn.HeaderText = "TEC B";
            homeTECsTECBColumn.Name = "homeTECsTECBColumn";
            homeTECsTECBColumn.ReadOnly = true;
            // 
            // homeTECsTECCColumn
            // 
            homeTECsTECCColumn.HeaderText = "TEC C";
            homeTECsTECCColumn.Name = "homeTECsTECCColumn";
            homeTECsTECCColumn.ReadOnly = true;
            // 
            // homeTECsTECDColumn
            // 
            homeTECsTECDColumn.HeaderText = "TEC D";
            homeTECsTECDColumn.Name = "homeTECsTECDColumn";
            homeTECsTECDColumn.ReadOnly = true;
            // 
            // homeTECsLabel
            // 
            homeTECsLabel.AutoSize = true;
            homeTECsLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            homeTECsLabel.Location = new Point(549, 3);
            homeTECsLabel.Name = "homeTECsLabel";
            homeTECsLabel.Size = new Size(48, 18);
            homeTECsLabel.TabIndex = 6;
            homeTECsLabel.Text = "TECs";
            // 
            // homeLEDsDataGridView
            // 
            homeLEDsDataGridView.AllowUserToAddRows = false;
            homeLEDsDataGridView.AllowUserToDeleteRows = false;
            homeLEDsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            homeLEDsDataGridView.Columns.AddRange(new DataGridViewColumn[] { homeLEDsPropertiesColumn, homeLEDsCy5Column, homeLEDsFAMColumn, homeLEDsHEXColumn, homeLEDsAttoColumn, homeLEDsAlexaColumn, homeLEDsCy55Column });
            homeLEDsDataGridView.Enabled = false;
            homeLEDsDataGridView.Location = new Point(8, 429);
            homeLEDsDataGridView.Name = "homeLEDsDataGridView";
            homeLEDsDataGridView.ReadOnly = true;
            homeLEDsDataGridView.RowHeadersVisible = false;
            homeLEDsDataGridView.RowTemplate.Height = 25;
            homeLEDsDataGridView.Size = new Size(513, 149);
            homeLEDsDataGridView.TabIndex = 5;
            // 
            // homeLEDsPropertiesColumn
            // 
            homeLEDsPropertiesColumn.HeaderText = "";
            homeLEDsPropertiesColumn.Name = "homeLEDsPropertiesColumn";
            homeLEDsPropertiesColumn.ReadOnly = true;
            homeLEDsPropertiesColumn.Width = 90;
            // 
            // homeLEDsCy5Column
            // 
            homeLEDsCy5Column.HeaderText = "Cy5";
            homeLEDsCy5Column.Name = "homeLEDsCy5Column";
            homeLEDsCy5Column.ReadOnly = true;
            homeLEDsCy5Column.Width = 70;
            // 
            // homeLEDsFAMColumn
            // 
            homeLEDsFAMColumn.HeaderText = "FAM";
            homeLEDsFAMColumn.Name = "homeLEDsFAMColumn";
            homeLEDsFAMColumn.ReadOnly = true;
            homeLEDsFAMColumn.Width = 70;
            // 
            // homeLEDsHEXColumn
            // 
            homeLEDsHEXColumn.HeaderText = "HEX";
            homeLEDsHEXColumn.Name = "homeLEDsHEXColumn";
            homeLEDsHEXColumn.ReadOnly = true;
            homeLEDsHEXColumn.Width = 70;
            // 
            // homeLEDsAttoColumn
            // 
            homeLEDsAttoColumn.HeaderText = "Atto";
            homeLEDsAttoColumn.Name = "homeLEDsAttoColumn";
            homeLEDsAttoColumn.ReadOnly = true;
            homeLEDsAttoColumn.Width = 70;
            // 
            // homeLEDsAlexaColumn
            // 
            homeLEDsAlexaColumn.HeaderText = "Alexa";
            homeLEDsAlexaColumn.Name = "homeLEDsAlexaColumn";
            homeLEDsAlexaColumn.ReadOnly = true;
            homeLEDsAlexaColumn.Width = 70;
            // 
            // homeLEDsCy55Column
            // 
            homeLEDsCy55Column.HeaderText = "Cy5.5";
            homeLEDsCy55Column.Name = "homeLEDsCy55Column";
            homeLEDsCy55Column.ReadOnly = true;
            homeLEDsCy55Column.Width = 70;
            // 
            // homeLEDsLabel
            // 
            homeLEDsLabel.AutoSize = true;
            homeLEDsLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            homeLEDsLabel.Location = new Point(8, 408);
            homeLEDsLabel.Name = "homeLEDsLabel";
            homeLEDsLabel.Size = new Size(48, 18);
            homeLEDsLabel.TabIndex = 4;
            homeLEDsLabel.Text = "LEDs";
            // 
            // homeCameraDataGridView
            // 
            homeCameraDataGridView.AllowUserToAddRows = false;
            homeCameraDataGridView.AllowUserToDeleteRows = false;
            homeCameraDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            homeCameraDataGridView.Columns.AddRange(new DataGridViewColumn[] { homeCameraStateColumn, homeCameraExposureColumn, homeCameraTempColumn });
            homeCameraDataGridView.Enabled = false;
            homeCameraDataGridView.Location = new Point(8, 335);
            homeCameraDataGridView.Name = "homeCameraDataGridView";
            homeCameraDataGridView.ReadOnly = true;
            homeCameraDataGridView.RowHeadersVisible = false;
            homeCameraDataGridView.RowTemplate.Height = 25;
            homeCameraDataGridView.Size = new Size(284, 70);
            homeCameraDataGridView.TabIndex = 3;
            // 
            // homeCameraStateColumn
            // 
            homeCameraStateColumn.HeaderText = "State";
            homeCameraStateColumn.Name = "homeCameraStateColumn";
            homeCameraStateColumn.ReadOnly = true;
            // 
            // homeCameraExposureColumn
            // 
            homeCameraExposureColumn.HeaderText = "Exposure (ms)";
            homeCameraExposureColumn.Name = "homeCameraExposureColumn";
            homeCameraExposureColumn.ReadOnly = true;
            homeCameraExposureColumn.Width = 120;
            // 
            // homeCameraTempColumn
            // 
            homeCameraTempColumn.HeaderText = "Temp (K)";
            homeCameraTempColumn.Name = "homeCameraTempColumn";
            homeCameraTempColumn.ReadOnly = true;
            homeCameraTempColumn.Width = 60;
            // 
            // homeCameraLabel
            // 
            homeCameraLabel.AutoSize = true;
            homeCameraLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            homeCameraLabel.Location = new Point(8, 314);
            homeCameraLabel.Name = "homeCameraLabel";
            homeCameraLabel.Size = new Size(65, 18);
            homeCameraLabel.TabIndex = 2;
            homeCameraLabel.Text = "Camera";
            // 
            // homeMotorsDataGridView
            // 
            homeMotorsDataGridView.AllowUserToAddRows = false;
            homeMotorsDataGridView.AllowUserToDeleteRows = false;
            homeMotorsDataGridView.AllowUserToResizeRows = false;
            homeMotorsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            homeMotorsDataGridView.Columns.AddRange(new DataGridViewColumn[] { homeMotorsMotorColumn, homeMotorsIOColumn, homeMotorsStateColumn, homeMotorsStepsColumn, homeMotorsSpeedColumn, homeMotorsHomeColumn });
            homeMotorsDataGridView.Enabled = false;
            homeMotorsDataGridView.Location = new Point(8, 24);
            homeMotorsDataGridView.Name = "homeMotorsDataGridView";
            homeMotorsDataGridView.ReadOnly = true;
            homeMotorsDataGridView.RowHeadersVisible = false;
            homeMotorsDataGridView.RowHeadersWidth = 10;
            homeMotorsDataGridView.RowTemplate.DefaultCellStyle.Font = new Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            homeMotorsDataGridView.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            homeMotorsDataGridView.RowTemplate.Height = 25;
            homeMotorsDataGridView.RowTemplate.Resizable = DataGridViewTriState.True;
            homeMotorsDataGridView.Size = new Size(513, 287);
            homeMotorsDataGridView.TabIndex = 1;
            // 
            // homeMotorsMotorColumn
            // 
            homeMotorsMotorColumn.HeaderText = "Motor";
            homeMotorsMotorColumn.Name = "homeMotorsMotorColumn";
            homeMotorsMotorColumn.ReadOnly = true;
            homeMotorsMotorColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            homeMotorsMotorColumn.Width = 80;
            // 
            // homeMotorsIOColumn
            // 
            homeMotorsIOColumn.HeaderText = "IO";
            homeMotorsIOColumn.Name = "homeMotorsIOColumn";
            homeMotorsIOColumn.ReadOnly = true;
            homeMotorsIOColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            homeMotorsIOColumn.ToolTipText = "Action status of motor";
            homeMotorsIOColumn.Width = 70;
            // 
            // homeMotorsStateColumn
            // 
            homeMotorsStateColumn.HeaderText = "State";
            homeMotorsStateColumn.Name = "homeMotorsStateColumn";
            homeMotorsStateColumn.ReadOnly = true;
            homeMotorsStateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // homeMotorsStepsColumn
            // 
            homeMotorsStepsColumn.HeaderText = "Position (μS)";
            homeMotorsStepsColumn.Name = "homeMotorsStepsColumn";
            homeMotorsStepsColumn.ReadOnly = true;
            homeMotorsStepsColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            homeMotorsStepsColumn.Width = 90;
            // 
            // homeMotorsSpeedColumn
            // 
            homeMotorsSpeedColumn.HeaderText = "Speed (μS/s)";
            homeMotorsSpeedColumn.Name = "homeMotorsSpeedColumn";
            homeMotorsSpeedColumn.ReadOnly = true;
            homeMotorsSpeedColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // homeMotorsHomeColumn
            // 
            homeMotorsHomeColumn.HeaderText = "Home";
            homeMotorsHomeColumn.Name = "homeMotorsHomeColumn";
            homeMotorsHomeColumn.ReadOnly = true;
            homeMotorsHomeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            homeMotorsHomeColumn.ToolTipText = "Home status of motor";
            homeMotorsHomeColumn.Width = 70;
            // 
            // homeMotorsLabel
            // 
            homeMotorsLabel.AutoSize = true;
            homeMotorsLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            homeMotorsLabel.Location = new Point(8, 3);
            homeMotorsLabel.Name = "homeMotorsLabel";
            homeMotorsLabel.Size = new Size(56, 18);
            homeMotorsLabel.TabIndex = 0;
            homeMotorsLabel.Text = "Motors";
            // 
            // runTabPage
            // 
            runTabPage.Controls.Add(runClearFormButton);
            runTabPage.Controls.Add(runRemoveAssayButton);
            runTabPage.Controls.Add(runAddAssayButton);
            runTabPage.Controls.Add(runAssayMetaDataGridView);
            runTabPage.Controls.Add(runRemoveSampleButton);
            runTabPage.Controls.Add(runAddSampleButton);
            runTabPage.Controls.Add(runSampleMetaDataGridView);
            runTabPage.Controls.Add(runImagingSetupDataGridView);
            runTabPage.Controls.Add(runExperimentDataGridView);
            runTabPage.Controls.Add(runRunButton);
            runTabPage.Controls.Add(runAssayMetaDataLabel);
            runTabPage.Controls.Add(runSampleMetaDataLabel);
            runTabPage.Controls.Add(runImagingSetupLabel);
            runTabPage.Controls.Add(tunExperimentDataLabel);
            runTabPage.Location = new Point(4, 24);
            runTabPage.Name = "runTabPage";
            runTabPage.Padding = new Padding(3);
            runTabPage.Size = new Size(1116, 584);
            runTabPage.TabIndex = 1;
            runTabPage.Text = "Run";
            runTabPage.UseVisualStyleBackColor = true;
            // 
            // runClearFormButton
            // 
            runClearFormButton.BackColor = Color.IndianRed;
            runClearFormButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runClearFormButton.ForeColor = Color.White;
            runClearFormButton.Location = new Point(999, 542);
            runClearFormButton.Name = "runClearFormButton";
            runClearFormButton.Size = new Size(109, 36);
            runClearFormButton.TabIndex = 13;
            runClearFormButton.Text = "Clear Form";
            runClearFormButton.UseVisualStyleBackColor = false;
            // 
            // runRemoveAssayButton
            // 
            runRemoveAssayButton.BackColor = Color.IndianRed;
            runRemoveAssayButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runRemoveAssayButton.ForeColor = Color.White;
            runRemoveAssayButton.Location = new Point(957, 453);
            runRemoveAssayButton.Name = "runRemoveAssayButton";
            runRemoveAssayButton.Size = new Size(151, 32);
            runRemoveAssayButton.TabIndex = 12;
            runRemoveAssayButton.Text = "Remove Assay";
            runRemoveAssayButton.UseVisualStyleBackColor = false;
            // 
            // runAddAssayButton
            // 
            runAddAssayButton.BackColor = Color.MediumSeaGreen;
            runAddAssayButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runAddAssayButton.ForeColor = Color.White;
            runAddAssayButton.Location = new Point(800, 453);
            runAddAssayButton.Name = "runAddAssayButton";
            runAddAssayButton.Size = new Size(151, 32);
            runAddAssayButton.TabIndex = 11;
            runAddAssayButton.Text = "Add Assay";
            runAddAssayButton.UseVisualStyleBackColor = false;
            // 
            // runAssayMetaDataGridView
            // 
            runAssayMetaDataGridView.AllowUserToAddRows = false;
            runAssayMetaDataGridView.AllowUserToDeleteRows = false;
            runAssayMetaDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            runAssayMetaDataGridView.Columns.AddRange(new DataGridViewColumn[] { runAssayMetaDataIDColumn, runAssayMetaDataNameColumn });
            runAssayMetaDataGridView.Location = new Point(781, 344);
            runAssayMetaDataGridView.Name = "runAssayMetaDataGridView";
            runAssayMetaDataGridView.RowHeadersVisible = false;
            runAssayMetaDataGridView.RowTemplate.Height = 25;
            runAssayMetaDataGridView.Size = new Size(327, 103);
            runAssayMetaDataGridView.TabIndex = 10;
            // 
            // runAssayMetaDataIDColumn
            // 
            runAssayMetaDataIDColumn.HeaderText = "ID";
            runAssayMetaDataIDColumn.Name = "runAssayMetaDataIDColumn";
            // 
            // runAssayMetaDataNameColumn
            // 
            runAssayMetaDataNameColumn.HeaderText = "Name";
            runAssayMetaDataNameColumn.Name = "runAssayMetaDataNameColumn";
            runAssayMetaDataNameColumn.Width = 224;
            // 
            // runRemoveSampleButton
            // 
            runRemoveSampleButton.BackColor = Color.IndianRed;
            runRemoveSampleButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runRemoveSampleButton.ForeColor = Color.White;
            runRemoveSampleButton.Location = new Point(957, 273);
            runRemoveSampleButton.Name = "runRemoveSampleButton";
            runRemoveSampleButton.Size = new Size(151, 32);
            runRemoveSampleButton.TabIndex = 9;
            runRemoveSampleButton.Text = "Remove Sample";
            runRemoveSampleButton.UseVisualStyleBackColor = false;
            runRemoveSampleButton.Click += runRemoveSampleButton_Click;
            // 
            // runAddSampleButton
            // 
            runAddSampleButton.BackColor = Color.MediumSeaGreen;
            runAddSampleButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runAddSampleButton.ForeColor = Color.White;
            runAddSampleButton.Location = new Point(800, 273);
            runAddSampleButton.Name = "runAddSampleButton";
            runAddSampleButton.Size = new Size(151, 32);
            runAddSampleButton.TabIndex = 8;
            runAddSampleButton.Text = "Add Sample";
            runAddSampleButton.UseVisualStyleBackColor = false;
            runAddSampleButton.Click += runAddSampleButton_Click;
            // 
            // runSampleMetaDataGridView
            // 
            runSampleMetaDataGridView.AllowUserToAddRows = false;
            runSampleMetaDataGridView.AllowUserToResizeColumns = false;
            runSampleMetaDataGridView.AllowUserToResizeRows = false;
            runSampleMetaDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            runSampleMetaDataGridView.Columns.AddRange(new DataGridViewColumn[] { runSampleMetaDataIDColumn, runSampleMetaDataNameColumn });
            runSampleMetaDataGridView.Location = new Point(781, 24);
            runSampleMetaDataGridView.Name = "runSampleMetaDataGridView";
            runSampleMetaDataGridView.RowHeadersVisible = false;
            runSampleMetaDataGridView.RowTemplate.Height = 25;
            runSampleMetaDataGridView.Size = new Size(327, 243);
            runSampleMetaDataGridView.TabIndex = 7;
            // 
            // runSampleMetaDataIDColumn
            // 
            runSampleMetaDataIDColumn.HeaderText = "ID";
            runSampleMetaDataIDColumn.Name = "runSampleMetaDataIDColumn";
            // 
            // runSampleMetaDataNameColumn
            // 
            runSampleMetaDataNameColumn.HeaderText = "Name";
            runSampleMetaDataNameColumn.Name = "runSampleMetaDataNameColumn";
            runSampleMetaDataNameColumn.Width = 224;
            // 
            // runImagingSetupDataGridView
            // 
            runImagingSetupDataGridView.AllowUserToAddRows = false;
            runImagingSetupDataGridView.AllowUserToDeleteRows = false;
            runImagingSetupDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            runImagingSetupDataGridView.Columns.AddRange(new DataGridViewColumn[] { runImagingSetupPropertyColumn, runImagingSetupValueColumn });
            runImagingSetupDataGridView.Location = new Point(396, 24);
            runImagingSetupDataGridView.Name = "runImagingSetupDataGridView";
            runImagingSetupDataGridView.RowHeadersVisible = false;
            runImagingSetupDataGridView.RowTemplate.Height = 25;
            runImagingSetupDataGridView.Size = new Size(379, 554);
            runImagingSetupDataGridView.TabIndex = 6;
            // 
            // runImagingSetupPropertyColumn
            // 
            runImagingSetupPropertyColumn.HeaderText = "Property";
            runImagingSetupPropertyColumn.Name = "runImagingSetupPropertyColumn";
            runImagingSetupPropertyColumn.ReadOnly = true;
            runImagingSetupPropertyColumn.Width = 170;
            // 
            // runImagingSetupValueColumn
            // 
            runImagingSetupValueColumn.HeaderText = "Value";
            runImagingSetupValueColumn.Name = "runImagingSetupValueColumn";
            runImagingSetupValueColumn.Width = 185;
            // 
            // runExperimentDataGridView
            // 
            runExperimentDataGridView.AllowUserToAddRows = false;
            runExperimentDataGridView.AllowUserToDeleteRows = false;
            runExperimentDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            runExperimentDataGridView.Columns.AddRange(new DataGridViewColumn[] { runExperimentDataPropertyColumn, runExperimentDataValueColumn });
            runExperimentDataGridView.Location = new Point(6, 24);
            runExperimentDataGridView.Name = "runExperimentDataGridView";
            runExperimentDataGridView.RowHeadersVisible = false;
            runExperimentDataGridView.RowTemplate.Height = 25;
            runExperimentDataGridView.Size = new Size(384, 554);
            runExperimentDataGridView.TabIndex = 5;
            // 
            // runExperimentDataPropertyColumn
            // 
            runExperimentDataPropertyColumn.HeaderText = "Property";
            runExperimentDataPropertyColumn.Name = "runExperimentDataPropertyColumn";
            runExperimentDataPropertyColumn.ReadOnly = true;
            runExperimentDataPropertyColumn.Width = 190;
            // 
            // runExperimentDataValueColumn
            // 
            runExperimentDataValueColumn.HeaderText = "Value";
            runExperimentDataValueColumn.Name = "runExperimentDataValueColumn";
            runExperimentDataValueColumn.Width = 190;
            // 
            // runRunButton
            // 
            runRunButton.BackColor = SystemColors.Highlight;
            runRunButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runRunButton.ForeColor = Color.White;
            runRunButton.Location = new Point(999, 500);
            runRunButton.Name = "runRunButton";
            runRunButton.Size = new Size(109, 36);
            runRunButton.TabIndex = 4;
            runRunButton.Text = "Run";
            runRunButton.UseVisualStyleBackColor = false;
            // 
            // runAssayMetaDataLabel
            // 
            runAssayMetaDataLabel.AutoSize = true;
            runAssayMetaDataLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runAssayMetaDataLabel.Location = new Point(781, 323);
            runAssayMetaDataLabel.Name = "runAssayMetaDataLabel";
            runAssayMetaDataLabel.Size = new Size(128, 18);
            runAssayMetaDataLabel.TabIndex = 3;
            runAssayMetaDataLabel.Text = "Assay Meta Data";
            // 
            // runSampleMetaDataLabel
            // 
            runSampleMetaDataLabel.AutoSize = true;
            runSampleMetaDataLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runSampleMetaDataLabel.Location = new Point(781, 3);
            runSampleMetaDataLabel.Name = "runSampleMetaDataLabel";
            runSampleMetaDataLabel.Size = new Size(139, 18);
            runSampleMetaDataLabel.TabIndex = 2;
            runSampleMetaDataLabel.Text = "Sample Meta Data";
            // 
            // runImagingSetupLabel
            // 
            runImagingSetupLabel.AutoSize = true;
            runImagingSetupLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            runImagingSetupLabel.Location = new Point(396, 3);
            runImagingSetupLabel.Name = "runImagingSetupLabel";
            runImagingSetupLabel.Size = new Size(108, 18);
            runImagingSetupLabel.TabIndex = 1;
            runImagingSetupLabel.Text = "Imaging Setup";
            // 
            // tunExperimentDataLabel
            // 
            tunExperimentDataLabel.AutoSize = true;
            tunExperimentDataLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tunExperimentDataLabel.Location = new Point(3, 3);
            tunExperimentDataLabel.Name = "tunExperimentDataLabel";
            tunExperimentDataLabel.Size = new Size(125, 18);
            tunExperimentDataLabel.TabIndex = 0;
            tunExperimentDataLabel.Text = "Experiment Data";
            // 
            // controlTabPage
            // 
            controlTabPage.Controls.Add(controlLEDsDataGridView);
            controlTabPage.Controls.Add(controlLEDsLabel);
            controlTabPage.Controls.Add(controlTECDSetTempButton);
            controlTabPage.Controls.Add(controlTECCSetTempButton);
            controlTabPage.Controls.Add(controlTECBSetTempButton);
            controlTabPage.Controls.Add(controlTECASetTempButton);
            controlTabPage.Controls.Add(controlTECsDataGridView);
            controlTabPage.Controls.Add(controlTECsLabel);
            controlTabPage.Controls.Add(controlHomeButton);
            controlTabPage.Controls.Add(controlMoveButton);
            controlTabPage.Controls.Add(controlMotorsComboBox);
            controlTabPage.Controls.Add(controlMotorsDataGridView);
            controlTabPage.Controls.Add(controlMotorsLabel);
            controlTabPage.Location = new Point(4, 24);
            controlTabPage.Name = "controlTabPage";
            controlTabPage.Size = new Size(1116, 584);
            controlTabPage.TabIndex = 2;
            controlTabPage.Text = "Control";
            controlTabPage.UseVisualStyleBackColor = true;
            // 
            // controlLEDsDataGridView
            // 
            controlLEDsDataGridView.AllowUserToAddRows = false;
            controlLEDsDataGridView.AllowUserToDeleteRows = false;
            controlLEDsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            controlLEDsDataGridView.Columns.AddRange(new DataGridViewColumn[] { controlLEDsPropertyColumn, controlLEDsCy5Column, controlLEDsFAMColumn, controlLEDsHEXColumn, controlLEDsAttoColumn, controlLEDsAlexaColumn, controlLEDsCy5p5Column });
            controlLEDsDataGridView.Location = new Point(8, 396);
            controlLEDsDataGridView.Name = "controlLEDsDataGridView";
            controlLEDsDataGridView.RowHeadersVisible = false;
            controlLEDsDataGridView.RowTemplate.Height = 25;
            controlLEDsDataGridView.Size = new Size(423, 171);
            controlLEDsDataGridView.TabIndex = 12;
            // 
            // controlLEDsPropertyColumn
            // 
            controlLEDsPropertyColumn.HeaderText = "";
            controlLEDsPropertyColumn.Name = "controlLEDsPropertyColumn";
            controlLEDsPropertyColumn.ReadOnly = true;
            controlLEDsPropertyColumn.Width = 120;
            // 
            // controlLEDsCy5Column
            // 
            controlLEDsCy5Column.HeaderText = "Cy5";
            controlLEDsCy5Column.Name = "controlLEDsCy5Column";
            controlLEDsCy5Column.Width = 50;
            // 
            // controlLEDsFAMColumn
            // 
            controlLEDsFAMColumn.HeaderText = "FAM";
            controlLEDsFAMColumn.Name = "controlLEDsFAMColumn";
            controlLEDsFAMColumn.Width = 50;
            // 
            // controlLEDsHEXColumn
            // 
            controlLEDsHEXColumn.HeaderText = "HEX";
            controlLEDsHEXColumn.Name = "controlLEDsHEXColumn";
            controlLEDsHEXColumn.Width = 50;
            // 
            // controlLEDsAttoColumn
            // 
            controlLEDsAttoColumn.HeaderText = "Atto";
            controlLEDsAttoColumn.Name = "controlLEDsAttoColumn";
            controlLEDsAttoColumn.Width = 50;
            // 
            // controlLEDsAlexaColumn
            // 
            controlLEDsAlexaColumn.HeaderText = "Alexa";
            controlLEDsAlexaColumn.Name = "controlLEDsAlexaColumn";
            controlLEDsAlexaColumn.Width = 50;
            // 
            // controlLEDsCy5p5Column
            // 
            controlLEDsCy5p5Column.HeaderText = "Cy5.5";
            controlLEDsCy5p5Column.Name = "controlLEDsCy5p5Column";
            controlLEDsCy5p5Column.Width = 50;
            // 
            // controlLEDsLabel
            // 
            controlLEDsLabel.AutoSize = true;
            controlLEDsLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlLEDsLabel.Location = new Point(8, 375);
            controlLEDsLabel.Name = "controlLEDsLabel";
            controlLEDsLabel.Size = new Size(48, 18);
            controlLEDsLabel.TabIndex = 11;
            controlLEDsLabel.Text = "LEDs";
            // 
            // controlTECDSetTempButton
            // 
            controlTECDSetTempButton.BackColor = SystemColors.Highlight;
            controlTECDSetTempButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlTECDSetTempButton.ForeColor = Color.White;
            controlTECDSetTempButton.Location = new Point(996, 545);
            controlTECDSetTempButton.Name = "controlTECDSetTempButton";
            controlTECDSetTempButton.Size = new Size(109, 36);
            controlTECDSetTempButton.TabIndex = 10;
            controlTECDSetTempButton.Text = "Set Temp";
            controlTECDSetTempButton.UseVisualStyleBackColor = false;
            // 
            // controlTECCSetTempButton
            // 
            controlTECCSetTempButton.BackColor = SystemColors.Highlight;
            controlTECCSetTempButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlTECCSetTempButton.ForeColor = Color.White;
            controlTECCSetTempButton.Location = new Point(881, 545);
            controlTECCSetTempButton.Name = "controlTECCSetTempButton";
            controlTECCSetTempButton.Size = new Size(109, 36);
            controlTECCSetTempButton.TabIndex = 9;
            controlTECCSetTempButton.Text = "Set Temp";
            controlTECCSetTempButton.UseVisualStyleBackColor = false;
            // 
            // controlTECBSetTempButton
            // 
            controlTECBSetTempButton.BackColor = SystemColors.Highlight;
            controlTECBSetTempButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlTECBSetTempButton.ForeColor = Color.White;
            controlTECBSetTempButton.Location = new Point(767, 545);
            controlTECBSetTempButton.Name = "controlTECBSetTempButton";
            controlTECBSetTempButton.Size = new Size(109, 36);
            controlTECBSetTempButton.TabIndex = 8;
            controlTECBSetTempButton.Text = "Set Temp";
            controlTECBSetTempButton.UseVisualStyleBackColor = false;
            // 
            // controlTECASetTempButton
            // 
            controlTECASetTempButton.BackColor = SystemColors.Highlight;
            controlTECASetTempButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlTECASetTempButton.ForeColor = Color.White;
            controlTECASetTempButton.Location = new Point(652, 545);
            controlTECASetTempButton.Name = "controlTECASetTempButton";
            controlTECASetTempButton.Size = new Size(109, 36);
            controlTECASetTempButton.TabIndex = 7;
            controlTECASetTempButton.Text = "Set Temp";
            controlTECASetTempButton.UseVisualStyleBackColor = false;
            // 
            // controlTECsDataGridView
            // 
            controlTECsDataGridView.AllowUserToAddRows = false;
            controlTECsDataGridView.AllowUserToDeleteRows = false;
            controlTECsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            controlTECsDataGridView.Columns.AddRange(new DataGridViewColumn[] { controlTECsPropertyColumn, controlTECsTECAColumn, controlTECsTECBColumn, controlTECsTECCColumn, controlTECsTECDColumn });
            controlTECsDataGridView.Location = new Point(539, 26);
            controlTECsDataGridView.Name = "controlTECsDataGridView";
            controlTECsDataGridView.RowHeadersVisible = false;
            controlTECsDataGridView.RowTemplate.Height = 25;
            controlTECsDataGridView.Size = new Size(569, 515);
            controlTECsDataGridView.TabIndex = 6;
            // 
            // controlTECsPropertyColumn
            // 
            controlTECsPropertyColumn.HeaderText = "";
            controlTECsPropertyColumn.Name = "controlTECsPropertyColumn";
            controlTECsPropertyColumn.ReadOnly = true;
            controlTECsPropertyColumn.Width = 140;
            // 
            // controlTECsTECAColumn
            // 
            controlTECsTECAColumn.HeaderText = "TEC A";
            controlTECsTECAColumn.Name = "controlTECsTECAColumn";
            controlTECsTECAColumn.Width = 105;
            // 
            // controlTECsTECBColumn
            // 
            controlTECsTECBColumn.HeaderText = "TEC B";
            controlTECsTECBColumn.Name = "controlTECsTECBColumn";
            controlTECsTECBColumn.Width = 105;
            // 
            // controlTECsTECCColumn
            // 
            controlTECsTECCColumn.HeaderText = "TEC C";
            controlTECsTECCColumn.Name = "controlTECsTECCColumn";
            controlTECsTECCColumn.Width = 105;
            // 
            // controlTECsTECDColumn
            // 
            controlTECsTECDColumn.HeaderText = "TEC D";
            controlTECsTECDColumn.Name = "controlTECsTECDColumn";
            controlTECsTECDColumn.Width = 105;
            // 
            // controlTECsLabel
            // 
            controlTECsLabel.AutoSize = true;
            controlTECsLabel.BackColor = Color.Transparent;
            controlTECsLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlTECsLabel.Location = new Point(539, 5);
            controlTECsLabel.Name = "controlTECsLabel";
            controlTECsLabel.Size = new Size(48, 18);
            controlTECsLabel.TabIndex = 5;
            controlTECsLabel.Text = "TECs";
            // 
            // controlHomeButton
            // 
            controlHomeButton.BackColor = Color.MediumSeaGreen;
            controlHomeButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlHomeButton.ForeColor = Color.White;
            controlHomeButton.Location = new Point(424, 325);
            controlHomeButton.Name = "controlHomeButton";
            controlHomeButton.Size = new Size(109, 36);
            controlHomeButton.TabIndex = 4;
            controlHomeButton.Text = "Home";
            controlHomeButton.UseVisualStyleBackColor = false;
            controlHomeButton.Click += controlHomeButton_Click;
            // 
            // controlMoveButton
            // 
            controlMoveButton.BackColor = SystemColors.Highlight;
            controlMoveButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlMoveButton.ForeColor = Color.White;
            controlMoveButton.Location = new Point(309, 326);
            controlMoveButton.Name = "controlMoveButton";
            controlMoveButton.Size = new Size(109, 36);
            controlMoveButton.TabIndex = 3;
            controlMoveButton.Text = "Move";
            controlMoveButton.UseVisualStyleBackColor = false;
            controlMoveButton.Click += controlMoveButton_Click;
            // 
            // controlMotorsComboBox
            // 
            controlMotorsComboBox.DisplayMember = "x";
            controlMotorsComboBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point);
            controlMotorsComboBox.FormattingEnabled = true;
            controlMotorsComboBox.ItemHeight = 15;
            controlMotorsComboBox.Items.AddRange(new object[] { "x", "y", "z", "Filter Wheel", "Clamp A", "Clamp B", "Clamp C", "Clamp D", "Tray AB", "Tray CD" });
            controlMotorsComboBox.Location = new Point(171, 333);
            controlMotorsComboBox.Name = "controlMotorsComboBox";
            controlMotorsComboBox.Size = new Size(132, 23);
            controlMotorsComboBox.TabIndex = 2;
            controlMotorsComboBox.ValueMember = "x";
            // 
            // controlMotorsDataGridView
            // 
            controlMotorsDataGridView.AllowUserToAddRows = false;
            controlMotorsDataGridView.AllowUserToDeleteRows = false;
            controlMotorsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            controlMotorsDataGridView.Columns.AddRange(new DataGridViewColumn[] { controlMotorsMotorColumn, controlMotorsIOColumn, controlMotorsStateColumn, controlMotorsPositionColumn, controlMotorsSpeedColumn, controlMotorsHomeColumn });
            controlMotorsDataGridView.Location = new Point(8, 26);
            controlMotorsDataGridView.Name = "controlMotorsDataGridView";
            controlMotorsDataGridView.RowHeadersVisible = false;
            controlMotorsDataGridView.RowTemplate.Height = 25;
            controlMotorsDataGridView.Size = new Size(525, 294);
            controlMotorsDataGridView.TabIndex = 1;
            // 
            // controlMotorsMotorColumn
            // 
            controlMotorsMotorColumn.HeaderText = "Motor";
            controlMotorsMotorColumn.Name = "controlMotorsMotorColumn";
            controlMotorsMotorColumn.ReadOnly = true;
            controlMotorsMotorColumn.Width = 80;
            // 
            // controlMotorsIOColumn
            // 
            controlMotorsIOColumn.HeaderText = "IO";
            controlMotorsIOColumn.Name = "controlMotorsIOColumn";
            controlMotorsIOColumn.ReadOnly = true;
            controlMotorsIOColumn.Width = 70;
            // 
            // controlMotorsStateColumn
            // 
            controlMotorsStateColumn.HeaderText = "State";
            controlMotorsStateColumn.Name = "controlMotorsStateColumn";
            controlMotorsStateColumn.ReadOnly = true;
            // 
            // controlMotorsPositionColumn
            // 
            controlMotorsPositionColumn.HeaderText = "Position (μS)";
            controlMotorsPositionColumn.Name = "controlMotorsPositionColumn";
            // 
            // controlMotorsSpeedColumn
            // 
            controlMotorsSpeedColumn.HeaderText = "Speed (μS/s)";
            controlMotorsSpeedColumn.Name = "controlMotorsSpeedColumn";
            // 
            // controlMotorsHomeColumn
            // 
            controlMotorsHomeColumn.HeaderText = "Home";
            controlMotorsHomeColumn.Name = "controlMotorsHomeColumn";
            controlMotorsHomeColumn.ReadOnly = true;
            controlMotorsHomeColumn.Width = 70;
            // 
            // controlMotorsLabel
            // 
            controlMotorsLabel.AutoSize = true;
            controlMotorsLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlMotorsLabel.Location = new Point(8, 5);
            controlMotorsLabel.Name = "controlMotorsLabel";
            controlMotorsLabel.Size = new Size(56, 18);
            controlMotorsLabel.TabIndex = 0;
            controlMotorsLabel.Text = "Motors";
            // 
            // thermocyclingTabPage
            // 
            thermocyclingTabPage.Controls.Add(thermocyclingAddGoToButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECDKillButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECCKillButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECBKillButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECAKillButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECDRunButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECCRunButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECBRunButton);
            thermocyclingTabPage.Controls.Add(thermocyclingTECARunButton);
            thermocyclingTabPage.Controls.Add(thermocyclingProtocolStatusesDataGridView);
            thermocyclingTabPage.Controls.Add(thermocyclingProtocolStatusesLabel);
            thermocyclingTabPage.Controls.Add(thermocyclingLoadProtocolButton);
            thermocyclingTabPage.Controls.Add(thermocyclingRemoveStepButton);
            thermocyclingTabPage.Controls.Add(thermocyclingEditStepButton);
            thermocyclingTabPage.Controls.Add(thermocyclingSaveProtocolButton);
            thermocyclingTabPage.Controls.Add(thermocyclingAddStepButton);
            thermocyclingTabPage.Controls.Add(thermocyclingPlotView);
            thermocyclingTabPage.Location = new Point(4, 24);
            thermocyclingTabPage.Name = "thermocyclingTabPage";
            thermocyclingTabPage.Size = new Size(1116, 584);
            thermocyclingTabPage.TabIndex = 3;
            thermocyclingTabPage.Text = "Thermocycling";
            thermocyclingTabPage.UseVisualStyleBackColor = true;
            // 
            // thermocyclingAddGoToButton
            // 
            thermocyclingAddGoToButton.BackColor = Color.MediumSeaGreen;
            thermocyclingAddGoToButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingAddGoToButton.ForeColor = Color.White;
            thermocyclingAddGoToButton.Location = new Point(8, 495);
            thermocyclingAddGoToButton.Name = "thermocyclingAddGoToButton";
            thermocyclingAddGoToButton.Size = new Size(129, 36);
            thermocyclingAddGoToButton.TabIndex = 16;
            thermocyclingAddGoToButton.Text = "Add GoTo";
            thermocyclingAddGoToButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECDKillButton
            // 
            thermocyclingTECDKillButton.BackColor = Color.IndianRed;
            thermocyclingTECDKillButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECDKillButton.ForeColor = Color.White;
            thermocyclingTECDKillButton.Location = new Point(1021, 313);
            thermocyclingTECDKillButton.Name = "thermocyclingTECDKillButton";
            thermocyclingTECDKillButton.Size = new Size(73, 36);
            thermocyclingTECDKillButton.TabIndex = 15;
            thermocyclingTECDKillButton.Text = "Kill";
            thermocyclingTECDKillButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECCKillButton
            // 
            thermocyclingTECCKillButton.BackColor = Color.IndianRed;
            thermocyclingTECCKillButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECCKillButton.ForeColor = Color.White;
            thermocyclingTECCKillButton.Location = new Point(927, 313);
            thermocyclingTECCKillButton.Name = "thermocyclingTECCKillButton";
            thermocyclingTECCKillButton.Size = new Size(73, 36);
            thermocyclingTECCKillButton.TabIndex = 14;
            thermocyclingTECCKillButton.Text = "Kill";
            thermocyclingTECCKillButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECBKillButton
            // 
            thermocyclingTECBKillButton.BackColor = Color.IndianRed;
            thermocyclingTECBKillButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECBKillButton.ForeColor = Color.White;
            thermocyclingTECBKillButton.Location = new Point(824, 313);
            thermocyclingTECBKillButton.Name = "thermocyclingTECBKillButton";
            thermocyclingTECBKillButton.Size = new Size(73, 36);
            thermocyclingTECBKillButton.TabIndex = 13;
            thermocyclingTECBKillButton.Text = "Kill";
            thermocyclingTECBKillButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECAKillButton
            // 
            thermocyclingTECAKillButton.BackColor = Color.IndianRed;
            thermocyclingTECAKillButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECAKillButton.ForeColor = Color.White;
            thermocyclingTECAKillButton.Location = new Point(720, 313);
            thermocyclingTECAKillButton.Name = "thermocyclingTECAKillButton";
            thermocyclingTECAKillButton.Size = new Size(73, 36);
            thermocyclingTECAKillButton.TabIndex = 12;
            thermocyclingTECAKillButton.Text = "Kill";
            thermocyclingTECAKillButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECDRunButton
            // 
            thermocyclingTECDRunButton.BackColor = SystemColors.Highlight;
            thermocyclingTECDRunButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECDRunButton.ForeColor = Color.White;
            thermocyclingTECDRunButton.Location = new Point(1021, 271);
            thermocyclingTECDRunButton.Name = "thermocyclingTECDRunButton";
            thermocyclingTECDRunButton.Size = new Size(73, 36);
            thermocyclingTECDRunButton.TabIndex = 11;
            thermocyclingTECDRunButton.Text = "Run";
            thermocyclingTECDRunButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECCRunButton
            // 
            thermocyclingTECCRunButton.BackColor = SystemColors.Highlight;
            thermocyclingTECCRunButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECCRunButton.ForeColor = Color.White;
            thermocyclingTECCRunButton.Location = new Point(927, 271);
            thermocyclingTECCRunButton.Name = "thermocyclingTECCRunButton";
            thermocyclingTECCRunButton.Size = new Size(73, 36);
            thermocyclingTECCRunButton.TabIndex = 10;
            thermocyclingTECCRunButton.Text = "Run";
            thermocyclingTECCRunButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECBRunButton
            // 
            thermocyclingTECBRunButton.BackColor = SystemColors.Highlight;
            thermocyclingTECBRunButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECBRunButton.ForeColor = Color.White;
            thermocyclingTECBRunButton.Location = new Point(824, 271);
            thermocyclingTECBRunButton.Name = "thermocyclingTECBRunButton";
            thermocyclingTECBRunButton.Size = new Size(73, 36);
            thermocyclingTECBRunButton.TabIndex = 9;
            thermocyclingTECBRunButton.Text = "Run";
            thermocyclingTECBRunButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingTECARunButton
            // 
            thermocyclingTECARunButton.BackColor = SystemColors.Highlight;
            thermocyclingTECARunButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingTECARunButton.ForeColor = Color.White;
            thermocyclingTECARunButton.Location = new Point(720, 271);
            thermocyclingTECARunButton.Name = "thermocyclingTECARunButton";
            thermocyclingTECARunButton.Size = new Size(73, 36);
            thermocyclingTECARunButton.TabIndex = 8;
            thermocyclingTECARunButton.Text = "Run";
            thermocyclingTECARunButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingProtocolStatusesDataGridView
            // 
            thermocyclingProtocolStatusesDataGridView.AllowUserToAddRows = false;
            thermocyclingProtocolStatusesDataGridView.AllowUserToDeleteRows = false;
            thermocyclingProtocolStatusesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            thermocyclingProtocolStatusesDataGridView.Columns.AddRange(new DataGridViewColumn[] { thermocyclingProtocolStatusesPropertyColumn, thermocyclingProtocolStatusesTECAColumn, thermocyclingProtocolStatusesTECBColumn, thermocyclingProtocolStatusesTECCColumn, thermocyclingProtocolStatusesTECDColumn });
            thermocyclingProtocolStatusesDataGridView.Location = new Point(571, 24);
            thermocyclingProtocolStatusesDataGridView.Name = "thermocyclingProtocolStatusesDataGridView";
            thermocyclingProtocolStatusesDataGridView.ReadOnly = true;
            thermocyclingProtocolStatusesDataGridView.RowHeadersVisible = false;
            thermocyclingProtocolStatusesDataGridView.RowTemplate.Height = 25;
            thermocyclingProtocolStatusesDataGridView.Size = new Size(537, 241);
            thermocyclingProtocolStatusesDataGridView.TabIndex = 7;
            // 
            // thermocyclingProtocolStatusesPropertyColumn
            // 
            thermocyclingProtocolStatusesPropertyColumn.HeaderText = "";
            thermocyclingProtocolStatusesPropertyColumn.Name = "thermocyclingProtocolStatusesPropertyColumn";
            thermocyclingProtocolStatusesPropertyColumn.ReadOnly = true;
            thermocyclingProtocolStatusesPropertyColumn.Width = 130;
            // 
            // thermocyclingProtocolStatusesTECAColumn
            // 
            thermocyclingProtocolStatusesTECAColumn.HeaderText = "TEC A";
            thermocyclingProtocolStatusesTECAColumn.Name = "thermocyclingProtocolStatusesTECAColumn";
            thermocyclingProtocolStatusesTECAColumn.ReadOnly = true;
            // 
            // thermocyclingProtocolStatusesTECBColumn
            // 
            thermocyclingProtocolStatusesTECBColumn.HeaderText = "TEC B";
            thermocyclingProtocolStatusesTECBColumn.Name = "thermocyclingProtocolStatusesTECBColumn";
            thermocyclingProtocolStatusesTECBColumn.ReadOnly = true;
            // 
            // thermocyclingProtocolStatusesTECCColumn
            // 
            thermocyclingProtocolStatusesTECCColumn.HeaderText = "TEC C";
            thermocyclingProtocolStatusesTECCColumn.Name = "thermocyclingProtocolStatusesTECCColumn";
            thermocyclingProtocolStatusesTECCColumn.ReadOnly = true;
            // 
            // thermocyclingProtocolStatusesTECDColumn
            // 
            thermocyclingProtocolStatusesTECDColumn.HeaderText = "TEC D";
            thermocyclingProtocolStatusesTECDColumn.Name = "thermocyclingProtocolStatusesTECDColumn";
            thermocyclingProtocolStatusesTECDColumn.ReadOnly = true;
            // 
            // thermocyclingProtocolStatusesLabel
            // 
            thermocyclingProtocolStatusesLabel.AutoSize = true;
            thermocyclingProtocolStatusesLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingProtocolStatusesLabel.Location = new Point(571, 3);
            thermocyclingProtocolStatusesLabel.Name = "thermocyclingProtocolStatusesLabel";
            thermocyclingProtocolStatusesLabel.Size = new Size(131, 18);
            thermocyclingProtocolStatusesLabel.TabIndex = 6;
            thermocyclingProtocolStatusesLabel.Text = "Protocol Statuses";
            // 
            // thermocyclingLoadProtocolButton
            // 
            thermocyclingLoadProtocolButton.BackColor = SystemColors.Highlight;
            thermocyclingLoadProtocolButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingLoadProtocolButton.ForeColor = Color.White;
            thermocyclingLoadProtocolButton.Location = new Point(143, 495);
            thermocyclingLoadProtocolButton.Name = "thermocyclingLoadProtocolButton";
            thermocyclingLoadProtocolButton.Size = new Size(129, 36);
            thermocyclingLoadProtocolButton.TabIndex = 5;
            thermocyclingLoadProtocolButton.Text = "Load Protocol";
            thermocyclingLoadProtocolButton.UseVisualStyleBackColor = false;
            thermocyclingLoadProtocolButton.Click += thermocyclingLoadProtocolButton_Click;
            // 
            // thermocyclingRemoveStepButton
            // 
            thermocyclingRemoveStepButton.BackColor = Color.IndianRed;
            thermocyclingRemoveStepButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingRemoveStepButton.ForeColor = Color.White;
            thermocyclingRemoveStepButton.Location = new Point(143, 537);
            thermocyclingRemoveStepButton.Name = "thermocyclingRemoveStepButton";
            thermocyclingRemoveStepButton.Size = new Size(129, 36);
            thermocyclingRemoveStepButton.TabIndex = 4;
            thermocyclingRemoveStepButton.Text = "Remove Step";
            thermocyclingRemoveStepButton.UseVisualStyleBackColor = false;
            // 
            // thermocyclingEditStepButton
            // 
            thermocyclingEditStepButton.BackColor = SystemColors.Highlight;
            thermocyclingEditStepButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingEditStepButton.ForeColor = Color.White;
            thermocyclingEditStepButton.Location = new Point(8, 537);
            thermocyclingEditStepButton.Name = "thermocyclingEditStepButton";
            thermocyclingEditStepButton.Size = new Size(129, 36);
            thermocyclingEditStepButton.TabIndex = 3;
            thermocyclingEditStepButton.Text = "Edit Step";
            thermocyclingEditStepButton.UseVisualStyleBackColor = false;
            thermocyclingEditStepButton.Click += thermocyclingEditStepButton_Click;
            // 
            // thermocyclingSaveProtocolButton
            // 
            thermocyclingSaveProtocolButton.BackColor = SystemColors.Highlight;
            thermocyclingSaveProtocolButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingSaveProtocolButton.ForeColor = Color.White;
            thermocyclingSaveProtocolButton.Location = new Point(143, 453);
            thermocyclingSaveProtocolButton.Name = "thermocyclingSaveProtocolButton";
            thermocyclingSaveProtocolButton.Size = new Size(129, 36);
            thermocyclingSaveProtocolButton.TabIndex = 2;
            thermocyclingSaveProtocolButton.Text = "Save Protocol";
            thermocyclingSaveProtocolButton.UseVisualStyleBackColor = false;
            thermocyclingSaveProtocolButton.Click += thermocyclingSaveProtocolButton_Click;
            // 
            // thermocyclingAddStepButton
            // 
            thermocyclingAddStepButton.BackColor = Color.MediumSeaGreen;
            thermocyclingAddStepButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            thermocyclingAddStepButton.ForeColor = Color.White;
            thermocyclingAddStepButton.Location = new Point(8, 453);
            thermocyclingAddStepButton.Name = "thermocyclingAddStepButton";
            thermocyclingAddStepButton.Size = new Size(129, 36);
            thermocyclingAddStepButton.TabIndex = 1;
            thermocyclingAddStepButton.Text = "Add Step";
            thermocyclingAddStepButton.UseVisualStyleBackColor = false;
            thermocyclingAddStepButton.Click += thermocyclingAddStepButton_Click;
            // 
            // thermocyclingPlotView
            // 
            thermocyclingPlotView.Location = new Point(8, 3);
            thermocyclingPlotView.Name = "thermocyclingPlotView";
            thermocyclingPlotView.PanCursor = Cursors.Hand;
            thermocyclingPlotView.Size = new Size(557, 444);
            thermocyclingPlotView.TabIndex = 0;
            thermocyclingPlotView.Text = "Thermocycling Plot View";
            thermocyclingPlotView.ZoomHorizontalCursor = Cursors.SizeWE;
            thermocyclingPlotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            thermocyclingPlotView.ZoomVerticalCursor = Cursors.SizeNS;
            thermocyclingPlotView.MouseDown += thermocyclingPlotView_MouseDown;
            // 
            // imagingTabPage
            // 
            imagingTabPage.Controls.Add(imagingLEDsDataGridView);
            imagingTabPage.Controls.Add(imagingLEDsLabel);
            imagingTabPage.Controls.Add(imagingMetaDataButton);
            imagingTabPage.Controls.Add(imagingMetaDataLabel);
            imagingTabPage.Controls.Add(imagingKillButon);
            imagingTabPage.Controls.Add(imagingScanButton);
            imagingTabPage.Controls.Add(imagingScanParametersDataGridView);
            imagingTabPage.Controls.Add(imagingScanParametersLabel);
            imagingTabPage.Controls.Add(imagingDzTextBox);
            imagingTabPage.Controls.Add(imagingDyTextBox);
            imagingTabPage.Controls.Add(imagingDxTextBox);
            imagingTabPage.Controls.Add(imagingDzLabel);
            imagingTabPage.Controls.Add(imagingDyLabel);
            imagingTabPage.Controls.Add(imagingDxLabel);
            imagingTabPage.Controls.Add(imagingCaptureImageButton);
            imagingTabPage.Controls.Add(imagingCameraViewLabel);
            imagingTabPage.Controls.Add(imagingStreamButton);
            imagingTabPage.Controls.Add(imagingPictureBox);
            imagingTabPage.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingTabPage.Location = new Point(4, 24);
            imagingTabPage.Name = "imagingTabPage";
            imagingTabPage.Size = new Size(1116, 584);
            imagingTabPage.TabIndex = 4;
            imagingTabPage.Text = "Imaging";
            imagingTabPage.UseVisualStyleBackColor = true;
            // 
            // imagingLEDsLabel
            // 
            imagingLEDsLabel.AutoSize = true;
            imagingLEDsLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingLEDsLabel.Location = new Point(264, 398);
            imagingLEDsLabel.Name = "imagingLEDsLabel";
            imagingLEDsLabel.Size = new Size(48, 18);
            imagingLEDsLabel.TabIndex = 16;
            imagingLEDsLabel.Text = "LEDs";
            // 
            // imagingMetaDataButton
            // 
            imagingMetaDataButton.BackColor = SystemColors.Highlight;
            imagingMetaDataButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingMetaDataButton.ForeColor = Color.White;
            imagingMetaDataButton.Location = new Point(692, 318);
            imagingMetaDataButton.Name = "imagingMetaDataButton";
            imagingMetaDataButton.Size = new Size(109, 36);
            imagingMetaDataButton.TabIndex = 15;
            imagingMetaDataButton.Text = "Form";
            imagingMetaDataButton.UseVisualStyleBackColor = false;
            // 
            // imagingMetaDataLabel
            // 
            imagingMetaDataLabel.AutoSize = true;
            imagingMetaDataLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingMetaDataLabel.Location = new Point(692, 297);
            imagingMetaDataLabel.Name = "imagingMetaDataLabel";
            imagingMetaDataLabel.Size = new Size(81, 18);
            imagingMetaDataLabel.TabIndex = 14;
            imagingMetaDataLabel.Text = "Meta Data";
            // 
            // imagingKillButon
            // 
            imagingKillButon.BackColor = Color.IndianRed;
            imagingKillButon.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingKillButon.ForeColor = Color.White;
            imagingKillButon.Location = new Point(1000, 339);
            imagingKillButon.Name = "imagingKillButon";
            imagingKillButon.Size = new Size(109, 36);
            imagingKillButon.TabIndex = 13;
            imagingKillButon.Text = "Kill";
            imagingKillButon.UseVisualStyleBackColor = false;
            // 
            // imagingScanButton
            // 
            imagingScanButton.BackColor = SystemColors.Highlight;
            imagingScanButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingScanButton.ForeColor = Color.White;
            imagingScanButton.Location = new Point(1000, 297);
            imagingScanButton.Name = "imagingScanButton";
            imagingScanButton.Size = new Size(109, 36);
            imagingScanButton.TabIndex = 12;
            imagingScanButton.Text = "Scan";
            imagingScanButton.UseVisualStyleBackColor = false;
            // 
            // imagingScanParametersDataGridView
            // 
            imagingScanParametersDataGridView.AllowUserToAddRows = false;
            imagingScanParametersDataGridView.AllowUserToDeleteRows = false;
            imagingScanParametersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            imagingScanParametersDataGridView.Columns.AddRange(new DataGridViewColumn[] { imagingScanParametersPropertyColumn, imagingScanParametersValueColumn });
            imagingScanParametersDataGridView.Location = new Point(692, 28);
            imagingScanParametersDataGridView.Name = "imagingScanParametersDataGridView";
            imagingScanParametersDataGridView.RowHeadersVisible = false;
            imagingScanParametersDataGridView.RowTemplate.Height = 25;
            imagingScanParametersDataGridView.Size = new Size(417, 263);
            imagingScanParametersDataGridView.TabIndex = 11;
            // 
            // imagingScanParametersPropertyColumn
            // 
            imagingScanParametersPropertyColumn.HeaderText = "";
            imagingScanParametersPropertyColumn.Name = "imagingScanParametersPropertyColumn";
            imagingScanParametersPropertyColumn.ReadOnly = true;
            imagingScanParametersPropertyColumn.Width = 190;
            // 
            // imagingScanParametersValueColumn
            // 
            imagingScanParametersValueColumn.HeaderText = "Value";
            imagingScanParametersValueColumn.Name = "imagingScanParametersValueColumn";
            imagingScanParametersValueColumn.Width = 200;
            // 
            // imagingScanParametersLabel
            // 
            imagingScanParametersLabel.AutoSize = true;
            imagingScanParametersLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingScanParametersLabel.Location = new Point(693, 7);
            imagingScanParametersLabel.Name = "imagingScanParametersLabel";
            imagingScanParametersLabel.Size = new Size(130, 18);
            imagingScanParametersLabel.TabIndex = 10;
            imagingScanParametersLabel.Text = "Scan Parameters";
            // 
            // imagingDzTextBox
            // 
            imagingDzTextBox.Location = new Point(92, 515);
            imagingDzTextBox.Name = "imagingDzTextBox";
            imagingDzTextBox.Size = new Size(135, 26);
            imagingDzTextBox.TabIndex = 9;
            // 
            // imagingDyTextBox
            // 
            imagingDyTextBox.Location = new Point(92, 479);
            imagingDyTextBox.Name = "imagingDyTextBox";
            imagingDyTextBox.Size = new Size(135, 26);
            imagingDyTextBox.TabIndex = 8;
            // 
            // imagingDxTextBox
            // 
            imagingDxTextBox.Location = new Point(92, 446);
            imagingDxTextBox.Name = "imagingDxTextBox";
            imagingDxTextBox.Size = new Size(135, 26);
            imagingDxTextBox.TabIndex = 7;
            // 
            // imagingDzLabel
            // 
            imagingDzLabel.AutoSize = true;
            imagingDzLabel.Location = new Point(62, 518);
            imagingDzLabel.Name = "imagingDzLabel";
            imagingDzLabel.Size = new Size(24, 18);
            imagingDzLabel.TabIndex = 6;
            imagingDzLabel.Text = "dz";
            // 
            // imagingDyLabel
            // 
            imagingDyLabel.AutoSize = true;
            imagingDyLabel.Location = new Point(62, 482);
            imagingDyLabel.Name = "imagingDyLabel";
            imagingDyLabel.Size = new Size(24, 18);
            imagingDyLabel.TabIndex = 5;
            imagingDyLabel.Text = "dy";
            // 
            // imagingDxLabel
            // 
            imagingDxLabel.AutoSize = true;
            imagingDxLabel.Location = new Point(62, 449);
            imagingDxLabel.Name = "imagingDxLabel";
            imagingDxLabel.Size = new Size(24, 18);
            imagingDxLabel.TabIndex = 4;
            imagingDxLabel.Text = "dx";
            // 
            // imagingCaptureImageButton
            // 
            imagingCaptureImageButton.BackColor = SystemColors.Highlight;
            imagingCaptureImageButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingCaptureImageButton.ForeColor = Color.White;
            imagingCaptureImageButton.Location = new Point(123, 398);
            imagingCaptureImageButton.Name = "imagingCaptureImageButton";
            imagingCaptureImageButton.Size = new Size(125, 36);
            imagingCaptureImageButton.TabIndex = 3;
            imagingCaptureImageButton.Text = "Capture Image";
            imagingCaptureImageButton.UseVisualStyleBackColor = false;
            // 
            // imagingCameraViewLabel
            // 
            imagingCameraViewLabel.AutoSize = true;
            imagingCameraViewLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingCameraViewLabel.Location = new Point(8, 7);
            imagingCameraViewLabel.Name = "imagingCameraViewLabel";
            imagingCameraViewLabel.Size = new Size(104, 18);
            imagingCameraViewLabel.TabIndex = 2;
            imagingCameraViewLabel.Text = "Camera View";
            // 
            // imagingStreamButton
            // 
            imagingStreamButton.BackColor = SystemColors.Highlight;
            imagingStreamButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            imagingStreamButton.ForeColor = Color.White;
            imagingStreamButton.Location = new Point(8, 398);
            imagingStreamButton.Name = "imagingStreamButton";
            imagingStreamButton.Size = new Size(109, 36);
            imagingStreamButton.TabIndex = 1;
            imagingStreamButton.Text = "Stream";
            imagingStreamButton.UseVisualStyleBackColor = false;
            imagingStreamButton.Click += imagingStreamButton_Click;
            // 
            // imagingPictureBox
            // 
            imagingPictureBox.Location = new Point(8, 28);
            imagingPictureBox.Name = "imagingPictureBox";
            imagingPictureBox.Size = new Size(671, 365);
            imagingPictureBox.TabIndex = 0;
            imagingPictureBox.TabStop = false;
            // 
            // settingsTabPage
            // 
            settingsTabPage.Location = new Point(4, 24);
            settingsTabPage.Name = "settingsTabPage";
            settingsTabPage.Size = new Size(1116, 584);
            settingsTabPage.TabIndex = 5;
            settingsTabPage.Text = "Settings";
            settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // metrologyTabPage
            // 
            metrologyTabPage.Location = new Point(4, 24);
            metrologyTabPage.Name = "metrologyTabPage";
            metrologyTabPage.Size = new Size(1116, 584);
            metrologyTabPage.TabIndex = 6;
            metrologyTabPage.Text = "Metrology";
            metrologyTabPage.UseVisualStyleBackColor = true;
            // 
            // softwareVersionLabel
            // 
            softwareVersionLabel.AutoSize = true;
            softwareVersionLabel.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            softwareVersionLabel.ForeColor = Color.White;
            softwareVersionLabel.Location = new Point(12, 623);
            softwareVersionLabel.Name = "softwareVersionLabel";
            softwareVersionLabel.Size = new Size(117, 16);
            softwareVersionLabel.TabIndex = 1;
            softwareVersionLabel.Text = "SW Version: 1.0.1";
            // 
            // firmwareVersionLabel
            // 
            firmwareVersionLabel.AutoSize = true;
            firmwareVersionLabel.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            firmwareVersionLabel.ForeColor = Color.White;
            firmwareVersionLabel.Location = new Point(147, 623);
            firmwareVersionLabel.Name = "firmwareVersionLabel";
            firmwareVersionLabel.Size = new Size(156, 16);
            firmwareVersionLabel.TabIndex = 2;
            firmwareVersionLabel.Text = "Firmware Version: 1.2.1";
            // 
            // apiVersionLabel
            // 
            apiVersionLabel.AutoSize = true;
            apiVersionLabel.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            apiVersionLabel.ForeColor = Color.White;
            apiVersionLabel.Location = new Point(318, 623);
            apiVersionLabel.Name = "apiVersionLabel";
            apiVersionLabel.Size = new Size(117, 16);
            apiVersionLabel.TabIndex = 3;
            apiVersionLabel.Text = "API Version: 1.0.1";
            // 
            // guiVersionLabel
            // 
            guiVersionLabel.AutoSize = true;
            guiVersionLabel.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            guiVersionLabel.ForeColor = Color.White;
            guiVersionLabel.Location = new Point(451, 623);
            guiVersionLabel.Name = "guiVersionLabel";
            guiVersionLabel.Size = new Size(118, 16);
            guiVersionLabel.TabIndex = 4;
            guiVersionLabel.Text = "GUI Version: 1.0.0";
            // 
            // userLabel
            // 
            userLabel.AutoSize = true;
            userLabel.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            userLabel.ForeColor = Color.White;
            userLabel.Location = new Point(585, 623);
            userLabel.Name = "userLabel";
            userLabel.Size = new Size(107, 16);
            userLabel.TabIndex = 5;
            userLabel.Text = "User: glc-biorad";
            // 
            // logoutButton
            // 
            logoutButton.BackColor = SystemColors.Highlight;
            logoutButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            logoutButton.ForeColor = Color.White;
            logoutButton.Location = new Point(896, 616);
            logoutButton.Name = "logoutButton";
            logoutButton.Size = new Size(98, 30);
            logoutButton.TabIndex = 6;
            logoutButton.Text = "Logout";
            logoutButton.UseVisualStyleBackColor = false;
            // 
            // resetButton
            // 
            resetButton.BackColor = Color.IndianRed;
            resetButton.FlatAppearance.BorderColor = Color.Red;
            resetButton.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            resetButton.ForeColor = Color.White;
            resetButton.Location = new Point(1000, 616);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(98, 30);
            resetButton.TabIndex = 7;
            resetButton.Text = "Reset";
            resetButton.UseVisualStyleBackColor = false;
            // 
            // imagingLEDsDataGridView
            // 
            imagingLEDsDataGridView.AllowUserToAddRows = false;
            imagingLEDsDataGridView.AllowUserToDeleteRows = false;
            imagingLEDsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            imagingLEDsDataGridView.Columns.AddRange(new DataGridViewColumn[] { imagingLEDsPropertyColumn, imagingLEDsCy5Column, imagingLEDsFAMColumn, imagingLEDsHEXColumn, imagingLEDsAttoColumn, imagingLEDsAlexaColumn, imagingLEDsCy5p5Column });
            imagingLEDsDataGridView.Location = new Point(264, 419);
            imagingLEDsDataGridView.Name = "imagingLEDsDataGridView";
            imagingLEDsDataGridView.RowHeadersVisible = false;
            imagingLEDsDataGridView.RowTemplate.Height = 25;
            imagingLEDsDataGridView.Size = new Size(844, 150);
            imagingLEDsDataGridView.TabIndex = 17;
            // 
            // imagingLEDsPropertyColumn
            // 
            imagingLEDsPropertyColumn.HeaderText = "";
            imagingLEDsPropertyColumn.Name = "imagingLEDsPropertyColumn";
            imagingLEDsPropertyColumn.ReadOnly = true;
            imagingLEDsPropertyColumn.Width = 180;
            // 
            // imagingLEDsCy5Column
            // 
            imagingLEDsCy5Column.HeaderText = "Cy5";
            imagingLEDsCy5Column.Name = "imagingLEDsCy5Column";
            imagingLEDsCy5Column.Width = 110;
            // 
            // imagingLEDsFAMColumn
            // 
            imagingLEDsFAMColumn.HeaderText = "FAM";
            imagingLEDsFAMColumn.Name = "imagingLEDsFAMColumn";
            imagingLEDsFAMColumn.Width = 110;
            // 
            // imagingLEDsHEXColumn
            // 
            imagingLEDsHEXColumn.HeaderText = "HEX";
            imagingLEDsHEXColumn.Name = "imagingLEDsHEXColumn";
            imagingLEDsHEXColumn.Width = 110;
            // 
            // imagingLEDsAttoColumn
            // 
            imagingLEDsAttoColumn.HeaderText = "Atto";
            imagingLEDsAttoColumn.Name = "imagingLEDsAttoColumn";
            imagingLEDsAttoColumn.Width = 110;
            // 
            // imagingLEDsAlexaColumn
            // 
            imagingLEDsAlexaColumn.HeaderText = "Alexa";
            imagingLEDsAlexaColumn.Name = "imagingLEDsAlexaColumn";
            imagingLEDsAlexaColumn.Width = 110;
            // 
            // imagingLEDsCy5p5Column
            // 
            imagingLEDsCy5p5Column.HeaderText = "Cy5.5";
            imagingLEDsCy5p5Column.Name = "imagingLEDsCy5p5Column";
            imagingLEDsCy5p5Column.Width = 110;
            // 
            // independentReaderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1124, 650);
            Controls.Add(resetButton);
            Controls.Add(logoutButton);
            Controls.Add(userLabel);
            Controls.Add(guiVersionLabel);
            Controls.Add(apiVersionLabel);
            Controls.Add(firmwareVersionLabel);
            Controls.Add(softwareVersionLabel);
            Controls.Add(tabControl);
            Name = "independentReaderForm";
            Text = "Independent Reader";
            tabControl.ResumeLayout(false);
            homeTabPage.ResumeLayout(false);
            homeTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)homeTECsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)homeLEDsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)homeCameraDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)homeMotorsDataGridView).EndInit();
            runTabPage.ResumeLayout(false);
            runTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)runAssayMetaDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)runSampleMetaDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)runImagingSetupDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)runExperimentDataGridView).EndInit();
            controlTabPage.ResumeLayout(false);
            controlTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)controlLEDsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)controlTECsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)controlMotorsDataGridView).EndInit();
            thermocyclingTabPage.ResumeLayout(false);
            thermocyclingTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)thermocyclingProtocolStatusesDataGridView).EndInit();
            imagingTabPage.ResumeLayout(false);
            imagingTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imagingScanParametersDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)imagingPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)imagingLEDsDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl;
        private TabPage homeTabPage;
        private TabPage runTabPage;
        private TabPage controlTabPage;
        private TabPage thermocyclingTabPage;
        private TabPage imagingTabPage;
        private TabPage settingsTabPage;
        private TabPage metrologyTabPage;
        private DataGridView homeMotorsDataGridView;
        private Label homeMotorsLabel;
        private DataGridView homeCameraDataGridView;
        private Label homeCameraLabel;
        private DataGridView homeLEDsDataGridView;
        private Label homeLEDsLabel;
        private DataGridView homeTECsDataGridView;
        private Label homeTECsLabel;
        private Label softwareVersionLabel;
        private Label firmwareVersionLabel;
        private Label apiVersionLabel;
        private Label guiVersionLabel;
        private Label userLabel;
        private Button logoutButton;
        private Button resetButton;
        private DataGridViewTextBoxColumn homeCameraStateColumn;
        private DataGridViewTextBoxColumn homeCameraExposureColumn;
        private DataGridViewTextBoxColumn homeCameraTempColumn;
        private DataGridView runAssayMetaDataGridView;
        private Button runRemoveSampleButton;
        private Button runAddSampleButton;
        private DataGridView runSampleMetaDataGridView;
        private DataGridView runImagingSetupDataGridView;
        private DataGridView runExperimentDataGridView;
        private Button runRunButton;
        private Label runAssayMetaDataLabel;
        private Label runSampleMetaDataLabel;
        private Label runImagingSetupLabel;
        private Label tunExperimentDataLabel;
        private Button runRemoveAssayButton;
        private Button runAddAssayButton;
        private DataGridViewTextBoxColumn runAssayMetaDataIDColumn;
        private DataGridViewTextBoxColumn runAssayMetaDataNameColumn;
        private DataGridViewTextBoxColumn runSampleMetaDataIDColumn;
        private DataGridViewTextBoxColumn runSampleMetaDataNameColumn;
        private DataGridViewTextBoxColumn runExperimentDataPropertyColumn;
        private DataGridViewTextBoxColumn runExperimentDataValueColumn;
        private Button runClearFormButton;
        private DataGridViewTextBoxColumn runImagingSetupPropertyColumn;
        private DataGridViewTextBoxColumn runImagingSetupValueColumn;
        private DataGridViewTextBoxColumn homeLEDsPropertiesColumn;
        private DataGridViewTextBoxColumn homeLEDsCy5Column;
        private DataGridViewTextBoxColumn homeLEDsFAMColumn;
        private DataGridViewTextBoxColumn homeLEDsHEXColumn;
        private DataGridViewTextBoxColumn homeLEDsAttoColumn;
        private DataGridViewTextBoxColumn homeLEDsAlexaColumn;
        private DataGridViewTextBoxColumn homeLEDsCy55Column;
        private Button controlMoveButton;
        private ComboBox controlMotorsComboBox;
        private DataGridView controlMotorsDataGridView;
        private Label controlMotorsLabel;
        private Label controlTECsLabel;
        private Button controlHomeButton;
        private DataGridViewTextBoxColumn homeMotorsMotorColumn;
        private DataGridViewTextBoxColumn homeMotorsIOColumn;
        private DataGridViewTextBoxColumn homeMotorsStateColumn;
        private DataGridViewTextBoxColumn homeMotorsStepsColumn;
        private DataGridViewTextBoxColumn homeMotorsSpeedColumn;
        private DataGridViewTextBoxColumn homeMotorsHomeColumn;
        private DataGridView controlLEDsDataGridView;
        private DataGridViewTextBoxColumn controlLEDsPropertyColumn;
        private DataGridViewTextBoxColumn controlLEDsCy5Column;
        private DataGridViewTextBoxColumn controlLEDsFAMColumn;
        private DataGridViewTextBoxColumn controlLEDsHEXColumn;
        private DataGridViewTextBoxColumn controlLEDsAttoColumn;
        private DataGridViewTextBoxColumn controlLEDsAlexaColumn;
        private DataGridViewTextBoxColumn controlLEDsCy5p5Column;
        private Label controlLEDsLabel;
        private Button controlTECDSetTempButton;
        private Button controlTECCSetTempButton;
        private Button controlTECBSetTempButton;
        private Button controlTECASetTempButton;
        private DataGridView controlTECsDataGridView;
        private DataGridViewTextBoxColumn homeTECsProprtyColumn;
        private DataGridViewTextBoxColumn homeTECsTECAColumn;
        private DataGridViewTextBoxColumn homeTECsTECBColumn;
        private DataGridViewTextBoxColumn homeTECsTECCColumn;
        private DataGridViewTextBoxColumn homeTECsTECDColumn;
        private DataGridViewTextBoxColumn controlTECsPropertyColumn;
        private DataGridViewTextBoxColumn controlTECsTECAColumn;
        private DataGridViewTextBoxColumn controlTECsTECBColumn;
        private DataGridViewTextBoxColumn controlTECsTECCColumn;
        private DataGridViewTextBoxColumn controlTECsTECDColumn;
        private DataGridViewTextBoxColumn controlMotorsMotorColumn;
        private DataGridViewTextBoxColumn controlMotorsIOColumn;
        private DataGridViewTextBoxColumn controlMotorsStateColumn;
        private DataGridViewTextBoxColumn controlMotorsPositionColumn;
        private DataGridViewTextBoxColumn controlMotorsSpeedColumn;
        private DataGridViewTextBoxColumn controlMotorsHomeColumn;
        private OxyPlot.WindowsForms.PlotView thermocyclingPlotView;
        private Button thermocyclingLoadProtocolButton;
        private Button thermocyclingRemoveStepButton;
        private Button thermocyclingEditStepButton;
        private Button thermocyclingSaveProtocolButton;
        private Button thermocyclingAddStepButton;
        private DataGridView thermocyclingProtocolStatusesDataGridView;
        private Label thermocyclingProtocolStatusesLabel;
        private DataGridViewTextBoxColumn thermocyclingProtocolStatusesPropertyColumn;
        private DataGridViewTextBoxColumn thermocyclingProtocolStatusesTECAColumn;
        private DataGridViewTextBoxColumn thermocyclingProtocolStatusesTECBColumn;
        private DataGridViewTextBoxColumn thermocyclingProtocolStatusesTECCColumn;
        private DataGridViewTextBoxColumn thermocyclingProtocolStatusesTECDColumn;
        private Button thermocyclingTECDKillButton;
        private Button thermocyclingTECCKillButton;
        private Button thermocyclingTECBKillButton;
        private Button thermocyclingTECAKillButton;
        private Button thermocyclingTECDRunButton;
        private Button thermocyclingTECCRunButton;
        private Button thermocyclingTECBRunButton;
        private Button thermocyclingTECARunButton;
        private Button thermocyclingAddGoToButton;
        private PictureBox imagingPictureBox;
        private Button imagingStreamButton;
        private Button imagingCaptureImageButton;
        private Label imagingCameraViewLabel;
        private Label imagingDxLabel;
        private Label imagingDzLabel;
        private Label imagingDyLabel;
        private DataGridView imagingScanParametersDataGridView;
        private Label imagingScanParametersLabel;
        private TextBox imagingDzTextBox;
        private TextBox imagingDyTextBox;
        private TextBox imagingDxTextBox;
        private DataGridViewTextBoxColumn imagingScanParametersPropertyColumn;
        private DataGridViewTextBoxColumn imagingScanParametersValueColumn;
        private Button imagingKillButon;
        private Button imagingScanButton;
        private Button imagingMetaDataButton;
        private Label imagingMetaDataLabel;
        private Label imagingLEDsLabel;
        private DataGridView imagingLEDsDataGridView;
        private DataGridViewTextBoxColumn imagingLEDsPropertyColumn;
        private DataGridViewTextBoxColumn imagingLEDsCy5Column;
        private DataGridViewTextBoxColumn imagingLEDsFAMColumn;
        private DataGridViewTextBoxColumn imagingLEDsHEXColumn;
        private DataGridViewTextBoxColumn imagingLEDsAttoColumn;
        private DataGridViewTextBoxColumn imagingLEDsAlexaColumn;
        private DataGridViewTextBoxColumn imagingLEDsCy5p5Column;
    }
}