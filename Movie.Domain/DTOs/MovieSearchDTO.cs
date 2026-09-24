namespace Movie.Domain.DTOs;

public class MovieSearchDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string StudioName { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public int ActorCount { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Title} | Year: {ReleaseYear} | Studio: {StudioName} | Country: {CountryName} | Actors: {ActorCount}";
    }
}