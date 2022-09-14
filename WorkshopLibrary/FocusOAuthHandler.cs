using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;

///<summary>
///  This extends the default OAuthHandler to use a configurable url to do authentication with.
///  This is useful, because AFAS Focus is served simultaniously from multiple urls.
///</summary>
public class FocusOAuthHandler: OAuthHandler<OAuthOptions>
{
  public FocusOAuthHandler(IOptionsMonitor<OAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
    : base(options, logger, encoder, clock)
  {
  }

  protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
  {
    Options.AuthorizationEndpoint = GetAfasBaseUrl(Context) + "/app/auth";
    return base.BuildChallengeUrl(properties, redirectUri);
  }

  protected override Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
  {
    Options.TokenEndpoint = GetAfasBaseUrl(Context) + "/app/token"; 
    return base.ExchangeCodeAsync(context);
  }

  public static void RegisterAfasBaseUrl(HttpContext context, string afasBaseUrl)
  {
    context.Response.Cookies.Append("afasBaseUrl", afasBaseUrl);
  }

  public static string GetAfasBaseUrl(HttpContext context)
  {
    var afasBaseUrl = context.Request.Cookies["afasBaseUrl"];
    if (string.IsNullOrEmpty(afasBaseUrl))
    {
      throw new NotSupportedException("Could not find afasBaseUrl cookie");
    }
    return afasBaseUrl;
  }
}