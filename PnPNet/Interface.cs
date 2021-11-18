using System;
using System.Collections.Generic;
using System.Linq;

namespace PnPNet
{
    public class Interface
    {
        public string Path { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
        public string Reference { get; set; }
        public string InstanceId { get; set; }
        public string Status { get; set; }
    }

    public static class Interfaces
    {
        /// <summary>
        /// pnputil /enum-devices [/enabled | /disabled] [/class &lt;GUID&gt;]
        /// Enumerate all device interfaces on the system.
        /// </summary>
        /// <param name="enabled">Enumerate only enabled interfaces on the system (Mutually exclusive with disabled)</param>
        /// <param name="disabled">Enumerate only disabled interfaces on the system (Mutually exclusive with enabled)</param>
        /// <param name="classGuid">Enumerate all interfaces with specific interface class GUID</param>
        /// <returns>List&lt;Interface&gt; of all interfaces on the system</returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<Interface> EnumInterfaces(bool enabled = false, bool disabled = false,
            string classGuid = null)
        {
            var command = "/enum-interfaces ";
            if (enabled ^ disabled) command += "/enabled ";
            if (disabled ^ enabled) command += "/disabled ";
            if (enabled && disabled) throw new ArgumentException("Enabled and Disabled can not both be true.");
            if (!string.IsNullOrWhiteSpace(classGuid)) command += $"/class {classGuid} ";

            List<string> output = CommandLine.PnPString(command).Split(Environment.NewLine).ToList();
            List<Interface> interfaces = new List<Interface>();
            Interface device = new Interface();

            var i = 0;
            foreach (var line in output)
            {
                i++;
                if (i < 2) continue;

                if (string.IsNullOrWhiteSpace(line))
                {
                    interfaces.Add(device);
                    continue;
                }

                if (line.StartsWith("Interface Path")) device.Path = Functions.Trim(line);
                if (line.StartsWith("Interface Description")) device.Description = Functions.Trim(line);
                if (line.StartsWith("Interface Class")) device.Guid = Functions.Trim(line);
                if (line.StartsWith("Reference")) device.Reference = Functions.Trim(line);
                if (line.StartsWith("Device Instance")) device.InstanceId = Functions.Trim(line);
                if (line.StartsWith("Interface Status")) device.Status = Functions.Trim(line);
            }

            return interfaces;
        }
    }
}