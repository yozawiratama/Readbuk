using System;
namespace Application.DTOs.Borrowing
{
    public class BorrowingReturnResponse
    {
        public Guid ID { get; set; }
        public string BookTitle { get; set; }
        public string BookCode { get; set; }
        public string BookVariantCode { get; set; }
        public decimal LateCharge { get; set; }
    }
}

