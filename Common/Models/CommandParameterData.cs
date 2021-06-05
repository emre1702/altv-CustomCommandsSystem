using System;

namespace CustomCommandSystem.Common.Models
{
    internal class CommandParameterData
    {
        public int UserInputLength { get; set; }
        public bool HasDefaultValue { get; set; }
        public object? DefaultValue { get; set; }
        public bool IsRemainingText { get; set; }
        public bool IsNullable { get; set; }
        public string? Name { get; set; }
#nullable disable
        public Type Type { get; set; }
#nullable enable
    }
}
