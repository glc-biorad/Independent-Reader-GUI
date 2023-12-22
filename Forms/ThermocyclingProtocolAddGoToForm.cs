using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Forms
{
    internal class ThermocyclingProtocolAddGotoForm : Form
    {
        // Private Widget Attributes
        private Label stepNumberLabel;
        private ComboBox stepNumberComboBox;
        private Label cycleCountLabel;
        private TextBox cycleCountTextBox;
        private Button okButton;
        private Button cancelButton;
        private ThermocyclingProtocolPlotManager plotManager = null;

        public ThermocyclingProtocolAddGotoForm(ThermocyclingProtocolPlotManager plotManager)
        {
            this.plotManager = plotManager;
            // Step number label and combo box initialization and placement
            stepNumberLabel = new Label();
            stepNumberLabel.Location = new Point(10, 5);
            stepNumberLabel.Text = "Step Number";
            stepNumberLabel.Size = new Size(180, 18);
            stepNumberLabel.Font = new Font(Name = "Arial", 12);
            stepNumberComboBox = new ComboBox();
            stepNumberComboBox.Location = new Point(10, 25);
            stepNumberComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 1; i <= plotManager.StepCount; i++)
            {
                stepNumberComboBox.Items.Add(i.ToString());
            }
            try
            {
                stepNumberComboBox.SelectedIndex = 0; // default
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Cannot add a GoTo step as the first step in a thermocycling protocol.", "Invalid Step", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            stepNumberComboBox.Location = new Point(10, 25);
            stepNumberComboBox.Size = new Size(180, 36);

            // Cycle count label and text box initialization and placement
            cycleCountLabel = new Label();
            cycleCountLabel.Text = "Cycle Count";
            cycleCountLabel.Location = new Point(10, 50);
            cycleCountLabel.Size = new Size(180, 18);
            cycleCountLabel.Font = new Font(Name = "Arial", 12);
            cycleCountTextBox = new TextBox();
            cycleCountTextBox.Location = new Point(10, 70);
            cycleCountTextBox.Size = new Size(180, 36);

            // Ok Button initialization and placement
            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(30, 105);
            okButton.Click += new EventHandler(okButton_Click);

            // Cancel Button initialization and placement
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(120, 105);
            cancelButton.DialogResult = DialogResult.Cancel;

            // Form settings
            ClientSize = new Size(200, 140);
            Controls.AddRange(new Control[]
            {
                stepNumberLabel,
                stepNumberComboBox,
                cycleCountLabel,
                cycleCountTextBox,
                okButton,
                cancelButton
            });
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = okButton;
            CancelButton = cancelButton;

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(stepNumberComboBox.SelectedItem.ToString(), out _))
            {
                MessageBox.Show("Please enter a valid step number (any previous step).", "Invalid Step", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!double.TryParse(cycleCountTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid cycle number (positive integer value).", "Invalid Step", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        public int StepNumber
        {
            get { return int.Parse(stepNumberComboBox.SelectedItem.ToString()); }
        }

        public int CycleCount
        {
            get { return int.Parse(cycleCountTextBox.Text); }
        }
    }
}
