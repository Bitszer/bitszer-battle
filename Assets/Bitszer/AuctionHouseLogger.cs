using System;

namespace Bitszer
{
    public sealed class AuctionHouseLogger
    {
        public bool IsDebugEnabled { get; set; }
        public bool IsWarnEnabled  { get; set; }

        private readonly string _name;

        public AuctionHouseLogger()
        {
            _name = nameof(AuctionHouse);
            
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

        public AuctionHouseLogger Enable()
        {
            IsDebugEnabled = true;
            IsWarnEnabled = true;
            return this;
        }

        public AuctionHouseLogger Disable(bool keepWarningsEnabled = true)
        {
            IsDebugEnabled = false;
            IsWarnEnabled = keepWarningsEnabled;
            return this;
        }

        /*
         * Protected.
         */

        private void DebugImpl(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        private void WarningImpl(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        private void ErrorImpl(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        /*
         * Private.
         */

        private string FormatMessage(object message)
        {
            return $"{_name}: {message}";
        }
    }
}