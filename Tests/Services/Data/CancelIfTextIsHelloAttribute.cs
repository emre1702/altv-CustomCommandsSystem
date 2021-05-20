using CustomCommandSystem.Common.Attributes;
using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System;

namespace CustomCommandSystem.Tests.Services.Data
{
    class CancelIfTextIsHelloAttribute : CustomCommandRequirementBaseAttribute
    {
        public override bool CanExecute(Player player, CustomCommandInfo? info, ArraySegment<object> methodArgs)
            => methodArgs[0].ToString() != "Hello";
    }
}
