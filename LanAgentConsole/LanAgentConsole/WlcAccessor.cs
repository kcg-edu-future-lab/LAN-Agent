﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LanAgentConsole
{
    /// <summary>
    /// Accesses to the Cisco Wireless LAN Controller (WLC).
    /// </summary>
    public class WlcAccessor : LanAccessor
    {
        /// <summary>
        /// Check whether the login is required by the LAN that this class represents.
        /// </summary>
        /// <returns>A value that indicates whether the login is required by the LAN that this class represents.</returns>
        /// <exception cref="HttpRequestException">In a network error.</exception>
        public override async Task<bool> IsLoginRequiredAsync()
        {
            using (var http = new HttpClient())
            {
                // Throws HttpRequestException in a network error.
                var response = await http.GetAsync(HeartbeatUri);

                return response.Headers.Location?.OriginalString.StartsWith(LoginUri) == true;
            }
        }

        /// <summary>
        /// Log in to the LAN.
        /// </summary>
        /// <param name="username">A username.</param>
        /// <param name="password">A password.</param>
        /// <returns>The result of the login.</returns>
        /// <exception cref="HttpRequestException">In a network error.</exception>
        /// <exception cref="InvalidOperationException">In an unexpected error.</exception>
        public override async Task<LoginResult> LoginAsync(string username, string password)
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
}
