﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dirigent.Common.Properties
{


	[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute( "Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0" )]
	public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
	{

		private static Settings defaultInstance = ( ( Settings )( global::System.Configuration.ApplicationSettingsBase.Synchronized( new Settings() ) ) );

		public static Settings Default
		{
			get
			{
				return defaultInstance;
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "127.0.0.1" )]
		public string MasterIP
		{
			get
			{
				return ( ( string )( this["MasterIP"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "239.121.121.121" )]
		public string McastIP
		{
			get
			{
				return ( ( string )( this["McastIP"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "0.0.0.0" )]
		public string LocalIP
		{
			get
			{
				return ( ( string )( this["LocalIP"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "0" )]
		public string McastAppStates
		{
			get
			{
				return ( ( string )( this["McastAppStates"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string MachineId
		{
			get
			{
				return ( ( string )( this["MachineId"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string ClientId
		{
			get
			{
				return ( ( string )( this["ClientId"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "5045" )]
		public int MasterPort
		{
			get
			{
				return ( ( int )( this["MasterPort"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string LogFile
		{
			get
			{
				return ( ( string )( this["LogFile"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string SharedConfigFile
		{
			get
			{
				return ( ( string )( this["SharedConfigFile"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string StartupPlan
		{
			get
			{
				return ( ( string )( this["StartupPlan"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string StartupScript
		{
			get
			{
				return ( ( string )( this["StartupScript"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "0" )]
		public string StartHidden
		{
			get
			{
				return ( ( string )( this["StartHidden"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string Mode
		{
			get
			{
				return ( ( string )( this["Mode"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string RootForRelativePaths
		{
			get
			{
				return ( ( string )( this["RootForRelativePaths"] ) );
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string MainFormLocation
		{
			get
			{
				return ( ( string )( this["MainFormLocation"] ) );
			}
			set
			{
				this["MainFormLocation"] = value;
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string IsMaster
		{
			get
			{
				return ( ( string )( this["IsMaster"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "5050" )]
		public int CLIPort
		{
			get
			{
				return ( ( int )( this["CLIPort"] ) );
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + S" )]
		public string StartPlanHotKey
		{
			get
			{
				return ( ( string )( this["StartPlanHotKey"] ) );
			}
			set
			{
				this["StartPlanHotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + K" )]
		public string KillPlanPlanHotKey
		{
			get
			{
				return ( ( string )( this["KillPlanPlanHotKey"] ) );
			}
			set
			{
				this["KillPlanPlanHotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + R" )]
		public string RestartPlanPlanHotKey
		{
			get
			{
				return ( ( string )( this["RestartPlanPlanHotKey"] ) );
			}
			set
			{
				this["RestartPlanPlanHotKey"] = value;
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string LocalConfigFile
		{
			get
			{
				return ( ( string )( this["LocalConfigFile"] ) );
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string ShowJustAppsFromCurrentPlan
		{
			get
			{
				return ( ( string )( this["ShowJustAppsFromCurrentPlan"] ) );
			}
			set
			{
				this["ShowJustAppsFromCurrentPlan"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 0" )]
		public string SelectPlan0HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan0HotKey"] ) );
			}
			set
			{
				this["SelectPlan0HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 1" )]
		public string SelectPlan1HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan1HotKey"] ) );
			}
			set
			{
				this["SelectPlan1HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 2" )]
		public string SelectPlan2HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan2HotKey"] ) );
			}
			set
			{
				this["SelectPlan2HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 3" )]
		public string SelectPlan3HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan3HotKey"] ) );
			}
			set
			{
				this["SelectPlan3HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 4" )]
		public string SelectPlan4HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan4HotKey"] ) );
			}
			set
			{
				this["SelectPlan4HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 5" )]
		public string SelectPlan5HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan5HotKey"] ) );
			}
			set
			{
				this["SelectPlan5HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 6" )]
		public string SelectPlan6HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan6HotKey"] ) );
			}
			set
			{
				this["SelectPlan6HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 7" )]
		public string SelectPlan7HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan7HotKey"] ) );
			}
			set
			{
				this["SelectPlan7HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 8" )]
		public string SelectPlan8HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan8HotKey"] ) );
			}
			set
			{
				this["SelectPlan8HotKey"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "Control + Shift + Alt + 9" )]
		public string SelectPlan9HotKey
		{
			get
			{
				return ( ( string )( this["SelectPlan9HotKey"] ) );
			}
			set
			{
				this["SelectPlan9HotKey"] = value;
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "500" )]
		public int TickPeriod
		{
			get
			{
				return ( ( int )( this["TickPeriod"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "50" )]
		public int MasterTickPeriod
		{
			get
			{
				return ( ( int )( this["MasterTickPeriod"] ) );
			}
		}

		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute( "" )]
		public string GuiAppExe
		{
			get
			{
				return ( ( string )( this["GuiAppExe"] ) );
			}
		}

	}
}
