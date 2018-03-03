using System.Diagnostics;
using MMBuddy.Dtos;
using System.Management;

namespace MMBuddy.Services
{
    public static class LeagueProcess
    {
        public static ServerInfo Initialize()
        {
            // Look for the client
            var process = Process.GetProcessesByName("LeagueClientUx");
            if (process.Length == 0 || process == null)
                return null;

            // Prepare a return object
            ServerInfo serverInfo = new ServerInfo();

            // Parse the command line arguments
            using (var searcher = new ManagementObjectSearcher(
                "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process[0].Id))
            {
                foreach (var @object in searcher.Get())
                {
                    string arguments = @object["CommandLine"].ToString();
                    string[] argumentArray = arguments.Split('"');

                    foreach(string argument in argumentArray)
                    {
                        if(argument.Contains("--remoting-auth-token"))
                        {
                            serverInfo.Token = argument.Split('=')[1];
                        }
                        if(argument.Contains("--app-port"))
                        {
                            serverInfo.Port = argument.Split('=')[1];
                        }
                        if(argument.Contains("--release"))
                        {
                            serverInfo.Release = argument.Split('=')[1];
                        }
                    }
                }
            }

            return serverInfo;
        }
    }
}
