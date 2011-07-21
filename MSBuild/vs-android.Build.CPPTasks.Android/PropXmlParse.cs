// ***********************************************************************************************
// (c) 2011 Gavin Pugh http://www.gavpugh.com/ - Released under the open-source zlib license
// ***********************************************************************************************

// Parser for MsBuild .xml property files. There's possibly a nicer Microsoft way of doing this, but because
// there's zero documentation for any of this stuff, I couldn't find it. This class will take a given
// property-sheet xml file, and a set of metadata. It'll spit out the correct commandline switches based
// on the metadata.
// Why? Sure beats the copy and pasted per-switch code you have to do with the TrackedVCToolTask. I just
// wanted to keep one place maintained. It effectively also makes the code data-driven, so new switches
// can be added without recompiling the dll.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

using Microsoft.Build.Framework;

namespace vs_android.Build.CPPTasks.Android
{
	class PropXmlParse
	{
		private Dictionary<string, Property> m_properties = new Dictionary<string, Property>();
		private string m_switchPrefix;

		public PropXmlParse( string path )
		{
			XmlTextReader reader = new XmlTextReader( path );

			m_switchPrefix = null;

			while ( reader.Read() )
			{
				switch ( reader.NodeType )
				{
					case XmlNodeType.Element: // The node is an element.
						{
							switch ( reader.Name )
							{
								case "Rule":
									m_switchPrefix = reader.GetAttribute( "SwitchPrefix" );
									break;

								case "StringListProperty":
									NewProperty( reader, new StringListProperty() );
									break;
								case "StringProperty":
								case "IntProperty":
									NewProperty( reader, new StringProperty() );
									break;
								case "BoolProperty":
									NewProperty( reader, new BoolProperty() );
									break;
								case "EnumProperty":
									NewProperty( reader, new EnumProperty() );
									break;
							}
						}
						break;
				}
			}
		}

        public string ProcessProperties(ITaskItem taskItem)
        {
            StringBuilder returnStr = new StringBuilder(Utils.EST_MAX_CMDLINE_LEN);

            foreach ( string metaName in taskItem.MetadataNames )
            {
                string propValue = taskItem.GetMetadata(metaName);                                
                string processed = ProcessProperty(metaName, propValue).Trim();

                if (( processed != null ) && ( processed.Length > 0 ))
                {
                    returnStr.Append(processed);
                    returnStr.Append(" ");
                }
            }

            return returnStr.ToString().Trim();
        }

		private string ProcessProperty( string propName, string propVal )
		{
			Property prop;
			if ( m_properties.TryGetValue( propName, out prop ) )
			{
				return prop.Process( propVal );
			}
			return string.Empty;
		}

		private void NewProperty( XmlTextReader xml, Property prop )
		{
			string name = xml.GetAttribute( "Name" );
			string switchPrefixOverride = xml.GetAttribute( "SwitchPrefix" );
			string separator = xml.GetAttribute( "Separator" );
            string includeInCmdLine = xml.GetAttribute( "IncludeInCommandLine" );
            string subType = xml.GetAttribute( "Subtype" );
			
			// Just need at least a valid name
			if ( name != null )
			{
				// Choose correct switch prefix
				string prefix = m_switchPrefix;
				if ( switchPrefixOverride != null )
				{
					prefix = switchPrefixOverride;
				}
				if ( prefix == null )
				{
					prefix = string.Empty;
				}

				// Separator for string, int and stringlist properties
				if ( separator == null )
				{
					separator = string.Empty;
				}

                if ( includeInCmdLine != null )
                {
                    if ( includeInCmdLine.ToLower() == "false" )
                    {
                        // Ignore the ones that aren't meant to be in the cmdline
                        return;
                    }
                }

                // Will quote fix for files or folder params
                bool shouldQuoteFix = false;
                if (subType != null)
                {
                    if ((subType.ToLower() == "file") || (subType.ToLower() == "folder"))
                    {
                        shouldQuoteFix = true;
                    }
                }

                prop.Setup(xml, prefix, separator, shouldQuoteFix );

				m_properties.Add( name, prop );
			}
		}

		public abstract class Property
		{
			abstract public string Process( string propVal );

