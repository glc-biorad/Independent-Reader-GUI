using Independent_Reader_GUI.Models;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Colorspace;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage a Report
    /// </summary>
    internal class ReportManager
    {
        private string filePath;
        private PdfWriter writer;
        private PdfDocument pdf;
        private Document document;
        private PdfFont mainTitleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        private int mainTitleFontSize = 32;
        private PdfFont subsectionTitleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        private int subsectionTitleFontSize = 16;
        private PdfFont subsectionTextFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        private int subsectionTextFontSize = 12;
        private DeviceRgb columnHeaderBackgroundColor = new DeviceRgb(235, 222, 199);
        private PdfFont columnHeaderFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        private TECPlotManager tecPlotManager;

        public ReportManager(string filePath)
        {
            this.filePath = filePath;
            writer = new PdfWriter(filePath);
            pdf = new PdfDocument(writer);
            document = new Document(pdf);
            tecPlotManager = new TECPlotManager();
        }

        public async Task CloseAsync()
        {
            document.Close();
            pdf.Close();
            writer.Close();
            await Task.Delay(1);
        }

        public async Task AddThermocyclingProtocolPlotAsync(ThermocyclingProtocol protocol)
        {
            await Task.Delay(1);
        }

        public async Task AddMainTitleAsync(string titleText)
        {
            Paragraph title = new Paragraph(titleText);
            title.SetFont(mainTitleFont);
            title.SetFontSize(mainTitleFontSize);
            title.SetMarginBottom(10);
            document.Add(title);
            await Task.Delay(1);
        }

        public async Task AddParagraphTextAsync(string paragraphText)
        {
            Paragraph paragraph = new Paragraph(paragraphText);
            paragraph.SetFont(subsectionTextFont);
            paragraph.SetFontSize(subsectionTextFontSize);
            paragraph.SetMarginBottom(10);
            document.Add(paragraph);
            await Task.Delay(1);
        }

        public async Task AddSubSectionAsync(string sectionTitle, string sectionText)
        {
            Paragraph title = new Paragraph(sectionTitle);
            title.SetFont(subsectionTitleFont);
            title.SetFontSize(subsectionTextFontSize);
            document.Add(title);
            Paragraph text = new Paragraph(sectionText);
            text.SetFont(subsectionTextFont);
            text.SetFontSize(subsectionTextFontSize);
            text.SetMarginBottom(10);
            document.Add(text);
            await Task.Delay(1);
        }

        /// <summary>
        /// Add data to be plotted to the report
        /// </summary>
        /// <param name="data">Data to be plotted</param>
        /// <param name="plotTitle">Title of the plot</param>
        /// <param name="yAxisTitle">Y-Axis title</param>
        /// <returns></returns>
        public async Task AddPlotAsync(List<Tuple<DateTime, double>> data, string plotTitle, string yAxisTitle)
        {
            // Create the PlotModel for this data
            var plotModel = tecPlotManager.CreatePlotModel(data, plotTitle, yAxisTitle);
            // Export the PlotModel to a png
            tecPlotManager.ExportPlotModelToPNG(plotModel, "plot.png", 600, 400);
            // Add the plot image to the PDF
            ImageData imageData = ImageDataFactory.Create("plot.png");
            iText.Layout.Element.Image plotImage = new iText.Layout.Element.Image(imageData);
            document.Add(plotImage);
            await Task.Delay(50);
            // TODO: Remove the plot.png tmp file
        }

        /// <summary>
        /// Add two sets of data to be plotted in the report
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="plotTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="data1Title"></param>
        /// <param name="data2Title"></param>
        /// <returns></returns>
        public async Task AddPlotAsync(List<Tuple<DateTime, double>> data1, List<Tuple<DateTime, double>> data2, string plotTitle, string yAxisTitle, 
            string data1Title, string data2Title)
        {
            // Create the PlotModel for this data
            var plotModel = tecPlotManager.CreatePlotModel(data1, data2, plotTitle, yAxisTitle, data1Title, data2Title);
            // Export the PlotModel to a png
            tecPlotManager.ExportPlotModelToPNG(plotModel, "plot.png", 600, 400);
            // Add the plot image to the PDF
            ImageData imageData = ImageDataFactory.Create("plot.png");
            iText.Layout.Element.Image plotImage = new iText.Layout.Element.Image(imageData);
            document.Add(plotImage);
            await Task.Delay(50);
            // TODO: Remove the plot.png tmp file
        }

        public async Task AddHeaderLogoAsync(string logoFilePath, int width, int height)
        {
            ImageData imageData = ImageDataFactory.Create(logoFilePath);
            iText.Layout.Element.Image logoImage = new iText.Layout.Element.Image(imageData);
            logoImage.SetWidth(width);
            logoImage.SetHeight(height);
            logoImage.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
            logoImage.SetMarginBottom(10);
            document.Add(logoImage);
            await Task.Delay(1);
        }

        public async Task AddTableAsync(DataGridView dataGridView)
        {
            Table table = new Table(dataGridView.ColumnCount);
            // Add Header Text to the table
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // TODO: Change the color of the background 
                Cell cell = new Cell();
                cell.Add(new Paragraph(column.HeaderText));
                cell.SetBackgroundColor(columnHeaderBackgroundColor);
                cell.SetFont(columnHeaderFont);
                table.AddHeaderCell(cell);
            }
            // Add the rows to the table
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Cell().Add(new Paragraph(cell.Value?.ToString() ?? "")));
                }
            }
            table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
            table.SetMarginBottom(10);
            document.Add(table);
            await Task.Delay(1);
        }
    }
}