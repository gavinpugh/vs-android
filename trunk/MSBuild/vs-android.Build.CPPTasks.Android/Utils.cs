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

    }
}
