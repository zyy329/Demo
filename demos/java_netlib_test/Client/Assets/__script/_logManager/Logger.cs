using UnityEngine;

/// <summary>
/// 日志处理类, 通过名字分类不同的日志
/// 方便统一管理控制日志的打印;
/// 线程安全;
/// Created by zyy on 2018/2/7.
/// </summary>
namespace Log
{
    public class Logger
    {
        private string _name;


        public Logger(string name)
        {
            this._name = name;
        }


        #region 日志输出接口;
        public void Debug(string info)
        {
            Log(LogLevel.DEBUG, info);
        }
        public void Info(string info)
        {
            Log(LogLevel.INFO, info);
        }
        public void Warning(string info)
        {
            Log(LogLevel.WARN, info);
        }
        public void Error(string info)
        {
            Log(LogLevel.ERROR, info);
        }
        #endregion

        #region 工具类;
        // 输出到控制台;
        private void Log(LogLevel outLevel, string info)
        {
            LogSetting set = LogSetManager.GetSetting(_name);
            // 当前输出级别 高于 日志级别 的才进行输出;
            if (outLevel >= set.LogLev)
            {
                // 添加标签信息;
                string logInfo = string.Format("[{0}]:{1}", _name, info);

                // 输出到控制台;
                switch (outLevel)
                {
                    case LogLevel.DEBUG:
                    case LogLevel.INFO:
                        {
                            UnityEngine.Debug.Log(logInfo);
                        }break;
                    case LogLevel.WARN:
                        {
                            UnityEngine.Debug.LogWarning(logInfo);
                        }break;
                    case LogLevel.ERROR:
                        {
                            UnityEngine.Debug.LogError(logInfo);
                        }break;
                    default:
                        {
                            // 异常;
                            UnityEngine.Debug.LogError(string.Format("日志输出异常; outLevel({0}) err; info:{1}", outLevel, logInfo));
                        }
                        break;
                }

                // 输出到文件;
                OutToFile(set.FilePath, logInfo);
            }

        }
        // 输出到文件;
        private void OutToFile(string filePath, string info)
        {
            if (filePath != null)
            {
                // 输出到文件
                // 暂未实现;
            }
        }
        #endregion
    }

}
