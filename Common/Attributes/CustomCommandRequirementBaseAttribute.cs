using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System;

namespace CustomCommandSystem.Common.Attributes
{
    /// <summary>
    /// Override this attribute with your custom attribute to add custom requirements/checks before a command gets executed.<br/>
    /// E.g. add a "RequiresAdminLevel(int adminLevel)" attribute which overrides this and return false in CanExecute, if the player has a lower admin level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class CustomCommandRequirementBaseAttribute : Attribute
    {
        /// <summary>
        ///     Implement this method to add a custom requirement/check before a command gets executed.
        /// </summary>
        /// <param name="player">The player who executed the command.</param>
        /// <param name="info">Optional info only if the method has this as parameter.</param>
        /// <param name="args">
        ///     Arguments passed to the method (without Player or CustomCommandInfo).<br/>
        ///     Example: "/kick Player1" => methodArgs will only contain Player1
        /// </param>
        /// <returns>If the requirement was met. Returning false will cancel the execution, BeforeCommandExecute will not be triggered.</returns>
        public abstract bool CanExecute(Player player, CustomCommandInfo? info, ArraySegment<object?> methodArgs);
    }
}
 