using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Movie.WebHost.Contracts;

public class ActorResponse
{
    [Key]
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string MiddleName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int Age { get; set; }

    [ForeignKey("MovieId")]
    public ICollection<Entities.Movie> Movies { get; set; } = new List<Entities.Movie>();
}