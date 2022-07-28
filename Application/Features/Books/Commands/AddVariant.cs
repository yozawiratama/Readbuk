using System;
using System.Text.Json.Serialization;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Commands
{
    public class AddVariantBookCommand : IRequest<Guid>
    {
        public Guid BookID { get; set; }
        public string Code { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<AddVariantBookCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(AddVariantBookCommand command, CancellationToken cancellationToken)
            {
                var book = _context.Books.Where(a => a.ID == command.BookID).FirstOrDefault();

                if (book == null)
                {
                    return default;
                }
                else
                {
                    book.Variants.Add(new BookVariant
                    {
                        Book = book,
                        Code = command.Code,
                        CreatedAt = DateTime.Now,
                        CreatedBy = command.SignedInUserId
                    });
                    await _context.SaveChangesAsync();
                    return book.ID;
                }
            }
        }
    }
}

