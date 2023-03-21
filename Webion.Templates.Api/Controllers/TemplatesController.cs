using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Templates.Infrastructure.Abstractions;
using Webion.Templates.Api.Model;
using Webion.Templates.Api.Mappings;

namespace Kaire.Templates.Api.Controllers;

[Route("templates")]
[ApiController]
public class TemplatesController : ControllerBase
{
    private readonly ITemplatesRepository _templates;

    public TemplatesController(ITemplatesRepository templates)
    {
        _templates = templates;
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(TemplateModel), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] TemplateModel template,
        CancellationToken cancellationToken
    )
    {
        var created = await _templates.CreateAsync(template.ToDbo(), cancellationToken);

        if(created is null)
            return BadRequest();

        return Ok(created.ToModel());
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<string>), 200)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Ok(await _templates.AllAsync(cancellationToken));
    }
}