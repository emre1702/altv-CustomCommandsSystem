using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System;

namespace CustomCommandSystem.Common.Delegates
{
    public delegate object? ConverterDelegate(Player player, UserInputData userInputData, ArraySegment<string> argumentsForConverter, CancelEventArgs cancelErrorMessageOnFail);
}
