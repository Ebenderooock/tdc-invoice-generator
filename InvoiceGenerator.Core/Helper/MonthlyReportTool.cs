using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using InvoiceGenerator.Core.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace InvoiceGenerator.Core.Helper;

public class MonthlyReportTool
{
    private readonly ApplicationDbContext _context;

    public MonthlyReportTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Invoice>> FetchInvoiceData(DateTime minDate, DateTime maxDate)
    {
        List<Invoice> invoices = await _context.Invoices
            .Where(d => d.InvoiceDate >= minDate && d.InvoiceDate <= maxDate && !d.IsDeleted)
            .Include(d => d.Client)
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

        cells[$"A{row}"].Value = invoice.InvoiceDate.ToString("dd/MM/yyyy");

        cells[$"B{row}"].Value = invoice.InvoiceDate.DayOfWeek.ToString();
        cells[$"B{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        cells[$"C{row}"].Value = invoice.Client.Name;
        cells[$"D{row}"].Value = invoice.PoNumber;

        cells[$"E{row}"].Value = invoice.BranchCode + invoice.GeneralWaybillNumber;
        cells[$"E{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        cells[$"F{row}"].Value = $"{item.Product?.Name} | {item.Product?.Code}";
        cells[$"G{row}"].Value = item.Quantity;
        cells[$"H{row}"].Value = item.UnitSize;
        cells[$"I{row}"].Value = item.TotalKg;
        cells[$"J{row}"].Value = item.Pallets;

        cells[$"G{row}:J{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        if (!string.IsNullOrWhiteSpace(item.BatchNumbers))
        {
            cells[$"K{row}"].Value = item.BatchNumbers;
            cells[$"K{row}"].Style.WrapText = true;
        }
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