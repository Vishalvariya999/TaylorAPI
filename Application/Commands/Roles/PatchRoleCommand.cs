using Application.Common.Exceptions;
using Domain.Data;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Roles
{
    public class PatchRoleCommand : IRequest<Role>
    {
        public int Key { get; set; }
        public Delta<Role> Role { get; set; }
        public class Handler : IRequestHandler<PatchRoleCommand, Role>
        {
            private readonly TaylorDBContext _context;

            public Handler(TaylorDBContext context)
            {
                _context = context;
            }

            public async Task<Role> Handle(PatchRoleCommand command, CancellationToken cancellationToken)
            {
                var entity = await _context.Roles.FindAsync(new object[] { command.Key }, cancellationToken);

                if (entity == null)
                    throw new NotFoundException(nameof(Role), command.Key);

                command.Role.Patch(entity);
                _context.Roles.Update(entity);

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Roles.AnyAsync(e => e.RoleId == command.Key, cancellationToken))
                        throw new NotFoundException(nameof(Role), command);

                    throw;
                }

                return entity;
            }
        }
    }
}
