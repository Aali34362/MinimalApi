using System.Drawing;
using Tesseract;
using UglyToad.PdfPig;

namespace OcrApi.PDFProcessing;

public class PdfOCRProcessor(ILogger<PdfOCRProcessor> logger) : IPdfOCRProcessor
{
    private readonly ILogger<PdfOCRProcessor> _logger = logger;

    ////public string ProcessPdf(string pdfPath, string tessDataPath, string language = "eng")
    ////{
    ////    if (!File.Exists(pdfPath))
    ////    {
    ////        throw new FileNotFoundException($"PDF file not found: {pdfPath}");
    ////    }

    ////    if (!Directory.Exists(tessDataPath))
    ////    {
    ////        throw new DirectoryNotFoundException($"Tessdata folder not found: {tessDataPath}");
    ////    }

    ////    var extractedText = new List<string>();

    ////    // Load the PDF
    ////    using (var pdf = PdfDocument.Open(pdfPath))
    ////    {
    ////        var pageIndex = 1;
    ////        foreach (var page in pdf.GetPages())
    ////        {
    ////            Console.WriteLine($"Processing page {pageIndex}...");
    ////            string pageText = ExtractTextFromPage(page, tessDataPath, language);
    ////            extractedText.Add($"Page {pageIndex}:\n{pageText}");
    ////            pageIndex++;
    ////        }
    ////    }

    ////    return string.Join("\n\n", extractedText);
    ////}

    ////private static string ExtractTextFromPage(UglyToad.PdfPig.Content.Page page, string tessDataPath, string language)
    ////{
    ////    // Save the page as an image for OCR
    ////    string tempImagePath = Path.Combine(Path.GetTempPath(), $"page-{page.Number}.png");

    ////    // Extract the page image (if available)
    ////    using (var bitmap = page.GetBitmap())
    ////    {
    ////        bitmap.Save(tempImagePath);
    ////    }

    ////    // Perform OCR on the image
    ////    using (var engine = new TesseractEngine(tessDataPath, language, EngineMode.Default))
    ////    {
    ////        using (var img = Pix.LoadFromFile(tempImagePath))
    ////        {
    ////            using (var pageOCR = engine.Process(img))
    ////            {
    ////                return pageOCR.GetText();
    ////            }
    ////        }
    ////    }
    ////}

    public string ProcessPdf(string pdfPath, string tessDataPath, string language = "eng")
    {
        if (!File.Exists(pdfPath))
        {
            throw new FileNotFoundException($"PDF file not found: {pdfPath}");
        }

        if (!Directory.Exists(tessDataPath))
        {
            throw new DirectoryNotFoundException($"Tessdata folder not found: {tessDataPath}");
        }

        var extractedText = new List<string>();

        // Load the PDF
        using (var pdf = PdfDocument.Open(pdfPath))
        {
            var pageIndex = 1;
            foreach (var page in pdf.GetPages())
            {
                _logger.LogInformation($"Processing page {pageIndex}...");
                using (var bitmap = RenderPageAsImage(page))
                {
                    string pageText = PerformOCR(bitmap, tessDataPath, language);
                    extractedText.Add($"Page {pageIndex}:\n{pageText}");
                }
                pageIndex++;
            }
        }

        return string.Join("\n\n", extractedText);
    }

    private static Bitmap RenderPageAsImage(Page page)
    {
        using (var pdfium = new PdfiumViewer.PdfDocument(page.Document.FilePath))
        {
            return pdfium.Render(page.Number - 1, 300, 300, PdfiumViewer.PdfRenderFlags.CorrectFromDpi);
        }
    }

    private static string PerformOCR(Bitmap bitmap, string tessDataPath, string language)
    {
        using (var engine = new TesseractEngine(tessDataPath, language, EngineMode.Default))
        {
            using (var img = PixConverter.ToPix(bitmap))
            {
                using (var pageOCR = engine.Process(img))
                {
                    return pageOCR.GetText();
                }
            }
        }
    }
}
