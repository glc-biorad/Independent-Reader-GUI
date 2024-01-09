using Independent_Reader_GUI.Exceptions;
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
        private XDocument xDocument;
        private XElement? scanningNode;
        private XElement? aNode;
        private XElement? bNode;
        private XElement? cNode;
        private XElement? dNode;
        private ScanningOption scanningOptionTECA = new ScanningOption();
        private ScanningOption scanningOptionTECB = new ScanningOption();
        private ScanningOption scanningOptionTECC = new ScanningOption();
        private ScanningOption scanningOptionTECD = new ScanningOption();
        public List<double> GlassOffsets = new List<double>();

        public ScanningOptions(Configuration configuration, DataGridView dataGridView)
        {
            this.configuration = configuration;
            // Load in the Scanning Data XML document
            xDocument = XDocument.Load(configuration.ScanningDataPath);
            // Get the root node "Scanning" and for each TEC
            scanningNode = xDocument.Root;
            aNode = scanningNode.Element("A");
            bNode = scanningNode.Element("B");
            cNode = scanningNode.Element("C");
            dNode = scanningNode.Element("D");
            ReadInScanningData(dataGridView, configuration.DefaultCartridge, configuration.DefaultGlassOffset, configuration.DefaultElastomer, configuration.DefaultBergquist);
        }

        /// <summary>
        /// Get scanning data for a particular Heater ,Cartridge, Glass Offset, Elastomer, and Bergquist option
        /// </summary>
        /// <param name="heater">Heater for the scanning data</param>
        /// <param name="cartridgeName">Cartridge for the scanning data</param>
        /// <param name="glassOffset">Glass Offset for the scanning data</param>
        /// <param name="elastomerName">Elastomer name for the scannig data</param>
        /// <param name="bergquistName">Bergquist name for the scanning data</param>
        /// <returns>ScanningOption</returns>
        public ScanningOption GetScanningOptionData(string heater, string cartridgeName, double glassOffset, string elastomerName, string bergquistName)
        {
            XElement? tecNode;
            HeaterOptions heaterOptions = new HeaterOptions();
            tecNode = (heater == heaterOptions.A) ? aNode
                : (heater == heaterOptions.B) ? bNode
                : (heater == heaterOptions.C) ? cNode
                : (heater == heaterOptions.D) ? dNode
                : null;
            ScanningOption scanningOption = new ScanningOption();
            SetScanningOptionFromTECNode(tecNode, scanningOption, cartridgeName, elastomerName, bergquistName, glassOffset);
            return scanningOption;
        }

        /// <summary>
        /// Check if a Combination of Elastomer, Cartridge, Glass Offset, and Bergquist exist in the XML data for Scanning
        /// </summary>
        /// <param name="cartridgeName"></param>
        /// <param name="glassOffset"></param>
        /// <param name="elastomerName"></param>
        /// <param name="bergquistName"></param>
        /// <returns></returns>
        private bool CombinationExists(string cartridgeName, double glassOffset, string elastomerName, string bergquistName)
        {
            // Set the combination
            List<string> combination = new List<string>
            {
                cartridgeName,
                glassOffset.ToString(),
                elastomerName,
                bergquistName,
            };
            // Set the list of nodes to check
            List<XElement> nodes = new List<XElement>
            {
                aNode,
                bNode,
                cNode,
                dNode,
            };
            // Check the nodes for this combination
            foreach (XElement node in nodes)
            {
                foreach (XElement configurationNode in node.Elements())
                {
                    List<string> nodeCombination = new List<string>
                {
                    configurationNode.Element("Cartridge").Value.ToString(),
                    configurationNode.Element("GlassOffset").Value.ToString(),
                    configurationNode.Element("Elastomer").Value.ToString(),
                    configurationNode.Element("Bergquist").Value.ToString(),
                };
                    if (nodeCombination.Equals(combination))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<XElement> GetTECXElementsFromCombination(string cartridgeName, double glassOffset, string elastomerName, string bergquistName)
        {
            // Set the combination
            List<string> combination = new List<string>
            {
                cartridgeName,
                glassOffset.ToString(),
                elastomerName,
                bergquistName,
            };
            // Set the list of nodes to check
            List<XElement> nodes = new List<XElement>
            {
                aNode,
                bNode,
                cNode,
                dNode,
            };
            // Setup the return 
            List<XElement> tecXElements = new List<XElement>();
            // Check the nodes for this combination
            foreach (XElement node in nodes)
            {
                foreach (XElement configurationNode in node.Elements())
                {
                    List<string> nodeCombination = new List<string>
                {
                    configurationNode.Element("Cartridge").Value.ToString(),
                    configurationNode.Element("GlassOffset").Value.ToString(),
                    configurationNode.Element("Elastomer").Value.ToString(),
                    configurationNode.Element("Bergquist").Value.ToString(),
                };
                    if (nodeCombination.Equals(combination))
                    {
                        tecXElements.Add(configurationNode);
                    }
                }
            }
            if (tecXElements.Count != 4)
            {
                throw new UnexpectedResultException($"Unexpected result, anticipated 4 entries for Scanning Data but found {tecXElements.Count}");
            }
            return tecXElements;
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
            // Read in the Scanning options for this combination
            // Obtain data from the XML file
            SetScanningOptionFromTECNode(aNode, scanningOptionTECA, cartridgeName, elastomerName, bergquistName, glassOffset);
            SetScanningOptionFromTECNode(bNode, scanningOptionTECB, cartridgeName, elastomerName, bergquistName, glassOffset);
            SetScanningOptionFromTECNode(cNode, scanningOptionTECC, cartridgeName, elastomerName, bergquistName, glassOffset);
            SetScanningOptionFromTECNode(dNode, scanningOptionTECD, cartridgeName, elastomerName, bergquistName, glassOffset);
            // Load this data into the Configure tab's ImageScanningDataGridView
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECA, 0);
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECB, 1);
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECC, 2);
            SetDataGridViewRowFromScanningOption(dataGridView, scanningOptionTECD, 3);
        }

        public void WriteInScanningData(DataGridViewManager dataGridViewManager, DataGridView dataGridView, 
            string cartridgeName, double glassOffset, string elastomerName, string bergquistName)
        {
            // Load the XDocument for updating or writing
            XDocument scanningXDocument = XDocument.Load(configuration.ScanningDataPath);
            // Get the TEC nodes
            IEnumerable<XElement> tecXElements = scanningXDocument.Root?.Elements();
            // Check if this combination already exists in the XML file
            if (CombinationExists(cartridgeName, glassOffset, elastomerName, bergquistName))
            {
                // Update the XML
                // Get the TEC Configuration XElements to update
                List<XElement> nodes = GetTECXElementsFromCombination(cartridgeName, glassOffset, elastomerName, bergquistName);
                // Iterate through these nodes and update
                int rowIndex = 0;
                foreach (var node in nodes)
                {
                    node.Element("X0").Value = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "x0 (\u03BCS)", rowIndex);
                    node.Element("Y0").Value = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "y0 (\u03BCS)", rowIndex);
                    node.Element("Z0").Value = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "z0 (\u03BCS)", rowIndex);
                    node.Element("FOVdX").Value = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "FOV dX (\u03BCS)", rowIndex);
                    node.Element("dY").Value = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "dY (\u03BCS)", rowIndex);
                    node.Element("RotationalOffset").Value = dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "Rotational Offset (\u00B0)", rowIndex);
                    rowIndex++;
                }
                scanningXDocument.Save(configuration.ScanningDataPath);
            }
            else
            {
                // Write the data
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                // Write a new configuration for each TEC
                int rowIndex = 0;
                foreach (var tecXElement in tecXElements)
                {
                    // Setup the configuration node for this TEC XElement
                    XElement configurationNode = new XElement("Configuration");
                    tecXElement.Add(configurationNode);
                    // Add the data to the Configuration node
                    XElement cartridgeNode = new XElement("Cartridge", cartridgeName);
                    configurationNode.Add(cartridgeNode);
                    XElement elastomerNode = new XElement("Elastomer", elastomerName);
                    configurationNode.Add(elastomerNode);
                    XElement bergquistNode = new XElement("Bergquist", bergquistName);
                    configurationNode.Add(bergquistNode);
                    XElement glassOffsetNode = new XElement("GlassOffset", glassOffset.ToString());
                    configurationNode.Add(glassOffsetNode);
                    XElement glassOffsetUnitsNode = new XElement("GlassOffsetUnits", SpatialUnits.Millimeter);
                    configurationNode.Add(glassOffsetUnitsNode);
                    int x0 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "x0 (\u03BCS)", rowIndex));
                    XElement x0Node = new XElement("X0", x0);
                    configurationNode.Add(x0Node);
                    int y0 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "y0 (\u03BCS)", rowIndex));
                    XElement y0Node = new XElement("Y0", y0);
                    configurationNode.Add(y0Node);
                    int z0 = int.Parse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "z0 (\u03BCS)", rowIndex));
                    XElement z0Node = new XElement("Z0", z0);
                    configurationNode.Add(z0Node);
                    int fovdx = int.Parse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "FOV dX (\u03BCS)", rowIndex));
                    XElement fovdxNode = new XElement("FOVdX", fovdx);
                    configurationNode.Add(fovdxNode);
                    int dy = int.Parse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "dY (\u03BCS)", rowIndex));
                    XElement dYNode = new XElement("dY", dy);
                    configurationNode.Add(dYNode);
                    int rotationalOffset = int.Parse(dataGridViewManager.GetColumnCellValueByColumnNameAndRowIndex(dataGridView, "Rotational Offset (\u00B0)", rowIndex));
                    XElement rotationalOffsetNode = new XElement("RotationalOffset", rotationalOffset);
                    configurationNode.Add(rotationalOffsetNode);
                    XElement unitsNode = new XElement("Units", SpatialUnits.Microsteps);
                    configurationNode.Add(unitsNode);
                    rowIndex++;
                }
                scanningXDocument.Save(configuration.ScanningDataPath);
            }
        }

        public List<double> GetGlassOffsets()
        {
            // Setup the return
            List<double> glassOffsets = new List<double>();
            // Iterate through a TEC Node adding anything unique
            foreach (var configurationNode in aNode.Elements())
            {
                double glassOffset = double.Parse(configurationNode.Element("GlassOffset").Value);
                if (!glassOffsets.Contains(glassOffset))
                {
                    glassOffsets.Add(glassOffset);
                }
            }
            return glassOffsets;
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
