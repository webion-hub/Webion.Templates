namespace Webion.Templates.Cli.Ui.Prompt;

public sealed class YesNoPrompt : IPrompt<bool>
{
    private readonly SelectionPrompt<bool> _prompt;

    public YesNoPrompt(string title)
    {
        _prompt = new SelectionPrompt<bool>()
            .Title(title)
            .AddChoices(true, false)
            .UseConverter(c => c
                ? "Yes"
                : "No"
            );
    }

    public bool Show(IAnsiConsole console)
    {
        return _prompt.Show(console);
    }

    public Task<bool> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        return _prompt.ShowAsync(console, cancellationToken);
    }

    public static bool AcceptInteractive(string title, bool skip)
    {
        if (skip)
            return true;

        return AnsiConsole.Prompt(new YesNoPrompt(
            title: title
        ));
    }
}