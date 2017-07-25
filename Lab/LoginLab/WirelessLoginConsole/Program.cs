using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WirelessLoginConsole
{
    class Program
    {
        static string LoginUri { get; } = ConfigurationManager.AppSettings["LoginUri"];
        static string Username { get; } = ConfigurationManager.AppSettings["Username"];
        static string Password { get; } = ConfigurationManager.AppSettings["Password"];

        static void Main(string[] args)
        {
            Login(Username, Password).Wait();
        }

        public static async Task Login(string username, string password)
        {
            var data = new Dictionary<string, string>
            {
                { "buttonClicked", "4" },
                { "err_flag", "0" },
                { "err_msg", "" },
                { "info_flag", "0" },
                { "info_msg", "" },
                { "redirect_url", "" },
                { "username", username },
                { "password", password },
            };
            var content = new FormUrlEncodedContent(data);

            using (var http = new HttpClient())
            {
                await http.PostAsync(LoginUri, content);
            }
        }
    }
}
