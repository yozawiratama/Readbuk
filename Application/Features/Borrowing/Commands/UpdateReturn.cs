using System;
using System.Text.Json.Serialization;
using Application.DTOs.Borrowing;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Borrowings.Commands
{
    public class UpdateReturnBorrowingCommand : IRequest<BorrowingReturnResponse>
    {
        public Guid ID { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<UpdateReturnBorrowingCommand, BorrowingReturnResponse>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<BorrowingReturnResponse> Handle(UpdateReturnBorrowingCommand command, CancellationToken cancellationToken)
            {
                var found = await _context.Borrowings
                    .Include(borrow => borrow.BookItem)
                        .ThenInclude(bookItem => bookItem.Book)
                    .Where(a => a.ID == command.ID).FirstOrDefaultAsync();

                if (found == null)
                {
                    return default;
                }
                else
                {
                    found.ReturnedAt = DateTime.Now;
                    found.LateCharge = 0;
                    found.ModifiedAt = DateTime.Now;
                    found.ModifiedBy = command.SignedInUserId;
                    TimeSpan lateday = found.MustReturnAt.Subtract(DateTime.Now);
                    if(lateday.TotalDays < 0)
                    {
                        found.LateCharge = found.BookItem.Book.Price * Convert.ToDecimal(lateday.TotalDays);
                    }
                    var bookItem = await _context.BookVariants.FirstOrDefaultAsync(w => w.ID == found.BookItem.ID);
                    bookItem.Status = BookVariantStatus.Available;
                    await _context.SaveChangesAsync();
                    return new BorrowingReturnResponse
                    {
                        ID = command.ID,
                        BookCode = found.BookItem.Book.Code,
                        BookVariantCode = found.BookItem.Code,
                        BookTitle = found.BookItem.Book.Title,
                        LateCharge = Convert.ToDecimal(found.LateCharge)
                    };
                }
            }
        }
    }
}

