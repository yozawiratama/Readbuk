using System;
using System.Text.Json.Serialization;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Borrowings.Commands
{
    public class CreateBorrowingCommand : IRequest<Guid>
    {
        public Guid BorrowerID { get; set; }
        public DateTime BorrowedAt { get; set; }
        public int howlong { get; set; }
        public Guid BookVariantID { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<CreateBorrowingCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public CommandHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<Guid> Handle(CreateBorrowingCommand command, CancellationToken cancellationToken)
            {
                var user = await _identityContext.Users.FirstOrDefaultAsync(w => w.Id == command.BorrowerID.ToString());
                if (user == null) throw new ApiException("Member nor found");
                var book = await _context.BookVariants
                    .Include(v => v.Book)
                    .FirstOrDefaultAsync(w => w.ID == command.BookVariantID);
                if (book == null) throw new ApiException($"Book not found");
                if(book.Status == BookVariantStatus.Borrowed) throw new ApiException($"Book already borrowed");
                var data = new Borrowing
                {
                    BorrowerID = command.BorrowerID,
                    BorrowerName = $"{user.FirstName} {user.LastName}",
                    BorrowedAt = command.BorrowedAt,
                    MustReturnAt = command.BorrowedAt.AddDays(command.howlong),
                    TotalRentPrice = book.Book.Price * command.howlong,
                    BookItem = book,
                    CreatedBy = command.SignedInUserId,
                    CreatedAt = DateTime.Now
                };
                book.Status = BookVariantStatus.Borrowed;
                _context.Borrowings.Add(data);
                await _context.SaveChangesAsync();
                return data.ID;
            }

        }
    }
}

