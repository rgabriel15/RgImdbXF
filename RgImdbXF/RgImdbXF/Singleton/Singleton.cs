using RgImdbXF.Models;
using RgImdbXF.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RgImdbXF.Singleton
{
    internal static class Singleton
    {
        #region Constants
        internal static readonly GenreService GenreService = null;
        internal static readonly SearchMovieService SearchMovieService = null;
        internal static readonly UpcomingMovieService UpcomingMovieService = null;
        #endregion

        #region Properties
        internal static IEnumerable<GenreModel> GenreCollection { get; private set; } = null;
        #endregion

        #region Functions
        static Singleton()
        {
            GenreService = new GenreService();
            SearchMovieService = new SearchMovieService();
            UpcomingMovieService = new UpcomingMovieService();
        }

        internal static async Task InitAsync()
        {
            var genreCollection = await GenreService.GetAsync();
            GenreCollection = genreCollection?.GenreCollection;
        }
        #endregion
    }
}
