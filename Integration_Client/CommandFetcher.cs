using static RAGE.Events;

namespace CustomCommandSystem.Client.Integration
{
    public class CommandFetcher : Script
    {
        public CommandFetcher()
        {
            OnPlayerCommand += PlayerCommand;
            OnConsoleCommand += ConsoleCommand;
        }

        private void PlayerCommand(string cmd, CancelEventArgs cancel)
        {
            cancel.Cancel = true;

            CallRemote("CustomCommandSystem:Call", cmd);
        }

        private void ConsoleCommand(string cmd)
           => CallRemote("CustomCommandSystem:Call", cmd);
    }
}
