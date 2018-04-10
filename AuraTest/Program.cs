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

using ajanata.AuraDriver.Client;
using System;

namespace AuraTest
{
    /// <summary>
    /// Demo program for AuraDriver. Assumes each controler on your motherboard has 7 LEDs; if not, adjust startingColors.
    /// </summary>
    class Program
    {
        private static LedColor[] startingColors = new LedColor[]
        {
            new LedColor(255,0,0),
            new LedColor(0,255,0),
            new LedColor(0,0,255),
            new LedColor(255,255,0),
            new LedColor(255,0,255),
            new LedColor(0,255,255),
            new LedColor(255,255,255)
        };

        static void Main(string[] args)
        {
            AuraDriver driver = new AuraDriver();

            int[] handles = driver.GetMotherboardControllers();
            Console.WriteLine("Found " + handles.Length);

            for (int x = 0; x < 10; x++)
            {
                foreach (int h in handles)
                {
                    Console.WriteLine("Handle " + h);
                    driver.SetMotherboardApplicationMode(h);
                    int ledCount = driver.GetMotherboardLedCount(h);
                    Console.WriteLine("Found " + ledCount);

                    LedColor[] colors = driver.GetMotherboardColors(h);
                    foreach (LedColor color in colors)
                    {
                        Console.WriteLine(color);
                        byte t = color.Red;
                        color.Red = color.Green;
                        color.Green = color.Blue;
                        color.Blue = t;
                    }
                    if (x == 0)
                    {
                        colors = startingColors;
                    }
                    driver.SetMotherboardColors(h, colors);
                }
                System.Threading.Thread.Sleep(2000);
            }

            foreach (int h in handles)
            {
                driver.SetMotherboardDefaultMode(h);
            }
        }
    }
}
