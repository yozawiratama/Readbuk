using System;
using System.Text.Json.Serialization;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<Guid>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public String? SignedInUserId { get; set; }
        public class CommandHandler : IRequestHandler<UpdateCategoryCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var found = _context.Categories.Where(a => a.ID == command.ID).FirstOrDefault();

                if (found == null)
                {
                    return default;
                }
                else
                {
                    found.Name = command.Name;
                    found.ModifiedBy = command.SignedInUserId;
                    await _context.SaveChangesAsync();
                    return found.ID;
                }
            }
        }
    }
}

