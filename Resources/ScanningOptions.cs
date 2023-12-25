using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Services;
using Org.BouncyCastle.Asn1.BC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Independent_Reader_GUI.Resources
{
    internal class ScanningOptions
    {
        private Configuration configuration;
        private ScanningOption scanningOptionTECA = new ScanningOption();
        private ScanningOption scanningOptionTECB = new ScanningOption();
        private ScanningOption scanningOptionTECC = new ScanningOption();
        private ScanningOption scanningOptionTECD = new ScanningOption();
        public List<double> GlassOffsets = new List<double>();

        public ScanningOptions(Configuration configuration, DataGridView dataGridView)
        {
            this.configuration = configuration;
            ReadInScanningData(dataGridView, configuration.DefaultCartridge, configuration.DefaultGlassOffset, configuration.DefaultElastomer, configuration.DefaultBergquist);
        }

        /// <summary>
        /// Read in the Scanning Data for a particular combination 
        /// </summary>
        /// <param name="cartridge">Cartridge name used during imaging</param>
        /// <param name="glassOffset">Glass Offset between the tray and the heater surface</param>
        /// <param name="elastomer">Elastomer name for the elastomer that is on top of the cartridge during imaging</param>
        /// <param name="bergquist">Bergquist name for the bergquist that is on top of the cartridge during imaging</param>
        public void ReadInScanningData(DataGridView dataGridView, string cartridgeName, double glassOffset, string elastomerName, string bergquistName)
        {
            // Load in the Scanning data file
            XDocument xDocument = XDocument.Load(configuration.ScanningDataPath);
            var scanningNode = xDocument.Root;
            // Read in the Scanning options for this combination
            // Obtain data from the XML file
            XElement? aNode = scanningNode.Element("A");
            SetScanningOptionFromTECNode(aNode, scanningOptionTECA, cartridgeName, elastomerName, bergquistName, glassOffset);
            XElement? bNode = scanningNode.Element("B");
            SetScanningOptionFromTECNode(bNode, scanningOptionTECB, cartridgeName, elastomerName, bergquistName, glassOffset);
            XElement? cNode = scanningNode.Element("C");
            SetScanningOptionFromTECNode(cNode, scanningOptionTECC, cartridgeName, elastomerName, bergquistName, glassOffset);
            XElement? dNode = scanningNode.Element("D");
            SetScanningOptionFromTECNode(dNode, scanningOptionTECD, cartridgeName, elastomerName, bergquistName, glassOffset);
            // Load this data into the Configure tab's ImageScanningDataGridView
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECA, 0);
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECB, 1);
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECC, 2);
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECD, 3);
        }

        /// <summary>
        /// Set the ScanningOption for a particular TEC
        /// </summary>
        /// <param name="scanningOptionNode">Main node containing the scanning data nodes</param>
        /// <param name="tecNode">Node for a particular TEC</param>
        /// <param name="scanningOption">ScanningOption to be set</param>
        /// <param name="cartridge">Cartridge</param>
        /// <param name="elastomer">Elastomer</param>
        /// <param name="bergquist">Bergquist</param>
        /// <param name="glassOffset">GlassOffset between the glass on the tray and the heater surface</param>
        private void SetScanningOptionFromTECNode(XElement? tecNode, ScanningOption scanningOption, 
            string cartridgeName, string elastomerName, string bergquistName, double glassOffset)
        {
            if (tecNode != null)
            {
                // Look through each Configuration
                foreach (var configurationNode in tecNode.Elements("Configuration"))
                {
                    // Look for the correct combination
                    string _cartridgeName = configurationNode.Element("Cartridge").Value;
                    string _elastomerName = configurationNode.Element("Elastomer").Value;
                    string _bergquistName = configurationNode.Element("Bergquist").Value;
                    if (double.TryParse(configurationNode.Element("GlassOffset").Value, out _))
                    {
                        double _glassOffset = double.Parse(configurationNode.Element("GlassOffset").Value);
                        if (!GlassOffsets.Contains(_glassOffset))
                        {
                            GlassOffsets.Add(_glassOffset);
                        }
                        if (_cartridgeName == cartridgeName && _elastomerName == elastomerName && _bergquistName == bergquistName && _glassOffset == glassOffset)
                        {
                            // Obtain all Scanning information for this combination
                            scanningOption.Heater = tecNode.Name.LocalName;
                            scanningOption.CartridgeName = _cartridgeName;
                            scanningOption.ElastomerName = _elastomerName;
                            scanningOption.BergquistName = _bergquistName;
                            scanningOption.GlassOffset = _glassOffset;
                            scanningOption.X0 = int.Parse(configurationNode.Element("X0").Value);
                            scanningOption.Y0 = int.Parse(configurationNode.Element("Y0").Value);
                            scanningOption.Z0 = int.Parse(configurationNode.Element("Z0").Value);
                            scanningOption.FOVdX = int.Parse(configurationNode.Element("FOVdX").Value);
                            scanningOption.dY = int.Parse(configurationNode.Element("dY").Value);
                            scanningOption.RotationalOffset = int.Parse(configurationNode.Element("RotationalOffset").Value);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the DataGridView's row by index using the ScanningOption data
        /// </summary>
        /// <param name="dataGridView">DataGridView to be set by the ScanningOption data</param>
        /// <param name="scanningOption">ScanningOption to be used for setting the DataGridView's row</param>
        /// <param name="rowIndex">Row index of the DataGridView to be updated</param>
        private void SetDataGridViewRowFromScanningOption(DataGridView dataGridView, ScanningOption scanningOption, int rowIndex)
        {
            DataGridViewManager dataGridViewManager = new DataGridViewManager();
            dataGridView.Rows.Add();
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView, scanningOption.Heater, rowIndex, "Heater");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView, scanningOption.X0.ToString(), rowIndex, "x0 (\u03BCS)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView, scanningOption.Y0.ToString(), rowIndex, "y0 (\u03BCS)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView, scanningOption.Z0.ToString(), rowIndex, "z0 (\u03BCS)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView, scanningOption.FOVdX.ToString(), rowIndex, "FOV dX (\u03BCS)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView, scanningOption.dY.ToString(), rowIndex, "dY (\u03BCS)");
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(dataGridView, scanningOption.RotationalOffset.ToString(), rowIndex, "Rotational Offset (\u00B0)");
        }

    }
}
