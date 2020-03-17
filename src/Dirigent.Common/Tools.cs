﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dirigent.Common;
using System.IO;
using System.Reflection;

namespace Dirigent.Common
{
	public class Tools
	{
		public static ILaunchPlan FindPlanByName(IEnumerable<ILaunchPlan> planRepo, string planName)
		{
			// find plan in the repository
			ILaunchPlan plan;
			try
			{
				plan = planRepo.First((i) => i.Name == planName);
				return plan;
			}
			catch
			{
				throw new UnknownPlanName(planName);
			}

		}

		public static string GetAppStateString(AppIdTuple t, AppState appState)
		{
			var sbFlags = new StringBuilder();
			if (appState.Started) sbFlags.Append("S");
			if (appState.StartFailed) sbFlags.Append("F");
			if (appState.Running) sbFlags.Append("R");
			if (appState.Killed) sbFlags.Append("K");
			if (appState.Initialized) sbFlags.Append("I");
			if (appState.PlanApplied) sbFlags.Append("P");
			if (appState.Dying) sbFlags.Append("D");
			if (appState.Restarting) sbFlags.Append("X");

			var now = DateTime.UtcNow;

			var stateStr = String.Format("APP:{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
				t.ToString(),
				sbFlags.ToString(),
				appState.ExitCode,
				(now - appState.LastChange).TotalSeconds,
				appState.CPU,
				appState.GPU,
				appState.Memory,
				appState.PlanName
			);

			return stateStr;
		}

		public static string GetPlanStateString(string planName, PlanState planState)
		{
			var stateStr = String.Format("PLAN:{0}:{1}", planName, planState.OpStatus.ToString());
			return stateStr;
		}

		// returs first line without CR/LF
		public static string JustFirstLine(string multiLineString)
		{
			var crPos = multiLineString.IndexOf('\r');
			var lfPos = multiLineString.IndexOf('\n');
			if (crPos >= 0 || lfPos >= 0)
			{
				return multiLineString.Substring(0, Math.Min(crPos, lfPos));
			}
			return multiLineString; // no other line found
		}

		public static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
}

}
