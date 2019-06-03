using System.ComponentModel.DataAnnotations;

namespace api.Models
{
  public interface IEntity<TKey>
  {
    [Key]
    TKey Id { get; set; }
  }
}