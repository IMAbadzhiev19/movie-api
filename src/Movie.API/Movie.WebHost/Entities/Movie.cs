using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.WebHost.Entities;

public class Movie
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new ();

    [ForeignKey("ActorId")]
    public ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public DateTime ReleasedOnUtc { get; set; }
}
