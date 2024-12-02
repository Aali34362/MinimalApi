namespace OcrApi.PDFProcessing;

public interface IPdfOCRProcessor
{
    string ProcessPdf(string pdfPath, string tessDataPath, string language = "eng");
}
