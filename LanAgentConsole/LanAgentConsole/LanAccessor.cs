using System;
using System.Threading.Tasks;

namespace LanAgentConsole
{
    public abstract class LanAccessor
    {
        public string LoginUri { get; set; }
        public string HeartbeatUri { get; set; }

        /// <summary>
        /// Check whether the user is already logged in to the LAN.
        /// </summary>
        /// <returns>A value that indicates whether the user is already logged in to the LAN.</returns>
        public abstract Task<bool> IsLoggedInAsync();

        /// <summary>
        /// Log in to the LAN.
        /// </summary>
        /// <param name="username">A username.</param>
        /// <param name="password">A password.</param>
        /// <returns>The result of the login.</returns>
        public abstract Task<LoginResult> LoginAsync(string username, string password);
    }

    public enum LoginResult
    {
        LoginSucceeded,
        LoginFailed,
        AlreadyLoggedIn,
    }
}
