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
        private string symbols = "";
        public string Symbols
        {
            get { return symbols; }
            set { symbols = value; }
        }

        private string preproFlags = "";
        [Description("Gets or sets the result."), Output]
        public string PreproFlags
        {
            get { return preproFlags; }
            set { preproFlags = value; }
        }

        public override bool Execute()
        {
            String[] ppsymbols = symbols.Split(';');

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

            preproFlags = flags.ToString();

            return true;
        }
    }
}
