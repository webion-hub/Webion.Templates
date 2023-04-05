using CliWrap.Buffered;
using Webion.Templates.Cli.Ui.Prompt;

namespace Webion.Templates.Cli.Commands;

internal sealed class EditCommand : InteractiveCommand
{
    public EditCommand() : base(
        name: "edit",
        description: "edit a template"
    )
    {
        AddArgument(new Argument<string>("name", "Template name"));
    }

    public new class Handler : AsyncCommandHandler
    {
        public string Name { get; set; } = null!;
        public bool Yes { get; set; } = false;
        private readonly ITemplatesClient _client;

        public Handler(ITemplatesClient client)
        {
            _client = client;
        }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            if (!YesNoPrompt.AskConfirmation("Are you sure you want to edit the template?", Yes))
                    return 0;

            return await AnsiConsole.Status().Spinner(Spinner.Known.Arc).StartAsync("Reading Template...", async ctx =>
            {
                var template = await _client.FindByNameAsync(Name, context.GetCancellationToken());

                if(template is null)
                {
                    AnsiConsole.MarkupLine("[red]Not found[/]");
                    return 1;
                }

                var file = $"/tmp/{Guid.NewGuid()}.html";

                ctx.Status("Opening editor...");
                await (template.Template | CliWrap.Cli.Wrap($"tee")
                    .WithArguments($"{file}"))
                    .ExecuteAsync(context.GetCancellationToken());

                ctx.Status("Editing template...");
                await CliWrap.Cli.Wrap($"code")
                    .WithArguments($"-n -w {file}")
                    .ExecuteAsync(context.GetCancellationToken());

                ctx.Status("Updating template...");
                var result = await CliWrap.Cli.Wrap($"cat")
                    .WithArguments($"{file}")
                    .ExecuteBufferedAsync(context.GetCancellationToken());

                await _client.UpdateAsync(template.Name, result.StandardOutput);

                AnsiConsole.MarkupLine("[blue]Edited[/]");
                return 0;
            });
        }
    }
}