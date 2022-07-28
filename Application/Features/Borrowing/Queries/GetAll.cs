using System;
using Application.DTOs.Author;
using Application.DTOs.Borrowing;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Borrowings.Queries
{
    public class GetAllBorrowingQuery : IRequest<IEnumerable<BorrowingItemResponse>>
    {

        public class QueryHandler : IRequestHandler<GetAllBorrowingQuery, IEnumerable<BorrowingItemResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public QueryHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<IEnumerable<BorrowingItemResponse>> Handle(GetAllBorrowingQuery query, CancellationToken cancellationToken)
            {
                //var users = await _identityContext.Users.FirstOrDefaultAsync();
                var found = await _context.Borrowings
                    .Include(borrow => borrow.BookItem)
                        .ThenInclude(bookItem => bookItem.Book)
                    .Select(s => new BorrowingItemResponse
                    {
                        ID = s.ID,
                        BorrowerID = s.BorrowerID,
                        BorrowerName = s.BorrowerName,
                        BookCode = s.BookItem.Book.Code,
                        BookVariantCode = s.BookItem.Code,
                        BookTitle = s.BookItem.Book.Title,
                        BorrowedAt = s.BorrowedAt,
                        LateCharge = s.LateCharge,
                        ReturnedAt = s.ReturnedAt,
                        Status = s.ReturnedAt == null? "NOT_RETURNED_YET" : "RETURNED"
                    })
                    .ToListAsync();
                if (found == null)
                {
                    return null;
                }
                return found;
            }
        }
    }
}

