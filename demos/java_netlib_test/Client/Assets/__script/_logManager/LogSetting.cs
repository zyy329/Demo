using System.Collections.Generic;

/// <summary>
/// 日志环境 统一设置类;
/// 线程安全;
/// Created by zyy on 2018/2/7.
/// </summary>
namespace Log
{
    /// <summary>
    /// 输出日志 参数设置
    /// </summary>
    public class LogSetting
    {
        // 日志级别; 
        private LogLevel _logLev = LogLevel.OFF;
        // 输出文件路径, 为空则不输出到文件;
        private string _filePath = null;

        #region Param interface
        public LogLevel LogLev
        {
            get
            {
                return _logLev;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }
        #endregion

        public LogSetting(LogLevel logLev, string logPath)
        {
            this._logLev = logLev;
            this._filePath = logPath;
        }
    }

    public static class LogSetManager
    {
        private static Dictionary<string, LogSetting> _settings = new Dictionary<string, LogSetting>();
        public static string ROOT_KEY_NAME = "root";

        // 添加日志配置;
        public static void Add(string logKeyName, LogLevel logLevel, string filePath)
        {
            _settings[logKeyName] = new LogSetting(logLevel, filePath);
        }

        // 获得对应日志配置;
        public static LogSetting GetSetting(string logName)
        {
            if (_settings.ContainsKey(logName) == true)
            {
                return _settings[logName];
            }
            else
            {
                if (_settings.ContainsKey(ROOT_KEY_NAME) == false)
                {
                    // 给予一个默认值; 多线程情况下, 可能重复加入, 不过不会导致异常, 可以容忍;
                    Add(ROOT_KEY_NAME, LogLevel.OFF, null);
                }
                return _settings[ROOT_KEY_NAME];
            }
        }
    }
}
