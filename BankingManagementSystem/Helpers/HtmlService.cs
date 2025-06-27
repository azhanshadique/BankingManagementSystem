using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BankingManagementSystem.Helpers
{
	public class HtmlService
	{
        public static string GetPageHtmlFromApi()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:44366/api/client/login";
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string html = response.Content.ReadAsStringAsync().Result;
                    return html;
                }
                else
                {
                    return $"<div>Error loading page: {response.StatusCode}</div>";
                }
            }
        }

    }
}


