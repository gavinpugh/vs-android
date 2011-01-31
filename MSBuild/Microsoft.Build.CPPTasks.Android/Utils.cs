using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Microsoft.Build.CPPTasks.Android
{
    class Utils
    {
        static public String FixSlashes( String s )
        {
            return s.Replace( '\\', '/' );
        }
    }
}
