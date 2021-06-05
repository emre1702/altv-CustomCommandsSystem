using CustomCommandSystem.Common.Models;
using GTANetworkAPI;

namespace CustomCommandSystem.Common.Delegates
{
    public delegate void BeforeCommandExecuteDelegate(Player player, UserInputData userInputData, object?[] args, CancelEventArgs cancelEventArgs);
}
