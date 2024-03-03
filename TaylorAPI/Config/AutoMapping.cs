using Application.Commands.Roles;
using AutoMapper;
using Domain.Models;

namespace TaylorAPI.Config
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // Create commands
            CreateMap<CreateRoleCommand, Role>().ReverseMap();

            // Update commands
            CreateMap<UpdateRoleCommand, Role>().ReverseMap();

            // Update to Create commands
            CreateMap<CreateRoleCommand, UpdateRoleCommand>().ReverseMap();
        }
    }
}
