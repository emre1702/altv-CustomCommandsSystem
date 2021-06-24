using CustomCommandsSystem.Services.Executer;

namespace CustomCommandsSystem.Integration
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
