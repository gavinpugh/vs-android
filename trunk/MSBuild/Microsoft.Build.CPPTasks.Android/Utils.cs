/************************************************************************************************
Utils.cs

(c) 2011 Gavin Pugh http://www.gavpugh.com/ - Released under the open-source zlib license
*************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Microsoft.Build.CPPTasks.Android
{
    class Utils
    {
        static public String FixSlashesForUnix( String s )
        {
            return s.Replace( '\\', '/' );
        }

        static public String FixSlashesForWindows(String s)
        {
            return s.Replace('/', '\\');
        }
    }
}
