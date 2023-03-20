using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Templates.Infrastructure.Abstractions;

namespace Kaire.Templates.Api.Controllers;

[Route("templates/{Template}")]
[ApiController]
public class TemplatesController : ControllerBase
{
    private readonly ITemplatesRepository _templates;

    [FromRoute]
    public string Template { get; init; } = null!;

    public TemplatesController(ITemplatesRepository templates)
    {
        _templates = templates;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> FindByNameAsync(CancellationToken cancellationToken)
    {
        return Ok(await _templates.FindByNameAsync(Template, cancellationToken));
        
    }
}