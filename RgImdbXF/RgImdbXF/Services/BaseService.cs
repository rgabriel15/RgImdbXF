using Newtonsoft.Json;
using RgImdbXF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RgImdbXF.Services
{
    internal abstract class BaseService<T> where T : BaseModel
    {
        #region Constants
        private const string ApiKey = "1f54bd990f1cdfb230adb312546d765d";
        private const string BaseUrl = @"http://api.themoviedb.org/3/";
        protected static readonly Uri BaseAddress = new Uri(BaseUrl);
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);
        protected const byte PageMin = 1;
        protected const ushort PageMax = 1000;
        #endregion

        #region Functions
        protected async Task<T> BaseGetAsync(
            string servicePath
            , int id)
        {
            servicePath = GetFormattedServiceName(servicePath);
            var requestUri = servicePath + '?' + "api_key=" + ApiKey + "&id=" + id.ToString();

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = Timeout;
                httpClient.BaseAddress = BaseAddress;

                var httpResponse = await httpClient.GetAsync(requestUri);

                if (httpResponse == null)
                {
                    throw new HttpRequestException();
                }

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var msg = JsonConvert.SerializeObject(httpResponse);
                    throw new HttpRequestException(msg);
                }

                var json = await httpResponse.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(json);
                return model;
            }
        }

        protected async Task<T> BaseGetAsync(
            string servicePath
            , Dictionary<string, object> queryCollection = null)
        {
            servicePath = GetFormattedServiceName(servicePath);
            var requestUri = servicePath + '?' + "api_key=" + ApiKey;

            if (queryCollection != null
                && queryCollection.Count > 0)
            {
                queryCollection =
                    queryCollection.Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value);
                var queryArray = queryCollection.Select(x => $"{x.Key}={x.Value.ToString()}").ToArray();
                requestUri += '&' + string.Join("&", queryArray);
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = Timeout;
                httpClient.BaseAddress = BaseAddress;

                var httpResponse = await httpClient.GetAsync(requestUri);

                if (httpResponse == null)
                {
                    throw new HttpRequestException();
                }

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var msg = JsonConvert.SerializeObject(httpResponse);
                    throw new HttpRequestException(msg);
                }

                var json = await httpResponse.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(json);
                return model;
            }
        }

        protected async Task<IEnumerable<T>> BaseGetCollectionAsync(
            string servicePath
            , Dictionary<string, object> queryCollection = null)
        {
            servicePath = GetFormattedServiceName(servicePath);
            var requestUri = servicePath + '?' + "api_key=" + ApiKey;

            if (queryCollection != null
                && queryCollection.Count > 0)
            {
                queryCollection =
                    queryCollection.Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value);
                var queryArray = queryCollection.Select(x => $"{x.Key}={x.Value.ToString()}").ToArray();
                requestUri += '&' + string.Join("&", queryArray);
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = Timeout;
                httpClient.BaseAddress = BaseAddress;

                var httpResponse = await httpClient.GetAsync(requestUri);

                if (httpResponse == null)
                {
                    throw new HttpRequestException();
                }

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var msg = JsonConvert.SerializeObject(httpResponse);
                    throw new HttpRequestException(msg);
                }

                var json = await httpResponse.Content.ReadAsStringAsync();
                var collection = JsonConvert.DeserializeObject<IEnumerable<T>>(json);
                return collection;
            }
        }

        private string GetFormattedServiceName(string servicePath)
        {
            if (string.IsNullOrWhiteSpace(servicePath))
                throw new ArgumentException("servicePath");

            servicePath = servicePath.Trim();
            return servicePath;
        }
        #endregion
    }
}
