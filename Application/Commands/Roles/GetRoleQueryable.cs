using Domain.Data;
using Domain.Models;
using MediatR;

namespace Application.Commands.Roles
{
    public class GetRoleQueryable : IRequest<IQueryable<Role>>
    {
        public class Handler : IRequestHandler<GetRoleQueryable, IQueryable<Role>>
        {
            private readonly TaylorDBContext _context;

            public Handler(TaylorDBContext context)
            {
                _context = context;
            }

            public async Task<IQueryable<Role>> Handle(GetRoleQueryable request, CancellationToken cancellationToken)
            {
                return _context.Roles.AsQueryable();
            }
        }
    }
}
