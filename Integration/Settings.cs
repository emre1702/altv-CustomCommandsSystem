using CustomCommandsSystem.Services.Utils;

namespace CustomCommandsSystem.Integration
{
    /// <summary>
    /// Contains the configurations for the command system.
    /// </summary>
    public static class Settings
    {
        /// <inheritdoc cref="Settings"/>
        public static ICommandsConfiguration Config => CommandsConfiguration.LazyInstance.Value;
    }
}
