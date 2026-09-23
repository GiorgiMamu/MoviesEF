namespace Movie.Domain.DTOs;

public class ActorDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<string> MovieTitles { get; set; } = new List<string>();
}