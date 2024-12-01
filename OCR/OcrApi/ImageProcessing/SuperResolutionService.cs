using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;

namespace OcrApi.ImageProcessing;

public class SuperResolutionService : ISuperResolutionService
{
    private readonly InferenceSession _session;

    public SuperResolutionService(IOptions<SuperResolutionOptions> options)
    {
        var modelPath = options.Value.ModelPath;

        if (string.IsNullOrEmpty(modelPath))
            throw new ArgumentException("ModelPath cannot be null or empty.", nameof(modelPath));

        _session = new InferenceSession(modelPath);
    }

    ////public Mat UpscaleImage(Mat image)
    ////{
    ////    // Resize the input image to the model's required dimensions
    ////    var resizedImage = new Mat();
    ////    Cv2.Resize(image, resizedImage, new Size(224, 224)); // Example size (depends on the model)

    ////    // Normalize image and convert to tensor
    ////    var inputTensor = ConvertMatToTensor(resizedImage);

    ////    // Run inference
    ////    using var results = _session.Run(new List<NamedOnnxValue>
    ////    {
    ////        NamedOnnxValue.CreateFromTensor("input", inputTensor)
    ////    });

    ////    // Get the output tensor
    ////    var outputTensor = results.First().AsTensor<float>();

    ////    // Convert tensor back to OpenCV Mat
    ////    return ConvertTensorToMat(outputTensor, resizedImage.Rows, resizedImage.Cols);
    ////}

    //public Mat UpscaleImage(Mat image)
    //{
    //    // Convert to grayscale (if model expects grayscale)
    //    var grayImage = image.CvtColor(ColorConversionCodes.BGR2GRAY);

    //    // Resize the image to match model input dimensions (example: 1xHxW)
    //    var resizedImage = new Mat();
    //    Cv2.Resize(grayImage, resizedImage, new Size(256, 256)); // Adjust size as needed

    //    // Convert OpenCV Mat to ONNX tensor (single channel, normalized if needed)
    //    var inputTensor = new DenseTensor<float>(new[] { 1, resizedImage.Rows, resizedImage.Cols }); // Batch size 1, HxW dimensions
    //    for (int i = 0; i < resizedImage.Rows; i++)
    //    {
    //        for (int j = 0; j < resizedImage.Cols; j++)
    //        {
    //            inputTensor[0, i, j] = resizedImage.At<byte>(i, j) / 255f; // Normalize pixel value if needed
    //        }
    //    }

    //    // Run inference
    //    using var results = _session.Run(new List<NamedOnnxValue>
    //{
    //    NamedOnnxValue.CreateFromTensor("input", inputTensor)
    //});

    //    // Process the output tensor back to an OpenCV Mat
    //    var output = results.First().AsTensor<float>();
    //    var outputImage = new Mat(resizedImage.Rows, resizedImage.Cols, MatType.CV_8UC1); // 1 channel, single byte

    //    for (int i = 0; i < outputImage.Rows; i++)
    //    {
    //        for (int j = 0; j < outputImage.Cols; j++)
    //        {
    //            outputImage.Set<byte>(i, j, (byte)(output[0, i, j] * 255)); // Scale back to 0-255
    //        }
    //    }

    //    return outputImage;
    //}

    public Mat UpscaleImage(Mat image)
    {
        // Convert to grayscale (if the model expects grayscale)
        var grayImage = image.CvtColor(ColorConversionCodes.BGR2GRAY);

        // Resize the image to match model input dimensions (example: 1xHxW)
        var resizedImage = new Mat();
        Cv2.Resize(grayImage, resizedImage, new Size(224, 224)); // Adjust size as needed

        // Convert OpenCV Mat to ONNX tensor (1 channel, normalized if needed)
        var inputTensor = new DenseTensor<float>(new[] { 1, 1, resizedImage.Rows, resizedImage.Cols }); // 1 batch, 1 channel, HxW dimensions
        for (int i = 0; i < resizedImage.Rows; i++)
        {
            for (int j = 0; j < resizedImage.Cols; j++)
            {
                inputTensor[0, 0, i, j] = resizedImage.At<byte>(i, j) / 255f; // Normalize pixel value if needed
            }
        }

        // Run inference
        using var results = _session.Run(new List<NamedOnnxValue>
    {
        NamedOnnxValue.CreateFromTensor("input", inputTensor)
    });

        // Process the output tensor back to an OpenCV Mat
        var output = results.First().AsTensor<float>();
        var outputImage = new Mat(resizedImage.Rows, resizedImage.Cols, MatType.CV_8UC1); // 1 channel, single byte

        for (int i = 0; i < outputImage.Rows; i++)
        {
            for (int j = 0; j < outputImage.Cols; j++)
            {
                outputImage.Set<byte>(i, j, (byte)(output[0, 0, i, j] * 255)); // Scale back to 0-255
            }
        }

        return outputImage;
    }

    private DenseTensor<float> ConvertMatToTensor(Mat image)
    {
        // Convert Mat to float array and normalize values
        var input = new DenseTensor<float>(new[] { 1, 3, image.Rows, image.Cols });
        var index = 0;

        for (var row = 0; row < image.Rows; row++)
        {
            for (var col = 0; col < image.Cols; col++)
            {
                var pixel = image.At<Vec3b>(row, col);
                input[0, 0, row, col] = pixel.Item2 / 255.0f; // Blue channel
                input[0, 1, row, col] = pixel.Item1 / 255.0f; // Green channel
                input[0, 2, row, col] = pixel.Item0 / 255.0f; // Red channel
            }
        }

        return input;
    }

    private Mat ConvertTensorToMat(Tensor<float> tensor, int rows, int cols)
    {
        var output = new Mat(rows, cols, MatType.CV_8UC3);
        var index = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                var b = (byte)(tensor[0, 0, row, col] * 255.0f);
                var g = (byte)(tensor[0, 1, row, col] * 255.0f);
                var r = (byte)(tensor[0, 2, row, col] * 255.0f);
                output.Set(row, col, new Vec3b(b, g, r));
            }
        }

        return output;
    }
}
