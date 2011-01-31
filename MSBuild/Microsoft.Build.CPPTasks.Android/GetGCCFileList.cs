using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Microsoft.Build.CPPTasks.Android
{
    public class GetGCCFileList : Task
    {
        private string[] paths = new string[0];
        public string[] Paths
        {
            get { return paths; }
            set { paths = value; }
        }

        private bool stripLeadingObjPaths = false;
        public bool StripLeadingObjPaths
        {
            get { return stripLeadingObjPaths; }
            set { stripLeadingObjPaths = value; }
        }

        private string objPath = "";
        [Description("Gets or sets the result."), Output]
        public string ObjPath
        {
            get { return objPath; }
            set { objPath = value; }
        }

        private string outFileList = "";
        [Description("Gets or sets the result."), Output]
        public string OutFileList
        {
            get { return outFileList; }
            set { outFileList = value; }
        }

        public override bool Execute()
        {
            List< string > objPaths = new List<string>();
            string firstCandidate = null;

            StringBuilder outList = new StringBuilder(16384);

            bool first = true;

            foreach (string path in paths)
            {
                string mutPath = Utils.FixSlashes(path);

                if ( stripLeadingObjPaths )
                {
                    if (Path.GetExtension(mutPath) == ".obj")
                    {
                        // With particularly large projects, the leading path to obj files can make the linker
                        // barf if the command line gets too long. What I'll do (just for these obj files), is
                        // strip out the leading path, and give it back

                        string leadingPath = Path.GetDirectoryName(mutPath);

                        if ( objPaths.Contains( leadingPath ) == false )
                        {
                            objPaths.Add(leadingPath);

                            // If we're putting object files in different directories, then this all breaks
                            if (objPaths.Count > 1)
                            {       
                                string errMsg = "Object files need to be built into the same directory: "
                                    + firstCandidate + " != " + mutPath ;

                                BuildErrorEventArgs errorEvent = new BuildErrorEventArgs(
                                    String.Empty, String.Empty, 
                                    String.Empty, 0, 0, 0, 0, 
                                    errMsg, String.Empty, String.Empty );  

                                this.BuildEngine.LogErrorEvent( errorEvent );
                                return false;
                            }
                            else
                            {
                                // Log filename to show in error message if we ever have more than one obj path
                                firstCandidate = mutPath;

                                objPath = leadingPath;
                            }
                        }

                        mutPath = Path.GetFileName(mutPath);
                    }
                }
                
                // Remove any existing double quotes
                mutPath = mutPath.Replace("\"", "");

                if (!first)
                {
                    outList.Append(" ");
                }

                outList.Append("\"");
                outList.Append(mutPath);
                outList.Append("\"");

                first = false;
            }

            outFileList = outList.ToString();

            return true;
        }
    }
}
