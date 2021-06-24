using CustomCommandsSystem.Common.Models;
using AltV.Net.Elements.Entities;
using System;
using System.ComponentModel;

namespace CustomCommandsSystem.Common.Delegates
{
    public delegate object? ConverterDelegate(IPlayer player, UserInputData userInputData, ArraySegment<string> argumentsForConverter, CancelEventArgs cancelErrorMessageOnFail);
}
