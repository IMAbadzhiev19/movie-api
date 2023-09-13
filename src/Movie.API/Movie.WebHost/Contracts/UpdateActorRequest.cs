namespace Movie.WebHost.Contracts;

public class UpdateActorRequest
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string MiddleName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int Age { get; set; }
}