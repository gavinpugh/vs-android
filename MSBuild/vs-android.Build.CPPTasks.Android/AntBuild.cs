// ***********************************************************************************************
// (c) 2011 Gavin Pugh http://www.gavpugh.com/ - Released under the open-source zlib license
// ***********************************************************************************************

// Apache Ant, Apk Building Task.

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
    public class AntBuild : TrackedVCToolTask
    {
        private const string BUILD_LIB_PATH = "libs\\armeabi";
        private const string BUILD_BIN_PATH = "bin";

        private string m_toolFileName;
        private string m_inputSoPath;
        private string m_armEabiSoPath;
        private string m_antOpts;

        public bool BuildingInIDE { get; set; }
        public string JVMHeapInitial { get; set; }
        public string JVMHeapMaximum { get; set; }

        [Required]
        public string AntBuildPath { get; set; }

        [Required]
        public string AntAndroidSdkPath { get; set; }

        [Required]
        public string AntJavaHomePath { get; set; }

        [Required]
        public string AntBuildType { get; set; }
        
        [Required]
        public string AntLibraryName { get; set; }
        
        [Required]
        public string GCCToolPath { get; set; }
        
        [Required]
        public virtual ITaskItem[] Sources { get; set; }
        
        [Output]
        public virtual string OutputFile { get; set; }

        [Output]
        public string ApkName { get; set; }

        [Output]
        public string ActivityName { get; set; }

        [Output]
        public string PackageName { get; set; }

        public AntBuild()
            : base(new ResourceManager("vs_android.Build.CppTasks.Android.Properties.Resources", Assembly.GetExecutingAssembly()))
        {

        }

        protected override bool ValidateParameters()
        {
            m_toolFileName = Path.GetFileNameWithoutExtension(ToolName);

            // Ant build directory check
            if ( Directory.Exists( AntBuildPath ) == false )
            {
                Log.LogError("Ant Build Path '" + AntBuildPath + "' does not exist");
                return false;
            }

            // Check that the build.xml exists
            string buildXml = Path.GetFullPath( AntBuildPath + "\\build.xml" );
            if (File.Exists(buildXml) == false)
            {
                Log.LogError("build.xml '" + buildXml + "' does not exist");
                return false;
            }

            // Check that the AndroidManifest.xml exists
            string manifestXml = Path.GetFullPath( AntBuildPath + "\\AndroidManifest.xml" );            
            if (File.Exists(manifestXml) == false)
            {
                Log.LogError("AndroidManifest.xml '" + manifestXml + "' does not exist");
                return false;
            }

            // Parse the xml to grab the finished apk path
            if ( ParseBuildXml( buildXml ) )
            {
                if ( AntBuildType.ToLower() == "debug" )
                {
                    OutputFile = Path.GetFullPath(AntBuildPath + "\\" + BUILD_BIN_PATH + "\\" + ApkName + "-debug.apk");
                }
                else
                {
                    OutputFile = Path.GetFullPath(AntBuildPath + "\\" + BUILD_BIN_PATH + "\\" + ApkName + "-release.apk");
                }
            }
            else
            {
                // Parse failed, oh dear.
                Log.LogError("Failed parsing '" + buildXml + "'");
                return false;
            }

            if ( ParseAndroidManifestXml( manifestXml ) == false )
            {
                // Parse failed, oh dear.
                Log.LogError("Failed parsing '" + manifestXml + "'");
                return false;
            }

            // Only one .so library should be input to this task
            if ( Sources.Length > 1 )
            {
                Log.LogError("More than one .so library being built!");
                return false;
            }

            m_inputSoPath = Path.GetFullPath(Sources[0].GetMetadata("FullPath"));

            // Copy the .so file into the correct place
            m_armEabiSoPath = Path.GetFullPath(AntBuildPath + "\\" + BUILD_LIB_PATH + "\\" + AntLibraryName + ".so");

            m_antOpts = string.Empty;
            if (JVMHeapInitial != null && JVMHeapInitial.Length > 0)
            {
                m_antOpts += "-Xms" + JVMHeapInitial + "m";
            }
            if (JVMHeapMaximum != null && JVMHeapMaximum.Length > 0)
            {
                if ( m_antOpts.Length > 0 )
                {
                    m_antOpts += " ";
                }
                m_antOpts += "-Xmx" + JVMHeapMaximum + "m";
            }

            return base.ValidateParameters();
        }

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            // Copy over the .so file to the correct directory in the build structure
            Directory.CreateDirectory(AntBuildPath + "\\" + BUILD_LIB_PATH);
            File.Copy(m_inputSoPath, m_armEabiSoPath, true);

            // Create local properties file from Android SDK Path
            WriteLocalProperties();

            // List of environment variables
            List<String> envList = new List<String>();

            // Set JAVA_HOME for the ant build
			// NOTE: 'envList' code is from 'mark.bozeman', see http://code.google.com/p/vs-android/issues/detail?id=15
			// NOTE: I kept the original env setting code, since for me, JAVA_HOME was no longer being set correctly, causing
			// NOTE: the ant build to fail.
            envList.Add("JAVA_HOME=" + AntJavaHomePath);
            System.Environment.SetEnvironmentVariable("JAVA_HOME", AntJavaHomePath, EnvironmentVariableTarget.Process);
            Log.LogMessage(MessageImportance.High, "Building using the JDK located here: '{0}'...", AntJavaHomePath);

            if (m_antOpts.Length > 0)
            {
                Log.LogMessage(MessageImportance.High, "Building using ANT_OPTS: '{0}'...", m_antOpts);
                envList.Add("ANT_OPTS=" + m_antOpts);
                System.Environment.SetEnvironmentVariable("ANT_OPTS", m_antOpts, EnvironmentVariableTarget.Process);
            }

            // Set environment variables
            this.EnvironmentVariables = envList.ToArray();

            return base.ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
        }

        private void WriteLocalProperties()
        {
            string localPropsFile = Path.GetFullPath(AntBuildPath + "\\local.properties");

            // Need double backslashes for this path
            string sdkPath = Path.GetFullPath(AntAndroidSdkPath).Replace( "\\", "\\\\" );

            string fileContents = vs_android.Build.CPPTasks.Android.Properties.Resources.localproperties_ant_file;
            fileContents = fileContents.Replace("{SDKDIR}", sdkPath);
            
            using (StreamWriter outfile = new StreamWriter(localPropsFile))
            {
                outfile.Write(fileContents);
            }
        }

        private bool ParseBuildXml( string xmlPath )
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
                            if ( attrib != null )
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
                        if ( reader.Name == "manifest" )
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

        protected override void RemoveTaskSpecificInputs(CanonicalTrackedInputFiles compactInputs)
        {
            // This is necessary because the VC tracker gets confused by the intermingling of reading and writing by the support apps

            foreach (KeyValuePair<string, Dictionary<string, string>> pair in compactInputs.DependencyTable)
            {
                List<string> delFiles = new List<string>();

                foreach (KeyValuePair<string, string> depFile in pair.Value)
                {
                    // Remove the -unaligned.apk file, it shouldn't be in the input list
                    if (depFile.Key.ToLowerInvariant().EndsWith("-unaligned.apk"))
                    {
                        delFiles.Add(depFile.Key);
                    }
                }

                // Do deletions
                foreach (string delFile in delFiles)
                {
                    pair.Value.Remove(delFile);
                }

                // Add the two .so files to the inputs
                pair.Value.Add(m_inputSoPath.ToUpperInvariant(), null);
                pair.Value.Add(m_armEabiSoPath.ToUpperInvariant(), null);
            }
        }

        protected override void RemoveTaskSpecificOutputs(CanonicalTrackedOutputFiles compactOutputs)
        {
            // Find each non-apk output, and delete it
            // This is necessary because the VC tracker gets confused by the intermingling of reading and writing by the support apps

            foreach (KeyValuePair<string, Dictionary<string, DateTime>> pair in compactOutputs.DependencyTable)
            {
                List<string> delFiles = new List<string>();

                foreach (KeyValuePair<string, DateTime> depFile in pair.Value)
                {
                    // Remove all non-apk files from the output list
                    if (depFile.Key.ToLowerInvariant().EndsWith(".apk") == false)
                    {
                        delFiles.Add(depFile.Key);
                    }
                }

                // Do deletions
                foreach (string delFile in delFiles)
                {
                    pair.Value.Remove(delFile);
                }
            }
        }

        public override bool AttributeFileTracking
        {
            get
            {
                return true;
            }
        }

        protected override string GetWorkingDirectory()
        {
            return AntBuildPath;
        }

        protected override string GenerateCommandLineCommands()
        {
            // Simply 'debug', or 'release'.
            return AntBuildType.ToLower();
        }

        protected override string GenerateResponseFileCommands()
        {
            return string.Empty;
        }

        protected override bool MaintainCompositeRootingMarkers
        {
            get
            {
                return true;
            }
        }

        public virtual string PlatformToolset
        {
            get
            {
                return "Ant";
            }
        }

        protected override Encoding ResponseFileEncoding
        {
            get
            {
                return Encoding.ASCII;
            }
        }

        protected override string ToolName
        {
            get
            {
                return GCCToolPath;
            }
        }

        protected override ITaskItem[] TrackedInputFiles
        {
            get
            {
                return Sources;
            }
        }

        protected override string TrackerIntermediateDirectory
        {
            get
            {
                if (this.TrackerLogDirectory != null)
                {
                    return this.TrackerLogDirectory;
                }
                return string.Empty;
            }
        }

        public virtual string TrackerLogDirectory
        {
            get
            {
                if (base.IsPropertySet("TrackerLogDirectory"))
                {
                    return base.ActiveToolSwitches["TrackerLogDirectory"].Value;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("TrackerLogDirectory");
                ToolSwitch switch2 = new ToolSwitch(ToolSwitchType.Directory)
                {
                    DisplayName = "Tracker Log Directory",
                    Description = "Tracker log directory.",
                    ArgumentRelationList = new ArrayList(),
                    Value = VCToolTask.EnsureTrailingSlash(value)
                };
                base.ActiveToolSwitches.Add("TrackerLogDirectory", switch2);
                base.AddActiveSwitchToolValue(switch2);
            }
        }

        protected override string CommandTLogName
        {
            get
            {
                return (m_toolFileName + ".command.1.tlog");
            }
        }
        
        protected override string[] ReadTLogNames
        {
            get
            {
                return new string[] { 
                    "cmd-java-zipalign.read.*.tlog", 
                    "cmd-java-zipalign.*.read.*.tlog",
                    "cmd-java-aapt.read.*.tlog", 
                    "cmd-java-aapt.*.read.*.tlog",
                    "cmd.read.*.tlog", 
                    "cmd.*.read.*.tlog",
                    "java.read.*.tlog", 
                    "java.*.read.*.tlog",
                };
            }
        }

        protected override string[] WriteTLogNames
        {
            get
            {
                return new string[] { 
                    "cmd-java-zipalign.write.*.tlog", 
                    "cmd-java-zipalign.*.write.*.tlog",
                    "cmd-java-aapt.write.*.tlog", 
                    "cmd-java-aapt.*.write.*.tlog",
                    "cmd.write.*.tlog", 
                    "cmd.*.write.*.tlog",
                    "java.write.*.tlog", 
                    "java.*.write.*.tlog",
                };
            }
        }

    }


}
