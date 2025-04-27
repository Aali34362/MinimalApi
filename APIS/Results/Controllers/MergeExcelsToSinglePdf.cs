using ClosedXML.Excel;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.IO;


namespace Results.Controllers;

public static class ExcelToPdf
{
    ////public static void MergeExcelsToSinglePdf(string[] excelFilePaths, string outputPdfPath)
    ////{
    ////    // Create a new PDF document
    ////    using (PdfDocument finalPdf = new PdfDocument())
    ////    {
    ////        foreach (var excelPath in excelFilePaths)
    ////        {
    ////            // Open the Excel file
    ////            using (ExcelEngine excelEngine = new ExcelEngine())
    ////            {
    ////                IApplication application = excelEngine.Excel;
    ////                application.DefaultVersion = ExcelVersion.Xlsx;

    ////                IWorkbook workbook = application.Workbooks.Open(excelPath);

    ////                // Convert Excel to PDF
    ////                ExcelToPdfConverter converter = new ExcelToPdfConverter(workbook);
    ////                ExcelToPdfConverterSettings settings = new ExcelToPdfConverterSettings();
    ////                settings.LayoutOptions = LayoutOptions.FitSheetOnOnePage;

    ////                PdfDocument pdfDocument = converter.Convert(settings);

    ////                // Merge converted PDF into the final PDF
    ////                finalPdf.ImportPageRange(pdfDocument, 0, pdfDocument.Pages.Count - 1);

    ////                pdfDocument.Close();
    ////            }
    ////        }

    ////        // Save final merged PDF
    ////        using (FileStream outputStream = new FileStream(outputPdfPath, FileMode.Create, FileAccess.Write))
    ////        {
    ////            finalPdf.Save(outputStream);
    ////        }
    ////    }
    ////}


    public static void MergeExcelsToSinglePdf_FreeWay(List<string> excelFiles, string outputPdfPath)
    {
        PdfDocument pdf = new PdfDocument();

        foreach (var filePath in excelFiles)
        {
            // 1. Load Excel
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // First sheet

                // 2. Save worksheet as a temporary image (draw manually)
                var img = RenderWorksheetToImage(worksheet);

                // 3. Add image to PDF
                var page = pdf.AddPage();
                page.Width = XUnit.FromMillimeter(210); // A4 width
                page.Height = XUnit.FromMillimeter(297); // A4 height

                using (var gfx = XGraphics.FromPdfPage(page))
                {
                    using (var ms = new MemoryStream())
                    {
                        img.SaveAsPng(ms);
                        ms.Position = 0;

                        var xImage = XImage.FromStream(() => ms);
                        gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);
                    }
                }
            }
        }

        // 4. Save merged PDF
        using (var fs = new FileStream(outputPdfPath, FileMode.Create, FileAccess.Write))
        {
            pdf.Save(fs);
        }
    }

    public static Image RenderWorksheetToImage(IXLWorksheet worksheet)
    {
        int width = 1000;
        int height = 1400;

        var img = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(width, height);
        img.Mutate(ctx =>
        {
            ctx.Fill(SixLabors.ImageSharp.Color.White);
            int y = 10;

            foreach (var row in worksheet.RowsUsed())
            {
                int x = 10;
                foreach (var cell in row.CellsUsed())
                {
                    ctx.DrawText(cell.GetString(), SystemFonts.CreateFont("Arial", 12), SixLabors.ImageSharp.Color.Black, new SixLabors.ImageSharp.PointF(x, y));
                    x += 150;
                }
                y += 25;
            }
        });

        return img;
    }
}
