using OpenCvSharp;

namespace OcrApi.ImageProcessing;

public interface ISuperResolutionService
{
    Mat UpscaleImage(Mat image);
}