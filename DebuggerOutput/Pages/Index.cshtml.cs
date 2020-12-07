using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DebuggerOutput.Pages
{
  public class IndexModel : PageModel
  {
    [BindProperty]
    public bool NoHeadersAvailable { get; set; } = false;

    [BindProperty]
    public List<KeyValuePair<string, string>> Headers { get => _headers; }

    [BindProperty]
    public bool NoQueriesAvailable { get; set; } = false;

    [BindProperty]
    public List<KeyValuePair<string, string>> QueryParms { get => _queryParms; }

    [BindProperty]
    public bool NoCookiesAvailable { get; set; } = false;

    [BindProperty]
    public List<KeyValuePair<string, string>> Cookies { get => _cookies; }



    private readonly ILogger<IndexModel> _logger;

    private readonly List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();
    private readonly List<KeyValuePair<string, string>> _queryParms = new List<KeyValuePair<string, string>>();
    private readonly List<KeyValuePair<string, string>> _cookies = new List<KeyValuePair<string, string>>();

    public IndexModel(ILogger<IndexModel> logger)
    {
      _logger = logger;
    }

    public void OnGet()
    {

      foreach(var header in HttpContext.Request.Headers)
      {
        _headers.Add(new KeyValuePair<string, string>(header.Key, header.Value));
      }
      if (_headers.Count == 0) NoHeadersAvailable = true;

      foreach (var queryParm in HttpContext.Request.Query)
      {
        _queryParms.Add(new KeyValuePair<string, string>(queryParm.Key, queryParm.Value));
      }
      if (_queryParms.Count == 0) NoQueriesAvailable = true;

      foreach (var cookie in HttpContext.Request.Cookies)
      {
        _cookies.Add(new KeyValuePair<string, string>(cookie.Key, cookie.Value));
      }
      if (_cookies.Count == 0) NoCookiesAvailable = true;

    }

  }
}
