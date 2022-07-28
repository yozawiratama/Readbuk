using System;
using System.Text.Json.Serialization;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Publishers.Commands
{
    public class UpdatePublisherCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<UpdatePublisherCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdatePublisherCommand command, CancellationToken cancellationToken)
            {
                var found = _context.Authors.Where(a => a.ID == command.ID).FirstOrDefault();

                if (found == null)
                {
                    return default;
                }
                else
                {
                    found.Name = command.Name;
                    found.ModifiedAt = DateTime.Now;
                    found.ModifiedBy = command.SignedInUserId;
                    await _context.SaveChangesAsync();
                    return found.ID;
                }
            }
        }
    }
}

