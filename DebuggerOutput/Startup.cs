using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebuggerOutput
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddRazorPages();
    }

    public void Configure(
      IApplicationBuilder app,
      IWebHostEnvironment env,
      ILoggerFactory loggerFactory)
    {

      app.UseMiddleware<LogHeadersMiddleware>();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.Map("/dump-headers", ShowHeaders);

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
      });
    }

    private static void ShowHeaders(IApplicationBuilder app)
    {
      app.Run(async context => {

        var rawHeaders = LogHeadersMiddleware.RequestHeaders.Select((x => $"{x.Key} : {x.Value}"));
        var requestHeaders = string.Join("<br/>", rawHeaders);

        rawHeaders = LogHeadersMiddleware.ResponseHeaders.Select((x => $"{x.Key} : {x.Value}"));
        var responseHeaders = string.Join("<br />", rawHeaders);

        var output = $"<h2>Unique request headers</h2>" +
          $"{requestHeaders} " +
          $"<h2>Unique response headers</h2>" +
          $"{responseHeaders}";

        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(output);
      });
    }

  }
}
