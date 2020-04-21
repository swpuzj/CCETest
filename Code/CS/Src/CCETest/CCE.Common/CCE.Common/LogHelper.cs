using System;
using Serilog;

namespace CCE.Common
{
    public class LogHelper
    {
        private readonly Lazy<ILogger> _loggerLazy;

        public static LogHelper CreateLogger<T>() where T : class
        {
            return new LogHelper(Log.ForContext<T>());
        }

        public static LogHelper CreateLogger(Type type)
        {
            return new LogHelper(Log.ForContext(type));
        }

        private LogHelper() { }
        private LogHelper(ILogger logger)
        {
            _loggerLazy = new Lazy<ILogger>(() => logger);
        }

        public void Info(Exception exception, string messageTemplate)
        {
            _loggerLazy.Value.Information(exception, messageTemplate);
        }

        public void Info(string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Information(messageTemplate, propertyValues);
        }

        public void Info<T>(string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Information(messageTemplate, propertyValue);
        }

        public void Info(string messageTemplate)
        {
            _loggerLazy.Value.Information(messageTemplate);
        }

        public void Warn<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Warning<T>(exception, messageTemplate, propertyValue);
        }

        public void Warn(string messageTemplate)
        {
            _loggerLazy.Value.Warning(messageTemplate);
        }

        public void Warn(Exception exception, string messageTemplate)
        {
            _loggerLazy.Value.Warning(exception, messageTemplate);
        }

        public void Warn(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Warning(exception, messageTemplate, propertyValues);
        }

        public void Warn(string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Warning(messageTemplate, propertyValues);
        }

        public void Warn<T>(string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Warning(messageTemplate, propertyValue);
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Debug<T>(messageTemplate, propertyValue);
        }

        public void Debug(string messageTemplate)
        {
            _loggerLazy.Value.Debug(messageTemplate);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            _loggerLazy.Value.Debug(exception, messageTemplate);
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Debug<T>(exception, messageTemplate, propertyValue);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Debug(exception, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Debug(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Error(exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate)
        {
            _loggerLazy.Value.Error(messageTemplate);
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Error<T>(messageTemplate, propertyValue);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            _loggerLazy.Value.Error(exception, messageTemplate);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Error(messageTemplate, propertyValues);
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Error<T>(exception, messageTemplate, propertyValue);
        }

        public void Fatal(string messageTemplate)
        {
            _loggerLazy.Value.Fatal(messageTemplate);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            _loggerLazy.Value.Fatal(exception, messageTemplate);
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Fatal<T>(exception, messageTemplate, propertyValue);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _loggerLazy.Value.Fatal(exception, messageTemplate, propertyValues);
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
            _loggerLazy.Value.Fatal<T>(messageTemplate, propertyValue);
        }
    }
}
