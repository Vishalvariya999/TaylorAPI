using Domain.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace TaylorAPI.Config
{
    public class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Role>(nameof(Role)).EntityType.HasKey(x => x.RoleId);
            return builder.GetEdmModel();
        }
    }
}
