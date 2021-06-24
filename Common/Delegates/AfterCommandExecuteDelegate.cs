using CustomCommandsSystem.Common.Models;
using AltV.Net.Elements.Entities;

namespace CustomCommandsSystem.Common.Delegates
{
    public delegate void AfterCommandExecuteDelegate(IPlayer player, UserInputData userInputData, object?[] args);
}
