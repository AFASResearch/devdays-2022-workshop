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
    var afasBaseUrl = Context.Session.GetString("afasBaseUrl");
    Options.AuthorizationEndpoint = afasBaseUrl + "/app/auth";
    return base.BuildChallengeUrl(properties, redirectUri);
  }

  protected override Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
  {
    var afasBaseUrl = Context.Session.GetString("afasBaseUrl");
    Options.TokenEndpoint = afasBaseUrl + "/app/token"; 
    return base.ExchangeCodeAsync(context);
  }

  public static void RegisterAfasBaseUrl(HttpContext context, string afasBaseUrl)
  {
    context.Session.SetString("afasBaseUrl", afasBaseUrl);
  }
}