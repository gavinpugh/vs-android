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
    public class GetGCCFilePath : Task
    {
        private string path = "";
        [Description("Gets or sets the result."), Output]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public override bool Execute()
        {
            path = Utils.FixSlashes(path);

            return true;
        }
    }
}
