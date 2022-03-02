using System;

namespace Utility.Logging
{
    public abstract class Log : ILog
    {
        public bool IsDebugEnabled { get; set; }
        public bool IsWarnEnabled  { get; set; }

        private readonly string _name;

        protected Log(string name)
        {
            _name = name;
            
            IsDebugEnabled = true;
            IsWarnEnabled = true;
        }

        public void Debug(object message)
        {
            if (IsDebugEnabled)
                DebugImpl(FormatMessage(message));
        }

        public void Debug(Exception exception)
        {
            if (IsDebugEnabled)
                DebugImpl(FormatMessage(exception));
        }

        public void Warn(object message)
        {
            if (IsWarnEnabled)
                WarningImpl(FormatMessage(message));
        }

        public void Warn(Exception exception)
        {
            if (IsWarnEnabled)
                WarningImpl(FormatMessage(exception));
        }

        public void Error(object message)
        {
            ErrorImpl(FormatMessage(message));
        }

        public void Error(Exception exception)
        {
            ErrorImpl(FormatMessage(exception));
        }

        public ILog Enable()
        {
            IsDebugEnabled = true;
            IsWarnEnabled = true;
            return this;
        }

        public ILog Disable(bool keepWarningsEnabled = true)
        {
            IsDebugEnabled = false;
            IsWarnEnabled = keepWarningsEnabled;
            return this;
        }

        /*
         * Protected.
         */

        protected abstract void DebugImpl(string message);
        protected abstract void WarningImpl(string message);
        protected abstract void ErrorImpl(string message);

        /*
         * Private.
         */

        private string FormatMessage(object message)
        {
            return $"{_name}: {message}";
        }
    }
}