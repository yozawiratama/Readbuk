using System;
namespace Application.DTOs.Borrowing
{
    public class BorrowingItemResponse
    {
        public Guid ID { get; set; }
        public string BookTitle { get; set; }
        public string BookCode { get; set; }
        public string BookVariantCode { get; set; }
        public Guid BorrowerID { get; set; }
        public string BorrowerName { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string Status { get; set; }
        public decimal? LateCharge { get; set; }
    }
}

