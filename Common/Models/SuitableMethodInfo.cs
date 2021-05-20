namespace CustomCommandSystem.Common.Models
{
    internal class SuitableMethodInfo
    {
        public CommandMethodData? MethodData { get; set; }
        public object[]? ConvertedArgs { get; set; }
        public bool FailedButExceptionOccured { get; set; }
    }
}
