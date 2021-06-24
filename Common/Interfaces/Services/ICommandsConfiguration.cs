using CustomCommandsSystem.Common.Enums;
using CustomCommandsSystem.Common.Models;
using System;

namespace CustomCommandsSystem.Services.Utils
{
    public interface ICommandsConfiguration
    {
        /// <summary>
        ///     Error if the command does not exist.<br/>
        ///     Set <see langword="null"/> if you want to disable it.
        /// </summary>
        string? CommandDoesNotExistError { get; set; }

        /// <summary>
        ///     Command prefix used.<br/>
        ///     Only really needed if you have a custom chat, trigger the remote event or call the method by yourself and don't want to have to remove the prefix by yourself.<br/>
        ///     Also used for usage if <see cref="UsageOutputType"/> is enabled.
        /// </summary>
        string CommandPrefix { get; set; }

        /// <summary>
        ///     Error if the command has been used incorrectly.<br/>
        ///     Set <see langword="null"/> if you want to disable it.
        /// </summary>
        string? CommandUsedIncorrectly { get; set; }

        /// <summary>
        ///     Set this to false if you don't want the command method to be executed in NAPI.Task.Run (in main thread).<br/>
        ///     Has propably no effect if the command method is async.
        /// </summary>
        bool RunCommandMethodInMainThread { get; set; }

        /// <summary>
        ///     Configure how the usage should be outputted.
        /// </summary>
        UsageOutputType UsageOutputType { get; set; }

        /// <summary>
        ///     If <see cref="UsageOutputType"/> is enabled, this property is used to output the "null" default value.
        /// </summary>
        string NullDefaultValueName { get; set; }

        /// <summary>
        ///     If <see cref="UsageOutputType"/> is enabled, this property is used as the prefix for a usage text.<br/>
        ///     This will only be used if there is only one method to output.<br/>
        ///     For multiple usages see <see cref="MultipleUsagesOutputPrefix"/>.<br/>
        ///     E.g. "USAGE: /myCommand [name]" -> Here the prefix is "USAGE: "
        /// </summary>
        string SingleUsageOutputPrefix { get; set; }

        /// <summary>
        ///     If <see cref="UsageOutputType"/> is enabled, this property is used as the prefix for usage text.<br/>
        ///     This will only be used if there are multiple methods to output.<br/>
        ///     For single usage see <see cref="SingleUsageOutputPrefix"/>.<br/>
        ///     E.g. "USAGES:<br/>/myCommand [number]<br/>/myCommand [name]"<br/>-> Here the prefix is "USAGES:"
        /// </summary>
        string MultipleUsagesOutputPrefix { get; set; }

        /// <summary>
        ///     If <see cref="UsageOutputType"/> is enabled and this property is true, the default values are included in the usage output.
        /// </summary>
        bool UsageAddDefaultValues { get; set; }

        /// <summary>
        ///     Currently the messages are all sent with player.SendChatMessage. If you want to change that behaviour, provide your own handler.<br/>
        ///     <example>
        ///         <code>
        ///             MessageOutputHandler = (data) => 
        ///             {
        ///                 var messages = data.MessageToOutput.Split(Environment.NewLine);
        ///                 NAPI.Task.Run(() =>
        ///                 {
        ///                     foreach (var line in messages)
        ///                         data.Player.SendChatMessage(line);
        ///                 }
        ///             });
        ///         </code>
        ///     </example>
        /// </summary>
        Action<CommandOutputData> MessageOutputHandler { get; set; }

        /// <summary>
        ///     Output for default player converter when player could not be found.
        /// </summary>
        string? PlayerNotFoundErrorMessage { get; set; }
    }
}