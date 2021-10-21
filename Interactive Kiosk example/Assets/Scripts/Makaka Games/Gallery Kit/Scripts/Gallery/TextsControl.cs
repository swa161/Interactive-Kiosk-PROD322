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

using UnityEngine;
using UnityEngine.UI;

using TMPro;

[HelpURL("https://makaka.org/unity-assets")]
public class TextsControl : MonoBehaviour
{
	public TextMeshProUGUI textHeader;
	public TextMeshProUGUI textMain;

	public Scrollbar textMainScrollbar;
	public float textMainScrollbarPos = 1f;

	public EscapeChars textMainStart = EscapeChars.NewLineAndTabulation;
	public EscapeChars textMainEnd = EscapeChars.NewLine;

	public void InitByPath(string pathToTextFile)
	{
		TextAsset[] textAssets = Resources.LoadAll<TextAsset>(pathToTextFile);

		if (textAssets.Length == 0) 
		{
			Debug.Log("Text is Missing in Resources!!!");
		}
		else
		{		
			TextAsset targetTextAsset = textAssets[0];

			Init(targetTextAsset.text);
		}
	}

	public void Init(string texts)
	{
		textHeader.text = TextHacks.GetFirstLineFrom(texts);

		textMain.text = textMainStart.GetString() 
			+ TextHacks.RemoveFirstLineFrom (texts) 
			+ textMainEnd.GetString();

		textMainScrollbar.value = textMainScrollbarPos;
	}

}
	