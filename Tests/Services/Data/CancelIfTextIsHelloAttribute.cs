using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Attributes;
using CustomCommandsSystem.Common.Models;
using System;

namespace CustomCommandsSystem.Tests.Services.Data
{
    class CancelIfTextIsHelloAttribute : CustomCommandRequirementBaseAttribute
    {
        public override bool CanExecute(IPlayer player, CustomCommandInfo? info, ArraySegment<object?> methodArgs)
            => methodArgs[0]!.ToString() != "Hello";
    }
}
