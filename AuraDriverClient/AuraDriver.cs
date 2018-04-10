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

namespace ajanata.AuraDriver.Client
{
    /// <summary>
    /// Wrapper class around the COM AuraDriver, which handles marshaling and COM instantiation.
    /// </summary>
    public class AuraDriver
    {
        private static readonly Guid implGuid = new Guid("99074DB3-287C-461A-AAC9-4A9FAAD8A508");
        private readonly IAuraDriver impl;

        public AuraDriver()
        {
            Type type = Type.GetTypeFromCLSID(implGuid, true);
            impl = (IAuraDriver)Activator.CreateInstance(type);
        }

        public int[] GetMotherboardControllers()
        {
            return impl.GetMotherboardControllers();
        }

        public void SetMotherboardApplicationMode(int handle)
        {
            impl.SetMotherboardApplicationMode(handle);
        }

        public void SetMotherboardDefaultMode(int handle)
        {
            impl.SetMotherboardDefaultMode(handle);
        }

        public int GetMotherboardLedCount(int handle)
        {
            return impl.GetMotherboardLedCount(handle);
        }

        public LedColor[] GetMotherboardColors(int handle)
        {
            byte[] bytes = impl.GetMotherboardColors(handle);
            LedColor[] colors = new LedColor[bytes.Length / 3];
            for (int i = 0; i < bytes.Length; i += 3)
            {
                LedColor color = new LedColor();
                color.Red = bytes[i];
                color.Green = bytes[i + 1];
                color.Blue = bytes[i + 2];
                colors[i / 3] = color;
            }
            return colors;
        }

        public void SetMotherboardColors(int handle, LedColor[] colors)
        {
            byte[] bytes = new byte[colors.Length * 3];
            for (int i = 0; i < bytes.Length; i += 3)
            {
                LedColor color = colors[i / 3];
                bytes[i] = color.Red;
                bytes[i + 1] = color.Green;
                bytes[i + 2] = color.Blue;
            }
            impl.SetMotherboardColors(handle, bytes);
        }
    }
}
