namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface ILogger
    {
        void LogError(string message);
        void LogWarning(string message);
    }
}