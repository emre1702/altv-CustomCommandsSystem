using System;

namespace CustomCommandSystem.Common.Attributes
{
    /// <summary>
    ///     Add this to the last parameter to make it the one getting all the remaining arguments.<br/><br/>
    ///     Example:<br/>
    ///     /kick Player1 This is the reason.<br/>
    ///     <code>public void KickCommand(Player admin, Player target, [CustomCommandRemainingText] string reason)</code><br/><br/>
    ///     If you don't add this attribute to reason, the system will think you passed too many arguments and fail.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CustomCommandRemainingTextAttribute : Attribute
    {
    }
}
