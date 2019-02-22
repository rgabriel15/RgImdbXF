using Newtonsoft.Json;
using System.Collections.Generic;

namespace RgImdbXF.Models
{
    /// <summary>
    /// https://developers.themoviedb.org/3/movies/get-upcoming
    /// </summary>
    public class UpcomingMovieModel : BaseModel
    {
        #region Properties
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("results")]
        public IEnumerable<MovieModel> MovieCollection { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
        #endregion
    }
}
