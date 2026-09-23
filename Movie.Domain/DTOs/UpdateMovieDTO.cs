using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.DTOs;

public class UpdateMovieDTO
{
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int StudioId { get; set; }
}