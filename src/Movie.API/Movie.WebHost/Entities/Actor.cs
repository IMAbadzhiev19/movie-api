using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.WebHost.Entities;

public class Actor
{
    [Key]
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    
    public string MiddleName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public int Age { get; set; }

    [ForeignKey("MovieId")]
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}