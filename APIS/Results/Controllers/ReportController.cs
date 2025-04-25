using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Results.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : Controller
{
    [HttpPost("generate")]
    public IActionResult GenerateReports()
    {
        // File paths
        string sourceFile = "C:\\Users\\Admin\\Downloads\\SampleResult.xlsx";
        string templateFile = "C:\\Users\\Admin\\Downloads\\RESULT.xlsx";
        string outputDir = "C:\\Users\\Admin\\Documents\\";

        // Mapping source column headers to template named cells
        var mappings = new Dictionary<string, string>
        {
            ["English"] = "XEnglish",
            ["U/H/I"] = "XUR_HN_IT",
            ["ECO"] = "XECO",
            ["BK"] = "XBK",
            ["OC"] = "XOC",
            ["SP/Maths"] = "XSP_MATHS",
            ["Total"] = "XTOTAL",
            ["Percentage"] = "XPercentage",
            ["Remarks"] = "XRemarks",
            ["NAME OF THE STUDENTS"] = "XStudentName",
            ["ROLL NO"] = "XRollNo",
            ["Date of Birth"] = "XDateOfBirth",
            ["G R NO"] = "XGRNO",
        };

        ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

        using var sourcePackage = new ExcelPackage(new FileInfo(sourceFile));
        var sourceSheet = sourcePackage.Workbook.Worksheets[0];

        int row = 2;

        while (!string.IsNullOrEmpty(sourceSheet.Cells[row, 1].Text))
        {
            var studentData = new Dictionary<string, string>();
            foreach (var kvp in mappings)
            {
                int colIndex = FindColumnIndex(sourceSheet, kvp.Key);
                studentData[kvp.Value] = sourceSheet.Cells[row, colIndex].Text;
            }
            studentData.Add("XEVS", "A");
            studentData.Add("XPT", "A");
            studentData.Add("XReOpens", "");
            var rollNo = studentData["XRollNo"];
            var name = studentData["XStudentName"];
            string newFileName = Path.Combine(outputDir, $"{rollNo}_{name.Replace(" ", "_")}.xlsx");

            try
            {
                System.IO.File.Copy(templateFile, newFileName, overwrite: true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error copying file: {ex.Message}");
            }

            using var templatePackage = new ExcelPackage(new FileInfo(newFileName));
            var templateSheet = templatePackage.Workbook.Worksheets["Sheet1"];

            try
            {
                templatePackage.Save();
                templatePackage.Dispose();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving the template file to initialize named cells: {ex.Message}");
            }

            using (var package = new ExcelPackage(new FileInfo(newFileName)))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Sheet1"]; 
                foreach (var cellMap in studentData)
                {
                    try
                    {
                        var namedRange = workbook.Names[cellMap.Key];
                        if (namedRange != null)
                        {
                            namedRange.Value = cellMap.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting value for {cellMap.Key}: {ex.Message}");
                    }
                }

                package.Save();
            }
            row++;
        }

        return Ok("Reports generated successfully.");
    }

    private int FindColumnIndex(ExcelWorksheet sheet, string header)
    {
        for (int col = 1; col <= sheet.Dimension.End.Column; col++)
        {
            string header1 = sheet.Cells[1, col].Text.Trim();
            string header2 = sheet.Cells[2, col].Text.Trim();

            if (header1.Equals(header, StringComparison.OrdinalIgnoreCase) ||
                header2.Equals(header, StringComparison.OrdinalIgnoreCase))
            {
                return col;
            }
        }
        throw new KeyNotFoundException($"The column '{header}' was not found in either of the first two rows.");
    }

    [HttpPost("generates")]
    public IActionResult GenerateReport()
    {
        // File paths and setup
        string sourceFile = "C:\\Users\\Admin\\Downloads\\SampleResult.xlsx";
        string templateFile = "C:\\Users\\Admin\\Downloads\\RESULT.xlsx";
        string outputDir = "C:\\Users\\Admin\\Documents\\Shaheen\\";

        using (var package = new ExcelPackage(new FileInfo(Path.Combine(outputDir, "2_KHAN_MARIYAM_IMRAN_SALMA.xlsx"))))
        {
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets["Sheet1"]; // Replace with your actual sheet name

            // Access named range
            var namedRange = workbook.Names["XEnglish"];
            if (namedRange != null)
            {
                namedRange.Value = "20";
            }

            package.Save();
        }

        return Ok("Reports generated successfully!");
    }

}


