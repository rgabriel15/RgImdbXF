using Newtonsoft.Json;
using System.Collections.Generic;

namespace RgImdbXF.Models
{
    /// <summary>
    /// https://developers.themoviedb.org/3/genres/get-movie-list
    /// </summary>
    public sealed class GenreCollectionModel : BaseModel
    {
        #region Properties
        [JsonProperty("genres")]
        public IEnumerable<GenreModel> GenreCollection { get; set; }
        #endregion
    }
}

