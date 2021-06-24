using CustomCommandsSystem.Common.Models;
using AltV.Net.Elements.Entities;
using System.ComponentModel;

namespace CustomCommandsSystem.Common.Delegates
{
    public delegate void BeforeCommandExecuteDelegate(IPlayer player, UserInputData userInputData, object?[] args, CancelEventArgs cancelEventArgs);
}
