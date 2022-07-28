using System;
using Application.DTOs.Author;
using Application.DTOs.Category;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryItemResponse>>
    {

        public class QueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryItemResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public QueryHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<IEnumerable<CategoryItemResponse>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
            {
                //var users = await _identityContext.Users.FirstOrDefaultAsync();
                var found = await _context.Categories.Select(s => new CategoryItemResponse
                {
                    ID = s.ID,
                    Name = s.Name
                }).ToListAsync();
                if (found == null)
                {
                    return null;
                }
                return found;
            }
        }
    }
}

