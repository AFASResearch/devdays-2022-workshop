using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;

public static class FocusAuthorization
{
  public static void SetUp(WebApplicationBuilder builder, string clientId, string clientSecret)
  {
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddAuthorization(options =>
    {
      options.AddPolicy("ConnectedToFocus", new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    });
    
    builder.Services.AddAuthentication(options =>
      {
        // If an authentication cookie is present, use it to get authentication information
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // If authentication is required, and no cookie is present, use Focus (configured below) to sign in
        options.DefaultChallengeScheme = "Focus";
      })
      .AddCookie() // cookie authentication middleware first
      .AddOAuth<OAuthOptions, FocusOAuthHandler>("Focus", oauthOptions =>
      {
        oauthOptions.AuthorizationEndpoint = "."; // To be set later in FocusOAuthHandler using focusUrl in Session 
        oauthOptions.TokenEndpoint = "."; // To be set later in FocusOAuthHandler
        oauthOptions.ClientId = clientId;
        oauthOptions.ClientSecret = clientSecret;
        oauthOptions.SaveTokens = true;
        oauthOptions.CallbackPath = new PathString("/authorization-code/callback");
        oauthOptions.UsePkce = true;
      });
  }
}