using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web1.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService _movieService;

        public ICollection<MovieDTO> Movies { get; set; } = new List<MovieDTO>();

        public IndexModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [BindProperty]
        public int Year { get; set; }

        [BindProperty]
        public string StudioName { get; set; } = string.Empty;

        [BindProperty]
        public int MinimumActorCount { get; set; }

        public async Task OnGetAsync()
        {
            Movies = await _movieService.GetAllMoviesAsync();
        }

        public async Task OnPostSearchAsync()
        {
            try
            {
                var searchResults = await _movieService.SearchMoviesByStudioAsync(
                    Year, StudioName, MinimumActorCount);

                Movies = searchResults
                    .Select(x => new MovieDTO
                    {
                        Id = x.Id,
                        Title = x.Title,
                        StudioName = x.StudioName,
                        ReleaseYear = x.ReleaseYear
                    })
                    .ToList();
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                Movies = await _movieService.GetAllMoviesAsync();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                TempData["Success"] = "Movie deleted successfully.";
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}