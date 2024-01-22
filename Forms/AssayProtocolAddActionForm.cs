using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Models.Consumables;
using Independent_Reader_GUI.Models.Protocols.AssayProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Independent_Reader_GUI.Forms
{
    public partial class AssayProtocolAddActionForm : Form
    {
        private string actionText; // ActionText based on the selected options

        public AssayProtocolAddActionForm()
        {
            InitializeComponent();
            //
            // Setup the page
            //
            SetupTipsTabPage();
        }

        private void SetupTipsTabPage()
        {
            //
            // Setup ComboBoxes and their Default values
            //
            addTipsTypeComboBox.DataSource = AssayProtocolActionTips.TypeOptions;
            addTipsTypeComboBox.Text = AssayProtocolActionTips.TypeOptions[0];
            addTipsTipComboBox.DataSource = TipOptions.Options;
            addTipsTipComboBox.Text = TipOptions.Options[0];
            addTipsTrayColumnBox.DataSource = TrayOptions.Options;
            addTipsTrayColumnBox.Text = TrayOptions.Options[0];
            addTipsColumnComboBox.DataSource = ColumnOptions.GetNOptions(8);
            addTipsColumnComboBox.Text = ColumnOptions.Options[0];
            addTipsDescriptionRichTextBox.Text = "";
            addTipsDisplayDescriptionCheckBox.Checked = false;
        }

        private void addTipsActionButton_Click(object sender, EventArgs e)
        {
            // Create the Assay Protocol Action
            string type = addTipsTypeComboBox.Text;
            string tip = addTipsTipComboBox.Text;
            string tray = addTipsTrayColumnBox.Text;
            int column = int.Parse(addTipsColumnComboBox.Text);
            string description = addTipsDescriptionRichTextBox.Text;
            bool displayDescription = addTipsDisplayDescriptionCheckBox.Checked;
            AssayProtocolActionTips action = new AssayProtocolActionTips(type, tip, tray, column, description, displayDescription);
            actionText = action.ActionText;
            DialogResult = DialogResult.OK;
            //Close();
        }

        public string GetDescription
        {
            get { return addTipsDescriptionRichTextBox.Text; }
        }

        public bool DisplayDescription
        {
            get { return addTipsDisplayDescriptionCheckBox.Checked; }
        }

        public string GetActionText
        {
            get { return actionText; }
        }
    }
}
