using OpenCvSharp;

namespace OcrApi.ImageProcessing;

public class ImagePreprocessor : IImagePreprocessor
{
    public void PreprocessImage(string inputImagePath, string outputImagePath)
    {
        // Read the image
        using var src = Cv2.ImRead(inputImagePath, ImreadModes.Color);

        // Check if the image is loaded properly
        if (src.Empty())
        {
            Console.WriteLine("Image not loaded. Check the path.");
            return;
        }

        // Step 1: Deblurring using GaussianBlur
        using var deblurred = new Mat();
        Cv2.GaussianBlur(src, deblurred, new Size(3, 3), 0);

        // Step 2: Convert to Grayscale
        using var gray = new Mat();
        Cv2.CvtColor(deblurred, gray, ColorConversionCodes.BGR2GRAY);

        // Step 3: Binarization (Thresholding)
        using var binary = new Mat();
        Cv2.Threshold(gray, binary, 127, 255, ThresholdTypes.Binary);

        // Step 4: Denoising using FastNlMeansDenoising
        using var denoised = new Mat();
        Cv2.FastNlMeansDenoising(binary, denoised, 30, 7, 21);

        // Step 5: Rotation Correction (Detect skew and rotate)
        using var corrected = CorrectSkew(denoised);

        // Save the preprocessed image
        Cv2.ImWrite(outputImagePath, corrected);

        Console.WriteLine($"Preprocessed image saved at: {outputImagePath}");
    }
    public void RePreprocessImage(string inputImagePath, string outputImagePath)
    {
        // Read the image
        using var src = Cv2.ImRead(inputImagePath, ImreadModes.Color);

        if (src.Empty())
        {
            Console.WriteLine("Image not loaded. Check the path.");
            return;
        }

        // Step 1: Increase Contrast
        using var contrastEnhanced = new Mat();
        Cv2.CvtColor(src, contrastEnhanced, ColorConversionCodes.BGR2GRAY);
        Cv2.EqualizeHist(contrastEnhanced, contrastEnhanced);

        // Step 2: Adaptive Thresholding
        using var adaptiveThreshold = new Mat();
        Cv2.AdaptiveThreshold(
            contrastEnhanced,
            adaptiveThreshold,
            255,
            AdaptiveThresholdTypes.GaussianC,
            ThresholdTypes.Binary,
            11,
            2);

        // Step 3: Dilation and Erosion
        var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(2, 2));
        Cv2.Dilate(adaptiveThreshold, adaptiveThreshold, kernel);
        Cv2.Erode(adaptiveThreshold, adaptiveThreshold, kernel);

        // Step 4: Unsharp Masking for Clarity
        using var blurred = new Mat();
        Cv2.GaussianBlur(adaptiveThreshold, blurred, new Size(5, 5), 0);
        using var sharp = new Mat();
        Cv2.AddWeighted(adaptiveThreshold, 1.5, blurred, -0.5, 0, sharp);

        // Step 5: Save the enhanced image
        Cv2.ImWrite(outputImagePath, sharp);

