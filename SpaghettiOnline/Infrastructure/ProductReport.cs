using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using SpaghettiOnline.Data;
using SpaghettiOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Infrastructure
{
    public class ProductReport
    {
        private readonly AppDbContext context;
        private IWebHostEnvironment env;

        public ProductReport(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        #region Declaration
        int maxColumns = 4;
        Document document;
        Font fontStyle;
        PdfPTable pdfPTable = new PdfPTable(4);
        PdfPCell pdfPCell;
        MemoryStream memoryStream = new MemoryStream();
        List<Product> products = new List<Product>();
        #endregion

        public byte[] Report(List<Product> products)
        {
            this.products = products;

            document = new Document();
            document.SetPageSize(PageSize.A4);
            document.SetMargins(5f, 5f, 20f, 5f);

            pdfPTable.WidthPercentage = 100;
            pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter docWrite = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            float[] sizes = new float[maxColumns];

            for (var i = 0; i < maxColumns; i++)
            {
                if (i == 0)
                {
                    sizes[i] = 20;
                }
                else
                {
                    sizes[i] = 100;
                }
            }

            pdfPTable.SetWidths(sizes);

            this.ReportHeader();
            this.EmptyRow(2);
            this.ReportBody();

            pdfPTable.HeaderRows = 2;
            document.Add(pdfPTable);

            document.Close();

            return memoryStream.ToArray();
        }

        private void ReportHeader()
        {
            pdfPCell = new PdfPCell(this.SetPageTitle());
            pdfPCell.Colspan = maxColumns;
            pdfPCell.Border = 0;
            pdfPTable.AddCell(pdfPCell);
            pdfPTable.CompleteRow();
        }

        private PdfPTable SetPageTitle()
        {
            PdfPTable pdfPTable = new PdfPTable(maxColumns);

            fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            pdfPCell = new PdfPCell(new Phrase("Products Report", fontStyle));
            pdfPCell.Colspan = maxColumns;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(pdfPCell);
            pdfPTable.CompleteRow();

            return pdfPTable;
        }

        private void EmptyRow(int nCount)
        {
            for (int i = 0; i < nCount; i++)
            {
                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.Colspan = maxColumns;
                pdfPCell.Border = 0;
                pdfPCell.ExtraParagraphSpace = 10;
                pdfPTable.AddCell(pdfPCell);
                pdfPTable.CompleteRow();
            }
        }

        private void ReportBody()
        {
            var fontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Table Header
            pdfPCell = new PdfPCell(new Phrase("Id", fontStyleBold));
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.Padding = 8;
            pdfPCell.BackgroundColor = BaseColor.LightGray;
            pdfPTable.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Name", fontStyleBold));
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.Padding = 8;
            pdfPCell.BackgroundColor = BaseColor.LightGray;
            pdfPTable.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Category", fontStyleBold));
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.Padding = 8;
            pdfPCell.BackgroundColor = BaseColor.LightGray;
            pdfPTable.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Price", fontStyleBold));
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.Padding = 8;
            pdfPCell.BackgroundColor = BaseColor.LightGray;
            pdfPTable.AddCell(pdfPCell);

            pdfPTable.CompleteRow();
            #endregion

            #region Table Body
            foreach (var product in products)
            {
                pdfPCell = new PdfPCell(new Phrase(product.Id.ToString(), fontStyle));
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.Padding = 8;
                pdfPCell.BackgroundColor = BaseColor.White;
                pdfPTable.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(product.Name, fontStyle));
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.Padding = 8;
                pdfPCell.BackgroundColor = BaseColor.White;
                pdfPTable.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(product.Category.Name, fontStyle));
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.Padding = 8;
                pdfPCell.BackgroundColor = BaseColor.White;
                pdfPTable.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(product.Price.ToString(), fontStyle));
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.Padding = 8;
                pdfPCell.BackgroundColor = BaseColor.White;
                pdfPTable.AddCell(pdfPCell);

                pdfPTable.CompleteRow();
            }
            #endregion
        }
    }
}
