using System;
using System.Collections.Generic;
using System.Linq;

namespace PnPNet
{
    public class Device
    {
        public string InstanceId { get; set; } 
        public string Description { get; set; }
        public string ClassName { get; set; }
        public string Guid { get; set; }
        public string Manufacturer { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
    }

    public static class Devices
    {
        /// <summary>
        /// pnputil /enum-devices [/connected | /disconnected] [/instanceid &lt;instance ID &gt;] [/class &lt;name | GUID&gt;] [/problem [&lt;code&gt;]] [/deviceids] [/bus &lt;name&gt;] [/relations] [/drivers] [/stack] [/interfaces] [/properties]
        /// Enumerate all devices on the system
        /// </summary>
        /// <param name="connected">Filter by connected devices (Mutually exclusive with disconnected)</param>
        /// <param name="disconnected">Filter by disconnected devices (Mutually exclusive with connected)</param>
        /// <param name="instanceid">Filter by device instance ID</param>
        /// <param name="classname">Filter by device class name</param>
        /// <param name="problem">Filter by devices with problems</param>
        /// <param name="problemcode">Filter by specific problem codes. (implies problem=true)</param>
        /// <param name="deviceids">Display hardware/compatible IDs</param>
        /// <param name="bus">Display bus enumerator name and bus type GUID or filter by bus enumerator name or bus type GUID</param>
        /// <param name="relations">Display parent and child device relations</param>
        /// <param name="drivers">Display matching and installed drivers</param>
        /// <param name="stack">Display device stack information</param>
        /// <param name="interfaces">Display device interfaces</param>
        /// <param name="properties">Display all device properties</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<Device> EnumDevices(bool connected = false, bool disconnected = false, 
            string instanceid = null, string classname = null,bool problem = false, string problemcode = null, bool deviceids = false,
            string bus = null, bool relations = false, bool drivers = false, bool stack = false,
            bool interfaces = false, bool properties = false)
        {
            var command = "/enum-devices ";
            if (connected ^ disconnected) command += "/connected ";
            if (disconnected ^ connected) command += "/disconnected ";
            if (connected && disconnected)
                throw new ArgumentException("Connected and Disconnected can not both be true.");
            if (!string.IsNullOrWhiteSpace(instanceid)) command += $"/instanceid {instanceid} ";
            if (!string.IsNullOrWhiteSpace(classname)) command += $"/class {classname} ";
            if (problem) command += "/problem ";
            if (!string.IsNullOrWhiteSpace(problemcode)) command += $"/problem {problem} ";
            if (deviceids) command += "/deviceids ";
            if (!string.IsNullOrWhiteSpace(bus)) command += $"/bus {bus} ";
            if (relations) command += "/relations ";
            if (drivers) command += "/drivers ";
            if (stack) command += "/stack ";
            if (interfaces) command += "/interfaces ";
            if (properties) command += "/properties ";

            Device device = new Device();
            List<Device> devices = new List<Device>();
            
            List<string> output = CommandLine.PnPString(command).Split(Environment.NewLine).ToList();

            var i = 0;
            foreach (var line in output)
            {
                i++;
                if (i < 2) continue;
                
                if (string.IsNullOrWhiteSpace(line))
                {
                    devices.Add(device);
                    continue;
                }

                if (line.StartsWith("Instance")) device.InstanceId = Functions.Trim(line);
                if (line.StartsWith("Device")) device.Description = Functions.Trim(line);
                if (line.StartsWith("Class Name")) device.ClassName = Functions.Trim(line);
                if (line.StartsWith("Class GUID")) device.Guid = Functions.Trim(line);
                if (line.StartsWith("Manufacturer")) device.Manufacturer = Functions.Trim(line);
                if (line.StartsWith("Status")) device.Status = Functions.Trim(line);
                if (line.StartsWith("Driver")) device.Name = Functions.Trim(line);

            }

            return devices;
        }
    }
}