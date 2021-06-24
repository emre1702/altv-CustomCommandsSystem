using CustomCommandsSystem.Common.Models;
using AltV.Net.Elements.Entities;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CustomCommandsSystem.Common.Delegates
{
    public delegate Task<object?> AsyncConverterDelegate(IPlayer player, UserInputData userInputData, ArraySegment<string> argumentsForConverter, CancelEventArgs cancelErrorMessageOnFail);
}
