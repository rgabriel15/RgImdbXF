using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Sing = RgImdbXF.Singleton.Singleton;

namespace RgImdbXF.Models
{
    /// <summary>
    /// https://developers.themoviedb.org/3/movies/get-movie-details
    /// </summary>
    public sealed class MovieModel : BaseModel
    {
        #region Constants
        private const string BaseImageUrl = @"https://image.tmdb.org/t/p/w185_and_h278_bestv2";
        #endregion

        #region Properties
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonIgnore]
        public string BackdropFullPath { get { return BaseImageUrl + BackdropPath; } }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonIgnore]
        public string PosterFullPath { get { return BaseImageUrl + PosterPath; } }

        [JsonProperty("genre_ids")]
        public IEnumerable<int> GenreIdCollection { get; set; }

        [JsonProperty("genres")]
        public IEnumerable<GenreModel> GenreCollection { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        private string genreCollectionString = null;
        [JsonIgnore]
        public string GenreCollectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(genreCollectionString))
                    genreCollectionString = GetGenreCollectionString();
                return genreCollectionString;
            }
        }
        #endregion

        #region Functions
        private string GetGenreCollectionString()
        {
            string ret = null;

            if (GenreCollection?.Count() > 0)
            {
                ret = string.Join(", ", GenreCollection.Select(x => x.Name));
            }
            else if (GenreIdCollection?.Count() > 0
                && Sing.GenreCollection?.Count() > 0)
            {
                var collection = Sing.GenreCollection
                    .Where(x => GenreIdCollection.Any(y => y == x.Id));
                ret = string.Join(", ", collection.Select(x => x.Name));
            }

            return ret;
        }
        #endregion
    }
}
