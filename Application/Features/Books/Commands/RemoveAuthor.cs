using System;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Commands
{
    public class RemoveAuthorBookByIdCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public class CommandHandler : IRequestHandler<RemoveAuthorBookByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(RemoveAuthorBookByIdCommand command, CancellationToken cancellationToken)
            {
                var found = await _context.BookAuthors.Where(a => a.ID == command.ID).FirstOrDefaultAsync();
                if (found == null) return default;

                _context.BookAuthors.RemoveRange(found);
                await _context.SaveChangesAsync();
                return found.ID;
            }
        }
    }
}

