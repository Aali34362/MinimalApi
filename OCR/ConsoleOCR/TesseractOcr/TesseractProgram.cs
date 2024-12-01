using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;
using Tesseract;
using SkiaSharp;
using ImageMagick;

namespace ConsoleApp1.TesseractOcr;

public static class TesseractProgram
{
    public static void TesseractMain()
    {
        // Set the TESSDATA_PREFIX environment variable
        string tessDataPath = @"C:\Program Files\Tesseract-OCR\";
        Environment.SetEnvironmentVariable("TESSDATA_PREFIX", tessDataPath, EnvironmentVariableTarget.Process);


        var testImagePath = "./TesseractOcr/large.png";
        string absolutetestImagePath = GetAbsolutePath(testImagePath);
        ////var testImagePath = "./TesseractOcr/claimbill.webp";
        ////string absolutetestImagePath = GetAbsolutePath(testImagePath);


        ////var oldImage = File.ReadAllBytes(absolutetestImagePath);
        //////var newImage = ConvertImageToTiff(oldImage);
        ////var newImage = ConvertWebpToTiff(oldImage);
        ////var largeTiff = $"{GetAbsolutePath()}/TesseractOcr/large.tiff";
        ////File.WriteAllBytes(largeTiff, newImage);

        var tessdataeng = @"C:\Program Files\Tesseract-OCR\tessdata";
        ////string tessdata = GetAbsolutePath(tessdataeng);


        try
        {
            //using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            using (var engine = new TesseractEngine(tessdataeng, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(absolutetestImagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                        Console.WriteLine("Text (GetText): \r\n{0}", text);
                        Console.WriteLine("Text (iterator):");
                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();

                            do
                            {
                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                            {
                                                Console.WriteLine("<BLOCK>");
                                            }

                                            Console.Write(iter.GetText(PageIteratorLevel.Word));
                                            Console.Write(" ");

                                            if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                        if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                        {
                                            Console.WriteLine();
                                        }
                                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                            } while (iter.Next(PageIteratorLevel.Block));
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Trace.TraceError(e.ToString());
            Console.WriteLine("Unexpected Error: " + e.Message);
            Console.WriteLine("Details: ");
            Console.WriteLine(e.ToString());
        }
        Console.Write("Press any key to continue . . . ");
        Console.ReadKey(true);
    }
    private static string GetAbsolutePath()
    {
        FileInfo _dataRoot = new FileInfo(Assembly.GetExecutingAssembly().Location);
        return _dataRoot.Directory!.Parent!.Parent!.Parent!.FullName;
    }

    private static string GetAbsolutePath(string relativePath)
    {
        FileInfo _dataRoot = new FileInfo(Assembly.GetExecutingAssembly().Location);
        string assemblyFolderPath = _dataRoot.Directory!.Parent!.Parent!.Parent!.FullName;
        return $"{assemblyFolderPath}{relativePath}";
    }

    private static byte[] ConvertImageToTiff(byte[] SourceImage)
    {
        //create a new byte array
        byte[] bin = new byte[0];

        //check if there is data
        if (SourceImage == null || SourceImage.Length == 0)
        {
            return bin;
        }

        //convert the byte array to a bitmap
        Bitmap NewImage;
        using (MemoryStream ms = new MemoryStream(SourceImage))
        {
            NewImage = new Bitmap(ms);
        }

        //set some properties
        Bitmap TempImage = new Bitmap(NewImage.Width, NewImage.Height);
        using (Graphics g = Graphics.FromImage(TempImage))
        {
            g.CompositingMode = CompositingMode.SourceCopy;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(NewImage, 0, 0, NewImage.Width, NewImage.Height);
        }
        NewImage = TempImage;

        //save the image to a stream
        using (MemoryStream ms = new MemoryStream())
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

            NewImage.Save(ms, GetEncoderInfo("image/tiff"), encoderParameters);
            bin = ms.ToArray();
        }

        //cleanup
        NewImage.Dispose();
        TempImage.Dispose();

        //return data
        return bin;
    }
    private static byte[] ConvertWebpToTiff(byte[] sourceImage)
    {
        if (sourceImage == null || sourceImage.Length == 0)
        {
            throw new ArgumentException("Source image is null or empty");
        }

        using (var image = new MagickImage(sourceImage))
        {
            image.Format = MagickFormat.Tiff;
            return image.ToByteArray();
        }
    }
    ////public static byte[] ConvertWebpToTiff(byte[] sourceImage)
    ////{
    ////    if (sourceImage == null || sourceImage.Length == 0)
    ////    {
    ////        throw new ArgumentException("Source image is null or empty");
    ////    }

    ////    // Load the .webp image into SkiaSharp
    ////    using (var webpStream = new MemoryStream(sourceImage))
    ////    using (var skBitmap = SKBitmap.Decode(webpStream))
    ////    {
    ////        if (skBitmap == null)
    ////        {
    ////            throw new ArgumentException("Unable to decode the WebP image.");
    ////        }

    ////        // Save the image as a .tiff
    ////        using (var tiffStream = new MemoryStream())
    ////        {
    ////            using (var skImage = SKImage.FromBitmap(skBitmap))
    ////            using (var skData = skImage.Encode(SKEncodedImageFormat.Tiff, 100))
    ////            {
    ////                skData.SaveTo(tiffStream);
    ////            }

    ////            return tiffStream.ToArray();
    ////        }
    ////    }
    ////}
    //get the correct encoder info
    private static ImageCodecInfo GetEncoderInfo(string MimeType)
    {
        ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
        for (int j = 0; j < encoders.Length; ++j)
        {
            if (encoders[j].MimeType.ToLower() == MimeType.ToLower())
                return encoders[j];
        }
        return null;
    }
}
