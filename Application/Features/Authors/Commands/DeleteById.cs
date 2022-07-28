using System;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authors.Commands
{
    public class DeleteAuthorByIdCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public class CommandHandler : IRequestHandler<DeleteAuthorByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeleteAuthorByIdCommand command, CancellationToken cancellationToken)
            {
                var found = await _context.Authors.Where(a => a.ID == command.ID).FirstOrDefaultAsync();
                if (found == null) return default;
                _context.Authors.Remove(found);
                await _context.SaveChangesAsync();
                return found.ID;
            }
        }
    }
}

