namespace Webion.Templates.Cli.Extensions;

public static class AnsiConsoleExtension
{
    public static async Task LoadAsync(this Status status, Task<Action> action)
    {
        await status
            .Spinner(Spinner.Known.Arc)
            .StartAsync("Fetching...", async ctx => await action);
    }
}