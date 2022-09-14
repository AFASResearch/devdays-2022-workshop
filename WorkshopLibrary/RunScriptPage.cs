using System.Net.WebSockets;
using System.Text;
static class RunScriptPage
{
  public static void Register(WebApplication app, Func<HttpContext, Func<string, Task>, Task> runScriptCallback)
  {
    app.UseWebSockets();
    app.MapGet("/run-script", async (context) =>
    {
      using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
      try
      {
        await runScriptCallback(context, Log);

        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
      }
      catch (Exception e)
      {
        await Log(e.Message);
        await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, null, CancellationToken.None);
      }

      async Task Log(string text)
      {
        app.Logger.LogInformation(text);
        await webSocket.SendAsync(Encoding.UTF8.GetBytes(text), WebSocketMessageType.Text, true, CancellationToken.None);
      }
    }).RequireAuthorization();
  }
}