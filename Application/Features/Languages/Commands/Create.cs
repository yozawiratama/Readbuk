using System;
using System.Text.Json.Serialization;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Languages.Commands
{
    public class CreateLanguageCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<CreateLanguageCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public CommandHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<Guid> Handle(CreateLanguageCommand command, CancellationToken cancellationToken)
            {
                var data = new Language
                {
                    Name = command.Name,
                    CreatedBy = command.SignedInUserId,
                    CreatedAt = DateTime.Now
                };
                _context.Languages.Add(data);
                await _context.SaveChangesAsync();
                return data.ID;
            }

        }
    }
}

