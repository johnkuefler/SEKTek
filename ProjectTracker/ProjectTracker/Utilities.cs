using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker
{
    public static class Utilities
    {
        public static async Task<string> WebServiceRequest(string url)
        {
            string output = "";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                output = await content.ReadAsStringAsync();
            }

            return output;
        }
    }
}
