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
using System.Collections.Generic;

[HelpURL("https://makaka.org/unity-assets")]
public class TabControl : MonoBehaviour 
{
	private string ActiveTabName 
	{
		get
		{
			return ResourcePaths.TabContents.currentTabName;
		}
		set
		{
			ResourcePaths.TabContents.currentTabName = value;
		}
	}

	public Transform tabContentsTransform;

	private List<TabContent> tabContents = new List<TabContent>();

	private void Start()
	{
		InitTabContentsWithConclusionOfActiveTab();

		InitActiveTab ();
	}

	private void InitTabContentsWithConclusionOfActiveTab()
	{
		foreach (Transform tabContentTransform in tabContentsTransform) 
		{
			tabContents.Add(new TabContent(tabContentTransform));

			//First Launch - conclude active tab from active tabContent
			if (ActiveTabName == string.Empty
				&& tabContentTransform.gameObject.activeSelf == true) 
			{
				ActiveTabName = tabContentTransform.name;
			}
			//Not First Launch of Scene - recall active tab from memory
			else if (ActiveTabName != tabContentTransform.name) 
			{
				tabContentTransform.gameObject.SetActive(false);
			}
		}
	}

	private void InitActiveTab()
	{
		Toggle[] tabToggles = GetComponentsInChildren<Toggle> ();

		foreach (Toggle toggle in tabToggles) 
		{
			if (toggle.name == ActiveTabName) 
			{
				toggle.isOn = true; //call ToggleTab(Transform toggle)

				break;
			}
		}
	}

	public void ToggleTab(Transform toggle)
	{
		TabContent targetTabContent = null;

		foreach (TabContent tabContent in tabContents) 
		{
			if (toggle.name == tabContent.GetName()) 
			{
				targetTabContent = tabContent;

				break;
			}
		}

		if (targetTabContent == null)
		{
			Debug.Log("Toggle's Name is not correct!!!");
		}
		else
		{
			targetTabContent.ToggleHideShow();

			if (toggle.GetComponent<Toggle>().isOn)
			{	
				ActiveTabName = toggle.name;
			}
		}
	}
}
