namespace CustomCommandsSystem.Common.Models
{
    public class UserInputData
    {
        /// <summary>
        ///     The used command.
        /// </summary>
        public string Command { get; }

        /// <summary>
        ///     The whole message the user wrote (in a cleaned state).
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Parsed arguments from the <see cref="Message"/>.<br/>.
        /// </summary>
        public string[] Arguments { get; }

        internal UserInputData(string cmd, string msg, string[] arguments)
        {
            Command = cmd;
            Message = msg;
            Arguments = arguments;
        }
    }
}
