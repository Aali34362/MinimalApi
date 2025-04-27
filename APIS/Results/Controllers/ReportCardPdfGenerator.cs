using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Results.Controllers;

//// QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

public class ReportCardPdfGenerator
{
    public static void GenerateExactReportCard(string excelFilePath, string outputPdfPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        using (var excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // Header Section
                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text(worksheet.Cells["A1"].Text)
                            .FontSize(14).Bold();

                        col.Item().AlignCenter().Text(worksheet.Cells["A2"].Text)
                            .FontSize(12);

                        col.Item().Height(10);

                        col.Item().AlignCenter().Text("ANNUAL REPORT")
                            .FontSize(16).Bold().Underline();
                    });

                    // Main Content - Using minimal spacing to fit content
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        // Student Information Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120); // STUDENT NAME
                                columns.ConstantColumn(60);  // CLASS
                                columns.ConstantColumn(50);  // GR.NO
                                columns.ConstantColumn(50);  // ROLL.NO
                                columns.ConstantColumn(80);  // D.O.B
                            });

                            // Header Row
                            table.Cell().BorderBottom(1).PaddingBottom(3).Text("STUDENT NAME").Bold();
                            table.Cell().BorderBottom(1).PaddingBottom(3).Text("CLASS").Bold();
                            table.Cell().BorderBottom(1).PaddingBottom(3).Text("GR.NO").Bold();
                            table.Cell().BorderBottom(1).PaddingBottom(3).Text("ROLL.NO").Bold();
                            table.Cell().BorderBottom(1).PaddingBottom(3).Text("D.O.B").Bold();

                            // Data Row
                            table.Cell().PaddingVertical(3).Text(worksheet.Cells["A6"].Text);
                            table.Cell().PaddingVertical(3).Text(worksheet.Cells["K6"].Text);
                            table.Cell().PaddingVertical(3).Text(worksheet.Cells["L6"].Text);
                            table.Cell().PaddingVertical(3).Text(worksheet.Cells["M6"].Text);
                            table.Cell().PaddingVertical(3).Text(worksheet.Cells["N6"].Text);
                        });

                        // Marks Table - Using smaller font and tighter spacing
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(90);  // Label column
                                columns.ConstantColumn(50);   // ENGLISH
                                columns.ConstantColumn(60);   // URDU/HINDI/IT
                                columns.ConstantColumn(60);   // ECONOMICS
                                columns.ConstantColumn(60);   // BK & A/C
                                columns.ConstantColumn(50);   // OC & M
                                columns.ConstantColumn(60);   // SP/MATHS
                                columns.ConstantColumn(30);   // PT
                                columns.ConstantColumn(30);   // EVS
                                columns.ConstantColumn(70);  // GRAND TOTAL
                                columns.ConstantColumn(60);  // PERCENTAGE
                                columns.ConstantColumn(90);  // PASSED/CONDONED/DETAINED
                            });

                            // Header Rows
                            table.Cell().ColumnSpan(12).Text("SUBJECT NAME").Bold().FontSize(9);

                            var headerCells = new[] { "", "ENGLISH", "URDU/HINDI/IT", "ECONOMICS",
                                                    "BK & A/C", "OC & M", "SP/MATHS", "PT", "EVS",
                                                    "GRAND TOTAL", "PERCENTAGE", "PASSED/CONDONED/DETAINED" };

                            foreach (var header in headerCells)
                            {
                                table.Cell().Text(header).Bold().FontSize(9);
                            }

                            // Data Rows with smaller font
                            AddCompactMarksRow(table, "MAXIMUM MARKS", worksheet, 11);
                            AddCompactMarksRow(table, "MINIMUM MARKS", worksheet, 12);
                            AddCompactMarksRow(table, "MARKS OBTAINED", worksheet, 13);
                        });

                        // Remarks Section - Compact version
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(200);
                            });

                            table.Cell().Text("TEACHER'S REMARK:").Bold().FontSize(9);
                            table.Cell().Text(worksheet.Cells["A18"].Text).FontSize(9);

                            table.Cell().Text("ATTENDANCE:").Bold().FontSize(9);
                            table.Cell().Text(worksheet.Cells["H18"].Text).FontSize(9);

                            table.Cell().Text("SCHOOL RE-OPENS ON:").Bold().FontSize(9);
                            table.Cell().Text(worksheet.Cells["M18"].Text).FontSize(9);
                        });

                        // Signatures Section - Compact version
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120);
                                columns.ConstantColumn(120);
                                columns.ConstantColumn(120);
                            });

                            table.Cell().Text("CLASS TEACHER'S SIGN:").Bold().FontSize(9);
                            table.Cell().Text("COLLEGE STAMP:").Bold().FontSize(9).AlignCenter();
                            table.Cell().Text("PRINCIPAL'S SIGN:").Bold().FontSize(9).AlignRight();

                            table.Cell().Text(worksheet.Cells["A24"].Text).FontSize(9);
                            table.Cell().Text("").FontSize(9);
                            table.Cell().Text(worksheet.Cells["J24"].Text).FontSize(9).AlignRight();
                        });

                        // Grade Key Section - Compact version
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(400);
                            });

                            table.Cell().Text("GRADE KEY").Bold().FontSize(9);

                            for (int row = 26; row <= 28; row++)
                            {
                                table.Cell().Text(worksheet.Cells[$"A{row}"].Text).FontSize(8);
                            }
                        });
                    });
                });
            }).GeneratePdf(outputPdfPath);
        }
    }

    private static void AddCompactMarksRow(TableDescriptor table, string label, ExcelWorksheet worksheet, int row)
    {
        table.Cell().Text(label).Bold().FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 3].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 4].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 5].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 6].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 7].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 8].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 9].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 10].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 11].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 12].Text).FontSize(9);
        table.Cell().Text(worksheet.Cells[row, 13].Text).FontSize(9);
    }

    ////public static void GenerateExactReportCard(string excelFilePath, string outputPdfPath)
    ////{
    ////    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

    ////    using (var excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
    ////    {
    ////        var worksheet = excelPackage.Workbook.Worksheets[0];

    ////        Document.Create(container =>
    ////        {
    ////            container.Page(page =>
    ////            {
    ////                page.Size(PageSizes.A4);
    ////                page.Margin(1, Unit.Centimetre);
    ////                page.DefaultTextStyle(x => x.FontSize(10));

    ////                // Header Section
    ////                page.Header().Column(col =>
    ////                {
    ////                    col.Item().AlignCenter().Text(worksheet.Cells["A1"].Text)
    ////                        .FontSize(14).Bold();

    ////                    col.Item().AlignCenter().Text(worksheet.Cells["A2"].Text)
    ////                        .FontSize(12);

    ////                    col.Item().Height(15);

    ////                    col.Item().AlignCenter().Text("ANNUAL REPORT")
    ////                        .FontSize(16).Bold().Underline();
    ////                });

    ////                // Main Content
    ////                page.Content().Column(col =>
    ////                {
    ////                    // Student Information Table
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        // Proper column definition with explicit counts
    ////                        table.ColumnsDefinition(columns =>
    ////                        {
    ////                            columns.ConstantColumn(100); // STUDENT NAME
    ////                            columns.ConstantColumn(60);  // CLASS
    ////                            columns.ConstantColumn(50);  // GR.NO
    ////                            columns.ConstantColumn(50);  // ROLL.NO
    ////                            columns.ConstantColumn(80);  // D.O.B
    ////                        });

    ////                        // Header Row
    ////                        table.Cell().BorderBottom(1).Text("STUDENT NAME").Bold();
    ////                        table.Cell().BorderBottom(1).Text("CLASS").Bold();
    ////                        table.Cell().BorderBottom(1).Text("GR.NO").Bold();
    ////                        table.Cell().BorderBottom(1).Text("ROLL.NO").Bold();
    ////                        table.Cell().BorderBottom(1).Text("D.O.B").Bold();

    ////                        // Data Row
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["A6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["K6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["L6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["M6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["N6"].Text);
    ////                    });

    ////                    col.Item().Height(20);

    ////                    // Marks Table - Simplified to avoid column issues
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        // Define exact column widths
    ////                        table.ColumnsDefinition(columns =>
    ////                        {
    ////                            columns.ConstantColumn(120);  // SUBJECT NAME
    ////                            columns.ConstantColumn(60);   // ENGLISH
    ////                            columns.ConstantColumn(80);   // URDU/HINDI/IT
    ////                            columns.ConstantColumn(70);   // ECONOMICS
    ////                            columns.ConstantColumn(70);   // BK & A/C
    ////                            columns.ConstantColumn(60);   // OC & M
    ////                            columns.ConstantColumn(70);   // SP/MATHS
    ////                            columns.ConstantColumn(40);   // PT
    ////                            columns.ConstantColumn(40);   // EVS
    ////                            columns.ConstantColumn(80);  // GRAND TOTAL
    ////                            columns.ConstantColumn(70);  // PERCENTAGE
    ////                            columns.ConstantColumn(100); // PASSED/CONDONED/DETAINED
    ////                        });

    ////                        // Header Row
    ////                        table.Cell().ColumnSpan(12).Text("SUBJECT NAME").Bold();

    ////                        // Subject Names Row
    ////                        table.Cell().Text("").Bold(); // Empty first cell
    ////                        table.Cell().Text("ENGLISH").Bold();
    ////                        table.Cell().Text("URDU/HINDI/IT").Bold();
    ////                        table.Cell().Text("ECONOMICS").Bold();
    ////                        table.Cell().Text("BK & A/C").Bold();
    ////                        table.Cell().Text("OC & M").Bold();
    ////                        table.Cell().Text("SP/MATHS").Bold();
    ////                        table.Cell().Text("PT").Bold();
    ////                        table.Cell().Text("EVS").Bold();
    ////                        table.Cell().Text("GRAND TOTAL").Bold();
    ////                        table.Cell().Text("PERCENTAGE").Bold();
    ////                        table.Cell().Text("PASSED/CONDONED/DETAINED").Bold();

    ////                        // Data Rows
    ////                        AddMarksRow(table, "MAXIMUM MARKS", worksheet, 11);
    ////                        AddMarksRow(table, "MINIMUM MARKS", worksheet, 12);
    ////                        AddMarksRow(table, "MARKS OBTAINED", worksheet, 13);
    ////                    });

    ////                    // Rest of the content...
    ////                    AddRemarksSection(col, worksheet);
    ////                    AddSignaturesSection(col, worksheet);
    ////                    AddGradeKeySection(col, worksheet);
    ////                });
    ////            });
    ////        }).GeneratePdf(outputPdfPath);
    ////    }
    ////}

    ////private static void AddMarksRow(TableDescriptor table, string label, ExcelWorksheet worksheet, int row)
    ////{
    ////    table.Cell().Text(label).Bold();
    ////    table.Cell().Text(worksheet.Cells[row, 3].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 4].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 5].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 6].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 7].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 8].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 9].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 10].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 11].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 12].Text);
    ////    table.Cell().Text(worksheet.Cells[row, 13].Text);
    ////}

    ////private static void AddRemarksSection(ColumnDescriptor column, ExcelWorksheet worksheet)
    ////{
    ////    column.Item().Height(20);
    ////    column.Item().Table(table =>
    ////    {
    ////        table.ColumnsDefinition(columns =>
    ////        {
    ////            columns.ConstantColumn(120);
    ////            columns.ConstantColumn(200);
    ////            columns.ConstantColumn(100);
    ////        });

    ////        table.Cell().Text("TEACHER'S REMARK:").Bold();
    ////        table.Cell().Text(worksheet.Cells["A18"].Text);
    ////        table.Cell();

    ////        table.Cell().Text("ATTENDANCE:").Bold();
    ////        table.Cell().Text(worksheet.Cells["H18"].Text);
    ////        table.Cell();

    ////        table.Cell().Text("SCHOOL RE-OPENS ON:").Bold();
    ////        table.Cell().Text(worksheet.Cells["M18"].Text);
    ////        table.Cell();
    ////    });
    ////}

    ////private static void AddSignaturesSection(ColumnDescriptor column, ExcelWorksheet worksheet)
    ////{
    ////    column.Item().Height(30);
    ////    column.Item().Table(table =>
    ////    {
    ////        table.ColumnsDefinition(columns =>
    ////        {
    ////            columns.ConstantColumn(150);
    ////            columns.ConstantColumn(150);
    ////            columns.ConstantColumn(150);
    ////        });

    ////        table.Cell().Text("CLASS TEACHER'S SIGN:").Bold();
    ////        table.Cell().Text("COLLEGE STAMP:").Bold().AlignCenter();
    ////        table.Cell().Text("PRINCIPAL'S SIGN:").Bold().AlignRight();

    ////        table.Cell().Text(worksheet.Cells["A24"].Text);
    ////        table.Cell().Text(""); // Empty for stamp
    ////        table.Cell().Text(worksheet.Cells["J24"].Text).AlignRight();
    ////    });
    ////}

    ////private static void AddGradeKeySection(ColumnDescriptor column, ExcelWorksheet worksheet)
    ////{
    ////    column.Item().Height(20);
    ////    column.Item().Table(table =>
    ////    {
    ////        table.ColumnsDefinition(columns =>
    ////        {
    ////            columns.ConstantColumn(450);
    ////        });

    ////        table.Cell().Text("GRADE KEY").Bold();

    ////        for (int row = 26; row <= 28; row++)
    ////        {
    ////            table.Cell().Text(worksheet.Cells[$"A{row}"].Text);
    ////        }
    ////    });
    ////}



    ////public static void GenerateExactReportCard(string excelFilePath, string outputPdfPath)
    ////{
    ////    // Set license context
    ////    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

    ////    using (var excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
    ////    {
    ////        var worksheet = excelPackage.Workbook.Worksheets[0];

    ////        Document.Create(container =>
    ////        {
    ////            container.Page(page =>
    ////            {
    ////                page.Size(PageSizes.A4);
    ////                page.Margin(1, Unit.Centimetre);
    ////                page.DefaultTextStyle(x => x.FontSize(10));

    ////                // Header Section
    ////                page.Header().Column(col =>
    ////                {
    ////                    col.Item().AlignCenter().Text(worksheet.Cells["A1"].Text)
    ////                        .FontSize(14).Bold();

    ////                    col.Item().AlignCenter().Text(worksheet.Cells["A2"].Text)
    ////                        .FontSize(12);

    ////                    col.Item().Height(15);

    ////                    col.Item().AlignCenter().Text("ANNUAL REPORT")
    ////                        .FontSize(16).Bold().Underline();

    ////                    col.Item().Height(20);
    ////                });

    ////                // Main Content
    ////                page.Content().Column(col =>
    ////                {
    ////                    // Student Information Table
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        table.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(3); // STUDENT NAME
    ////                            cols.RelativeColumn(2); // CLASS
    ////                            cols.RelativeColumn(2); // GR.NO
    ////                            cols.RelativeColumn(2); // ROLL.NO
    ////                            cols.RelativeColumn(2); // D.O.B
    ////                        });

    ////                        // Header Row
    ////                        table.Cell().BorderBottom(1).Text("STUDENT NAME").Bold();
    ////                        table.Cell().BorderBottom(1).Text("CLASS").Bold();
    ////                        table.Cell().BorderBottom(1).Text("GR.NO").Bold();
    ////                        table.Cell().BorderBottom(1).Text("ROLL.NO").Bold();
    ////                        table.Cell().BorderBottom(1).Text("D.O.B").Bold();

    ////                        // Data Row
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["A6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["K6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["L6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["M6"].Text);
    ////                        table.Cell().PaddingVertical(5).Text(worksheet.Cells["N6"].Text);
    ////                    });

    ////                    col.Item().Height(20);

    ////                    // Marks Table
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        // Define columns (13 columns total)
    ////                        table.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(3); // SUBJECT NAME
    ////                            cols.RelativeColumn(); // ENGLISH
    ////                            cols.RelativeColumn(); // URDU/HINDI/IT
    ////                            cols.RelativeColumn(); // ECONOMICS
    ////                            cols.RelativeColumn(); // BK & A/C
    ////                            cols.RelativeColumn(); // OC & M
    ////                            cols.RelativeColumn(); // SP/MATHS
    ////                            cols.RelativeColumn(); // PT
    ////                            cols.RelativeColumn(); // EVS
    ////                            cols.RelativeColumn(2); // GRAND TOTAL
    ////                            cols.RelativeColumn(2); // PERCENTAGE
    ////                            cols.RelativeColumn(2); // PASSED/CONDONED/DETAINED
    ////                        });

    ////                        // Header Row
    ////                        table.Cell().ColumnSpan(13).BorderBottom(1)
    ////                            .Text("SUBJECT NAME").Bold();

    ////                        // Subject Names Row
    ////                        table.Cell(); // Empty first cell
    ////                        table.Cell().Text("ENGLISH").Bold();
    ////                        table.Cell().Text("URDU/HINDI/IT").Bold();
    ////                        table.Cell().Text("ECONOMICS").Bold();
    ////                        table.Cell().Text("BK & A/C").Bold();
    ////                        table.Cell().Text("OC & M").Bold();
    ////                        table.Cell().Text("SP/MATHS").Bold();
    ////                        table.Cell().Text("PT").Bold();
    ////                        table.Cell().Text("EVS").Bold();
    ////                        table.Cell().Text("GRAND TOTAL").Bold();
    ////                        table.Cell().Text("PERCENTAGE").Bold();
    ////                        table.Cell().Text("PASSED/CONDONED/DETAINED").Bold();

    ////                        // Maximum Marks Row
    ////                        table.Cell().Text("MAXIMUM MARKS").Bold();
    ////                        for (int col = 3; col <= 8; col++)
    ////                            table.Cell().Text(worksheet.Cells[11, col].Text);
    ////                        table.Cell().Text("GRADE").Bold();
    ////                        table.Cell().Text("GRADE").Bold();
    ////                        table.Cell().Text(worksheet.Cells["J11"].Text);
    ////                        table.Cell().Text(worksheet.Cells["K11"].Text);
    ////                        table.Cell().Text(worksheet.Cells["L11"].Text);

    ////                        // Minimum Marks Row
    ////                        table.Cell().Text("MINIMUM MARKS").Bold();
    ////                        for (int col = 3; col <= 8; col++)
    ////                            table.Cell().Text(worksheet.Cells[12, col].Text);
    ////                        table.Cell().ColumnSpan(5); // Empty cells

    ////                        // Marks Obtained Row
    ////                        table.Cell().Text("MARKS OBTAINED").Bold();
    ////                        for (int col = 3; col <= 8; col++)
    ////                            table.Cell().Text(worksheet.Cells[13, col].Text);
    ////                        table.Cell().Text(worksheet.Cells["I13"].Text);
    ////                        table.Cell().Text(worksheet.Cells["J13"].Text);
    ////                        table.Cell().Text(worksheet.Cells["K13"].Text);
    ////                        table.Cell().Text(worksheet.Cells["L13"].Text);
    ////                        table.Cell().Text(worksheet.Cells["M13"].Text);
    ////                    });

    ////                    col.Item().Height(20);

    ////                    // Remarks Section
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        table.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(4);
    ////                            cols.RelativeColumn(8);
    ////                            cols.RelativeColumn();
    ////                        });

    ////                        table.Cell().Text("TEACHER'S REMARK:").Bold();
    ////                        table.Cell().Text(worksheet.Cells["A18"].Text);
    ////                        table.Cell();

    ////                        table.Cell().Text("ATTENDANCE:").Bold();
    ////                        table.Cell().Text(worksheet.Cells["H18"].Text);
    ////                        table.Cell();

    ////                        table.Cell().Text("SCHOOL RE-OPENS ON:").Bold();
    ////                        table.Cell().Text(worksheet.Cells["M18"].Text);
    ////                        table.Cell();
    ////                    });

    ////                    col.Item().Height(30);

    ////                    // Signatures Section
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        table.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(4);
    ////                            cols.RelativeColumn(4);
    ////                            cols.RelativeColumn(4);
    ////                        });

    ////                        table.Cell().Text("CLASS TEACHER'S SIGN:").Bold();
    ////                        table.Cell().Text("COLLEGE STAMP:").Bold().AlignCenter();
    ////                        table.Cell().Text("PRINCIPAL'S SIGN:").Bold().AlignRight();

    ////                        table.Cell().Text(worksheet.Cells["A24"].Text);
    ////                        table.Cell(); // Empty for stamp
    ////                        table.Cell().Text(worksheet.Cells["J24"].Text).AlignRight();
    ////                    });

    ////                    col.Item().Height(20);

    ////                    // Grade Key Section
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        table.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(6);
    ////                            for (int i = 0; i < 6; i++)
    ////                                cols.RelativeColumn();
    ////                        });

    ////                        table.Cell().ColumnSpan(13).Text("GRADE KEY").Bold();

    ////                        // Grade definitions
    ////                        for (int row = 26; row <= 28; row++)
    ////                        {
    ////                            table.Cell().ColumnSpan(13).Text(worksheet.Cells[$"A{row}"].Text);
    ////                        }
    ////                    });
    ////                });
    ////            });
    ////        }).GeneratePdf(outputPdfPath);
    ////    }
    ////}

    ////public static void GeneratePdfFromExcel1(string excelFilePath, string outputPdfPath)
    ////{
    ////    // Enable EPPlus license (required for non-commercial use)
    ////    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

    ////    // Load Excel file
    ////    using (var excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
    ////    {
    ////        var worksheet = excelPackage.Workbook.Worksheets[0];

    ////        // Generate PDF using QuestPDF
    ////        Document.Create(container =>
    ////        {
    ////            container.Page(page =>
    ////            {
    ////                page.Size(PageSizes.A4);
    ////                page.Margin(30);
    ////                page.DefaultTextStyle(TextStyle.Default.FontSize(10));

    ////                // Header (School Name & Address)
    ////                page.Header().Column(col =>
    ////                {
    ////                    col.Item().Text(worksheet.Cells["A1"].Text)
    ////                        .FontSize(14).Bold().AlignCenter();

    ////                    col.Item().Text(worksheet.Cells["A2"].Text)
    ////                        .FontSize(12).AlignCenter();

    ////                    col.Item().Text("ANNUAL REPORT")
    ////                        .FontSize(16).Bold().AlignCenter();
    ////                });

    ////                // Student Information Section
    ////                page.Content().Column(col =>
    ////                {
    ////                    col.Spacing(10);

    ////                    // Student Info Table
    ////                    col.Item().Table(table =>
    ////                    {
    ////                        table.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(); // Label
    ////                            cols.RelativeColumn(3); // Value
    ////                        });

    ////                        AddTableRow(table, "Student Name:", worksheet.Cells["A6"].Text);
    ////                        AddTableRow(table, "Class:", worksheet.Cells["K6"].Text);
    ////                        AddTableRow(table, "GR No:", worksheet.Cells["L6"].Text);
    ////                        AddTableRow(table, "Roll No:", worksheet.Cells["M6"].Text);
    ////                        AddTableRow(table, "Date of Birth:", worksheet.Cells["N6"].Text);
    ////                    });

    ////                    // Marks Table
    ////                    col.Item().Text("ACADEMIC PERFORMANCE").Bold().FontSize(14);
    ////                    col.Item().Table(marksTable =>
    ////                    {
    ////                        marksTable.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(); // Subject
    ////                            cols.RelativeColumn(); // English
    ////                            cols.RelativeColumn(); // Hindi/Urdu
    ////                            cols.RelativeColumn(); // Economics
    ////                            cols.RelativeColumn(); // BK & A/C
    ////                            cols.RelativeColumn(); // OC & M
    ////                            cols.RelativeColumn(); // SP/Maths
    ////                            cols.RelativeColumn(); // Total
    ////                        });

    ////                        // Headers
    ////                        AddMarksHeader(marksTable, "Subject");
    ////                        AddMarksHeader(marksTable, "English");
    ////                        AddMarksHeader(marksTable, "Urdu/Hindi/IT");
    ////                        AddMarksHeader(marksTable, "Economics");
    ////                        AddMarksHeader(marksTable, "BK & A/C");
    ////                        AddMarksHeader(marksTable, "OC & M");
    ////                        AddMarksHeader(marksTable, "SP/Maths");
    ////                        AddMarksHeader(marksTable, "Total");

    ////                        // Data (assuming marks are in rows 13-17)
    ////                        for (int row = 13; row <= 17; row++)
    ////                        {
    ////                            var subject = worksheet.Cells[$"A{row}"].Text;
    ////                            if (!string.IsNullOrEmpty(subject))
    ////                            {
    ////                                AddMarksCell(marksTable, subject);
    ////                                for (int col = 3; col <= 8; col++)
    ////                                    AddMarksCell(marksTable, worksheet.Cells[row, col].Text);
    ////                                AddMarksCell(marksTable, worksheet.Cells[$"J{row}"].Text);
    ////                            }
    ////                        }
    ////                    });

    ////                    // Footer (Remarks, Signatures)
    ////                    col.Item().Text("REMARKS").Bold().FontSize(14);
    ////                    col.Item().Table(footerTable =>
    ////                    {
    ////                        footerTable.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn(2); // Label
    ////                            cols.RelativeColumn(5); // Value
    ////                        });

    ////                        AddTableRow(footerTable, "Teacher's Remarks:", worksheet.Cells["A18"].Text);
    ////                        AddTableRow(footerTable, "Attendance:", worksheet.Cells["G18"].Text);
    ////                        AddTableRow(footerTable, "School Reopens On:", worksheet.Cells["K18"].Text);
    ////                    });

    ////                    // Signatures
    ////                    col.Item().Table(signTable =>
    ////                    {
    ////                        signTable.ColumnsDefinition(cols =>
    ////                        {
    ////                            cols.RelativeColumn();
    ////                            cols.RelativeColumn();
    ////                            cols.RelativeColumn();
    ////                        });

    ////                        signTable.Cell().Text("Class Teacher:").Bold();
    ////                        signTable.Cell().Text("College Stamp:").Bold().AlignCenter();
    ////                        signTable.Cell().Text("Principal:").Bold().AlignRight();

    ////                        signTable.Cell().Text(worksheet.Cells["A24"].Text);
    ////                        signTable.Cell().Text("");
    ////                        signTable.Cell().Text(worksheet.Cells["J24"].Text).AlignRight();
    ////                    });

    ////                    // Grade Key
    ////                    col.Item().Text("GRADE KEY").Bold().FontSize(14);
    ////                    for (int row = 26; row <= 28; row++)
    ////                    {
    ////                        col.Item().Text(worksheet.Cells[$"A{row}"].Text);
    ////                    }
    ////                });
    ////            });
    ////        }).GeneratePdf(outputPdfPath);
    ////    }
    ////}

    ////private static void AddTableRow(TableDescriptor table, string label, string value)
    ////{
    ////    table.Cell().Text(label).Bold();
    ////    table.Cell().Text(value);
    ////}

    ////private static void AddMarksHeader(TableDescriptor table, string text)
    ////{
    ////    table.Cell().Background(Colors.Grey.Lighten2).Text(text).Bold();
    ////}

    ////private static void AddMarksCell(TableDescriptor table, string text)
    ////{
    ////    table.Cell().Border(1).Padding(5).Text(text);
    ////}
}
