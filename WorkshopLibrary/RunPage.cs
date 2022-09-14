static class RunPage
{
  public static void Register(WebApplication app)
  {
    app.MapGet("/run", async (context) =>
    {
      context.Response.ContentType = "text/html; charset=UTF-8";
      await context.Response.WriteAsync(@"
<html>
  <head>
    <title>Run</title>
  </head>
  <body>
    <h1>Run</h1>
    <ul id=""log"">
      <li>Opening websocket</li>
    </ul>
    <script>
      function log(text) {
        let liNode = document.createElement(""li"");
        liNode.appendChild(document.createTextNode(text));
        document.getElementById(""log"").appendChild(liNode);
      }

      let webSocket = new WebSocket(""ws"" + (document.location + ""-script"").substr(4));

      webSocket.addEventListener(""open"", () => { log(""websocket connected""); });
      webSocket.addEventListener(""message"", (evt) => { log(""server: "" + evt.data); });
      webSocket.addEventListener(""error"", () => { log(""websocket error""); });
      webSocket.addEventListener(""close"", () => { log(""websocket disconnected""); });
    </script>
  </body>
</html>
");
    }).RequireAuthorization();
  }
}