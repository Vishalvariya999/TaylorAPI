using AutoMapper;
using Domain.Data;
using Domain.Models;
using MediatR;

namespace Application.Commands.Roles
{
    public class CreateRoleCommand : IRequest<Role>
    {
        public short RoleId { get; set; }
        public string Name { get; set; }
        public class Handler : IRequestHandler<CreateRoleCommand, Role>
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
            public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
            {
                if (command.RoleId != default(int))
                    return await _mediator.Send(_mapper.Map<UpdateRoleCommand>(command), cancellationToken);

                var Role = _mapper.Map<Role>(command);
                _context.Roles.Add(Role);
                await _context.SaveChangesAsync(cancellationToken);
                return Role;
            }
        }
    }
}
