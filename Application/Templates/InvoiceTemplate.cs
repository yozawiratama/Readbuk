using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Templates
{
    public static class InvoiceTemplate
    {
        public static string GetHTMLString()
        {
            var data = new[]
            {
                new { Name= "Pepsodent", Price = 12500, Amount = 3000 },
                new { Name= "Lifebuoy", Price = 28000, Amount = 1000 },
                new { Name= "Indomie", Price = 10000, Amount = 1000 },
            };
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Invoice #00912</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <th>Price</th>
                                        <th>Amount</th>
                                        <th>Total</th>
                                    </tr>");
            foreach (var emp in data)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", emp.Name, emp.Price, emp.Amount, emp.Price*emp.Amount);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
