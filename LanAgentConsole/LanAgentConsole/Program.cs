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
            var accessors = new LanAccessor[]
            {
                new WlcAccessor
                {
                    LoginUri = "https://wlc.int.kcg.edu/login.html",
                    HeartbeatUri = "http://www.kcg.ac.jp/images/index/center_line_bg.gif",
                },
                new AdefAccessor
                {
                    LoginUri = "http://10.10.10.10:8080/cgi-bin/adeflogin.cgi",
                    HeartbeatUri = "http://www.kcg.ac.jp/images/index/center_line_bg.gif",
                },
            };

            var accessor = accessors[0];

            var isLoginRequired = await accessor.IsLoginRequiredAsync();
            if (!isLoginRequired) return;

            var result = await accessor.LoginAsync(Username, Password);
            Console.WriteLine(result);
        }
    }
}
