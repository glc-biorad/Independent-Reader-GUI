using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Forms
{
    internal class ThermocyclingProtocolEditStepForm : Form
    {
        // Private attributes
        private Label stepNumberLabel;
        private ComboBox stepNumberComboBox;
        private Label stepTemperatureLabel;
        private TextBox stepTemperatureTextBox;
        private Label stepTimeLabel;
        private TextBox stepTimeTextBox;
        private Button okButton;
        private Button cancelButton;
        private ThermocyclingProtocolPlotManager plotManager = null;

        public ThermocyclingProtocolEditStepForm(ThermocyclingProtocolPlotManager plotManager)
        {
            this.plotManager = plotManager;
            // Step Number text box initialization and placement
            stepNumberComboBox = new ComboBox();
            stepNumberComboBox.Location = new Point(10, 25);
            stepNumberComboBox.Size = new Size(180, 36);
            stepNumberComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            int stepCount = plotManager.StepCount;
            var stepNumberOptions = Enumerable.Range(1, stepCount).ToList();
            foreach (int stepNumber in stepNumberOptions)
            {
                stepNumberComboBox.Items.Add(stepNumber);
            }
            stepNumberComboBox.SelectedIndex = 0; // default to first item in the drop down list

            // Step Number label initialization and placement
            stepNumberLabel = new Label();
            stepNumberLabel.Text = "Step Number";
            stepNumberLabel.Location = new Point(10, 5);
            stepNumberLabel.Size = new Size(180, 18);
            stepNumberLabel.Font = new Font(Name = "Arial", 12);

            // Step Time text box initialization and placement
            stepTemperatureTextBox = new TextBox();
            stepTemperatureTextBox.Location = new Point(10, 70);
            stepTemperatureTextBox.Size = new Size(180, 36);

            // Step Time label initialization and placement
            stepTemperatureLabel = new Label();
            stepTemperatureLabel.Text = "Step Temperature (\u00B0C)";
            stepTemperatureLabel.Location = new Point(10, 50);
            stepTemperatureLabel.Size = new Size(180, 18);
            stepTemperatureLabel.Font = new Font(Name = "Arial", 12);

            // Step Time text box initialization and placement
            stepTimeTextBox = new TextBox();
            stepTimeTextBox.Location = new Point(10, 125);
            stepTimeTextBox.Size = new Size(180, 36);

            // Step Time label initialization and placement
            stepTimeLabel = new Label();
            stepTimeLabel.Text = "Step Time (s)";
            stepTimeLabel.Location = new Point(10, 95);
            stepTimeLabel.Size = new Size(180, 18);
            stepTimeLabel.Font = new Font(Name = "Arial", 12);

            // Ok Button initialization and placement
            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(30, 160);
            okButton.Click += new EventHandler(okButton_Click);

            // Cancel Button initialization and placement
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(120, 160);
            cancelButton.DialogResult = DialogResult.Cancel;

            // Form settings
            ClientSize = new Size(200, 200);
            Controls.AddRange(new Control[]
            {
                stepNumberComboBox,
                stepNumberLabel,
                stepTemperatureLabel,
                stepTemperatureTextBox,
                stepTimeTextBox,
                stepTimeLabel,
                okButton,
                cancelButton
            });
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = okButton;
            CancelButton = cancelButton;

            // Wire up the text change events for text boxes
            //stepTemperatureTextBox.TextChanged += (s, e) => StepTemperature_TextChanged(s, e);
            //stepTimeTextBox.TextChanged += (s, e) => StepTime_TextChanged(s, e);

            // Wire up selection change comitted events for combo boxes
            stepNumberComboBox.SelectionChangeCommitted += (s, e) => stepNumberComboBox_SelectionChangeCommitted(s, e);
        }

        public int StepNumber
        {
            get { return int.Parse(stepNumberComboBox.SelectedItem.ToString()); }
        }

        public double StepTemperature
        {
            get { return double.Parse(stepTemperatureTextBox.Text); }
        }

        public double StepTime
        {
            get { return double.Parse(stepTimeTextBox.Text); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(stepTemperatureTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid step temperature (\u00B0C)", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!int.TryParse(stepTimeTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid step time (s)", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void stepNumberComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int stepNumber = int.Parse(stepNumberComboBox.SelectedItem.ToString());
            // Get the step
            ThermocyclingProtocolStep step = plotManager.GetStep(stepNumber);
            // Fill the text boxes for temperature and time
            stepTemperatureTextBox.Text = step.Temperature.ToString();
            stepTimeTextBox.Text = step.Time.ToString();
        }
    }
}
