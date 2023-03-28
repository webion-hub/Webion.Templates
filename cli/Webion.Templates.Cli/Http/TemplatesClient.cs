using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Webion.Templates.Cli.Model;
using Webion.Templates.Cli.Options;

namespace Webion.Templates.Cli.Http;

public sealed class TemplatesClient : HttpClient
{
    public TemplatesClient(IOptions<TemplatesOptions> options)
    {
        BaseAddress = new Uri(options.Value.Url);
    }

    public async Task<IEnumerable<string>> GetAllAsync(CancellationToken cancellationToken)
    {
        var response = await GetAsync($"/templates/", cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var templates = JsonConvert.DeserializeObject<List<string>>(json);

        return templates ?? Enumerable.Empty<string>();
    }

    public async Task<TemplateModel?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        var response = await GetAsync($"/templates/{name}", cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TemplateModel?>(json) ?? null;
    }

    public async Task<bool> CreateAsync(TemplateModel template, CancellationToken cancellationToken)
    {
        var content = JsonConvert.SerializeObject(template);
        var response = await PostAsync($"/templates/", new StringContent(content, Encoding.UTF8, "application/json"), cancellationToken);

        return(response.StatusCode != HttpStatusCode.BadRequest);
    }

    public async Task<bool> RemoveAsync(string name, CancellationToken cancellationToken)
    {
        var response = await DeleteAsync($"/templates/{name}", cancellationToken);

        return(response.StatusCode != HttpStatusCode.NotFound);
    }
}