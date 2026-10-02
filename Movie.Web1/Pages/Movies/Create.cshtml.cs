using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Domain.DTOs;
using Movie.Service.Interfaces;

namespace Movie.Web1.Pages.Movies
{
    public class CreateModel : PageModel
    {

        private readonly IMovieService _movieService;

        public CreateModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public CreateMovieDTO Movie { get; set; } = new();


        public async Task OnPostAsync()
        {
            try
            {


                await _movieService.AddMovieAsync(Movie);
                TempData["Success"] = "Movie created successfully.";

                Movie = new();
                ModelState.Clear();


            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }


    }
}