			public void Setup( XmlTextReader xml, string switchPrefix, string separator, bool quoteFix )
			{
				m_switchPrefix = switchPrefix;
				m_separator = separator;
                m_quoteFix = quoteFix;

				Debug.Assert( m_switchPrefix != null ); 
				Debug.Assert( m_separator != null );

				SetupProperty( xml );
			}
            
            protected string FixString( string str )
            {
                if ( m_quoteFix == false )
                {
                    // Just fix the slashes, no quoting
                    return Utils.PathFixSlashes(str);
                }
                else
                {
                    // Slash fixing AND possible quoting
                    return Utils.PathSanitize(str);
                }
            }

			abstract protected void SetupProperty( XmlTextReader xml );

			protected string m_switchPrefix;
            protected string m_separator;
            protected bool m_quoteFix;
		}
		
		public class EnumProperty : Property
		{
			override public string Process( string propVal )
			{
				string found;
				if ( m_switches.TryGetValue( propVal, out found ) )
				{
					return found;
				}

				return string.Empty;
			}

			override protected void SetupProperty( XmlTextReader xml )
			{
				// switchString is just the prefix for enum properties
				int nestLevel = 1;

				while ( xml.Read() )
				{
					switch ( xml.NodeType )
					{
						case XmlNodeType.Element: // The node is an element.
							{
								if ( xml.IsEmptyElement == false )
								{
									nestLevel++;
								}

								if ( xml.Name == "EnumValue" )
								{
									string switchVal = xml.GetAttribute( "Switch" );
									string nameStr = xml.GetAttribute( "Name" );

									if ( nameStr != null )
									{
										if ( switchVal != null )
										{
											m_switches.Add( nameStr, m_switchPrefix + switchVal );
										}
										else
										{
											m_switches.Add( nameStr, string.Empty );
										}
									}
								}
							}
							break;

						case XmlNodeType.EndElement: // The node is an element.
							{
								nestLevel--;

								if ( nestLevel == 0 )
								{
									return;
								}
							}
							break;
					}
				}
			}
			
			private Dictionary<string, string> m_switches = new Dictionary<string, string>();
		}

		public class BoolProperty : Property
		{
			override public string Process( string propVal )
			{
				if ( propVal.ToLower() == "true" )
				{
					if ( m_trueSwitch != null )
					{
						return m_switchPrefix + m_trueSwitch;
					}
				}
                else if ( propVal.ToLower() != "ignore" )
                {
				    if ( m_falseSwitch != null )
				    {
					    return m_switchPrefix + m_falseSwitch;
				    }
                }
				return string.Empty;
			}

			override protected void SetupProperty( XmlTextReader xml )
			{
				m_trueSwitch = xml.GetAttribute( "Switch" );
				m_falseSwitch = xml.GetAttribute( "ReverseSwitch" );
			}

			private string m_falseSwitch;
			private string m_trueSwitch;
		}

		public class StringProperty : Property
		{
			override public string Process( string propVal )
            {
                if (m_switch == null)
                {
                    return FixString(propVal);
                }

                if (propVal.Length > 0)
                {
                    // Ignore switches entirely if we don't have one
                    if (m_switch != null)
                    {
                        return m_switchPrefix + m_switch + m_separator + FixString(propVal);
                    }
                    else
                    {
                        return FixString(propVal);
                    }
                }

                return string.Empty;
			}

			override protected void SetupProperty( XmlTextReader xml )
			{
				m_switch = xml.GetAttribute( "Switch" );
			}

			private string m_switch;
		}

		public class StringListProperty : Property
		{
			override public string Process( string propVal )
            {
				StringBuilder sBuilder = new StringBuilder(1024);
				string [] strings = propVal.Split( ';' );

				foreach ( string str in strings )
				{
                    if (str.Length > 0)
                    {
                        // Ignore switches entirely if we don't have one
                        if (m_switch != null)
                        {
                            sBuilder.Append(m_switchPrefix);
                            sBuilder.Append(m_switch);
                            sBuilder.Append(m_separator);
                        }

                        sBuilder.Append(FixString(str));
                        sBuilder.Append(" ");
                    }
				}

				return sBuilder.ToString();
			}

			override protected void SetupProperty( XmlTextReader xml )
			{
				m_switch = xml.GetAttribute( "Switch" );
			}

			private string m_switch;
		}
	}
}
