using CustomCommandsSystem.Services.Executer;

namespace CustomCommandsSystem.Integration
{
    internal class Main
    {
        public Main()
        {
            _ = new Core.Init();
            MethodExecuter.BeforeCommandExecute = Events.OnBeforeCommandExecute;
            MethodExecuter.AfterCommandExecute = Events.OnAfterCommandExecute;
            _ = new ClientEvents();
        }
    }
}
