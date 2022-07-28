using System;
using Application.DTOs.Language;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Languages.Queries
{
    public class GetAllLanguagesQuery : IRequest<IEnumerable<LanguageItemResponse>>
    {

        public class QueryHandler : IRequestHandler<GetAllLanguagesQuery, IEnumerable<LanguageItemResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public QueryHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<IEnumerable<LanguageItemResponse>> Handle(GetAllLanguagesQuery query, CancellationToken cancellationToken)
            {
                //var users = await _identityContext.Users.FirstOrDefaultAsync();
                var found = await _context.Languages.Select(s => new LanguageItemResponse
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

