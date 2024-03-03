using Application.Common.Exceptions;
using Domain.Data;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Roles
{
    public class DeleteRoleCommand : IRequest<Role>
    {
        public int Key { get; set; }
        public class Handler : IRequestHandler<DeleteRoleCommand, Role>
        {
            private readonly TaylorDBContext _context;

            public Handler(TaylorDBContext context)
            {
                _context = context;
            }

            public async Task<Role> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
            {
                var entity = await _context.Roles.SingleOrDefaultAsync(x => x.RoleId == command.Key, cancellationToken);

                if (entity == null)
                    throw new NotFoundException(nameof(Role), command.Key);

                entity.Deleted = true;
                await _context.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
