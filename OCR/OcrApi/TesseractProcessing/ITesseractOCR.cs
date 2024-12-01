using Tesseract;

namespace OcrApi.TesseractProcessing;

public interface ITesseractOCR
{
    string PerformOCR(string imagePath, string language = "eng", int psm = 6, EngineMode engineMode = EngineMode.Default);
}
