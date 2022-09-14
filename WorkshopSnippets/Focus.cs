public class FocusSnippets
{
  public async Task AccessToken(HttpContext context, Func<string, Task> Log)
  {
    var accessToken = await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.GetTokenAsync(context, "access_token");
    await Log("AccessToken " + accessToken);

    var focusClient = new HttpClient();
    focusClient.DefaultRequestHeaders.Add("AccessToken", accessToken);
  }

  public async Task QueryOrganizations(HttpContext context, Func<string, Task> Log, HttpClient focusClient)
  {
    var getOrganizationsUrl = FocusOAuthHandler.GetAfasBaseUrl(context) + "/api/organisaties-workshop";
    await Log("Querying " + getOrganizationsUrl);
    var organizations = await focusClient.GetFromJsonAsync<List<FocusOrganization>>(getOrganizationsUrl);
    await Log(string.Format("Found {0} results", organizations.Count));
  }
}