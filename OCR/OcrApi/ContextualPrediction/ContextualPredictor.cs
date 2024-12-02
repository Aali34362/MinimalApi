using FuzzySharp;

namespace OcrApi.ContextualPrediction;

public static class ContextualPredictor
{
    /// <summary>
    /// Matches OCR text to the nearest valid word from a predefined dictionary.
    /// </summary>
    /// <param name="ocrText">The text extracted by OCR.</param>
    /// <param name="validWords">The dictionary of valid terms.</param>
    /// <returns>The corrected text.</returns>
    public static string PredictNearestWord(string ocrText, List<string> validWords)
    {
        if (string.IsNullOrEmpty(ocrText) || validWords == null || validWords.Count == 0)
        {
            return ocrText;
        }

        // Use FuzzySharp to find the best match
        var result = Process.ExtractOne(ocrText, validWords);
        return result != null && result.Score > 70 ? result.Value : ocrText;
    }
}
