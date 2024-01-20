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
        public AssayProtocolAddActionForm()
        {
            InitializeComponent();
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
        }
    }
}
