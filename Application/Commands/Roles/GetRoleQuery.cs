using Domain.Data;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Roles
{
    public class GetRoleQuery : IRequest<Role>
    {
        public int Key { get; set; }
        public class Handler : IRequestHandler<GetRoleQuery, Role>
        {
            private readonly TaylorDBContext _context;

            public Handler(TaylorDBContext context)
            {
                _context = context;
            }

            public async Task<Role> Handle(GetRoleQuery query, CancellationToken cancellationToken)
            {
                var entity = await _context.Roles.Where(x => x.RoleId == query.Key).FirstOrDefaultAsync();

                return entity;
            }
        }
    }
}
