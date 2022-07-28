using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Borrowing;
using Application.Features.Borrowings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Readbuk.Admin.Attributes;
using Readbuk.Admin.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ClosedXML.Excel;
using System.Data;
using iText.Html2pdf;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Readbuk.Admin.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly ILogger<BorrowingController> _logger;

        public BorrowingController(ILogger<BorrowingController> logger)
        {
            _logger = logger;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("token"))) return RedirectToAction("Index", "Auth");
            List<BorrowingItemResponse> result = new List<BorrowingItemResponse>();
            string token = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token}");
            HttpResponseMessage response = await client.GetAsync("https://localhost:7090/api/v1/Borrowing");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<BorrowingItemResponse>>();
            }
            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> BookReturn([Bind("BookVariantID")] ReturnBookModel param)
        {
            string token = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token}");
            HttpResponseMessage response = await client.PostAsync($"https://localhost:7090/api/v1/Borrowing/{param.BookVariantID}", new StringContent("{\"id\": \""+param.BookVariantID+"\"}", Encoding.UTF8, "application/json"));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadExcel()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("token"))) return RedirectToAction("Index", "Auth");
            List<BorrowingItemResponse> result = new List<BorrowingItemResponse>();
            string token = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token}");
            HttpResponseMessage response = await client.GetAsync("https://localhost:7090/api/v1/Borrowing");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<BorrowingItemResponse>>();
            }
            var wb = new XLWorkbook();

            DataTable dataTable = new DataTable();
            dataTable.TableName = "Report";
            dataTable.Columns.Add("Judul", typeof(string));
            dataTable.Columns.Add("Code", typeof(string));
            dataTable.Columns.Add("Tgl Peminjaman", typeof(DateTime));
            dataTable.Columns.Add("Nama Peminjam", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));

            result.ForEach(it =>
            {
                dataTable.Rows.Add(it.BookTitle, $"{it.BookCode} - {it.BookVariantCode}", it.BorrowedAt, it.BorrowerName, it.Status);
            });


            wb.Worksheets.Add(dataTable);

            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                var content = stream.ToArray();
                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Report.xlsx");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("token"))) return RedirectToAction("Index", "Auth");
            List<BorrowingItemResponse> result = new List<BorrowingItemResponse>();
            string token = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token}");
            HttpResponseMessage response = await client.GetAsync("https://localhost:7090/api/v1/Borrowing");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<BorrowingItemResponse>>();
            }
            var wb = new XLWorkbook();

            DataTable dataTable = new DataTable();
            dataTable.TableName = "Report";
            dataTable.Columns.Add("Judul", typeof(string));
            dataTable.Columns.Add("Code", typeof(string));
            dataTable.Columns.Add("Tgl Peminjaman", typeof(DateTime));
            dataTable.Columns.Add("Nama Peminjam", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));

            result.ForEach(it =>
            {
                dataTable.Rows.Add(it.BookTitle, $"{it.BookCode} - {it.BookVariantCode}", it.BorrowedAt, it.BorrowerName, it.Status);
            });
            using (MemoryStream stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(ConvertDataTableToHTML(dataTable), stream);
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

    }
}

