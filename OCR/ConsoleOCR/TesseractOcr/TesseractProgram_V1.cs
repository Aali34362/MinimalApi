using Tesseract;

namespace ConsoleOCR.TesseractOcr;

public static class TesseractProgram_V1
{
    public static void TesseractProgram_V1_Main()
    {
        string imagePath = @"path\to\your\image.jpg";
        string tessDataPath = @"path\to\tessdata";

        try
        {
            using (var ocrEngine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = ocrEngine.Process(img))
                    {
                        string text = page.GetText();
                        Console.WriteLine("Extracted Text: ");
                        Console.WriteLine(text);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
