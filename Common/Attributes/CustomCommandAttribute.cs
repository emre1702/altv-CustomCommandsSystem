using System;

namespace CustomCommandSystem.Common.Attributes
{
    /// <summary>
    ///     Mark a method as command method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomCommandAttribute : Attribute
    {
        public string Command { get; }
        public int Priority { get; }

        /// <inheritdoc cref="CustomCommandAttribute"/>
        /// <param name="command">Command used for this method.</param>
        /// <param name="priority">
        ///     If multiple methods for the same command are similiar, the priority defines which one is used.<br/>
        ///     Higher priority => will be tried first<br/><br/>
        ///     <para>Example:<br/>
        ///     /kick [Id/Name] has two methods, one with <see cref="int"/> and one with <see cref="string"/> as parameter.<br/>
        ///     Here you should give the method with <see cref="int"/> as parameter a higher priority so that this method is tried first<br/>
        ///     Reason: every <see cref="int"/> can be converted to a <see cref="string"/>, but not every <see cref="string"/> to an <see cref="int"/>.<br/>
        ///     That means the one with <see cref="string"/> as parameter would always be executed and the one with <see cref="int"/> never.</para>
        /// </param>
        public CustomCommandAttribute(string command, int priority = 0)
        {
            Command = command;
            Priority = priority;
        }
    }
}
