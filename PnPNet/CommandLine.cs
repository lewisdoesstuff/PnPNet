using System;
using System.Diagnostics;

namespace PnPNet
{
    class CommandLine
    {
        static int PnP(string cmd)
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
    }
}