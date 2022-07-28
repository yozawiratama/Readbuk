using System;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Publishers.Commands
{
    public class DeletePublisherByIdCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public class CommandHandler : IRequestHandler<DeletePublisherByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeletePublisherByIdCommand command, CancellationToken cancellationToken)
            {
                var found = await _context.Publishers.Where(a => a.ID == command.ID).FirstOrDefaultAsync();
                if (found == null) return default;
                _context.Publishers.Remove(found);
                await _context.SaveChangesAsync();
                return found.ID;
            }
        }
    }
}

