using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Webion.Templates.Cli.Abstraction;
using Webion.Templates.Cli.Model;
using Webion.Templates.Cli.Options;

namespace Webion.Templates.Cli.Http;

public sealed class TemplatesClient : ITemplatesClient
{
    private readonly HttpClient _client;

    public TemplatesClient(IOptions<TemplatesOptions> options, HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(options.Value.Url);
    }

    public async Task<IEnumerable<string>> GetAllAsync(CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync($"/templates/", cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var templates = JsonConvert.DeserializeObject<List<string>>(json);

        return templates ?? Enumerable.Empty<string>();
    }

    public async Task<TemplateModel?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync($"/templates/{name}", cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TemplateModel?>(json) ?? null;
    }

    public async Task<bool> CreateAsync(TemplateModel template, CancellationToken cancellationToken)
    {
        var content = JsonConvert.SerializeObject(template);
        var response = await _client.PostAsync($"/templates/", new StringContent(content, Encoding.UTF8, "application/json"), cancellationToken);

        return(response.StatusCode != HttpStatusCode.BadRequest);
    }

    public async Task<bool> RemoveAsync(string name, CancellationToken cancellationToken)
    {
        var response = await _client.DeleteAsync($"/templates/{name}", cancellationToken);

        return(response.StatusCode != HttpStatusCode.NotFound);
    }
}