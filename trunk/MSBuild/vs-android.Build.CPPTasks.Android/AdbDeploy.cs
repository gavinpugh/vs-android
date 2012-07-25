// ***********************************************************************************************
// (c) 2012 Gavin Pugh http://www.gavpugh.com/ - Released under the open-source zlib license
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
    public class AdbDeploy : TrackedVCToolTask
    {
        public bool BuildingInIDE { get; set; }

        [Required]
        public string AntBuildPath { get; set; }

        [Required]
        public string AntBuildType { get; set; }

        [Required]
        public string AdbPath { get; set; }

        [Required]
        public string Params { get; set; }

        [Required]
        public string DeviceArgs { get; set; }

        public string GenerateCmdFilePath { get; set; }
		
        private AntBuildParser m_parser = new AntBuildParser();

        private string m_toolFileName;

        public AdbDeploy()
            : base(new ResourceManager("vs_android.Build.CppTasks.Android.Properties.Resources", Assembly.GetExecutingAssembly()))
        {

        }

        private void WriteDebugRunCmdFile()
        {            
            string destCmdFile = Path.GetFullPath(GenerateCmdFilePath);

            using (StreamWriter outfile = new StreamWriter(destCmdFile))
            {
                outfile.Write(string.Format("{0} {1} shell am start -n {2}/{3}\n", AdbPath, MakeStringReplacements(DeviceArgs), m_parser.PackageName, m_parser.ActivityName));
            }
        }

        protected override bool ValidateParameters()
        {
            m_toolFileName = Path.GetFileNameWithoutExtension(ToolName);

            if ( !m_parser.Parse( AntBuildPath, AntBuildType, Log, false ) )
            {
                return false;
            }

            return base.ValidateParameters();
        }

        public override void Cancel()
        {
            Process.Start(AdbPath, "kill-server");

            base.Cancel();
        }

        public override bool Execute()
        {
            return base.Execute();
        }

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            Log.LogMessage(MessageImportance.High, "{0} {1}", pathToTool, commandLineCommands);

			if ( ( GenerateCmdFilePath != null ) && ( GenerateCmdFilePath.Length > 0 ) )
			{
				WriteDebugRunCmdFile();
			}

			if ( commandLineCommands.Contains( "wait-for-device" ) || commandLineCommands.Contains( "start-server" ) )
			{
				// Hack to spawn a process, instead of waiting on it
				Process.Start( pathToTool, commandLineCommands );
				return 0;
			}
			else
			{
				return base.ExecuteTool( pathToTool, responseFileCommands, commandLineCommands );
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

        private string MakeStringReplacements( string theString )
        {
            string paramCopy = theString;
            paramCopy = paramCopy.Replace("{PackageName}", m_parser.PackageName);
            paramCopy = paramCopy.Replace("{ApkPath}", "\"" + m_parser.OutputFile + "\"");
            paramCopy = paramCopy.Replace("{ActivityName}", m_parser.ActivityName);
            return paramCopy.Trim();
        }

        protected override string GenerateCommandLineCommands()
        {
            return (MakeStringReplacements(DeviceArgs) + " " + MakeStringReplacements(Params)).Trim();
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
                return "Adb";
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
                return AdbPath;
            }
        }

        protected override ITaskItem[] TrackedInputFiles
        {
            get
            {
                return new TaskItem[] { new TaskItem( m_parser.OutputFile) };
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
                return new string[] { (m_toolFileName + ".read.*.tlog"), (m_toolFileName + ".*.read.*.tlog") };
            }
        }

        protected override string[] WriteTLogNames
        {
            get
            {
                return new string[] { (m_toolFileName + ".write.*.tlog"), (m_toolFileName + ".*.write.*.tlog") };
            }
        }

    }


}
