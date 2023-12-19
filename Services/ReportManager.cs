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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class ReportManager
    {
        private string title = string.Empty; // Convention will be dPCR Report ({experiment_name})
        private string filePath;
        private PdfWriter writer;
        private PdfDocument pdf;
        private Document document;
        private PdfFont sectionTitleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        private int sectionTitleFontSize = 16;
        private PdfFont sectionTextFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        private int sectionTextFontSize = 12;
        private DeviceRgb columnHeaderBackgroundColor = new DeviceRgb(235, 222, 199);
        private PdfFont columnHeaderFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        public ReportManager(string filePath)
        {
            this.filePath = filePath;
            writer = new PdfWriter(filePath);
            pdf = new PdfDocument(writer);
            document = new Document(pdf);
        }

        public void Close()
        {
            document.Close();
            pdf.Close();
            writer.Close();
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        public void AddSubSection(string sectionTitle, string sectionText)
        {
            Paragraph title = new Paragraph(sectionTitle);
            title.SetFont(sectionTitleFont);
            title.SetFontSize(sectionTextFontSize);
            document.Add(title);
            Paragraph text = new Paragraph(sectionText);
            text.SetFont(sectionTextFont);
            text.SetFontSize(sectionTextFontSize);
            text.SetMarginBottom(10);
            document.Add(text);
        }

        public void AddHeaderLogo(string logoFilePath, int width, int height)
        {
            ImageData imageData = ImageDataFactory.Create(logoFilePath);
            iText.Layout.Element.Image logoImage = new iText.Layout.Element.Image(imageData);
            logoImage.SetWidth(width);
            logoImage.SetHeight(height);
            logoImage.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
            logoImage.SetMarginBottom(10);
            document.Add(logoImage);
        }

        public void AddTable(DataGridView dataGridView)
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
        }
    }
}