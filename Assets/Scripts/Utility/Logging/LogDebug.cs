namespace Utility.Logging
{
    public sealed class LogDebug : Log
    {
        public LogDebug(string name) : base(name)
        {
        }
        
        protected override void DebugImpl(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        protected override void WarningImpl(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        protected override void ErrorImpl(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}