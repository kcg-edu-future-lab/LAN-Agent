using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WlcAgentConsole
{
    class Program
    {
        const string LoginUri = "https://wlc.int.kcg.edu/login.html";
        const string HeartbeatUri = "http://www.kcg.ac.jp/images/index/center_line_bg.gif";

        static void Main(string[] args)
        {
            var r = IsLoginRequired();
            r.Wait();
        }

        /// <summary>
        /// Check whether the login is required by WLC.
        /// </summary>
        /// <returns>A value that indicates whether the login is required by WLC.</returns>
        /// <exception cref="HttpRequestException">In network error.</exception>
        public static async Task<bool> IsLoginRequired()
        {
            using (var http = new HttpClient())
            {
                // Throws HttpRequestException in network error.
                var response = await http.GetAsync(HeartbeatUri);

                return response.Headers.Location?.OriginalString.StartsWith(LoginUri) == true;
            }
        }
    }
}
