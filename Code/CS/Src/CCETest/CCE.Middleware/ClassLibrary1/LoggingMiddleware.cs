using CCE.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCE.Middleware
{
    public class LoggingMiddleware
    {
        readonly RequestDelegate _next;
        readonly DiagnosticContext _diagnosticContext;
        readonly MessageTemplate _messageTemplate;
        static readonly LogEventProperty[] NoProperties = new LogEventProperty[0];
        static readonly string logOutputTemplate = "Request Method:{RequestMethod} Path:{RequestPath} Response Sateus:{StatusCode} in {Elapsed:0.0000} ms";

        public LoggingMiddleware(RequestDelegate next, DiagnosticContext diagnosticContext)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
            _messageTemplate = new MessageTemplateParser().Parse(logOutputTemplate);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var start = Stopwatch.GetTimestamp();

            var collector = _diagnosticContext.BeginCollection();
            try
            {
                using (LogContext.PushProperty("requestid", httpContext.GetRequestId()))
                {
                    await _next(httpContext);
                    var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                    var statusCode = httpContext.Response.StatusCode;
                    LogCompletion(httpContext, collector, statusCode, elapsedMs, null);
                }
            }
            catch (Exception ex)
                when (LogCompletion(httpContext, collector, 500, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex))
            {
            }
            finally
            {
                collector.Dispose();
            }
        }

        bool LogCompletion(HttpContext httpContext, DiagnosticContextCollector collector, int statusCode, double elapsedMs, Exception ex)
        {
            var logger = Log.ForContext<LoggingMiddleware>();

            if (!collector.TryComplete(out var collectedProperties))
                collectedProperties = NoProperties;

            var properties = collectedProperties.Concat(new[]
            {
                new LogEventProperty("RequestMethod", new ScalarValue(httpContext.Request.Method)),
                new LogEventProperty("RequestPath", new ScalarValue(GetPath(httpContext))),
                new LogEventProperty("RequestHeader",new ScalarValue(GetHeader(httpContext))),
                new LogEventProperty("StatusCode", new ScalarValue(statusCode)),
                new LogEventProperty("Elapsed", new ScalarValue(elapsedMs))
            });

            var evt = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, ex, _messageTemplate, properties);

            logger.Write(evt);

            return false;
        }

        static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }

        static string GetPath(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? httpContext.Request.Path.ToString();
        }

        static string GetHeader(HttpContext httpContext)
        {
            return string.Empty;
            //return string.Join(",", httpContext.Request.Headers.ToArray());
        }
    }
}
