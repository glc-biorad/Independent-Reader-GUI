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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.homeTabPage = new System.Windows.Forms.TabPage();
            this.homeTECsDataGridView = new System.Windows.Forms.DataGridView();
            this.homeTECsProprtyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeTECsTECAColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeTECsTECBColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeTECsTECCColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeTECsTECDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeTECsLabel = new System.Windows.Forms.Label();
            this.homeLEDsDataGridView = new System.Windows.Forms.DataGridView();
            this.homeLEDsPropertiesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeLEDsCy5Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeLEDsFAMColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeLEDsHEXColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeLEDsAttoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeLEDsAlexaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeLEDsCy55Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeLEDsLabel = new System.Windows.Forms.Label();
            this.homeCameraDataGridView = new System.Windows.Forms.DataGridView();
            this.homeCameraLabel = new System.Windows.Forms.Label();
            this.homeMotorsDataGridView = new System.Windows.Forms.DataGridView();
            this.homeMotorsMotorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeMotorsIOColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeMotorsStateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeMotorsStepsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeMotorsSpeedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeMotorsHomeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeMotorsLabel = new System.Windows.Forms.Label();
            this.runTabPage = new System.Windows.Forms.TabPage();
            this.controlTabPage = new System.Windows.Forms.TabPage();
            this.thermocyclingTabPage = new System.Windows.Forms.TabPage();
            this.imagingTabPage = new System.Windows.Forms.TabPage();
            this.settingsTabPage = new System.Windows.Forms.TabPage();
            this.metrologyTabPage = new System.Windows.Forms.TabPage();
            this.softwareVersionLabel = new System.Windows.Forms.Label();
            this.firmwareVersionLabel = new System.Windows.Forms.Label();
            this.apiVersionLabel = new System.Windows.Forms.Label();
            this.guiVersionLabel = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.logoutButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.homeCameraStateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeCameraExposureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeCameraTempColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl.SuspendLayout();
            this.homeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.homeTECsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeLEDsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeCameraDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeMotorsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.homeTabPage);
            this.tabControl.Controls.Add(this.runTabPage);
            this.tabControl.Controls.Add(this.controlTabPage);
            this.tabControl.Controls.Add(this.thermocyclingTabPage);
            this.tabControl.Controls.Add(this.imagingTabPage);
            this.tabControl.Controls.Add(this.settingsTabPage);
            this.tabControl.Controls.Add(this.metrologyTabPage);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1124, 612);
            this.tabControl.TabIndex = 0;
            // 
            // homeTabPage
            // 
            this.homeTabPage.Controls.Add(this.homeTECsDataGridView);
            this.homeTabPage.Controls.Add(this.homeTECsLabel);
            this.homeTabPage.Controls.Add(this.homeLEDsDataGridView);
            this.homeTabPage.Controls.Add(this.homeLEDsLabel);
            this.homeTabPage.Controls.Add(this.homeCameraDataGridView);
            this.homeTabPage.Controls.Add(this.homeCameraLabel);
            this.homeTabPage.Controls.Add(this.homeMotorsDataGridView);
            this.homeTabPage.Controls.Add(this.homeMotorsLabel);
            this.homeTabPage.Location = new System.Drawing.Point(4, 24);
            this.homeTabPage.Name = "homeTabPage";
            this.homeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.homeTabPage.Size = new System.Drawing.Size(1116, 584);
            this.homeTabPage.TabIndex = 0;
            this.homeTabPage.Text = "Home";
            this.homeTabPage.UseVisualStyleBackColor = true;
            // 
            // homeTECsDataGridView
            // 
            this.homeTECsDataGridView.AllowUserToAddRows = false;
            this.homeTECsDataGridView.AllowUserToDeleteRows = false;
            this.homeTECsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.homeTECsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.homeTECsProprtyColumn,
            this.homeTECsTECAColumn,
            this.homeTECsTECBColumn,
            this.homeTECsTECCColumn,
            this.homeTECsTECDColumn});
            this.homeTECsDataGridView.Location = new System.Drawing.Point(550, 24);
            this.homeTECsDataGridView.Name = "homeTECsDataGridView";
            this.homeTECsDataGridView.ReadOnly = true;
            this.homeTECsDataGridView.RowHeadersVisible = false;
            this.homeTECsDataGridView.RowTemplate.Height = 25;
            this.homeTECsDataGridView.Size = new System.Drawing.Size(544, 402);
            this.homeTECsDataGridView.TabIndex = 7;
            // 
            // homeTECsProprtyColumn
            // 
            this.homeTECsProprtyColumn.HeaderText = "";
            this.homeTECsProprtyColumn.Name = "homeTECsProprtyColumn";
            this.homeTECsProprtyColumn.ReadOnly = true;
            this.homeTECsProprtyColumn.Width = 140;
            // 
            // homeTECsTECAColumn
            // 
            this.homeTECsTECAColumn.HeaderText = "TEC A";
            this.homeTECsTECAColumn.Name = "homeTECsTECAColumn";
            this.homeTECsTECAColumn.ReadOnly = true;
            // 
            // homeTECsTECBColumn
            // 
            this.homeTECsTECBColumn.HeaderText = "TEC B";
            this.homeTECsTECBColumn.Name = "homeTECsTECBColumn";
            this.homeTECsTECBColumn.ReadOnly = true;
            // 
            // homeTECsTECCColumn
            // 
            this.homeTECsTECCColumn.HeaderText = "TEC C";
            this.homeTECsTECCColumn.Name = "homeTECsTECCColumn";
            this.homeTECsTECCColumn.ReadOnly = true;
            // 
            // homeTECsTECDColumn
            // 
            this.homeTECsTECDColumn.HeaderText = "TEC D";
            this.homeTECsTECDColumn.Name = "homeTECsTECDColumn";
            this.homeTECsTECDColumn.ReadOnly = true;
            // 
            // homeTECsLabel
            // 
            this.homeTECsLabel.AutoSize = true;
            this.homeTECsLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.homeTECsLabel.Location = new System.Drawing.Point(550, 3);
            this.homeTECsLabel.Name = "homeTECsLabel";
            this.homeTECsLabel.Size = new System.Drawing.Size(48, 18);
            this.homeTECsLabel.TabIndex = 6;
            this.homeTECsLabel.Text = "TECs";
            // 
            // homeLEDsDataGridView
            // 
            this.homeLEDsDataGridView.AllowUserToAddRows = false;
            this.homeLEDsDataGridView.AllowUserToDeleteRows = false;
            this.homeLEDsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.homeLEDsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.homeLEDsPropertiesColumn,
            this.homeLEDsCy5Column,
            this.homeLEDsFAMColumn,
            this.homeLEDsHEXColumn,
            this.homeLEDsAttoColumn,
            this.homeLEDsAlexaColumn,
            this.homeLEDsCy55Column});
            this.homeLEDsDataGridView.Location = new System.Drawing.Point(8, 429);
            this.homeLEDsDataGridView.Name = "homeLEDsDataGridView";
            this.homeLEDsDataGridView.ReadOnly = true;
            this.homeLEDsDataGridView.RowHeadersVisible = false;
            this.homeLEDsDataGridView.RowTemplate.Height = 25;
            this.homeLEDsDataGridView.Size = new System.Drawing.Size(344, 149);
            this.homeLEDsDataGridView.TabIndex = 5;
            // 
            // homeLEDsPropertiesColumn
            // 
            this.homeLEDsPropertiesColumn.HeaderText = "";
            this.homeLEDsPropertiesColumn.Name = "homeLEDsPropertiesColumn";
            this.homeLEDsPropertiesColumn.ReadOnly = true;
            // 
            // homeLEDsCy5Column
            // 
            this.homeLEDsCy5Column.HeaderText = "Cy5";
            this.homeLEDsCy5Column.Name = "homeLEDsCy5Column";
            this.homeLEDsCy5Column.ReadOnly = true;
            this.homeLEDsCy5Column.Width = 40;
            // 
            // homeLEDsFAMColumn
            // 
            this.homeLEDsFAMColumn.HeaderText = "FAM";
            this.homeLEDsFAMColumn.Name = "homeLEDsFAMColumn";
            this.homeLEDsFAMColumn.ReadOnly = true;
            this.homeLEDsFAMColumn.Width = 40;
            // 
            // homeLEDsHEXColumn
            // 
            this.homeLEDsHEXColumn.HeaderText = "HEX";
            this.homeLEDsHEXColumn.Name = "homeLEDsHEXColumn";
            this.homeLEDsHEXColumn.ReadOnly = true;
            this.homeLEDsHEXColumn.Width = 40;
            // 
            // homeLEDsAttoColumn
            // 
            this.homeLEDsAttoColumn.HeaderText = "Atto";
            this.homeLEDsAttoColumn.Name = "homeLEDsAttoColumn";
            this.homeLEDsAttoColumn.ReadOnly = true;
            this.homeLEDsAttoColumn.Width = 40;
            // 
            // homeLEDsAlexaColumn
            // 
            this.homeLEDsAlexaColumn.HeaderText = "Alexa";
            this.homeLEDsAlexaColumn.Name = "homeLEDsAlexaColumn";
            this.homeLEDsAlexaColumn.ReadOnly = true;
            this.homeLEDsAlexaColumn.Width = 40;
            // 
            // homeLEDsCy55Column
            // 
            this.homeLEDsCy55Column.HeaderText = "Cy5.5";
            this.homeLEDsCy55Column.Name = "homeLEDsCy55Column";
            this.homeLEDsCy55Column.ReadOnly = true;
            this.homeLEDsCy55Column.Width = 40;
            // 
            // homeLEDsLabel
            // 
            this.homeLEDsLabel.AutoSize = true;
            this.homeLEDsLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.homeLEDsLabel.Location = new System.Drawing.Point(8, 408);
            this.homeLEDsLabel.Name = "homeLEDsLabel";
            this.homeLEDsLabel.Size = new System.Drawing.Size(48, 18);
            this.homeLEDsLabel.TabIndex = 4;
            this.homeLEDsLabel.Text = "LEDs";
            // 
            // homeCameraDataGridView
            // 
            this.homeCameraDataGridView.AllowUserToAddRows = false;
            this.homeCameraDataGridView.AllowUserToDeleteRows = false;
            this.homeCameraDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.homeCameraDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.homeCameraStateColumn,
            this.homeCameraExposureColumn,
            this.homeCameraTempColumn});
            this.homeCameraDataGridView.Location = new System.Drawing.Point(8, 335);
            this.homeCameraDataGridView.Name = "homeCameraDataGridView";
            this.homeCameraDataGridView.ReadOnly = true;
            this.homeCameraDataGridView.RowHeadersVisible = false;
            this.homeCameraDataGridView.RowTemplate.Height = 25;
            this.homeCameraDataGridView.Size = new System.Drawing.Size(284, 70);
            this.homeCameraDataGridView.TabIndex = 3;
            // 
            // homeCameraLabel
            // 
            this.homeCameraLabel.AutoSize = true;
            this.homeCameraLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.homeCameraLabel.Location = new System.Drawing.Point(8, 314);
            this.homeCameraLabel.Name = "homeCameraLabel";
            this.homeCameraLabel.Size = new System.Drawing.Size(65, 18);
            this.homeCameraLabel.TabIndex = 2;
            this.homeCameraLabel.Text = "Camera";
            // 
            // homeMotorsDataGridView
            // 
            this.homeMotorsDataGridView.AllowUserToAddRows = false;
            this.homeMotorsDataGridView.AllowUserToDeleteRows = false;
            this.homeMotorsDataGridView.AllowUserToResizeRows = false;
            this.homeMotorsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.homeMotorsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.homeMotorsMotorColumn,
            this.homeMotorsIOColumn,
            this.homeMotorsStateColumn,
            this.homeMotorsStepsColumn,
            this.homeMotorsSpeedColumn,
            this.homeMotorsHomeColumn});
            this.homeMotorsDataGridView.Location = new System.Drawing.Point(8, 24);
            this.homeMotorsDataGridView.Name = "homeMotorsDataGridView";
            this.homeMotorsDataGridView.ReadOnly = true;
            this.homeMotorsDataGridView.RowHeadersVisible = false;
            this.homeMotorsDataGridView.RowHeadersWidth = 10;
            this.homeMotorsDataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.homeMotorsDataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.homeMotorsDataGridView.RowTemplate.Height = 25;
            this.homeMotorsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.homeMotorsDataGridView.Size = new System.Drawing.Size(513, 287);
            this.homeMotorsDataGridView.TabIndex = 1;
            // 
            // homeMotorsMotorColumn
            // 
            this.homeMotorsMotorColumn.HeaderText = "Motor";
            this.homeMotorsMotorColumn.Name = "homeMotorsMotorColumn";
            this.homeMotorsMotorColumn.ReadOnly = true;
            this.homeMotorsMotorColumn.Width = 80;
            // 
            // homeMotorsIOColumn
            // 
            this.homeMotorsIOColumn.HeaderText = "IO";
            this.homeMotorsIOColumn.Name = "homeMotorsIOColumn";
            this.homeMotorsIOColumn.ReadOnly = true;
            this.homeMotorsIOColumn.ToolTipText = "Action status of motor";
            this.homeMotorsIOColumn.Width = 70;
            // 
            // homeMotorsStateColumn
            // 
            this.homeMotorsStateColumn.HeaderText = "State";
            this.homeMotorsStateColumn.Name = "homeMotorsStateColumn";
            this.homeMotorsStateColumn.ReadOnly = true;
            // 
            // homeMotorsStepsColumn
            // 
            this.homeMotorsStepsColumn.HeaderText = "Steps (us)";
            this.homeMotorsStepsColumn.Name = "homeMotorsStepsColumn";
            this.homeMotorsStepsColumn.ReadOnly = true;
            this.homeMotorsStepsColumn.Width = 90;
            // 
            // homeMotorsSpeedColumn
            // 
            this.homeMotorsSpeedColumn.HeaderText = "Speed (us/s)";
            this.homeMotorsSpeedColumn.Name = "homeMotorsSpeedColumn";
            this.homeMotorsSpeedColumn.ReadOnly = true;
            // 
            // homeMotorsHomeColumn
            // 
            this.homeMotorsHomeColumn.HeaderText = "Home";
            this.homeMotorsHomeColumn.Name = "homeMotorsHomeColumn";
            this.homeMotorsHomeColumn.ReadOnly = true;
            this.homeMotorsHomeColumn.ToolTipText = "Home status of motor";
            this.homeMotorsHomeColumn.Width = 70;
            // 
            // homeMotorsLabel
            // 
            this.homeMotorsLabel.AutoSize = true;
            this.homeMotorsLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.homeMotorsLabel.Location = new System.Drawing.Point(8, 3);
            this.homeMotorsLabel.Name = "homeMotorsLabel";
            this.homeMotorsLabel.Size = new System.Drawing.Size(56, 18);
            this.homeMotorsLabel.TabIndex = 0;
            this.homeMotorsLabel.Text = "Motors";
            // 
            // runTabPage
            // 
            this.runTabPage.Location = new System.Drawing.Point(4, 24);
            this.runTabPage.Name = "runTabPage";
            this.runTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.runTabPage.Size = new System.Drawing.Size(1116, 584);
            this.runTabPage.TabIndex = 1;
            this.runTabPage.Text = "Run";
            this.runTabPage.UseVisualStyleBackColor = true;
            // 
            // controlTabPage
            // 
            this.controlTabPage.Location = new System.Drawing.Point(4, 24);
            this.controlTabPage.Name = "controlTabPage";
            this.controlTabPage.Size = new System.Drawing.Size(1116, 584);
            this.controlTabPage.TabIndex = 2;
            this.controlTabPage.Text = "Control";
            this.controlTabPage.UseVisualStyleBackColor = true;
            // 
            // thermocyclingTabPage
            // 
            this.thermocyclingTabPage.Location = new System.Drawing.Point(4, 24);
            this.thermocyclingTabPage.Name = "thermocyclingTabPage";
            this.thermocyclingTabPage.Size = new System.Drawing.Size(1116, 584);
            this.thermocyclingTabPage.TabIndex = 3;
            this.thermocyclingTabPage.Text = "Thermocycling";
            this.thermocyclingTabPage.UseVisualStyleBackColor = true;
            // 
            // imagingTabPage
            // 
            this.imagingTabPage.Location = new System.Drawing.Point(4, 24);
            this.imagingTabPage.Name = "imagingTabPage";
            this.imagingTabPage.Size = new System.Drawing.Size(1116, 584);
            this.imagingTabPage.TabIndex = 4;
            this.imagingTabPage.Text = "Imaging";
            this.imagingTabPage.UseVisualStyleBackColor = true;
            // 
            // settingsTabPage
            // 
            this.settingsTabPage.Location = new System.Drawing.Point(4, 24);
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.Size = new System.Drawing.Size(1116, 584);
            this.settingsTabPage.TabIndex = 5;
            this.settingsTabPage.Text = "Settings";
            this.settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // metrologyTabPage
            // 
            this.metrologyTabPage.Location = new System.Drawing.Point(4, 24);
            this.metrologyTabPage.Name = "metrologyTabPage";
            this.metrologyTabPage.Size = new System.Drawing.Size(1116, 584);
            this.metrologyTabPage.TabIndex = 6;
            this.metrologyTabPage.Text = "Metrology";
            this.metrologyTabPage.UseVisualStyleBackColor = true;
            // 
            // softwareVersionLabel
            // 
            this.softwareVersionLabel.AutoSize = true;
            this.softwareVersionLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.softwareVersionLabel.ForeColor = System.Drawing.Color.White;
            this.softwareVersionLabel.Location = new System.Drawing.Point(12, 623);
            this.softwareVersionLabel.Name = "softwareVersionLabel";
            this.softwareVersionLabel.Size = new System.Drawing.Size(117, 16);
            this.softwareVersionLabel.TabIndex = 1;
            this.softwareVersionLabel.Text = "SW Version: 1.0.1";
            // 
            // firmwareVersionLabel
            // 
            this.firmwareVersionLabel.AutoSize = true;
            this.firmwareVersionLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.firmwareVersionLabel.ForeColor = System.Drawing.Color.White;
            this.firmwareVersionLabel.Location = new System.Drawing.Point(147, 623);
            this.firmwareVersionLabel.Name = "firmwareVersionLabel";
            this.firmwareVersionLabel.Size = new System.Drawing.Size(156, 16);
            this.firmwareVersionLabel.TabIndex = 2;
            this.firmwareVersionLabel.Text = "Firmware Version: 1.2.1";
            // 
            // apiVersionLabel
            // 
            this.apiVersionLabel.AutoSize = true;
            this.apiVersionLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.apiVersionLabel.ForeColor = System.Drawing.Color.White;
            this.apiVersionLabel.Location = new System.Drawing.Point(318, 623);
            this.apiVersionLabel.Name = "apiVersionLabel";
            this.apiVersionLabel.Size = new System.Drawing.Size(117, 16);
            this.apiVersionLabel.TabIndex = 3;
            this.apiVersionLabel.Text = "API Version: 1.0.1";
            // 
            // guiVersionLabel
            // 
            this.guiVersionLabel.AutoSize = true;
            this.guiVersionLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.guiVersionLabel.ForeColor = System.Drawing.Color.White;
            this.guiVersionLabel.Location = new System.Drawing.Point(451, 623);
            this.guiVersionLabel.Name = "guiVersionLabel";
            this.guiVersionLabel.Size = new System.Drawing.Size(118, 16);
            this.guiVersionLabel.TabIndex = 4;
            this.guiVersionLabel.Text = "GUI Version: 1.0.0";
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.userLabel.ForeColor = System.Drawing.Color.White;
            this.userLabel.Location = new System.Drawing.Point(585, 623);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(107, 16);
            this.userLabel.TabIndex = 5;
            this.userLabel.Text = "User: glc-biorad";
            // 
            // logoutButton
            // 
            this.logoutButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.logoutButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.logoutButton.ForeColor = System.Drawing.Color.White;
            this.logoutButton.Location = new System.Drawing.Point(896, 616);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(98, 30);
            this.logoutButton.TabIndex = 6;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = false;
            // 
            // resetButton
            // 
            this.resetButton.BackColor = System.Drawing.Color.Red;
            this.resetButton.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.resetButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.resetButton.ForeColor = System.Drawing.Color.White;
            this.resetButton.Location = new System.Drawing.Point(1000, 616);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(98, 30);
            this.resetButton.TabIndex = 7;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = false;
            // 
            // homeCameraStateColumn
            // 
            this.homeCameraStateColumn.HeaderText = "State";
            this.homeCameraStateColumn.Name = "homeCameraStateColumn";
            this.homeCameraStateColumn.ReadOnly = true;
            // 
            // homeCameraExposureColumn
            // 
            this.homeCameraExposureColumn.HeaderText = "Exposure (ms)";
            this.homeCameraExposureColumn.Name = "homeCameraExposureColumn";
            this.homeCameraExposureColumn.ReadOnly = true;
            this.homeCameraExposureColumn.Width = 120;
            // 
            // homeCameraTempColumn
            // 
            this.homeCameraTempColumn.HeaderText = "Temp (K)";
            this.homeCameraTempColumn.Name = "homeCameraTempColumn";
            this.homeCameraTempColumn.ReadOnly = true;
            this.homeCameraTempColumn.Width = 60;
            // 
            // independentReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1124, 650);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.guiVersionLabel);
            this.Controls.Add(this.apiVersionLabel);
            this.Controls.Add(this.firmwareVersionLabel);
            this.Controls.Add(this.softwareVersionLabel);
            this.Controls.Add(this.tabControl);
            this.Name = "independentReaderForm";
            this.Text = "Independent Reader";
            this.tabControl.ResumeLayout(false);
            this.homeTabPage.ResumeLayout(false);
            this.homeTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.homeTECsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeLEDsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeCameraDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeMotorsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private DataGridViewTextBoxColumn homeMotorsMotorColumn;
        private DataGridViewTextBoxColumn homeMotorsIOColumn;
        private DataGridViewTextBoxColumn homeMotorsStateColumn;
        private DataGridViewTextBoxColumn homeMotorsStepsColumn;
        private DataGridViewTextBoxColumn homeMotorsSpeedColumn;
        private DataGridViewTextBoxColumn homeMotorsHomeColumn;
        private Label homeMotorsLabel;
        private DataGridView homeCameraDataGridView;
        private Label homeCameraLabel;
        private DataGridView homeLEDsDataGridView;
        private Label homeLEDsLabel;
        private DataGridViewTextBoxColumn homeLEDsPropertiesColumn;
        private DataGridViewTextBoxColumn homeLEDsCy5Column;
        private DataGridViewTextBoxColumn homeLEDsFAMColumn;
        private DataGridViewTextBoxColumn homeLEDsHEXColumn;
        private DataGridViewTextBoxColumn homeLEDsAttoColumn;
        private DataGridViewTextBoxColumn homeLEDsAlexaColumn;
        private DataGridViewTextBoxColumn homeLEDsCy55Column;
        private DataGridView homeTECsDataGridView;
        private Label homeTECsLabel;
        private DataGridViewTextBoxColumn homeTECsProprtyColumn;
        private DataGridViewTextBoxColumn homeTECsTECAColumn;
        private DataGridViewTextBoxColumn homeTECsTECBColumn;
        private DataGridViewTextBoxColumn homeTECsTECCColumn;
        private DataGridViewTextBoxColumn homeTECsTECDColumn;
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
    }
}