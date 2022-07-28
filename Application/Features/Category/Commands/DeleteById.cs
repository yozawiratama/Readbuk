using System;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Commands
{
    public class DeleteCategoryByIdCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public class CommandHandler : IRequestHandler<DeleteCategoryByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeleteCategoryByIdCommand command, CancellationToken cancellationToken)
            {
                var found = await _context.Categories.Where(a => a.ID == command.ID).FirstOrDefaultAsync();
                if (found == null) return default;
                _context.Categories.Remove(found);
                await _context.SaveChangesAsync();
                return found.ID;
            }
        }
    }
}