        Console.WriteLine($"Enhanced image saved at: {outputImagePath}");
    }
    private Mat CorrectSkew(Mat src)
    {
        using var edges = new Mat();
        Cv2.Canny(src, edges, 50, 150);

        var lines = Cv2.HoughLines(edges, 1, Math.PI / 180, 200);
        if (lines.Length > 0)
        {
            var angle = lines[0].Theta * 180 / Math.PI; // First line's angle
            if (angle > 45) angle -= 90; // Normalize the angle to a range
            var center = new Point2f(src.Width / 2, src.Height / 2);
            var rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, 1);
            var rotated = new Mat();
            Cv2.WarpAffine(src, rotated, rotationMatrix, src.Size());
            return rotated;
        }

        return src; // Return original if no rotation needed
    }

    // Adaptive Thresholding
    public static Mat ApplyAdaptiveThreshold(Mat image)
    {
        var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
        var adaptiveThresh = gray.AdaptiveThreshold(255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);
        return adaptiveThresh;
    }
    //Contrast Adjustment (Histogram Equalization)
    //public static Mat EnhanceContrast(Mat image)
    //{
    //    var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
    //    var equalized = new Mat();
    //    Cv2.EqualizeHist(gray, equalized);
    //    return equalized;
    //}
    public static Mat EnhanceContrast(Mat image)
    {
        if (image.Channels() == 1)
        {
            // If the image is grayscale, convert it to a 3-channel (RGB) image to apply color-based contrast enhancement
            Cv2.CvtColor(image, image, ColorConversionCodes.GRAY2BGR);
        }

        // Convert to grayscale for contrast enhancement (if needed)
        var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);

        // Apply histogram equalization or any other contrast adjustment
        var enhancedContrast = new Mat();
        Cv2.EqualizeHist(gray, enhancedContrast);

        // Return the enhanced contrast image
        return enhancedContrast;
    }
    //Sharpness Enhancement (Unsharp Masking)
    public static Mat EnhanceSharpness(Mat image)
    {
        var blurred = new Mat();
        Cv2.GaussianBlur(image, blurred, new Size(5, 5), 0);
        var sharp = new Mat();
        Cv2.AddWeighted(image, 1.5, blurred, -0.5, 0, sharp);
        return sharp;
    }
    //Dilation and Erosion
    public static Mat ApplyMorphology(Mat image)
    {
        var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(2, 2));
        var dilated = new Mat();
        Cv2.Dilate(image, dilated, kernel);
        var eroded = new Mat();
        Cv2.Erode(dilated, eroded, kernel);
        return eroded;
    }
    //OCR-Tuned Preprocessing
    ////public Mat OCRPreprocess(Mat image)
    ////{
    ////    var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
    ////    var blurred = new Mat();
    ////    Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);
    ////    var adaptiveThresh = blurred.AdaptiveThreshold(255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 11, 15);
    ////    var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));
    ////    var morphed = new Mat();
    ////    Cv2.MorphologyEx(adaptiveThresh, morphed, MorphTypes.Close, kernel);
    ////    return morphed;
    ////}
    public Mat OCRPreprocess(Mat image)
    {
        // Check if image is empty
        if (image.Empty())
        {
            throw new ArgumentException("The input image is empty.");
        }

        // Check if the image has the correct number of channels (grayscale)
        if (image.Channels() != 3 && image.Channels() != 1)
        {
            throw new ArgumentException("The input image must have 1 or 3 channels.");
        }

        // Convert the image to grayscale (if it has 3 channels, otherwise it's already grayscale)
        Mat gray;
        if (image.Channels() == 3)
        {
            gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
        }
        else
        {
            gray = image; // If already grayscale, no need to convert
        }

        // Apply Gaussian blur
        var blurred = new Mat();
        Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

        // Apply adaptive thresholding
        var adaptiveThresh = blurred.AdaptiveThreshold(255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 11, 15);

        // Create a kernel for morphological operations
        var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));

        // Apply morphological transformation (close operation)
        var morphed = new Mat();
        Cv2.MorphologyEx(adaptiveThresh, morphed, MorphTypes.Close, kernel);

        return morphed;
    }

    //Full Preprocessing Pipeline
    public Mat PreprocessImage(string inputPath)
    {
        var image = Cv2.ImRead(inputPath);

        // Step 1: Adaptive Thresholding
        var adaptiveThresh = ApplyAdaptiveThreshold(image);

        // Step 2: Contrast Adjustment
        var contrastEnhanced = EnhanceContrast(adaptiveThresh);

        // Step 3: Sharpness Enhancement
        var sharpImage = EnhanceSharpness(contrastEnhanced);

        // Step 4: Morphological Operations
        var finalImage = ApplyMorphology(sharpImage);

        return finalImage;
    }
    //Super-Resolution (Optional)
}
