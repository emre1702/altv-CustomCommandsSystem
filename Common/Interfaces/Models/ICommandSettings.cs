using System.Collections.Generic;

namespace CustomCommandSystem.Common.Interfaces.Models
{
    internal interface ICommandSettings
    {
        string Command { get; set; }
        List<string> Aliases { get; set; }
        bool Hidden { get; set; }
        int RequiredAdminLevel { get; set; }
    }
}
