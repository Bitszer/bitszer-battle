using System;

namespace Utility.Logging
{
    public interface ILog
    {
        bool IsDebugEnabled { get; set; }
        bool IsWarnEnabled  { get; set; }

        void Debug(object message);
        void Debug(Exception exception);

        void Warn(object message);
        void Warn(Exception exception);

        void Error(object message);
        void Error(Exception exception);

        ILog Enable();
        ILog Disable(bool keepWarningsEnabled = true);
    }
}