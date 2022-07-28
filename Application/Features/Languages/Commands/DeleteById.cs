using System;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Languages.Commands
{
    public class DeleteLanguageByIdCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public class CommandHandler : IRequestHandler<DeleteLanguageByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeleteLanguageByIdCommand command, CancellationToken cancellationToken)
            {
                var found = await _context.Languages.Where(a => a.ID == command.ID).FirstOrDefaultAsync();
                if (found == null) return default;
                _context.Languages.Remove(found);
                await _context.SaveChangesAsync();
                return found.ID;
            }
        }
    }
}

