using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace vs_android_launch
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"vs-android_launch.cmd");

                string[] lines = text.Split('\n');

                foreach ( string line in lines )
                {
                    int adbIndex = line.IndexOf("adb.exe");
                    int adbLen = "adb.exe".Length;

                    if (adbIndex > 0)
                    {
                        string adbPath = line.Substring(0, adbIndex + adbLen);
                        string adbParams = line.Substring(adbIndex + adbLen + 1);

                        foreach (string s in args)
                        {
                            adbParams += " ";
                            adbParams += s;
                        }

                        Process proc = Process.Start(adbPath, adbParams);
                        proc.WaitForExit();
                    }
                }                
            }
            catch
            {

            }

        }
    }
}
