using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCE.Common
{
  public  class ApplicationInit
    {
        public static void Init()
        {
            InitLogger();
        }

        private static void InitLogger()
        {
            string logOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}|{MicroserviceName}|{Level}|{ThreadId}|{requestid}|{SourceContext}|{Message}{NewLine}{Exception}";

            LoggerConfiguration configuration = new LoggerConfiguration()
               .MinimumLevel.ControlledBy(new LoggingLevelSwitch())
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .Enrich.WithThreadId()
               .Enrich.FromLogContext()
               .Enrich.WithProperty("MicroserviceName", "CCETest")
               .WriteTo.Console(outputTemplate: logOutputTemplate);

            Log.Logger = configuration.CreateLogger();
        }
    }
}
