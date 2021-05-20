namespace CustomCommandSystem.Common.Delegates
{
    internal delegate object FastInvokeHandler(object? target, object[] parameters);
    internal delegate object FastInvokeHandlerStatic(object[] parameters);
}
