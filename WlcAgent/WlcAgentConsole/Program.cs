using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WlcAgentConsole
{
    class Program
    {
        static string LoginUri { get; } = ConfigurationManager.AppSettings["LoginUri"];
        static string HeartbeatUri { get; } = ConfigurationManager.AppSettings["HeartbeatUri"];

        static string Username { get; } = ConfigurationManager.AppSettings["Username"];
        static string Password { get; } = ConfigurationManager.AppSettings["Password"];

        static void Main(string[] args)
        {
            var r = IsLoginRequired();
            r.Wait();
            if (!r.Result) return;

            var l = Login(Username, Password);
            l.Wait();
            Console.WriteLine(l.Result);
        }

        /// <summary>
        /// Check whether the login is required by WLC.
        /// </summary>
        /// <returns>A value that indicates whether the login is required by WLC.</returns>
        /// <exception cref="HttpRequestException">In a network error.</exception>
        public static async Task<bool> IsLoginRequired()
        {
            using (var http = new HttpClient())
            {
                // Throws HttpRequestException in a network error.
                var response = await http.GetAsync(HeartbeatUri);

                return response.Headers.Location?.OriginalString.StartsWith(LoginUri) == true;
            }
        }

        /// <summary>
        /// Log in to WLC.
        /// </summary>
        /// <param name="username">A username.</param>
        /// <param name="password">A password.</param>
        /// <returns>The result of the login.</returns>
        /// <exception cref="HttpRequestException">In a network error.</exception>
        /// <exception cref="InvalidOperationException">In an unexpected error.</exception>
        public static async Task<LoginResult> Login(string username, string password)
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
                // Throws HttpRequestException in a network error.
                var response = await http.PostAsync(LoginUri, content);
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException("Unexpected Error.");

                var responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody.Contains("Login Successful"))
                    return LoginResult.LoginSucceeded;
                if (responseBody.Contains("Login Error"))
                    return LoginResult.LoginFailed;
                if (responseBody.Contains("Web Authentication Failure"))
                    return LoginResult.AlreadyLoggedIn;
            }

            throw new InvalidOperationException("Unexpected Error.");
        }
    }

    public enum LoginResult
    {
        LoginSucceeded,
        LoginFailed,
        AlreadyLoggedIn,
    }
}
