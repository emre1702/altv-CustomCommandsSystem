using GTANetworkAPI;
using System;
using System.Threading.Tasks;

namespace CustomCommandSystem.Common.Delegates
{
    public delegate Task<object?> AsyncConverterDelegate(Player player, ArraySegment<string> argumentsForConverter, CancelEventArgs cancelErrorMessageOnFail);
}
