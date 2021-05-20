using GTANetworkAPI;
using System;

namespace CustomCommandSystem.Common.Delegates
{
    public delegate object? ConverterDelegate(Player player, ArraySegment<string> argumentsForConverter, CancelEventArgs cancelErrorMessageOnFail);
}
