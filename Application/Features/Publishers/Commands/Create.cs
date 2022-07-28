using System;
using System.Text.Json.Serialization;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Publishers.Commands
{
    public class CreatePublisherCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<CreatePublisherCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityDbContext _identityContext;
            public CommandHandler(IApplicationDbContext context, IIdentityDbContext identityDbContext)
            {
                _context = context;
                _identityContext = identityDbContext;
            }
            public async Task<Guid> Handle(CreatePublisherCommand command, CancellationToken cancellationToken)
            {
                var data = new Publisher
                {
                    Name = command.Name,
                    CreatedBy = command.SignedInUserId,
                    CreatedAt = DateTime.Now
                };
                _context.Publishers.Add(data);
                await _context.SaveChangesAsync();
                return data.ID;
            }

        }
    }
}

