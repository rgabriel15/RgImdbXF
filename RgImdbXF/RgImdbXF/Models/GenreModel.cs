using Newtonsoft.Json;

namespace RgImdbXF.Models
{
    /// <summary>
    /// https://developers.themoviedb.org/3/genres/get-movie-list
    /// </summary>
    public sealed class GenreModel : BaseModel
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; }
        #endregion
    }
}
