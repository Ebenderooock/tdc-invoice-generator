/*// Copyright 2024 - Eben de Roock
// Created: 2024${File.CreatedMonth}${File.CreatedDay} @21:6

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using InvoiceGenerator.Core.Models;

namespace InvoiceGenerator.Core.Helper
{
    public static class InvoiceDownloader
    {
        public static byte[] DownloadInvoiceExcel(Invoice invoice, ApplicationDbContext context, string templatePath)
        {
            Branch branch = context.Branches.FirstOrDefault(b =>
                b.Code.Equals(invoice.BranchCode, StringComparison.InvariantCultureIgnoreCase));

            List<InvoiceItem> invoiceItems = invoice.InvoiceItems
                .Where(i => !i.IsDeleted)
                .OrderBy(d => d.Order)
                .ToList();

            DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

            FileInfo template = new FileInfo(templatePath);

            if (template.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(template))
                {
                    ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Copy("MasterSheet", "Sheet1");

                    // Fill invoice data
                    SetInvoiceData(sheet, invoice, branch);

                    int row = 28;
                    int sheetCount = 1;
                    int pageCount = 1;

                    // Fill invoice items
                    foreach (InvoiceItem item in invoiceItems)
                    {
                        if (row >= 46)
                        {
                            sheetCount++;
                            pageCount++;
                            sheet = excelPackage.Workbook.Worksheets.Copy("MasterSheet", $"Sheet{sheetCount}");
                            row = 28;
                            SetInvoiceData(sheet, invoice, branch);
                        }

                        SetCellValue(sheet, $"E{row}", $"{item.Product.Name} | {item.Product.Code}");
                        SetCellValue(sheet, $"A{row}", item.Quantity);
                        SetCellValue(sheet, $"B{row}", item.UnitSize?.ToString("0.###"), "");
                        SetCellValue(sheet, $"D{row}", item.TotalKg?.ToString("0.###"), "");
                        SetCellValue(sheet, $"K{row}", item.Pallets);

                        ProcessBatchNumbers(excelPackage, sheet, row, item.BatchNumbers, invoice, branch, ref sheetCount);

                        row++;
                    }

                    // Delete the MasterSheet
                    excelPackage.Workbook.Worksheets.Delete("MasterSheet");

                    // Update page count in each worksheet
                    UpdatePageCount(excelPackage, pageCount);

                    // Set active cell in the first worksheet
                    excelPackage.Workbook.Worksheets[0].View.ActiveCell = "I23";

                    // Download the Excel file
                    return excelPackage.GetAsByteArray();
                }
            }

            return null;
        }

        private static void SetCellValue(ExcelWorksheet sheet, string cell, object value, string defaultValue = null)
        {
            sheet.Cells[cell].Value = value ?? defaultValue;
        }

        private static void SetInvoiceData(ExcelWorksheet sheet, Invoice invoice, Branch branch)
        {
            SetCellValue(sheet, "I8", invoice.GeneralWaybillNumber + invoice.BranchCode);
            SetCellValue(sheet, "I10", invoice.PoNumber);
            SetCellValue(sheet, "I12", invoice.InvoiceDate.Date.ToString("dd/MM/yyyy"));

            SetCellValue(sheet, "D14", invoice.Client.Name);
            SetCellValue(sheet, "D20", invoice.Client.Address, "None Provided");

            if (!string.IsNullOrEmpty(invoice.TransporterId))
            {
                string transporterPoNumber = string.IsNullOrWhiteSpace(invoice.TransporterPoNumber) ? "" : $" ({invoice.TransporterPoNumber})";
                SetCellValue(sheet, "D23", $"{invoice.Transporter.Name}{transporterPoNumber}");
                SetCellValue(sheet, "D25", invoice.Transporter.ContactPerson);
                SetCellValue(sheet, "I25", invoice.Transporter.ContactNumber);
            }

            SetCellValue(sheet, "D16", invoice.Client.ContactPersonName, "None Provided");
            SetCellValue(sheet, "D18", invoice.Client.ContactPersonPhoneNumber, "None Provided");

            if (branch != null)
            {
                SetCellValue(sheet, "I16", branch.ContactPerson);
                SetCellValue(sheet, "I18", branch.ContactNumber);
                SetCellValue(sheet, "I20", branch.Address);
            }
        }

        private static ExcelPackage ProcessBatchNumbers(ExcelPackage excelPackage, ExcelWorksheet sheet, int row, string batchNumbers, Invoice invoice, Branch branch,
            ref int sheetCount)
        {
                if (batchNumbers == null) return excelPackage;

                int maxLengthPerRow = 21;

                while (batchNumbers.Length > maxLengthPerRow)
                {
                    if (row >= 45)
                    {
                        sheet = excelPackage.Workbook.Worksheets.Copy("MasterSheet", $"Sheet{++sheetCount}");
                        row = 28;
                        SetInvoiceData(sheet, invoice, branch);
                    }

                    string substring = batchNumbers.Substring(0, maxLengthPerRow).Trim();
                    SetCellValue(sheet, $"J{row}", substring);
                    sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                    row++;

                    batchNumbers = batchNumbers.Substring(maxLengthPerRow);
                }

                if (row >= 45)
                {
                    sheet = excelPackage.Workbook.Worksheets.Copy("MasterSheet", $"Sheet{++sheetCount}");
                    row = 28;
                    SetInvoiceData(sheet, invoice, branch);
                }

                if (!string.IsNullOrWhiteSpace(batchNumbers))
                {
                    SetCellValue(sheet, $"J{row}", batchNumbers.Trim());
                    sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                    row++;
                }

                return excelPackage;
        }


        private static void UpdatePageCount(ExcelPackage ep, int pageCount)
        {
            int worksheetCount = 1;
            ExcelWorksheet dispatchSheet = ep.Workbook.Worksheets["Dispatch Picking Slip Pg1"];

            foreach (ExcelWorksheet worksheet in ep.Workbook.Worksheets)
            {
                if (worksheet.Name != "Dispatch Picking Slip Pg1" && worksheet.Name != "Dispatch Picking Slip Pg2")
                {
                    worksheet.Cells["K6"].Value = $"Page {worksheetCount} of {pageCount}";
                    worksheetCount++;
                    ep.Workbook.Worksheets.MoveBefore(worksheet.Index, dispatchSheet.Index);
                }
            }
        }
    }
}*/