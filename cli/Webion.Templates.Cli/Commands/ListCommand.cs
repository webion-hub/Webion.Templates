using Webion.Templates.Cli.Http;

namespace Webion.Templates.Cli.Commands;

internal sealed class ListCommand : Command
{
    public ListCommand() : base(
        name: "list",
        description: "list all the templates"
    )
    {
    }

    public new class Handler : AsyncCommandHandler
    {
        private readonly TemplatesClient _client;

        public Handler(TemplatesClient client)
        {
            _client = client;
        }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            var templates = await AnsiConsole
                .Status()
                .Spinner(Spinner.Known.Arc)
                .StartAsync("Fetching...", async ctx =>
                {
                    return await _client.GetAllAsync(context.GetCancellationToken());
                });

            var table = new Table();
            table.AddColumn("Templates");

            foreach (var template in templates)
                table.AddRow(template);

            AnsiConsole.Write(table);

            return 0;
        }
    }
}