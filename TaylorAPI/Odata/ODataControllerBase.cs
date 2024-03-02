using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace TaylorAPI.Odata
{
    [ODataRouteComponent("odata")]
    [AllowAnonymous]
    public class ODataControllerBase : ODataController
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
