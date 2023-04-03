using Webion.Templates.Cli.Http;
using Webion.Templates.Cli.Ui.Prompt;

namespace Webion.Templates.Cli.Commands;

internal sealed class DeleteCommand : InteractiveCommand
{
    public DeleteCommand() : base(
        name: "delete",
        description: "delete a template"
    )
    {
        AddArgument(new Argument<string>("name", "Template name"));
    }

    public new class Handler : AsyncCommandHandler
    {
        public bool Yes { get; set; } = false;
        public string Name { get; set; } = null!;
        private readonly TemplatesClient _client;

        public Handler(TemplatesClient client)
        {
            _client = client;
        }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            if (!YesNoPrompt.AskConfirmation("Are you sure you want to delete the template?", Yes))
                return 0;
                
            return await AnsiConsole.Status().Spinner(Spinner.Known.Arc).StartAsync("Deleting Template...", async ctx =>
            {
                var deleted = await _client.RemoveAsync(Name, context.GetCancellationToken());

                if(!deleted)
                {
                    AnsiConsole.MarkupLine("[red]Not found[/]");
                    return 1;
                }

                AnsiConsole.MarkupLine("[blue]Deleted[/]");
                return 0;
            });
        }
    }
}