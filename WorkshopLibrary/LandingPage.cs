using System.Web;
using Microsoft.AspNetCore.Authentication;

static class LandingPage
{
  public static void Register(WebApplication app)
  {
    app.MapGet("/landing-page", async (context) =>
    {
      // await context.SignOutAsync();
      var focusUrl = context.Request.Query["afasBaseUrl"].Single();
      FocusOAuthHandler.RegisterFocusUrl(context, focusUrl);
      context.Response.ContentType = "text/html; charset=UTF-8";
      await context.Response.WriteAsync(
        "<h1>LandingPage</h1> Wil je een koppeling leggen met AFAS Focus op " 
        + HttpUtility.HtmlEncode(focusUrl)+"?<br> <a href=/run>ja</a>"
      );
    });
  }
}