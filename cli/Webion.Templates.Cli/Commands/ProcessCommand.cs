using CliWrap.Buffered;

namespace Webion.Templates.Cli.Commands;

internal sealed class ProcessCommand : Command
{
    public ProcessCommand() : base(
        name: "process",
        description: "process a template"
    )
    {
        AddArgument(new Argument<string>("name", "Template name"));
    }

    public new class Handler : AsyncCommandHandler
    {
        public string Name { get; set; } = null!;
        private readonly ITemplatesClient _client;

        public Handler(ITemplatesClient client)
        {
            _client = client;
        }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            return await AnsiConsole.Status().Spinner(Spinner.Known.Arc).StartAsync("Reading Template...", async ctx =>
            {
                var template = await _client.FindByNameAsync(Name, context.GetCancellationToken());

                if(template is null)
                {
                    AnsiConsole.MarkupLine("[red]Not found[/]");
                    return 1;
                }

                var file = $"/tmp/{Guid.NewGuid()}.json";

                ctx.Status("Opening editor...");
                await ("" | CliWrap.Cli.Wrap($"tee")
                    .WithArguments($"{file}"))
                    .ExecuteAsync(context.GetCancellationToken());

                ctx.Status("Editing data...");
                await CliWrap.Cli.Wrap($"code")
                    .WithArguments($"-n -w {file}")
                    .ExecuteAsync(context.GetCancellationToken());

                ctx.Status("Processing data...");
                var result = await CliWrap.Cli.Wrap($"cat")
                    .WithArguments($"{file}")
                    .ExecuteBufferedAsync(context.GetCancellationToken());

                var processed = await _client.ProcessAsync(template.Name, result.StandardOutput, context.GetCancellationToken());

                var table = new Table();
                table.AddColumn("Name");
                table.AddColumn("Template");
                table.AddRow(Name, processed);
                
                AnsiConsole.Write(table);
                return 0;
            });
        }
    }
}