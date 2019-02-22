using RgImdbXF.Models;
using System;

namespace RgImdbXF.ViewModels
{
    public class MovieDetailViewModel : BaseViewModel
    {
        #region Variables
        //private static MovieService _movieService = null;
        #endregion

        #region Properties
        private MovieModel _movie = null;
        public MovieModel Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }
        #endregion

        #region Functions
        public MovieDetailViewModel(MovieModel movie = null/*, MovieService movieService*/)
        {
            //_movieService = movieService ?? throw new ArgumentException("movieService");
            Movie = movie ?? throw new ArgumentException("movie");
            Title = Movie.OriginalTitle;
        }
        #endregion
    }
}