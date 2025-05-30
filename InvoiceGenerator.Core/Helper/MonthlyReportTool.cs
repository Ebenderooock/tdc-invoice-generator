using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using InvoiceGenerator.Core.Enumerables;
using InvoiceGenerator.Core.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace InvoiceGenerator.Core.Helper
{
    public class MonthlyReportTool
    {
        private readonly ApplicationDbContext _context;

        private readonly Dictionary<MonthlyReportColumn, string> _columns = new  ()
        {
            { MonthlyReportColumn.Date, "A" },
            { MonthlyReportColumn.Day, "B" },
            { MonthlyReportColumn.Company, "C" },
            {MonthlyReportColumn.AccountNumber, "D" },
            { MonthlyReportColumn.Poa, "E" },
            { MonthlyReportColumn.Delivery, "F" },
            { MonthlyReportColumn.Transporter, "G" },
            { MonthlyReportColumn.TransporterPoNumber, "H" },
            { MonthlyReportColumn.Product, "I" },
            { MonthlyReportColumn.Quantity, "J" },
            { MonthlyReportColumn.UnitSize, "K" },
            { MonthlyReportColumn.TotalKg, "L" },
            { MonthlyReportColumn.Pallets, "M" },
            { MonthlyReportColumn.BatchNumber, "N" },
        };


        public MonthlyReportTool(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> FetchInvoiceData(DateTime minDate, DateTime maxDate)
        {
            List<Invoice> invoices = await _context.Invoices
                .Where(d => d.InvoiceDate >= minDate && d.InvoiceDate <= maxDate && !d.IsDeleted)
                .Include(d => d.Client)
                .Include(t => t.Transporter)
                .AsNoTracking()
                .OrderBy(d => d.InvoiceDate)
                .ToListAsync();


            List<string> invoiceIds = invoices.Select(i => i.Id).ToList();

            List<InvoiceItem> items = await _context.InvoiceItems
                .Where(i => invoiceIds.Contains(i.InvoiceId) && !i.IsDeleted)
                .Include(i => i.Product)
                .AsNoTracking()
                .OrderBy(i => i.Order)
                .ToListAsync();

            Dictionary<string, List<InvoiceItem>> itemGroups = items.GroupBy(i => i.InvoiceId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (Invoice invoice in invoices)
            {
                invoice.InvoiceItems = itemGroups.TryGetValue(invoice.Id, out List<InvoiceItem> grouped) ? grouped : new List<InvoiceItem>();
            }

            return invoices;
        }

        public void PopulateExcelRow(ExcelWorksheet sheet, int row, Invoice invoice, InvoiceItem item)
        {
            ExcelRange cells = sheet.Cells;

            cells[GetColumn(MonthlyReportColumn.Date, row)].Value = invoice.InvoiceDate.ToString("dd/MM/yyyy");
            cells[GetColumn(MonthlyReportColumn.Day, row)].Value = invoice.InvoiceDate.DayOfWeek.ToString();
            cells[GetColumn(MonthlyReportColumn.Day, row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cells[GetColumn(MonthlyReportColumn.Company, row)].Value = invoice.Client.Name;
            cells[GetColumn(MonthlyReportColumn.AccountNumber, row)].Value = invoice.Client.AccountNumber ?? string.Empty;
            cells[GetColumn(MonthlyReportColumn.Poa, row)].Value = invoice.PoNumber;
            cells[GetColumn(MonthlyReportColumn.Delivery, row)].Value = invoice.BranchCode + invoice.GeneralWaybillNumber;
            cells[GetColumn(MonthlyReportColumn.Delivery, row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cells[GetColumn(MonthlyReportColumn.Transporter, row)].Value = invoice.Transporter?.Name ?? string.Empty;
            cells[GetColumn(MonthlyReportColumn.TransporterPoNumber, row)].Value = invoice.TransporterPoNumber ?? string.Empty;
            cells[GetColumn(MonthlyReportColumn.Product, row)].Value = $"{item.Product?.Name} | {item.Product?.Code}";
            cells[GetColumn(MonthlyReportColumn.Quantity, row)].Value = item.Quantity;
            cells[GetColumn(MonthlyReportColumn.UnitSize, row)].Value = item.UnitSize;
            cells[GetColumn(MonthlyReportColumn.TotalKg, row)].Value = item.TotalKg;
            cells[GetColumn(MonthlyReportColumn.Pallets, row)].Value = item.Pallets;
            cells[GetColumn(MonthlyReportColumn.Pallets, row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            if (string.IsNullOrWhiteSpace(item.BatchNumbers))
            {
                return;
            }

            cells[GetColumn(MonthlyReportColumn.BatchNumber, row)].Value = item.BatchNumbers;
            cells[GetColumn(MonthlyReportColumn.BatchNumber, row)].Style.WrapText = true;
        }

        private string GetColumn(MonthlyReportColumn column, int row)
        {
            return _columns[column] + row;
        }

        public void AddPaginationHeader(ExcelPackage ep, int totalPages)
        {
            int count = 1;
            foreach (ExcelWorksheet sheet in ep.Workbook.Worksheets)
            {
                sheet.Cells["K5"].Value = $"Page {count} of {totalPages}";
                count++;
            }
        }
    }
}