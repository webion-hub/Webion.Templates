using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Webion.Templates.Api.Mappings;
using Webion.Templates.Api.Model;
using Webion.Templates.Infrastructure.Abstractions;
using Webion.Templates.Mustache.Abstractions;

namespace Kaire.Templates.Api.Controllers;

[AllowAnonymous]
[Route("templates/{TemplateName}")]
[ApiController]
public class TemplateController : ControllerBase
{
    private readonly ITemplatesRepository _templates;
    private readonly ITemplateProcessService _process;
    private readonly ILogger<TemplateController> _logger;

    [FromRoute]
    public string TemplateName { get; init; } = null!;

    public TemplateController(ITemplatesRepository templates, ITemplateProcessService process, ILogger<TemplateController> logger)
    {
        _templates = templates;
        _process = process;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(TemplateModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> FindByNameAsync(CancellationToken cancellationToken)
    {
        var template = await _templates.FindByNameAsync(TemplateName, cancellationToken);

        if(template is null)
            return NotFound(null);

        return Ok(template.ToModel());
    }

    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(CancellationToken cancellationToken)
    {
        var deleted = await _templates.DeleteAsync(TemplateName);

        if (!deleted)
            return NotFound();

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync([FromBody] string value, CancellationToken cancellationToken)
    {
        var updated = await _templates.UpdateAsync(TemplateName, value);

        if (!updated)
            return NotFound();

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ProcessTemplateAsync(
        [FromBody] string view,
        CancellationToken cancellationToken
    )
    {
        var json = JsonConvert.DeserializeObject(view);

        var template = await _templates.FindByNameAsync(TemplateName, cancellationToken);
        if(template is null)
            return NotFound();

        return Ok(await _process.ProcessAsync(json, template.Template, cancellationToken));
    }
}