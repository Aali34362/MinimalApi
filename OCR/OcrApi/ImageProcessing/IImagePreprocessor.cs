using OpenCvSharp;

namespace OcrApi.ImageProcessing;

public interface IImagePreprocessor
{
    void PreprocessImage(string inputImagePath, string outputImagePath);
    void RePreprocessImage(string inputImagePath, string outputImagePath);
    Mat PreprocessImage(string inputPath);
    Mat OCRPreprocess(Mat image);
}
