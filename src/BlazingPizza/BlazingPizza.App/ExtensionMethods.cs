using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazingPizza.App
{
    public static class ExtensionMethods
    {

        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string requestUri)
        {
            var value = await httpClient.GetStringAsync(requestUri);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static async Task<T> PostJsonAsync<T>(this HttpClient httpClient, string requestUri, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            var result = await httpClient.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

    }
}
