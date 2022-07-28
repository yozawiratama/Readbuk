using System;
using System.Text.Json.Serialization;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Commands
{
    public class CreateBookCommand : IRequest<Guid>
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int PublishYear { get; set; }
        public Guid CategoryID { get; set; }
        public Guid PublisherID { get; set; }
        public List<Guid> AuthorIDs { get; set; }
        public List<string> VariantCodes { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<CreateBookCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public CommandHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(w => w.ID == command.CategoryID);
                if(category == null)
                    throw new ApiException($"Category not found with this ID {command.CategoryID}");

                var publisher = await _context.Publishers.FirstOrDefaultAsync(w => w.ID == command.PublisherID);
                if(publisher == null)
                    throw new ApiException($"Publisher not found with this ID {command.PublisherID}");

                var data = new Book
                {
                    Code = command.Code,
                    Title = command.Title,
                    Description = command.Description,
                    Price = command.Price,
                    PublishYear = command.PublishYear,
                    Category = category,
                    Publisher = publisher
                };

                var authors = await _context.Authors.Where(w => command.AuthorIDs.Contains(w.ID)).ToListAsync();

                List<BookAuthor> bookAuthors = new List<BookAuthor>();
                List<BookVariant> bookVariants = new List<BookVariant>();

                authors.ForEach(ai =>
                {
                    data.Authors.Add(new BookAuthor
                    {
                        Book = data,
                        Author = ai
                    });
                });

                command.VariantCodes.ForEach(vc =>
                {
                    data.Variants.Add(new BookVariant
                    {
                        Book = data,
                        Code = vc,
                        Status = BookVariantStatus.Available,
                        CreatedBy = command.SignedInUserId,
                        CreatedAt = DateTime.Now
                    });
                });

                _context.Books.Add(data);
                await _context.SaveChangesAsync();
                return data.ID;
            }

        }
    }
}

