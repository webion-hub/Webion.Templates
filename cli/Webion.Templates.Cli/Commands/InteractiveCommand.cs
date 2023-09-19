namespace Webion.Templates.Cli.Commands;

public class InteractiveCommand : Command
{
    public bool Yes { get; set; } = false;

    public InteractiveCommand(string name, string description = "") : base(name, description)
    {
        AddOption(new Option<bool>("--yes"));
    }
}