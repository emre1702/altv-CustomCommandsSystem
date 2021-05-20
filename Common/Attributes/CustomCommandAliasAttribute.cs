using System;

namespace CustomCommandSystem.Common.Attributes
{
    /// <summary>
    ///     Define one or more aliases for a command method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomCommandAliasAttribute : Attribute
    {
        public string[] Aliases { get; }

        /// <inheritdoc cref="CustomCommandAliasAttribute"/>
        /// <param name="aliases">One or more alises</param>
        public CustomCommandAliasAttribute(params string[] aliases)
            => Aliases = aliases;
    }
}
