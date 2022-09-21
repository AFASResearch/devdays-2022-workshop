public class KvkOrganization
{
  public string? vestigingsnummer { get; set; }
  public string? kvkNummer { get; set; }
  public string? rsin { get; set; }
  public List<KvkHandelsnaam>? handelsnamen { get; set; }
  public string? naam { get; set; }
}

public class KvkHandelsnaam
{
  public string? naam { get; set; }
  public int? volgorde { get; set; }
}
