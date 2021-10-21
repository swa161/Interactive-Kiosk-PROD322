/*
===================================================================
Unity Assets by MAKAKA GAMES: https://makaka.org/o/all-unity-assets
===================================================================

Online Docs (Latest): https://makaka.org/unity-assets
Offline Docs: You have a PDF file in the package folder.

=======
SUPPORT
=======

First of all, read the docs. If it didn’t help, get the support.

Web: https://makaka.org/support
Email: info@makaka.org

If you find a bug or you can’t use the asset as you need, 
please first send email to info@makaka.org
before leaving a review to the asset store.

I am here to help you and to improve my products for the best.
*/

using System;

public static class TextHacks
{
	public static string GetString(this EscapeChars escapeChar)
	{
		switch (escapeChar) 
		{
			case EscapeChars.NewLineAndTabulation:
				return "\n\t";
			case EscapeChars.NewLineAnd4Spaces:
					return "\n    ";
			case EscapeChars.NewLine: 
				return "\n";
			case EscapeChars.Tabulation: 
				return "\t";
			case EscapeChars.Space: 
				return " ";
			case EscapeChars.None: 
				return string.Empty;
			default:
				return string.Empty;
		}
	}

	public static string GetFirstLineFrom(string str)
	{
		var newLineIndex = GetFisrtNewLineIndexFrom (str);

		return newLineIndex == -1 ? str : str.Substring(0, newLineIndex);
	}

	public static string RemoveFirstLineFrom(string str)
	{
		var newLineIndex = GetFisrtNewLineIndexFrom (str);

		return newLineIndex == -1 ? str : str.Substring(newLineIndex, str.Length - newLineIndex).Trim();
	}

	public static int GetFisrtNewLineIndexFrom (string str)
	{
		return str.IndexOf (Environment.NewLine);
	}
}

public enum EscapeChars
{
	NewLine,
	Tabulation,
	NewLineAndTabulation,
	NewLineAnd4Spaces,
	Space,
	None
};
