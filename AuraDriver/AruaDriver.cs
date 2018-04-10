/**
 * Copyright (c) 2018, Andy Janata
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted
 * provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice, this list of conditions
 *   and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice, this list of
 *   conditions and the following disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
 * WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ajanata.AuraDriver
{
    /// <summary>
    /// Aura driver implementation. Needs a lot of logging and eror handling still.
    /// </summary>
    [Guid("99074DB3-287C-461A-AAC9-4A9FAAD8A508")]
    [ComVisible(true)]
    public class AuraDriver : IAuraDriver
    {
        private readonly static string LOG_SOURCE = "AuraDriver.NET";

        private void ensureLog()
        {
            if (!EventLog.SourceExists(LOG_SOURCE))
            {
                EventLog.CreateEventSource(LOG_SOURCE, "Application");
            }
        }

        private void Log(string msg)
        {
            EventLog.WriteEntry(LOG_SOURCE, msg, EventLogEntryType.Information);
        }

        public int[] GetMotherboardControllers()
        {
            ensureLog();

            int count = Wrapper.EnumerateMbController(IntPtr.Zero, 0);
            // TODO error handling
            int[] ret = new int[count];

            IntPtr handles = Marshal.AllocHGlobal(count * IntPtr.Size);
            int err = Wrapper.EnumerateMbController(handles, count);
            // TODO error handling
            Marshal.Copy(handles, ret, 0, count);
            Marshal.FreeHGlobal(handles);

            //EventLog.WriteEntry(LOG_SOURCE, "GetMotherboardControllers found " + count, EventLogEntryType.Information);
            return ret;
        }

        public void SetMotherboardApplicationMode(int handle)
        {
            // TODO error handling
            Wrapper.SetMbMode(new IntPtr(handle), 1);
        }

        public void SetMotherboardDefaultMode(int handle)
        {
            // TODO error handling
            Wrapper.SetMbMode(new IntPtr(handle), 0);
        }

        public int GetMotherboardLedCount(int handle)
        {
            // TODO error handling? says it returns 0 on error, but that's effectively a no-op success
            return Wrapper.GetMbLedCount(new IntPtr(handle));
        }

        public byte[] GetMotherboardColors(int handle)
        {
            int ledCount = GetMotherboardLedCount(handle);

            // 3 bytes per LED for color
            IntPtr p = Marshal.AllocHGlobal(ledCount * 3);
            // TODO error handling
            Wrapper.GetMbColor(new IntPtr(handle), p, ledCount);
            byte[] colors = new byte[ledCount * 3];
            Marshal.Copy(p, colors, 0, ledCount * 3);
            Marshal.FreeHGlobal(p);
            return colors;
        }

        public void SetMotherboardColors(int handle, byte[] colors)
        {
            int ledCount = GetMotherboardLedCount(handle);
            // this order to avoid int division truncation issues
            if (ledCount * 3 != colors.Length)
            {
                throw new ArgumentException("Incorrect number of colors for LEDs on controller.");
            }

            byte[] bytes = colors;
            IntPtr p = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, p, bytes.Length);
            Wrapper.SetMbColor(new IntPtr(handle), p, bytes.Length);
            Marshal.FreeHGlobal(p);
        }
    }
}