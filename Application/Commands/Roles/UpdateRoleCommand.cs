using AutoMapper;
using Domain.Data;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Roles
{
    public class UpdateRoleCommand : IRequest<Role>
    {
        public int Key { get; set; }
        public short RoleId { get; set; }
        public string? Name { get; set; }
        public class Handler : IRequestHandler<UpdateRoleCommand, Role>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly TaylorDBContext _context;

            public Handler(IMapper mapper, IMediator mediator, TaylorDBContext context)
            {
                _mapper = mapper;
                _mediator = mediator;
                _context = context;
            }

            public async Task<Role> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
            {
                var entity = await _context.Roles.SingleOrDefaultAsync(x => x.RoleId == command.RoleId, cancellationToken);

                entity.Name = command.Name;

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return entity;
            }
        }
    }
}
