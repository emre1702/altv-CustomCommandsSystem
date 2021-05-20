using GTANetworkAPI;

namespace CustomCommandSystem.Common.Delegates
{
    public delegate void BeforeCommandExecuteDelegate(Player player, string command, object[] args, CancelEventArgs cancelEventArgs);
}
