using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ark.Util
{
	public enum SystemText
	{
		EMPTY,
		SLASH,
		HYPHEN,
		CR,
		LF,
		CRLF,
		DOUBLE_SLASH,
		PLUS,
		COMMA,
		TAB,
		SPACE,
		PIPE,
	}

	public static partial class SystemTextUtil
	{
		static string[] StrSystemText =
		{
			"",
			"/",
			"-",
			"\r",
			"\n",
			"\r\n",
			"//",
			"+",
			",",
			"\t",
			" ",
			"|",
		};

		public static string Text(this SystemText value, params string[] args)
		{
			if (args.Length > 0)
			{
				return string.Format(StrSystemText[(int)value], args);
			}
			else
			{
				return StrSystemText[(int)value];
			}
		}
	}
}