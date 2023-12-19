using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
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

        public ReportManager(string filePath)
        {
            this.filePath = filePath;
            writer = new PdfWriter(filePath);
            pdf = new PdfDocument(writer);
            document = new Document(pdf);
        }

        public void Close()
        {
            writer.Close();
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        public void AddTable(DataGridView dataGridView)
        {
            Table table = new Table(dataGridView.ColumnCount);
            // Add Header Text to the table
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(column.HeaderText)));
            }
            // Add the rows to the table
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Cell().Add(new Paragraph(cell.Value?.ToString() ?? "")));
                }
            }
            document.Add(table);
        }
    }
}