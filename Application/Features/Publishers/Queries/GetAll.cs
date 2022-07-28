using System;
using Application.DTOs.Publisher;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Publishers.Queries
{
    public class GetAllPublishersQuery : IRequest<IEnumerable<PublisherItemResponse>>
    {

        public class QueryHandler : IRequestHandler<GetAllPublishersQuery, IEnumerable<PublisherItemResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public QueryHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<IEnumerable<PublisherItemResponse>> Handle(GetAllPublishersQuery query, CancellationToken cancellationToken)
            {
                //var users = await _identityContext.Users.FirstOrDefaultAsync();
                var found = await _context
                    .Publishers
                    .Select(s => new PublisherItemResponse
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

