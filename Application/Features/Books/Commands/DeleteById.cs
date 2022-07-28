using System;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Commands
{
    public class DeleteBookByIdCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public class CommandHandler : IRequestHandler<DeleteBookByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeleteBookByIdCommand command, CancellationToken cancellationToken)
            {
                var found = await _context.Books.Where(a => a.ID == command.ID).FirstOrDefaultAsync();
                if (found == null) return default;

                var authors = await _context.BookAuthors.Where(w => w.Book == found).ToListAsync();
                var variants = await _context.BookVariants.Where(w => w.Book == found).ToListAsync();

                _context.Books.Remove(found);
                _context.BookAuthors.RemoveRange(authors);
                _context.BookVariants.RemoveRange(variants);
                await _context.SaveChangesAsync();
                return found.ID;
            }
        }
    }
}

