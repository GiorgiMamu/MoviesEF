using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.DTOs
{
    public class UpdateActorMovieDTO
    {
        public ICollection<int> MovieIds { get; set; } = new List<int>();
    }
}
