namespace Application.Hateoas.DTOs
{
  public class LinkDTO
  {
    public LinkDTO(string rel, string href, string method)
    {
      Rel = rel;
      Href = href;
      Method = method;
    }
    public string Href { get; }
    public string Rel { get; }
    public string Method { get; }
  }
}
