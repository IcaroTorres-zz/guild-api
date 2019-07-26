using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
  public class Entity<TKey>
  {
    [Key]
    public TKey Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public object CreatedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
    public object ModifiedBy { get; set; }
  }
}