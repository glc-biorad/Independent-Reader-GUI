using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Forms
{
    internal class ThermocyclingProtocolAddStepForm : Form
    {
        // Private attributes
        private Label stepTemperatureLabel;
        private TextBox stepTemperatureTextBox;
        private Label stepTimeLabel;
        private TextBox stepTimeTextBox;
        private Label stepTypeNameLabel;
        private ComboBox stepTypeNameComboBox;
        private Button okButton;
        private Button cancelButton;

        public ThermocyclingProtocolAddStepForm()
        {
            // Step temperature text box initialization and placement
            stepTemperatureTextBox = new TextBox();
            stepTemperatureTextBox.Location = new Point(10, 25);
            stepTemperatureTextBox.Size = new Size(180, 36);

            // Step Temperature label initialization and placement
            stepTemperatureLabel = new Label();
            stepTemperatureLabel.Text = "Step Temperature (\u00B0C)";
            stepTemperatureLabel.Location = new Point(10, 5);
            stepTemperatureLabel.Size = new Size(180, 18);
            stepTemperatureLabel.Font = new Font(Name = "Arial", 12);

            // Step Time text box initialization and placement
            stepTimeTextBox = new TextBox();
            stepTimeTextBox.Location = new Point(10, 70);
            stepTimeTextBox.Size = new Size(180, 36);

            // Step Time label initialization and placement
            stepTimeLabel = new Label();
            stepTimeLabel.Text = "Step Time (s)";
            stepTimeLabel.Location = new Point(10, 50);
            stepTimeLabel.Size = new Size(180, 18);
            stepTimeLabel.Font = new Font(Name = "Arial", 12);

            // Step Type Name combo box initialization and placement
            stepTypeNameComboBox = new ComboBox();
            stepTypeNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            stepTypeNameComboBox.Items.AddRange(new object[]
            {
                ThermocyclingProtocolStepType.Set,
                ThermocyclingProtocolStepType.Hold
            });
            stepTypeNameComboBox.SelectedIndex = 0; // default to first item in the drop down list
            stepTypeNameComboBox.Location = new Point(10, 115);
            stepTypeNameComboBox.Size = new Size(180, 36);

            // Step Type Name label initialization and placement
            stepTypeNameLabel = new Label();
            stepTypeNameLabel.Text = "Step Type";
            stepTypeNameLabel.Location = new Point(10, 95);
            stepTypeNameLabel.Size = new Size(180, 18);
            stepTypeNameLabel.Font = new Font(Name = "Arial", 12);

            // Ok Button initialization and placement
            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(30, 150);
            okButton.Click += new EventHandler(okButton_Click);

            // Cancel Button initialization and placement
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(120, 150);
            cancelButton.DialogResult = DialogResult.Cancel;

            // Form settings
            ClientSize = new Size(200, 195);
            Controls.AddRange(new Control[]
            {
                stepTemperatureTextBox,
                stepTemperatureLabel,
                stepTypeNameComboBox,
                stepTypeNameLabel,
                stepTimeLabel,
                stepTimeTextBox,
                okButton,
                cancelButton
            });
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = okButton;
            CancelButton = cancelButton;

            // Wire up the text change events for text boxes
            stepTemperatureTextBox.TextChanged += (s, e) => StepTemperature_TextChanged(s, e);
            stepTimeTextBox.TextChanged += (s, e) => StepTime_TextChanged(s, e);

            // Wire up selection change comitted events for combo boxes
            stepTypeNameComboBox.SelectionChangeCommitted += (s, e) => StepTypeNameComboBox_SelectedChangeCommitted(s, e);
        }

        public double StepTemperature
        {
            get { return double.Parse(stepTemperatureTextBox.Text); }
        }

        public string StepTypeName
        {
            get { return stepTypeNameComboBox.SelectedItem.ToString(); }
        }

        public double StepTime
        {
            get { return double.Parse(stepTimeTextBox.Text); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(stepTemperatureTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid temperature value in \u00B0C.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!double.TryParse(stepTimeTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid time value in seconds.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void StepTemperature_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(stepTemperatureTextBox.Text, out _))
            {
                stepTemperatureTextBox.ForeColor = Color.Red;
                return;
            }
            else
            {
                stepTemperatureTextBox.ForeColor = Color.Black;
                return;
            }
        }

        private void StepTime_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(stepTimeTextBox.Text, out _))
            {
                stepTimeTextBox.ForeColor = Color.Red;
                return;
            }
            else
            {
                stepTimeTextBox.ForeColor = Color.Black;
                return;
            }
        }

        private void StepTypeNameComboBox_SelectedChangeCommitted(object sender, EventArgs e)
        {
            var stepTypeName = stepTypeNameComboBox.SelectedItem.ToString();
            if (stepTypeName != null)
            {
                if (stepTypeName.Equals(ThermocyclingProtocolStepType.Hold))
                {
                    stepTimeTextBox.Text = "\u221E";
                    stepTimeTextBox.Enabled = false;
                }
                else
                {
                    stepTimeTextBox.Enabled = true;
                    stepTimeTextBox.Text = string.Empty;
                }
            }
        }
    }
}
