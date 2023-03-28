using Webion.Templates.Cli.Http;
using Webion.Templates.Cli.Model;
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
        private readonly TemplatesClient _client;

        public Handler(TemplatesClient client)
        {
            _client = client;
        }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            var template = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Arc)
                .StartAsync("Fetching...", async ctx =>
                {
                    return await _client.FindByNameAsync(Name, context.GetCancellationToken());
                });

            if(template?.Template is null)
            {
                AnsiConsole.MarkupLine("[red]Not found[/]");
                return 1;
            }

            if (!YesNoPrompt.AcceptInteractive("Are you sure you want to edit the template?", Yes))
                return 0;

            var file = $"/tmp/{Guid.NewGuid()}.html";

            await (template.Template | CliWrap.Cli.Wrap($"tee")
                .WithArguments($"{file}"))
                .ExecuteAsync(context.GetCancellationToken());

            await CliWrap.Cli.Wrap($"code")
                .WithArguments($"-n -w {file}")
                .ExecuteAsync(context.GetCancellationToken());

            var result = await CliWrap.Cli.Wrap($"cat")
                .WithArguments($"{file}")
                .ExecuteBufferedAsync(context.GetCancellationToken());

            await _client.RemoveAsync(Name, context.GetCancellationToken());
            await _client.CreateAsync(
                new TemplateModel
                {
                    Name = Name,
                    Template = result.StandardOutput
                },
                context.GetCancellationToken()
            );

            AnsiConsole.MarkupLine("[blue]Edited[/]");
            return 0;
        }
    }
}