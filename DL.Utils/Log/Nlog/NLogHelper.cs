using NLog;
using System;

namespace DL.Utils.Log.Nlog
{
    public class NLogHelper
    {

        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Debug(Type source, string message, params object[] ps)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Debug(message, ps);
            }
        }

        public static void Error(object message)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
        }

        public static void Fatal(object message)
        {
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }
        }

        public static void Info(object message)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }

        public static void Warn(object message)
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
        }
    }
}
