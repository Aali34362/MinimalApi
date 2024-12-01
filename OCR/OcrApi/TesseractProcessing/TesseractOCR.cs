using Tesseract;

namespace OcrApi.TesseractProcessing;

public class TesseractOCR : ITesseractOCR
{
    public string PerformOCR(string imagePath, string language = "eng", int psm = 6, EngineMode engineMode = EngineMode.Default)
    {
        // Ensure tessdata path exists
        ////string tessDataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");
        string tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";
        if (!Directory.Exists(tessDataPath))
        {
            throw new DirectoryNotFoundException($"Tessdata folder not found at {tessDataPath}");
        }

        // Initialize Tesseract Engine
        using var engine = new TesseractEngine(tessDataPath, language, engineMode);

        // Configure Page Segmentation Mode (PSM)
        engine.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"); // Example whitelist
        engine.SetVariable("user_defined_dpi", "300"); // Set DPI to improve recognition for low-quality images

        // Load the image
        using var img = Pix.LoadFromFile(imagePath);

        // Perform OCR
        using var page = engine.Process(img, PageSegMode.SingleBlock); // Use appropriate PSM for your case

        // Extract text
        string ocrText = page.GetText();

        // Extract confidence level (optional)
        float meanConfidence = page.GetMeanConfidence() * 100;

        Console.WriteLine($"OCR Confidence: {meanConfidence}%");
        return ocrText;
    }
}
