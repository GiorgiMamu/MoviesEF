using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.DTOs
{
    public class UpdateActorDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
