namespace Webion.Templates.Cli.Commands;

public sealed class TemplateCommand : RootCommand
{
    public TemplateCommand() : base("template")
    {
        AddCommand(new ListCommand());
        AddCommand(new ShowCommand());
        AddCommand(new DeleteCommand());
        AddCommand(new AddCommand());
        AddCommand(new EditCommand());
    }
}