/************************************************************************************************
GetGCCPreproFlags.cs

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
    public class GetGCCPreproFlags : Task
    {
        [Required]
        public string Symbols { get; set; }

        [Output]
        public string PreproFlags { get; set; }

        public GetGCCPreproFlags()
        {
            Symbols = "";
            PreproFlags = "";
        }

        public override bool Execute()
        {
            String[] ppsymbols = Symbols.Split(';');

            StringBuilder flags = new StringBuilder(1024);

            foreach (string sym in ppsymbols)
            {
                string mutsym = sym.Trim();
                if (mutsym != "")
                {
                    flags.Append("-D");
                    flags.Append(mutsym);
                    flags.Append(" ");
                }
            }

            PreproFlags = flags.ToString();

            return true;
        }
    }
}
