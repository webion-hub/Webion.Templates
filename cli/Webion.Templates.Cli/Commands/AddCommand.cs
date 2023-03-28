using Webion.Templates.Cli.Http;
using Webion.Templates.Cli.Model;
using CliWrap.Buffered;

namespace Webion.Templates.Cli.Commands;

internal sealed class AddCommand : System.CommandLine.Command
{
    public AddCommand() : base(
        name: "add",
        description: "add a template"
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
            var file = $"/tmp/{Guid.NewGuid()}.html";
            await CliWrap.Cli.Wrap($"code")
                .WithArguments($"-n -w {file}")
                .ExecuteAsync(context.GetCancellationToken());

            var result = await CliWrap.Cli.Wrap($"cat")
                .WithArguments($"{file}")
                .ExecuteBufferedAsync(context.GetCancellationToken());

            var created = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Arc)
                .StartAsync("Fetching...", async ctx =>
                {
                    return await _client.CreateAsync(
                        new TemplateModel
                        {
                            Name = Name,
                            Template = result.StandardOutput
                        },
                        context.GetCancellationToken()
                    );
                });

            if(!created)
            {
                AnsiConsole.MarkupLine("[red]Conflict[/]");
                return 1;
            }

            AnsiConsole.MarkupLine("[blue]Created[/]");
            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Template");

            table.AddRow(Name, result.StandardOutput);

            AnsiConsole.Write(table);
            return 0;
        }
    }
}