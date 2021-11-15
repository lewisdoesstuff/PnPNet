using System;
using System.Diagnostics;

namespace PnPNet
{
    internal static class CommandLine
    {
        internal static int PnP(string cmd)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                FileName = "cmd.exe",
                RedirectStandardError = false,
                RedirectStandardInput = false,
                UseShellExecute = false,
                Arguments = $"/C pnputil.exe {cmd}"
            };

            p.Start();
            p.WaitForExit();
            return p.ExitCode;
        }

        internal static string PnPString(string cmd)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                FileName = "cmd.exe",
                RedirectStandardError = false,
                RedirectStandardInput = false,
                UseShellExecute = false,
                Arguments = $"/C pnputil.exe {cmd}"
            };

            p.Start();
            string stdout = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return stdout;
        }
    }
}