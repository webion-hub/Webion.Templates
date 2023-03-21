using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Webion.Templates.Infrastructure.Abstractions;
using Webion.Templates.Mustache.Abstractions;

namespace Kaire.Templates.Api.Controllers;

[Route("templates/{Template}")]
[ApiController]
public class TemplateController : ControllerBase
{
    private readonly ITemplatesRepository _templates;
    private readonly ITemplateProcessService _process;
    private readonly ILogger<TemplateController> _logger;

    [FromRoute]
    public string Template { get; init; } = null!;

    public TemplateController(ITemplatesRepository templates, ITemplateProcessService process, ILogger<TemplateController> logger)
    {
        _templates = templates;
        _process = process;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> FindByNameAsync(CancellationToken cancellationToken)
    {
        var template = await _templates.FindByNameAsync(Template, cancellationToken);

        if(template is null)
            return NotFound();

        return Ok(template);
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> ProcessTemplateAsync(
        [FromBody] string view,
        CancellationToken cancellationToken
    )
    {
        var json = JsonConvert.DeserializeObject(view);

        _logger.LogCritical(JsonConvert.SerializeObject(json));

        var template = await _templates.FindByNameAsync(Template, cancellationToken);
        if(template is null)
            return NotFound();

        return Ok(await _process.ProcessAsync(json, template.Template, cancellationToken));
    }
}