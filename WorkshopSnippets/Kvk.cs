public class KvkSnippets
{
  public async Task<bool> FindByKvk(string kvkNummer, string apiKey, Func<string, Task> Log)
  {
    using var kvkClient = new HttpClient();
    kvkClient.DefaultRequestHeaders.Add("apikey", apiKey);

    var url = string.Format("https://api.kvk.nl/api/v1/basisprofielen/{0}/hoofdvestiging", kvkNummer);
    await Log("Querying " + url);

    try {
      var result = await kvkClient.GetFromJsonAsync<KvkOrganization>(url);
      var name = result!.naam;
      if (result!.handelsnamen != null) 
      {
        name = string.Join(", ", result!.handelsnamen!.OrderBy(h => h.volgorde).Select(h => h.naam));
      }
      await Log("Found " + name);
    } 
    catch (HttpRequestException e)
    {
      if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        await Log("Not Found");
        return false;
      }
      else
      {
        throw;
      }
    }
    return true;
  }
}