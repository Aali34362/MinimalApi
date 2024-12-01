using Microsoft.AspNetCore.Mvc;
using OcrApi.ImageProcessing;
using OpenCvSharp;

namespace OcrApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageProcessingController(IImagePreprocessor imagePreprocessor, ISuperResolutionService superResolutionService) : ControllerBase
{
    private readonly IImagePreprocessor _imagePreprocessor = imagePreprocessor;
    private readonly ISuperResolutionService _superResolutionService = superResolutionService;


    [HttpPost("preprocess")]
    public IActionResult PreprocessImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        var inputPath = Path.Combine("Images\\Uploads", file.FileName);
        var outputPath = Path.Combine("Images\\Processed", $"processed_{DateTime.Now.ToString("yyyymmddhhMMss")}_{file.FileName}");

        // Save uploaded file
        using (var stream = new FileStream(inputPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        // Preprocess the image
        _imagePreprocessor.PreprocessImage(inputPath, outputPath);
        ////_imagePreprocessor.RePreprocessImage(inputPath, outputPath);

        return Ok(new { Message = "Image processed successfully.", ProcessedImagePath = outputPath });
    }
    [HttpPost("RePreprocess")]
    public IActionResult RePreprocessImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        var inputPath = Path.Combine("Images\\Uploads", file.FileName);
        var outputPath = Path.Combine("Images\\Processed", $"processed_{DateTime.Now.ToString("yyyymmddhhMMss")}_{file.FileName}");

        // Save uploaded file
        using (var stream = new FileStream(inputPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        // Preprocess the image
        ////_imagePreprocessor.PreprocessImage(inputPath, outputPath);
        _imagePreprocessor.RePreprocessImage(inputPath, outputPath);

        return Ok(new { Message = "Image processed successfully.", ProcessedImagePath = outputPath });
    }

    [HttpPost("process")]
    public IActionResult ProcessImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        var tempFilePath = Path.GetTempFileName();
        using (var stream = System.IO.File.Create(tempFilePath))
        {
            file.CopyTo(stream);
        }

        var processedImage = _imagePreprocessor.PreprocessImage(tempFilePath);

        // Save or return the processed image
        var outputPath = Path.ChangeExtension(tempFilePath, ".processed.png");
        processedImage.SaveImage(outputPath);

        return PhysicalFile(outputPath, "image/png", Path.GetFileName(outputPath));
    }
    [HttpPost("superResolution")]
    public IActionResult SuperResolution(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        var tempFilePath = Path.GetTempFileName();
        using (var stream = System.IO.File.Create(tempFilePath))
        {
            file.CopyTo(stream);
        }

        // Load the image
        var image = Cv2.ImRead(tempFilePath);

        // Step 1: Apply super-resolution
        var enhancedImage = _superResolutionService.UpscaleImage(image);

        // Step 2: Preprocess the image for OCR
        var preprocessedImage = _imagePreprocessor.OCRPreprocess(enhancedImage);

        // Save or return the processed image
        var outputPath = Path.ChangeExtension(tempFilePath, ".processed.png");
        preprocessedImage.SaveImage(outputPath);

        return PhysicalFile(outputPath, "image/png", Path.GetFileName(outputPath));
    }
}
