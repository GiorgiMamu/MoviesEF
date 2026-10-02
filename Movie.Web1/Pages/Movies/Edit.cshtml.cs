using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web1.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly IMovieService _movieService;

        public EditModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public UpdateMovieDTO Movie { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var existing = await _movieService.GetMovieByIdAsync(Id);

                Movie = new UpdateMovieDTO
                {
                    Title = existing.Title,
                    ReleaseYear = existing.ReleaseYear,
                    StudioId = existing.StudioId
                };
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _movieService.UpdateMovieAsync(Id, Movie);
                TempData["Success"] = "Movie updated successfully.";
                return RedirectToPage("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}