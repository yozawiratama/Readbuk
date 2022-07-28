using System;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Authors.Commands
{
    public class UpdateAuthorCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public class CommandHandler : IRequestHandler<UpdateAuthorCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
            {
                var found = _context.Authors.Where(a => a.ID == command.ID).FirstOrDefault();

                if (found == null)
                {
                    return default;
                }
                else
                {
                    found.Name = command.Name;
                    await _context.SaveChangesAsync();
                    return found.ID;
                }
            }
        }
    }
}

