using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LanAgentConsole
{
    class Program
    {
        static string Username { get; } = ConfigurationManager.AppSettings["Username"];
        static string Password { get; } = ConfigurationManager.AppSettings["Password"];

        static void Main(string[] args)
        {
            try
            {
                Main2().Wait();
            }
            catch (AggregateException ex) when (ex.InnerException is HttpRequestException)
            {
                Console.WriteLine("Network Error");
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        static async Task Main2()
        {
            var accessor = new WlcAccessor
            {
                LoginUri = "https://wlc.int.kcg.edu/login.html",
                HeartbeatUri = "http://www.kcg.ac.jp/images/index/center_line_bg.gif",
            };

            var isLoggedIn = await accessor.IsLoggedInAsync();
            if (isLoggedIn) return;

            var result = await accessor.LoginAsync(Username, Password);
            Console.WriteLine(result);
        }
    }
}
