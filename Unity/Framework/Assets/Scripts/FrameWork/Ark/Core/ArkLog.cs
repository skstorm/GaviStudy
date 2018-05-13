using System.Collections.Generic;
using gipo.util;
using System;

namespace Ark.Core
{
	public static class ArkLogLevelDefine
	{
		public const uint Debug = 1;     // 0001
		public const uint Info = 2;  // 0010
		public const uint Warning = 4; // 0100
		public const uint Error = 8;     // 1000
	}

	public static class ArkLog
	{
		const string PREFIX_ARK_LOG = "ARK";

		static Dictionary<uint, string> prefixDict = null;

		static ISetting _setting = null;

		static string GetLogPrefix(string colorName, string level)
		{
			return String.Format("<b> <color='{0}'>[{1}][{2}] </color> </b>",
				colorName,
				PREFIX_ARK_LOG,
				level);
		}

		static ArkLog()
		{
		}

		public static void Init(ISetting setting)
		{
			prefixDict = new Dictionary<uint, string>()
			{
				{ArkLogLevelDefine.Error, GetLogPrefix ("red", "Error")},
				{ArkLogLevelDefine.Warning,  GetLogPrefix ("yellow", "Warning")},
				{ArkLogLevelDefine.Info,  GetLogPrefix ("green",  "Info")},
				{ArkLogLevelDefine.Debug, GetLogPrefix ("black",  "Debug")},
			};

			_setting = setting;
		}

		public static void Release()
		{
			prefixDict = null;
			_setting = null;
		}

		public static void Info(string message)
		{
			ShowMessage(message, ArkLogLevelDefine.Info);
		}

		public static void Debug(string message)
		{
			ShowMessage(message, ArkLogLevelDefine.Debug);
		}

		public static void Warning(string message)
		{
			ShowMessage(message, ArkLogLevelDefine.Warning);
		}

		public static void Error(string message)
		{
			ShowMessage(message, ArkLogLevelDefine.Error);
		}

		private static void ShowMessage(string message, uint level)
		{
			if (!CanShowLogLevel(level))
			{
				return;
			}

			PosInfos posInfoCallLog = new PosInfos(3);
			UnityEngine.Debug.Log(prefixDict[level] + message + "\n " + posInfoCallLog.ToString() + " \n\n");
		}
		
		private static bool CanShowLogLevel(uint level)
		{
			return (_setting.DisplayLogLevel & level) == level;
		}
	}
}
