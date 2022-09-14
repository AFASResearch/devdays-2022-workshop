using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Authentication;

static class RunScriptPage
{
  public static void Register(WebApplication app, Func<HttpContext, Func<string, Task>, Task> runScriptCallback)
  {
    app.UseWebSockets();
    app.MapGet("/run-script", async (context) =>
    {
      // Not needed: 
      // var refreshToken = await context.GetTokenAsync("refresh_token");
      var accessToken = await context.GetTokenAsync("access_token");

      using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
      await runScriptCallback(context, Log);

      await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);

      async Task Log(string text)
      {
        app.Logger.LogInformation(text);
        await webSocket.SendAsync(Encoding.UTF8.GetBytes(text), WebSocketMessageType.Text, true, CancellationToken.None);
      }
    }).RequireAuthorization();
  }
}