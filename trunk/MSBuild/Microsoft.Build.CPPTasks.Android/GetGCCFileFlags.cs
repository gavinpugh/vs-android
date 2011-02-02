/************************************************************************************************
GetGCCFileFlags.cs

(c) 2011 Gavin Pugh http://www.gavpugh.com/ - Released under the open-source zlib license
*************************************************************************************************/

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
    public class GetGCCFileFlags : Task
    {
        public string Paths { get; set; }

        [Required]
        public string Flag { get; set; }

        [Output]
        public string ReturnFlags { get; set; }

        public GetGCCFileFlags()
        {
            Paths = "";
            Flag = "";
            ReturnFlags = "";
        }

        public override bool Execute()
        {
            String[] paths = this.Paths.Split(';');

            StringBuilder flags = new StringBuilder(1024);

            foreach (string path in paths)
            {
                string mutpath = path.Trim();
                if (mutpath != "")
                {
                    flags.Append(Flag);
                    flags.Append("\"");
                    flags.Append(Utils.FixSlashesForUnix(mutpath));
                    flags.Append("\" ");
                }
            }

            ReturnFlags = flags.ToString();

            return true;
        }
    }
}
