using System;
using Application.DTOs.Author;
using Application.DTOs.Book;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Queries
{
    public class GetAllBooksByAuthorNameQuery : IRequest<IEnumerable<BookItemResponse>>
    {
        public string Search { get; set; }
        public class QueryHandler : IRequestHandler<GetAllBooksByAuthorNameQuery, IEnumerable<BookItemResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public QueryHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<IEnumerable<BookItemResponse>> Handle(GetAllBooksByAuthorNameQuery query, CancellationToken cancellationToken)
            {
                var authorIDs = await _context.Authors.Where(w => w.Name.Contains(query.Search.Trim())).Select(s => s.ID).ToListAsync();
                var bookIDs = await _context.BookAuthors
                    .Include(i => i.Author)
                    .Include(i => i.Book)
                    .Where(w => authorIDs
                    .Contains(w.Author.ID))
                    .Select(s => s.Book.ID).ToListAsync();
                var book = await _context.Books
                    .Where(w => bookIDs.Contains(w.ID))
                    .Include(book => book.Publisher)
                    .Include(book => book.Variants)
                    .Include(book => book.Category)
                    .Include(book => book.Authors)
                        .ThenInclude(author => author.Author)
                    .ToListAsync();
                if (book == null)
                {
                    return null;
                }
                List<BookItemResponse> books = new List<BookItemResponse>();
                book.ForEach(b =>
                {
                    b.Variants.ToList().ForEach(v =>
                    {
                        books.Add(new BookItemResponse
                        {
                            BookID = b.ID,
                            VariantID = v.ID,
                            CategoryID = b.Category.ID,
                            PublisherID = b.Publisher.ID,
                            BookCode = b.Code,
                            VariantCode = v.Code,
                            Title = b.Title,
                            Description = b.Description,
                            Price = b.Price,
                            PublishYear = b.PublishYear,
                            PublisherName = b.Publisher.Name,
                            CategoryName = b.Category.Name,
                            Authors = b.Authors.Select(s => new AuthorItemResponse
                            {
                                ID = s.ID,
                                Name = s.Author.Name
                            })
                        }); ;
                    });
                });
                return books;
            }
        }
    }
}

