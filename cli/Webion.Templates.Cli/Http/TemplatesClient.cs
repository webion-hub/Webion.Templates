using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
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
        var templates = await _client.GetFromJsonAsync<List<string>>($"/templates/", cancellationToken);

        return templates ?? Enumerable.Empty<string>();
    }

    public async Task<TemplateModel?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        var template = await _client.GetFromJsonAsync<TemplateModel?>($"/templates/{name}", cancellationToken);

        return template;
    }

    public async Task<bool> CreateAsync(TemplateModel template)
    {
        var response = await _client.PostAsJsonAsync($"/templates/", template);

        return(response.StatusCode != HttpStatusCode.BadRequest);
    }

    public async Task<bool> RemoveAsync(string name)
    {
        var response = await _client.DeleteAsync($"/templates/{name}");

        return (response.StatusCode != HttpStatusCode.NotFound);
    }

    public async Task<bool> UpdateAsync(string name, string value)
    {
        var response = await _client.PutAsJsonAsync($"/templates/{name}", value);

        return (response.StatusCode != HttpStatusCode.NotFound);
    }
}