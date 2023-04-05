using Webion.Templates.Cli.Http;

namespace Webion.Templates.Cli.Commands;

internal sealed class ShowCommand : Command
{
    public ShowCommand() : base(
        name: "show",
        description: "show the selected template"
    )
    {
        AddArgument(new Argument<string>("name", "Template name"));
    }

    public new class Handler : AsyncCommandHandler
    {
        public string Name { get; set; } = null!;
        private readonly TemplatesClient _client;

        public Handler(TemplatesClient client)
        {
            _client = client;
        }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            return await AnsiConsole.Status().Spinner(Spinner.Known.Arc).StartAsync("Fetching...", async ctx =>
            {
                var template = await _client.FindByNameAsync(Name, context.GetCancellationToken());
                
                if(template is null)
                {
                    AnsiConsole.MarkupLine("[red]Not found[/]");
                    return 0;
                }

                var table = new Table();
                table.AddColumn("Name");
                table.AddColumn("Template");

                table.AddRow(Name, template.Template);

                AnsiConsole.Write(table);
                return 0;
            });
        }
    }
}