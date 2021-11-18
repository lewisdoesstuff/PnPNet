using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PnPNet
{
    public class Driver
    {
        public string PublishedName { get; set; }
        public string OriginalName { get; set; }
        public string ProviderName { get; set; }
        public string ClassName { get; set; }
        public string Guid { get; set; }
        public string Version { get; set; }
        public string Signer { get; set; }
    }

    public static class Drivers
    {
        /// <summary>
        /// pnputil /add-driver &lt;filename.inf | *.inf&gt; [/subdirs] [/install] [/reboot]
        /// Adds a given driver to the system driver store.
        /// </summary>
        /// <param name="path">Path to .inf file or directory (subdirs = true)</param>
        /// <param name="subdirs">Search subdirectories for driver packages</param>
        /// <param name="install">Install/update drivers on matching devices</param>
        /// <param name="reboot">Reboot the system if needed to complete the install.</param>
        /// <returns>PnPUtil.exe exit code</returns>
        /// <exception cref="FormatException">When subdirs=true, path must be a path to a directory, not to a file.</exception>
        public static int Add(string path, bool subdirs = false, bool install = false, bool reboot = false)
        {
            var command = $"/add-driver {path} ";
            if (subdirs && File.GetAttributes(path) != FileAttributes.Directory)
            {
                throw new FormatException("Path must be a path to a directory when subdirs is true.");
            }
            if (install) command += "/install ";
            if (reboot) command += "/reboot ";

            return CommandLine.PnP(command);
        }

        /// <summary>
        /// pnputil /delete-driver &lt;oem#.inf&gt; [/uninstall] [/force] [/reboot]
        /// Deltes a given driver package from the driver store.
        /// </summary>
        /// <param name="path">Path to oem driver file</param>
        /// <param name="uninstall">Uninstall driver package from any devices using it</param>
        /// <param name="force">Force uninstallation, even if in use by other devices</param>
        /// <param name="reboot">Reboot the system if needed to complete the uninstall</param>
        /// <returns>PnPUtil.exe exit code</returns>
        public static int Delete(string path, bool uninstall = false, bool force = false, bool reboot = false)
        {
            var command = $"/delete-driver {path}";
            if (uninstall) command += "/uninstall ";
            if (force) command += "/force ";
            if (reboot) command += "/reboot ";

            return CommandLine.PnP(command);
        }

        /// <summary>
        /// pnputil /enum-drivers
        /// Returns a List&lt;Driver&gt; of all 3rd party drivers in the driver store.
        /// </summary>
        /// <returns>A List&lt;Driver&gt; of all drivers (3rd party) in the driver store.</returns>
        public static List<Driver> EnumDrivers()
        {
            List<string> output = CommandLine.PnPString("/enum-drivers").Split(Environment.NewLine).ToList();
            Driver driver = new Driver();
            List<Driver> drivers = new List<Driver>();
            
            var i = 0;
            foreach (var line in output)
            {
                i++;
                if (i < 2) continue;
                
                if (string.IsNullOrWhiteSpace(line))
                {
                    drivers.Add(driver);
                    continue;
                }

                if (line.StartsWith("Published")) driver.PublishedName = Functions.Trim(line);
                if (line.StartsWith("Original")) driver.OriginalName = Functions.Trim(line);
                if (line.StartsWith("Provider")) driver.ProviderName = Functions.Trim(line);
                if (line.StartsWith("Class Name")) driver.ClassName = Functions.Trim(line);
                if (line.StartsWith("Class GUID")) driver.Guid = Functions.Trim(line);
                if (line.StartsWith("Driver")) driver.Version = Functions.Trim(line);
                if (line.StartsWith("Signer")) driver.Signer = Functions.Trim(line);
            }

            return drivers;
        }

        /// <summary>
        /// pnputil /disable-device &lt;instance ID&gt; | /deviceid &lt;device ID&gt; [/reboot]
        /// Disable a device on the system.
        /// </summary>
        /// <param name="id">Instance or Device (with deviceid = true) ID of the device to be disabled</param>
        /// <param name="deviceid">Disable all devices with the matching device ID</param>
        /// <param name="reboot">Reboot the system if needed to complete the operation</param>
        /// <returns>PnPUtil.exe exit code</returns>
        public static int Disable(string id, bool deviceid = false, bool reboot = false)
        {
            var command = "/disable-device ";
            if (deviceid) command += "/deviceid ";
            command += $"{id}";
            if (reboot) command += "/reboot";

            return CommandLine.PnP(command);
        }

        /// <summary>
        /// pnputil /enable-device &lt;instance ID&gt; | /deviceid &lt;device ID&gt; [/reboot]
        /// Enable a device on the system.
        /// </summary>
        /// <param name="id">Instance or Device (with deviceid = true) ID of the device to be enabled</param>
        /// <param name="deviceid">Disable all devices with the matching device ID</param>
        /// <param name="reboot">Reboot the system if needed to complete the operation</param>
        /// <returns>PnPUtil.exe exit code</returns>
        public static int Enable(string id, bool deviceid = false, bool reboot = false)
        {
            var command = "/enable-device ";
            if (deviceid) command += "/deviceid ";
            command += $"{id}";
            if (reboot) command += "/reboot";

            return CommandLine.PnP(command);
        }
        
        /// <summary>
        /// pnputil /restart-device &lt;instance ID&gt; | /deviceid &lt;Device ID&gt; [/reboot]
        /// Restart a device on the system.
        /// </summary>
        /// <param name="id">Instance or Device (with deviceid = true) ID of the device to be restarted</param>
        /// <param name="deviceid">Restart all devices with the matching device ID</param>
        /// <param name="reboot">Reboot the system if needed to complete the operation</param>
        /// <returns>PnPUtil.exe exit code</returns>
        public static int Restart(string id, bool deviceid = false, bool reboot = false)
        {
            var command = "/restart-device ";
            if (deviceid) command += "/deviceid ";
            command += $"{id}";
            if (reboot) command += "/reboot";

            return CommandLine.PnP(command);
        }

        /// <summary>
        /// pnputil /remove-device &lt;instance ID&gt; | /deviceid &lt;device ID&gt; [/subtree] [/reboot]
        /// Attempt to remove a device from the system.
        /// </summary>
        /// <param name="id">Instance or Device (with deviceid = true) ID of the device to be removed</param>
        /// <param name="deviceid">Remove all devices with the matching device ID</param>
        /// <param name="subtree"></param>
        /// <param name="reboot">Reboot the system if needed to complete the operation</param>
        /// <returns>PnPUtil.exe exit code</returns>
        public static int Remove(string id, bool deviceid = false, bool subtree = false, bool reboot = false)
        {
            var command = "/remove-device ";
            if (deviceid) command += "/deviceid ";
            command += $"{id}";
            if (subtree) command += "/subtree";
            if (reboot) command += "/reboot";

            return CommandLine.PnP(command);
        }

        /// <summary>
        /// pnputil /scan-devices [/instanceid &lt;instance ID&gt;] [/async]
        /// Scan the system for any device hardware changes.
        /// </summary>
        /// <param name="instance">Scan the device subtree for changes</param>
        /// <param name="async">Scan for changes asynchronously</param>
        /// <returns></returns>
        public static int Scan(string instance = null, bool async = false)
        {
            var command = "/scan-devices ";
            if (!string.IsNullOrWhiteSpace(instance)) command += $"/instanceid {instance} ";
            if (async) command += "/async ";

            return CommandLine.PnP(command);
        }


    }
}