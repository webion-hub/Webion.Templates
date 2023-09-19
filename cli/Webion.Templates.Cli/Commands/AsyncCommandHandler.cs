namespace Webion.Templates.Cli.Commands;

public abstract class AsyncCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        return 1;
    }

    public abstract Task<int> InvokeAsync(InvocationContext context);
}