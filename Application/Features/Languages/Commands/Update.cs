using System;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Languages.Commands
{
    public class UpdateLanguageCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public class CommandHandler : IRequestHandler<UpdateLanguageCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdateLanguageCommand command, CancellationToken cancellationToken)
            {
                var found = _context.Languages.Where(a => a.ID == command.ID).FirstOrDefault();

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

