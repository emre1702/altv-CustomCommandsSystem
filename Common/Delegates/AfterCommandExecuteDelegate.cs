using CustomCommandSystem.Common.Models;
using GTANetworkAPI;

namespace CustomCommandSystem.Common.Delegates
{
    public delegate void AfterCommandExecuteDelegate(Player player, UserInputData userInputData, object?[] args);
}
