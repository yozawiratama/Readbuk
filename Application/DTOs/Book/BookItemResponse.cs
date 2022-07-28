using System;
using Application.DTOs.Author;

namespace Application.DTOs.Book
{
    public class BookItemResponse
    {
        public Guid BookID { get; set; }
        public Guid VariantID { get; set; }
        public Guid PublisherID { get; set; }
        public Guid CategoryID { get; set; }
        public string BookCode { get; set; }
        public string VariantCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int PublishYear { get; set; }
        public IEnumerable<AuthorItemResponse> Authors { get; set; }
        public string PublisherName { get; set; }
        public string CategoryName { get; set; }
    }
}

