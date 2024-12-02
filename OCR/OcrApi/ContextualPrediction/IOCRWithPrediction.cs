namespace OcrApi.ContextualPrediction;

public interface IOCRWithPrediction
{
    string PerformOCRWithPrediction(string imagePath, List<string> validWords);
}
