using System;
using Application.DTOs.Author;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authors.Queries
{
    public class GetAllAuthorsQuery : IRequest<IEnumerable<AuthorItemResponse>>
    {

        public class QueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorItemResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public QueryHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<IEnumerable<AuthorItemResponse>> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
            {
                //var users = await _identityContext.Users.FirstOrDefaultAsync();
                var productList = await _context.Authors.ToListAsync();
                if (productList == null)
                {
                    return null;
                }
                return productList
                    .AsReadOnly()
                    .Select(s => new AuthorItemResponse
                    {
                        ID = s.ID,
                        Name = s.Name
                    });
            }
        }
    }
}

