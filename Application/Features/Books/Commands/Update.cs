using System;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authors.Commands
{
    public class UpdateBookCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int PublishYear { get; set; }
        public Guid CategoryID { get; set; }
        public Guid PublisherID { get; set; }
        public class CommandHandler : IRequestHandler<UpdateBookCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
            {
                var book = _context.Books.Where(a => a.ID == command.ID).FirstOrDefault();

                if (book == null)
                {
                    return default;
                }
                else
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(w => w.ID == command.CategoryID);
                    if (category == null)
                        throw new ApiException($"Category not found with this ID {command.CategoryID}");

                    var publisher = await _context.Publishers.FirstOrDefaultAsync(w => w.ID == command.PublisherID);
                    if (publisher == null)
                        throw new ApiException($"Publisher not found with this ID {command.PublisherID}");

                    book.Title = command.Title;
                    book.Description = command.Description;
                    book.Price = command.Price;
                    book.PublishYear = command.PublishYear;
                    book.Category = category;
                    book.Publisher = publisher;
                    await _context.SaveChangesAsync();
                    return book.ID;
                }
            }
        }
    }
}

