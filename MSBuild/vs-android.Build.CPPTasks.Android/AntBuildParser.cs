
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml;

using Microsoft.Build.Framework;
using Microsoft.Build.CPPTasks;
using Microsoft.Build.Utilities;

namespace vs_android.Build.CPPTasks.Android
{
    public class AntBuildParser
    {
        private const string BUILD_BIN_PATH = "bin";

        public string OutputFile { get; set; }
        public string ApkName { get; set; }
        public string PackageName { get; set; }
        public string ActivityName { get; set; }
        
        public bool Parse(string antBuildPath, string antBuildType, TaskLoggingHelper log, bool outputInQuotes)
        {
            // Ant build directory check
            if (Directory.Exists(antBuildPath) == false)
            {
                log.LogError("Ant Build Path '" + antBuildPath + "' does not exist");
                return false;
            }

            // Check that the build.xml exists
            string buildXml = Path.GetFullPath(antBuildPath + "\\build.xml");
            if (File.Exists(buildXml) == false)
            {
                log.LogError("build.xml '" + buildXml + "' does not exist");
                return false;
            }

            // Check that the AndroidManifest.xml exists
            string manifestXml = Path.GetFullPath(antBuildPath + "\\AndroidManifest.xml");
            if (File.Exists(manifestXml) == false)
            {
                log.LogError("AndroidManifest.xml '" + manifestXml + "' does not exist");
                return false;
            }

            // Parse the xml to grab the finished apk path
            if (ParseBuildXml(buildXml))
            {
                if (antBuildType.ToLower() == "debug")
                {
					OutputFile = Path.GetFullPath(antBuildPath + "\\" + BUILD_BIN_PATH + "\\" + ApkName + "-debug.apk");
                }
                else
                {
					OutputFile = Path.GetFullPath(antBuildPath + "\\" + BUILD_BIN_PATH + "\\" + ApkName + "-release.apk");
                }

				if ( outputInQuotes )
				{
					OutputFile = "\"" + OutputFile + "\"";
				}
            }
            else
            {
                // Parse failed, oh dear.
                log.LogError("Failed parsing '" + buildXml + "'");
                return false;
            }

            if (ParseAndroidManifestXml(manifestXml) == false)
            {
                // Parse failed, oh dear.
                log.LogError("Failed parsing '" + manifestXml + "'");
                return false;
            }

            return true;
        }

        private bool ParseBuildXml(string xmlPath)
        {
            // Parse the Apk Name out of the build.xml file
            XmlTextReader reader = new XmlTextReader(xmlPath);
            string currElem = string.Empty;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "project")
                        {
                            string attrib = reader.GetAttribute("name");
                            if (attrib != null)
                            {
                                ApkName = attrib;
                                return true;
                            }
                        }
                        break;
                }
            }

            return false;
        }

        private bool ParseAndroidManifestXml(string xmlPath)
        {
            // Parse the Package and Activity name out of the AndroidManifest.xml file
            XmlTextReader reader = new XmlTextReader(xmlPath);
            string currElem = string.Empty;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "manifest")
                        {
                            string attrib = reader.GetAttribute("package");
                            if (attrib != null)
                            {
                                PackageName = attrib;
                            }
                        }
                        else if (reader.Name == "activity")
                        {
                            string attrib = reader.GetAttribute("android:name");
                            if (attrib != null)
                            {
                                ActivityName = attrib;
                            }
                        }
                        break;
                }
            }

            return (PackageName.Length > 0 && ActivityName.Length > 0);
        }
    }
}
