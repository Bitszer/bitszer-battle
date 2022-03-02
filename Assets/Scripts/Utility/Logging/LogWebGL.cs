#if UNITY_WEBGL
    using System.Runtime.InteropServices;
#endif

namespace Utility.Logging
{
    public sealed class LogWebGL : Log
    {
        #if UNITY_WEBGL
        
            [DllImport("__Internal")]
            private static extern void LogDebug(string message);

            [DllImport("__Internal")]
            private static extern void LogWarning(string message);
            
            [DllImport("__Internal")]
            private static extern void LogError(string message);
        
        #endif

        public LogWebGL(string name) : base(name)
        {
        }
        
        protected override void DebugImpl(string message)
        {
            #if UNITY_WEBGL
                LogDebug(message);
            #endif
        }

        protected override void WarningImpl(string message)
        {
            #if UNITY_WEBGL
                LogWarning(message);
            #endif
        }

        protected override void ErrorImpl(string message)
        {
            #if UNITY_WEBGL
                LogError(message);
            #endif
        }
    }
}