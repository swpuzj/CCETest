using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCE.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRequestId(this HttpContext httpContext)
        {
            string requestId = Guid.NewGuid().ToString("N");
            if (httpContext.Request.Headers.TryGetValue("request-id", out StringValues requestIdValue))
            {
                requestId = requestIdValue.ToString();
            }
            else
            {
                httpContext.Request.Headers.Add("request-id", requestId);
            }

            return requestId;
        }
    }
}
