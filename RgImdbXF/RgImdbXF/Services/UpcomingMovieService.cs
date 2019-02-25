using RgImdbXF.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RgImdbXF.Services
{
    internal sealed class UpcomingMovieService : BaseService<UpcomingMovieModel>
    {
        #region Constants
        private const string ServicePath = "movie/upcoming";
        #endregion

        #region Functions
        internal async Task<UpcomingMovieModel> GetAsync(uint page = 1)
        {
            if (page < PageMin
                || page > PageMax)
                throw new ArgumentException($"[page] must be an integer between {PageMin.ToString()} and {PageMax.ToString()}.", "page");

            var queryCollection = new Dictionary<string, object>
            {
                { "page", page }
            };

            var model = await BaseGetAsync(ServicePath, queryCollection);
            return model;
        }
        #endregion
    }
}
