using OcrApi.ImageProcessing;
using OcrApi.TesseractProcessing;

namespace OcrApi.ContextualPrediction;

public class OCRWithPrediction(ITesseractOCR tesseractOCR) : IOCRWithPrediction
{
    private readonly ITesseractOCR _tesseractOCR = tesseractOCR;

    public string PerformOCRWithPrediction(string imagePath, List<string> validWords)
    {
        // Step 1: Perform OCR (using Tesseract or similar library)
        string extractedText = _tesseractOCR.PerformOCR(imagePath);

        // Step 2: Split OCR text into words for processing
        var words = extractedText.Split(new[] { ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Step 3: Predict nearest valid words
        var correctedWords = new List<string>();
        foreach (var word in words)
        {
            correctedWords.Add(ContextualPredictor.PredictNearestWord(word, validWords));
        }

        // Step 4: Reconstruct the corrected text
        return string.Join(" ", correctedWords);
    }
}
