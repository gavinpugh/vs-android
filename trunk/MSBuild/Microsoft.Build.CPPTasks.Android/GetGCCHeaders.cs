/************************************************************************************************
GetGCCHeaders.cs

(c) 2011 Gavin Pugh http://www.gavpugh.com/ - Released under the open-source zlib license
*************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Microsoft.Build.CPPTasks.Android
{
    public class GetGCCHeaders : Task
    {
        [Output]
        public ITaskItem[] HeaderList { get; set; }

        // Path to the GCC executable to run
        [Required]
        public string GCCPath { get; set; }

        // Name of the c/c++ file we're compiling
        [Required]
        public string CPPFile { get; set; }

        // Params to pass to instance of GCC
        [Required]
        public string Params { get; set; }

        // Path we're running from, so we can convert relative to abs paths
        [Required]
        public string CurrentPath { get; set; }

        const int EST_MAX_FILES = 128;
        const int EST_MAX_PATH_LEN = 256;

        List<string> fileList = new List<String>(EST_MAX_FILES);

        string output = "";

        private void AddPathToList( string path )
        {
            // A single backslash would be the end of line. The output is make-style dependencies
            if (path.ToString() == "\\")
            {
                return;
            }

            string winPath = Utils.FixSlashesForWindows(path);

            if ( Path.IsPathRooted( winPath ) == false )
            {
                winPath = Path.Combine(Utils.FixSlashesForWindows(CurrentPath), winPath);
            }

            winPath = Path.GetFullPath(winPath);

            if (System.IO.File.Exists(winPath) == false)
            {
                // Missing headers screw up MSBuild deps
                Log.LogMessage("Header file is missing: {0}\n{1}", winPath, output);
                return;
            }

            fileList.Add(winPath);
        }

        private bool PopulateFileList( string output )
        {
            // Parses the output string into a list of header files

            // Format is:
            // main.o: main.cpp \
            //  C:/android-ndk-r5/platforms/android-9/arch-arm/usr/include/stdio.h \
            //  C:/android-ndk-r5/platforms/android-9/arch-arm/usr/include/sys/cdefs.h \
            //  C:/android-ndk-r5/platforms/android-9/arch-arm/usr/include/sys/cdefs_elf.h \
            //  C:/android-ndk-r5/platforms/android-9/arch-arm/usr/include/sys/_types.h \
            //  C:/android-ndk-r5/platforms/android-9/arch-arm/usr/include/machine/_types.h \

            int cppFileCharIndex = 0;

            StringBuilder path = new StringBuilder(EST_MAX_PATH_LEN);
            bool inQuotes = false;
            bool doingHeaders = false;
            foreach (char c in output)
            {
                if (doingHeaders == false)
                {
                    if (cppFileCharIndex == CPPFile.Length)
                    {
                        // All done, just passed the cpp file.
                        doingHeaders = true;
                    }
                    else
                    {
                        if (Char.ToLower(c) == Char.ToLower(CPPFile[cppFileCharIndex]))
                        {
                            cppFileCharIndex++;
                        }
                        else
                        {
                            cppFileCharIndex = 0;
                        }
                    }
                }
                else
                {
                    bool terminated = false;

                    if (inQuotes)
                    {
                        if (c == '\"')
                        {
                            // Done with quotes. Save this out.
                            inQuotes = false;
                            terminated = true;
                        }
                        else
                        {
                            // In quotes, so anything goes
                            path.Append(c);
                        }
                    }
                    else
                    {
                        if (c == '\"')
                        {
                            // Start quotes?
                            inQuotes = true;

                            if (path.Length > 0)
                            {
                                Log.LogMessage("Bad quotes in output: {0}", output);

                                // Incorrect quotes
                                return false;
                            }
                        }
                        else if ((c == 13) || (c == 10) || (c == ' ') || (c == '\t'))
                        {
                            // Whitespace. If we have anything, then save it out. Otherwise ignore it
                            if (path.Length > 0)
                            {
                                terminated = true;
                            }
                        }
                        else
                        {
                            // Append to this path
                            path.Append(c);
                        }
                    }

                    if (terminated)
                    {
                        AddPathToList(path.ToString());

                        path.Length = 0;
                    }
                }
            }

            return true;
        }

        public override bool Execute()
        {
            // Correct any forward slashes
            GCCPath = Utils.FixSlashesForWindows(GCCPath);

            Process compile = new Process();
            compile.StartInfo.FileName = GCCPath;
            compile.StartInfo.Arguments = Params;
            compile.StartInfo.CreateNoWindow = true;
            compile.StartInfo.UseShellExecute = false;
            compile.StartInfo.RedirectStandardError = false;
            compile.StartInfo.RedirectStandardOutput = true;

            compile.Start();

            // Synchronously read the standard output of the spawned process.
            output = compile.StandardOutput.ReadToEnd();
            compile.WaitForExit();

            if (output == null)
            {
                // Command had problems running
                Log.LogMessage("Failed running {0} {1}", GCCPath, Params);
                return false;
            }

            if ( PopulateFileList( output ) == false )
            {
                Log.LogMessage("Failed parsing output: {0}", output);
                return false;
            }
            
            HeaderList = new ITaskItem[fileList.Count];
            int i = 0;
            foreach (string path in fileList)
            {
                HeaderList[i] = new TaskItem(path);
                i++;
            }
                        
            return true;
        }
    }
}
