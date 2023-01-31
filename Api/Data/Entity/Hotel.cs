using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entity;

public class Hotel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required double Rating { get; set; }
    public int CountryId { get; set; }
    [Required] public Country? Country { get; set; }
}