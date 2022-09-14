static class RunPage
{
  public static void Register(WebApplication app)
  {
    app.MapGet("/run", () =>
    {
      return Results.File("run.html", "text/html");
    }).RequireAuthorization();
  }
}