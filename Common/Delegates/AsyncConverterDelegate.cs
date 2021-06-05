using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System;
using System.Threading.Tasks;

namespace CustomCommandSystem.Common.Delegates
{
    public delegate Task<object?> AsyncConverterDelegate(Player player, UserInputData userInputData, ArraySegment<string> argumentsForConverter, CancelEventArgs cancelErrorMessageOnFail);
}
