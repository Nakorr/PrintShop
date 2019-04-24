using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace PrintView
{
    public static class APICustomer
    {
        private static HttpClient Customer = new HttpClient();
        public static void Connect()
        {
            Customer.BaseAddress = new Uri(ConfigurationManager.AppSettings["IPAddress"]);
            Customer.DefaultRequestHeaders.Accept.Clear();
            Customer.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public static T GetRequest<T>(string requestUrl)
        {
            var response = Customer.GetAsync(requestUrl);
            if (response.Result.IsSuccessStatusCode)
            {
                return response.Result.Content.ReadAsAsync<T>().Result;
            }
            throw new Exception(response.Result.Content.ReadAsStringAsync().Result);
            
        }
        public static U PostRequest<T, U>(string requestUrl, T model)
        {
            var response = Customer.PostAsJsonAsync(requestUrl, model);
            if (response.Result.IsSuccessStatusCode)
            {
                if (typeof(U) == typeof(bool))
                {
                    return default(U);
                }
                return response.Result.Content.ReadAsAsync<U>().Result;
            }
            throw new Exception(response.Result.Content.ReadAsStringAsync().Result);
        }
    }
}