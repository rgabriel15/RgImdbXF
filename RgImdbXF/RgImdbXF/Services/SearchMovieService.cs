using RgImdbXF.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RgImdbXF.Services
{
    /// <summary>
    /// https://developers.themoviedb.org/3/search/search-movies
    /// </summary>
    internal sealed class SearchMovieService : BaseService<SearchMovieModel>
    {
        #region Constants
        private const string ServicePath = "search/movie";
        #endregion

        #region Functions
        internal async Task<SearchMovieModel> GetAsync(string query, uint page = 1)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("query");

            if (page < PageMin
                || page > PageMax)
                throw new ArgumentException($"[page] must be an integer between {PageMin.ToString()} and {PageMax.ToString()}.", "page");

            var queryCollection = new Dictionary<string, object>
            {
                { "query", query.Trim() }
                , { "page", page }
            };

            var model = await BaseGetAsync(ServicePath, queryCollection);
            return model;
        }
        #endregion
    }
}

