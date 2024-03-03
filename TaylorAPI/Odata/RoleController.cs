using Application.Commands.Roles;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;

namespace TaylorAPI.Odata
{
    public class RoleController : ODataControllerBase
    {
        [EnableQuery]
        [HttpGet]
        public async Task<IQueryable<Role>> Get() =>
                   await Mediator.Send(new GetRoleQueryable());

        [HttpGet("{key:int}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Get([FromODataUri] int key) =>
            Ok(await Mediator.Send(new GetRoleQuery { Key = key }));

        // PUT: odata/Role(5)
        [HttpPut]
        public async Task<IActionResult> Put([FromODataUri] long key, [FromBody] UpdateRoleCommand command, CancellationToken cancellationToken = default)
        {
            if (key != command.RoleId || !ModelState.IsValid) return BadRequest(ModelState);
            return Updated(await Mediator.Send(command, cancellationToken));
        }

        //POST: odata/Role
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRoleCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await Mediator.Send(command, cancellationToken);
            return Created($"odata/{nameof(AppSetting)}({response.RoleId})", response);
        }

        // PATCH/MERGE: odata/Role(5)
        [AcceptVerbs("PATCH")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Role> command, CancellationToken cancellationToken = default)
        {
            if (key == default || key != command.GetInstance().RoleId || !ModelState.IsValid) return BadRequest(ModelState);
            return Updated(await Mediator.Send(new PatchRoleCommand { Key = key, Role = command }, cancellationToken));
        }

        // DELETE: odata/Role(5)
        [HttpDelete]
        public async Task<IActionResult> Delete([FromODataUri] int key, CancellationToken cancellationToken = default)
        {
            if (key == default || !ModelState.IsValid) return BadRequest(ModelState);
            await Mediator.Send(new DeleteRoleCommand { Key = key }, cancellationToken);
            return NoContent();
        }
    }
}
