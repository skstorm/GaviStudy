using System.Collections.Generic;

namespace Ark.Util
{
	public enum ESystemText
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
	
	public static class SystemTextUtil
	{
		private static readonly Dictionary<ESystemText, string> _dicStrSystemText = new Dictionary<ESystemText, string>
		{
			{ ESystemText.EMPTY, ""},
			{ ESystemText.SLASH, "/"},
			{ ESystemText.HYPHEN, "-"},
			{ ESystemText.CR, "\r"},
			{ ESystemText.LF, "\n"},
			{ ESystemText.CRLF, "\r\n"},
			{ ESystemText.DOUBLE_SLASH, "//"},
			{ ESystemText.PLUS, "+"},
			{ ESystemText.COMMA, ","},
			{ ESystemText.TAB, "\t"},
			{ ESystemText.SPACE, " "},
			{ ESystemText.PIPE, "|"},
		};

		public static string Text(this ESystemText value)
		{
			return _dicStrSystemText[value];
		}
	}
}