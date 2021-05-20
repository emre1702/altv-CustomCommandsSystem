using CustomCommandSystem.Services.Executer;

namespace CustomCommandSystem.Integration
{
    internal class Main
    {
        public Main()
        {
            new Core.Init();
            MethodExecuter.BeforeCommandExecute = Events.OnBeforeCommandExecute;
            MethodExecuter.AfterCommandExecute = Events.OnAfterCommandExecute;
            new ClientEvents();
        }
    }
}
