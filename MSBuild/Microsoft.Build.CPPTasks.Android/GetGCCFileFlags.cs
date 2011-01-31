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
        private string paths = "";
        public string Paths
        {
            get { return paths; }
            set { paths = value; }
        }

        private string flag = "";
        public string Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        private string returnFlags = "";
        [Description("Gets or sets the result."), Output]
        public string ReturnFlags
        {
            get { return returnFlags; }
            set { returnFlags = value; }
        }

        public override bool Execute()
        {
            String[] paths = this.paths.Split(';');

            StringBuilder flags = new StringBuilder(1024);

            foreach (string path in paths)
            {
                string mutpath = path.Trim();
                if (mutpath != "")
                {
                    flags.Append(flag);
                    flags.Append("\"");
                    flags.Append(Utils.FixSlashes(mutpath));
                    flags.Append("\" ");
                }
            }

            returnFlags = flags.ToString();

            return true;
        }
    }
}
