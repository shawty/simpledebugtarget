using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebuggerOutput
{
  public class LogHeadersMiddleware
  {
    private readonly RequestDelegate next;

    public static readonly List<KeyValuePair<string, string>> RequestHeaders = new List<KeyValuePair<string, string>>();
    public static readonly List<KeyValuePair<string, string>> ResponseHeaders = new List<KeyValuePair<string, string>>();

    public LogHeadersMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      var uniqueRequestHeaders = context.Request
        .Headers
        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value));

      RequestHeaders.AddRange(uniqueRequestHeaders);

      await next.Invoke(context);

      var uniqueResponseHeaders = context.Response
        .Headers
        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value));

      ResponseHeaders.AddRange(uniqueResponseHeaders);

    }
  }
}
