namespace DimaChat.Client.Commands;

public class GenericCommand<T> : Command where T : class
{
    private static readonly Func<T, bool> DefaultCanExecute = _ => true;
    private readonly Action<T> executeAction;
    private readonly Func<T, bool> canExecuteFunc;

    public GenericCommand(Action<T> action) : this(action, DefaultCanExecute)
    {
    }

    public GenericCommand(Action<T> executeAction, Func<T, bool> canExecuteAction)
    {
        this.executeAction = executeAction;
        canExecuteFunc = canExecuteAction;
    }

    protected override void ExecuteCmd(object parameter)
    {
        executeAction(parameter as T);
    }

    protected override bool CanExecuteCmd(object parameter)
    {
        return canExecuteFunc(parameter as T);
    }
}
