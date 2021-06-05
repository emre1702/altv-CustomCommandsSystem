namespace CustomCommandSystem.Common.Enums
{
    public enum UsageOutputType
    {
        /// <summary>
        ///     No usage output.
        /// </summary>
        Disabled,

        /// <summary>
        ///     Only output the usage of one method for that command.<br/>
        ///     If priorities are set, the usage of the one with the highest priority will be outputted.
        /// </summary>
        OneUsage,

        /// <summary>
        ///     Only output the usage of one method for that command if the arguments amount was correct but wrong types have been provided.<br/>
        ///     If priorities are set, the usage of the one with the highest priority will be outputted.
        /// </summary>
        OneUsageOnWrongTypes,

        /// <summary>
        ///     Output the usages of all methods for that command.<br/>
        ///     Ordered by priority.
        /// </summary>
        AllUsages,

        /// <summary>
        ///     Output the usages of all methods for that command if the arguments amount was correct but wrong types have been provided.<br/>
        ///     Ordered by priority.
        /// </summary>
        AllUsagesOnWrongTypes,
        
    }
}
