/// <summary>
/// 日志对象 集中定义处;
/// Created by zyy on 2018/2/7.
/// </summary>
namespace Log
{
    public static class Loggers
    {
        public static Logger nomal = new Logger("nomal");
        public static Logger net = new Logger("net");

    }

    public static class LogSetInit
    {
        public static void Init()
        {
#if UNITY_EDITOR
            LogSetManager.Add(LogSetManager.ROOT_KEY_NAME, LogLevel.INFO, null);
            LogSetManager.Add("net", LogLevel.DEBUG, null);
#else
            LogSetManager.Add(LogSetManager.ROOT_KEY_NAME, LogLevel.ERROR, null);
            LogSetManager.Add("net", LogLevel.ERROR, null);
#endif
        }
    }
}
