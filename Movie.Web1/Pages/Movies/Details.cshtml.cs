using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web1.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly IMovieService _movieService;

        public DetailsModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public MovieDTO Movie { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Movie = await _movieService.GetMovieByIdAsync(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return Page();
        }
    }
}