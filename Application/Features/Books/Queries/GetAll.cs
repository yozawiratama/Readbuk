using System;
using Application.DTOs.Author;
using Application.DTOs.Book;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Queries
{
    public class GetAllBooksQuery : IRequest<IEnumerable<BookItemResponse>>
    {

        public class QueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookItemResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public QueryHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<IEnumerable<BookItemResponse>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
            {
                //var users = await _identityContext.Users.FirstOrDefaultAsync();
                var book = await _context.Books
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

