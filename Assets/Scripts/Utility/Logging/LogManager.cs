namespace Utility.Logging
{
    public static class LogManager
    {
        public static ILog GetLogger<T>()
        {
            return GetLogger(typeof(T).Name);
        }

        public static ILog GetLogger(string name)
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return new LogWebGL(name);
            #else
                return new LogDebug(name);
            #endif
        }
    }
}