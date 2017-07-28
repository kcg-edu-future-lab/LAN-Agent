using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanAgentConsole
{
    class Program
    {
        static string Username { get; } = ConfigurationManager.AppSettings["Username"];
        static string Password { get; } = ConfigurationManager.AppSettings["Password"];

        static void Main(string[] args)
        {
        }
    }
}
