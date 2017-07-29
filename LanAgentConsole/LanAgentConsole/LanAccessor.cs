using System;
using System.Threading.Tasks;

namespace LanAgentConsole
{
    /// <summary>
    /// Accesses to a LAN controller.
    /// </summary>
    public abstract class LanAccessor
    {
        /// <summary>
        /// Gets or sets the URI to post the account information.
        /// </summary>
        public string LoginUri { get; set; }

        /// <summary>
        /// Gets or sets the URI to check whether the network is available.
        /// </summary>
        public string HeartbeatUri { get; set; }

        /// <summary>
        /// Check whether the login is required by the LAN that this class represents.
        /// </summary>
        /// <returns>A value that indicates whether the login is required by the LAN that this class represents.</returns>
        public abstract Task<bool> IsLoginRequiredAsync();

        /// <summary>
        /// Log in to the LAN.
        /// </summary>
        /// <param name="username">A username.</param>
        /// <param name="password">A password.</param>
        /// <returns>The result of the login.</returns>
        public abstract Task<LoginResult> LoginAsync(string username, string password);
    }

    /// <summary>
    /// Represents the result of a login.
    /// </summary>
    public enum LoginResult
    {
        LoginSucceeded,
        LoginFailed,
        AlreadyLoggedIn,
    }
}
