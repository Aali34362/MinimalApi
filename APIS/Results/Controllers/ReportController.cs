using DocumentFormat.OpenXml.Spreadsheet;
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
        string sourceFile = "C:\\Users\\Admin\\Downloads\\Rough Result COM 2024 - 2025.xlsx";
        string templateFile = "C:\\Users\\Admin\\Downloads\\ComResult.xlsx";
        string outputDir = "C:\\Users\\Admin\\Documents\\Shaheen";

        var mappings = new Dictionary<string, (string mainColumn, string supColumn, string destination)>
        {
            ["English"] = ("English", "EG", "XEnglish")!,
            ["U/H/I"] = ("U/H/I", "UHIG", "XUR_HN_IT")!,

            ["ECO"] = ("ECO", "ECG", "XECO"),
            ["BK"] = ("BK", "BKG", "XBK"),
            ["OC"] = ("OC", "OCG", "XOC"),
            ["SP/Maths"] = ("SP/Maths", "MG", "XSP_MATHS")!,

            ////["PHYSICS"] = ("PHYSICS", "PG", "XPHYSICS"),
            ////["CHEMISTRY"] = ("CHEMISTRY", "CG", "XCHEMISTRY"),
            ////["BIOLOGY"] = ("BIOLOGY", "BG", "XBIOLOGY"),
            ////["MATHS / GEO"] = ("MATHS / GEO", "MG", "XMATHS_GEO"),

            ["Total"] = ("Total", "TG", "XTOTAL")!,
            ["Percentage"] = ("Percentage", null, "XPercentage")!,
            ["Remarks"] = ("Remarks", null, "XRemarks")!,
            ["Attendance"] = ("Attendance", null, "XATTENDANVE")!,
            ["PT"] = ("PT", null, "XPT")!,
            ["EVS"] = ("EVS", null, "XEVS")!,

            ["NAME OF THE STUDENTS"] = ("NAME OF THE STUDENTS", null, "XStudentName")!,
            ["ROLL NO"] = ("ROLL NO", null, "XRollNo")!,
            ["G R NO"] = ("G R NO", null, "XGRNO")!,
            ["Date of Birth"] = ("Date of Birth", null, "XDateOfBirth")!,
            ["TEACHER'S REMARK"] = ("TEACHER'S REMARK", null, "XTEACHERREMARK")!,
        };

        ////var mappings = new Dictionary<string, string>
        ////{
        ////    ["English"] = "XEnglish",
        ////    ["U/H/I"] = "XUR_HN_IT",

        ////    ["ECO"] = "XECO",
        ////    ["BK"] = "XBK",
        ////    ["OC"] = "XOC",
        ////    ["SP/Maths"] = "XSP_MATHS",


        ////    ////["PHYSICS"] = "XPHYSICS",
        ////    ////["CHEMISTRY"] = "XCHEMISTRY",
        ////    ////["BIOLOGY"] = "XBIOLOGY",
        ////    ////["MATHS / GEO"] = "XMATHS_GEO",



        ////    ["Total"] = "XTOTAL",
        ////    ["Percentage"] = "XPercentage",
        ////    ["Remarks"] = "XRemarks",
        ////    ["Date of Birth"] = "XDateOfBirth",
        ////    ["Attendance"] = "XATTENDANVE",
        ////    ["PT"] = "XPT",
        ////    ["EVS"] = "XEVS",

        ////    ["NAME OF THE STUDENTS"] = "XStudentName",
        ////    ["ROLL NO"] = "XRollNo",
        ////    ["G R NO"] = "XGRNO",
        ////};

        ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

        using var sourcePackage = new ExcelPackage(new FileInfo(sourceFile));
        var sourceSheet = sourcePackage.Workbook.Worksheets[0];

        int row = 2;

        while (!string.IsNullOrEmpty(sourceSheet.Cells[row, 1].Text))
        {
            var studentData = new Dictionary<string, (string main, string superscript)>();

            foreach (var map in mappings)
            {
                var (mainColName, supColName, destinationName) = map.Value;

                int mainColIndex = FindColumnIndex(sourceSheet, mainColName);
                string mainValue = mainColIndex > 0 ? sourceSheet.Cells[row, mainColIndex].Text : "";

                string supValue = "";
                if (!string.IsNullOrEmpty(supColName))
                {
                    int supColIndex = FindColumnIndex(sourceSheet, supColName);
                    if (supColIndex > 0)
                    {
                        supValue = sourceSheet.Cells[row, supColIndex].Text;
                    }
                }

                studentData[destinationName] = (mainValue, supValue);
            }

            var rollNo = studentData["XRollNo"].main;
            var name = studentData["XStudentName"].main;
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

                foreach (var data in studentData)
                {
                    var destinationName = data.Key;
                    var (mainValue, supValue) = data.Value;

                    var namedRange = workbook.Names[destinationName];
                    if (namedRange != null)
                    {
                        var ws = namedRange.Worksheet;
                        var address = new OfficeOpenXml.ExcelAddress(namedRange.Address);

                        var cell = ws.Cells[address.Start.Row, address.Start.Column];

                        cell.Value = null;

                        if (!string.IsNullOrEmpty(mainValue))
                        {
                            if (!string.IsNullOrEmpty(supValue))
                            {
                                var richText = cell.RichText;

                                richText.Add(mainValue);

                                var supPart = richText.Add($"+{supValue}");
                                supPart.VerticalAlign = OfficeOpenXml.Style.ExcelVerticalAlignmentFont.Superscript;
                                supPart.Color = System.Drawing.Color.Red;
                            }
                            else
                            {
                                cell.Value = mainValue;
                            }
                        }
                    }
                }
                package.Save();
            }

            row++;
        }



        ////while (!string.IsNullOrEmpty(sourceSheet.Cells[row, 1].Text))
        ////{
        ////    var studentData = new Dictionary<string, string>();
        ////    foreach (var kvp in mappings)
        ////    {
        ////        int colIndex = FindColumnIndex(sourceSheet, kvp.Key);
        ////        if (colIndex > 0)
        ////        {
        ////            studentData[kvp.Value] = sourceSheet.Cells[row, colIndex].Text;
        ////        }
        ////    }



        ////    var rollNo = studentData["XRollNo"];
        ////    var name = studentData["XStudentName"];
        ////    string newFileName = Path.Combine(outputDir, $"{rollNo}_{name.Replace(" ", "_")}.xlsx");

        ////    try
        ////    {
        ////        System.IO.File.Copy(templateFile, newFileName, overwrite: true);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, $"Error copying file: {ex.Message}");
        ////    }

        ////    using var templatePackage = new ExcelPackage(new FileInfo(newFileName));
        ////    var templateSheet = templatePackage.Workbook.Worksheets["Sheet1"];

        ////    try
        ////    {
        ////        templatePackage.Save();
        ////        templatePackage.Dispose();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, $"Error saving the template file to initialize named cells: {ex.Message}");
        ////    }

        ////    using (var package = new ExcelPackage(new FileInfo(newFileName)))
        ////    {
        ////        var workbook = package.Workbook;
        ////        var worksheet = workbook.Worksheets["Sheet1"];
        ////        foreach (var cellMap in studentData)
        ////        {
        ////            try
        ////            {
        ////                var namedRange = workbook.Names[cellMap.Key];
        ////                if (namedRange != null)
        ////                {
        ////                    var value = cellMap.Value;

        ////                    var cell = namedRange.Worksheet.Cells[namedRange.Address];

        ////                    // === Superscript Handling Start ===
        ////                    if (!string.IsNullOrEmpty(value) && value.Contains("^"))
        ////                    {
        ////                        var parts = value.Split('^');
        ////                        var normalText = parts[0];
        ////                        var superscriptText = parts.Length > 1 ? parts[1] : "";

        ////                        cell.RichText.Clear();

        ////                        var normal = cell.RichText.Add(normalText);
        ////                        var superscript = cell.RichText.Add(superscriptText);
        ////                        superscript.VerticalAlign = OfficeOpenXml.Style.ExcelVerticalAlignmentFont.Superscript;
        ////                    }
        ////                    else
        ////                    {
        ////                        namedRange.Value = value;
        ////                    }
        ////                    // === Superscript Handling End ===
        ////                }
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                Console.WriteLine($"Error setting value for {cellMap.Key}: {ex.Message}");
        ////            }
        ////        }

        ////        package.Save();
        ////    }
        ////    row++;
        ////}











        ////string excelFile = @"C:\Users\Admin\Documents\Shaheen\1_KHAN_SHIFA_MOHAMMED_KALAM_ABEDA_.xlsx";
        ////string pdfOutput = @"C:\Users\Admin\Documents\Shaheen\CommerceReportCard.pdf";
        ////List<string> excelFiles = new List<string>
        ////{
        ////    @"C:\Users\Admin\Documents\Shaheen\1_KHAN_SHIFA_MOHAMMED_KALAM_ABEDA_.xlsx",
        ////    @"C:\Users\Admin\Documents\Shaheen\2_KHAN_MARIYAM_IMRAN_SALMA.xlsx",
        ////    @"C:\Users\Admin\Documents\Shaheen\3_KHAN_TUBA_HANIF_SHAMIM_BANO.xlsx"
        ////};
        ////ExcelToPdf.MergeExcelsToSinglePdf_FreeWay(excelFiles, pdfOutput);

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


