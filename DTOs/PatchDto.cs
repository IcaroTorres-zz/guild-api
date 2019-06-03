using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
  public class PatchDto
  {
    public string PropertyName { get; set; }
    public string PropertyValue { get; set; }
    // public PatchAction Action { get; set; }
  }
  // public enum PatchAction
  // {
  //     Add,
  //     Remove,
  //     Transfer
  // }
}
