using System;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Commands
{
    public class AddAuthorBookCommand : IRequest<Guid>
    {
        public Guid BookID { get; set; }
        public Guid AuthorID { get; set; }
        public class CommandHandler : IRequestHandler<AddAuthorBookCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(AddAuthorBookCommand command, CancellationToken cancellationToken)
            {
                var book = _context.Books.Where(a => a.ID == command.BookID).FirstOrDefault();

                if (book == null)
                {
                    return default;
                }
                else
                {
                    var author = await _context.Authors.FirstOrDefaultAsync(w => w.ID == command.AuthorID);
                    if (author == null)
                        throw new ApiException($"Author not found with this ID {command.AuthorID}");
                    book.Authors.Add(new BookAuthor
                    {
                        Book = book,
                        Author = author
                    });
                    await _context.SaveChangesAsync();
                    return book.ID;
                }
            }
        }
    }
}

