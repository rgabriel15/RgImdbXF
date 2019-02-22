using Newtonsoft.Json;

namespace RgImdbXF.Models
{
    public abstract class BaseModel
    {
        #region Properties
        [JsonProperty("id")]
        public int Id { get; set; }
        #endregion
    }
}

