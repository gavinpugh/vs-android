/************************************************************************************************
GetGCCFilePath.cs

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
    public class GetGCCFilePath : Task
    {
        [Required, Output]
        public string Path { get; set; }

        public GetGCCFilePath()
        {
            Path = "";
        }

        public override bool Execute()
        {
            Path = Utils.FixSlashesForUnix(Path);

            return true;
        }
    }
}
