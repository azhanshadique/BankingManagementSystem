using BankingManagementSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace BankingManagementSystem.Helpers
{
    //public static class HttpClientProvider
    //{
    //    private static readonly string baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];

    //    private static readonly Lazy<HttpClient> httpClient = new Lazy<HttpClient>(() =>
    //    {
    //        var client = HttpClientFactory.Create(new JwtHttpClientHandler()); // automatically adds Bearer token from cookie
    //        client.BaseAddress = new Uri(baseApiUrl);
    //        return client;
    //    });

    //    public static HttpClient GetClient()
    //    {
    //        return httpClient.Value;
    //    }
    //}


    public static class HttpClientProvider
    {
        private static readonly string baseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];

        private static readonly HttpClient httpClient = HttpClientFactory.Create(new JwtHttpClientHandler());

        static HttpClientProvider()
        {
            httpClient.BaseAddress = new Uri(baseApiUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClient GetClient()
        {
            return httpClient;
        }
    }

}



