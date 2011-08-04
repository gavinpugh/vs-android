// ***********************************************************************************************
// (c) 2011 Gavin Pugh http://www.gavpugh.com/ - Released under the open-source zlib license
// ***********************************************************************************************

// Misc pathing utils, and some constants.
 
using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;

namespace vs_android.Build.CPPTasks.Android
{
	internal class Utils
    {
        public const int EST_MAX_CMDLINE_LEN = 16384;
        public const int EST_MAX_PATH_LEN = 256;

        public static string PathSanitize(string path)
        {
            path = path.Trim();

            // Remove quotes if it already has them. We'll re-add them if necessary.
            if ( path.Length > 1 )
            {
                if ( path.StartsWith( "\"" ) && path.EndsWith( "\"" ) )
                {
                    path = path.Substring( 0, path.Length - 2 );
                }
            }

            // Quotes if path has spaces
            if ( ( path.IndexOf(' ') != -1 ) && ( path.IndexOf('\"') == -1 ) ) 
            {
                path = "\"" + path + "\"";
            }

            // Forward slashes please
            path = path.Replace('\\', '/');

            return path;
        }
        
        public static string PathFixSlashes(string path)
        {
            // Forward slashes please
            path = path.Trim();
            path = path.Replace('\\', '/');

            return path;
        }
        
        // Based on code from : http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/d60f0793-cc92-48fb-b867-dd113dabcd5c
        public static void KillSpecificSubProcess(string exeName)
        {
            // UNTESTED!

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select " + exeName + " From Win32_Process Where ParentProcessID=" + Process.GetCurrentProcess().Id);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                int thisPid = Convert.ToInt32(mo["ProcessID"]);
                try
                {
                    Process proc = Process.GetProcessById(thisPid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {

                }
            }
        }


        // Most aggressive (hardest) regex first. We early out after the first match.
        private static readonly string[] GCC_REGEX_MATCH = {
												 @"^\s*In file included from (.?.?[^:]*.*?):([1-9]\d*):(.*$)",		// "In file included from CppSource/demo.c:32:"
												 @"^\s*(.?.?[^:]*.*?):([1-9]\d*):([1-9]\d*):(.*$)",					// "CppSource/importgl.c:25:17: error: new.h: No such file or directory"
												 @"^\s*(.?.?[^:]*.*?):([1-9]\d*):(.*$)",							// "CppSource/demo.c:51: error: conflicting types for 'seedRandom'"
												 @"^\s*(.?.?[^:]*.*?):(.?.?[^:]*.*?):([1-9]\d*):(.*$)",				// "Android/Debug/app-android.o:C:\Projects\vs-android_samples\san-angeles/CppSource/app-android.c:38: first defined here"
											 };

        private static readonly string[] GCC_REGEX_FILENAME = {
												 @"$1",
												 @"$1",
												 @"$1",
												 @"$2",
											 };

        private static readonly string[] GCC_REGEX_REPLACE = {
												 @"($2): includes this header: $3",
												 @"($2,$3): $4",
												 @"($2): $3",
												 @"($3): '$1' $4",
											 };

        public static string GCCOutputReplace(string line)
        {
            // Replaces given GCC style output, with output that obeys Visual Studio's 'jump to line' formatting. So:
            //		CppSource/demo.c:51: error: conflicting types for 'seedRandom'
            // becomes:
            //		c:\Projects\san-angeles\CppSource\demo.c(51) error: conflicting types for 'seedRandom'

            for (int i = 0; i < GCC_REGEX_MATCH.Length; i++)
            {
                // Do we match this...?
                Regex regEx = new Regex(GCC_REGEX_MATCH[i]);
                if (regEx.IsMatch(line))
                {
                    // Good, we do. For now just grab the filename portion
                    string filename = regEx.Replace(line, GCC_REGEX_FILENAME[i]);

                    try
                    {
                        // Brackets cause issues:
                        // C:\Projects\vs-android\vs-android_samples\san-angeles\CppSource\cams.h(49) error; (near initialization for 'sCamTracks[0]')
                        // So I'll remove them first
                        //line = line.Replace("(", "");
                        //line = line.Replace(")", "");

                        // Attempt to convert to a fullpath. We'll drop out of this regex attempt if it fails, it'll throw an Exception.
                        string absPath = Path.GetFullPath(filename);

                        // All good then. Just do the final regex replace, appended after the resolved filename.
                        string lastBit = regEx.Replace(line, GCC_REGEX_REPLACE[i]);
                        
                        string newLine = absPath + lastBit;

                        return newLine;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return line;
        }

    }
}
