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

[HelpURL("https://makaka.org/unity-assets")]
public class TabContent
{
	private Transform tabContentTransform;

	private bool isHidden = true;

	public TabContent(Transform tabContentTransform)
	{
		this.tabContentTransform = tabContentTransform;
	}

	public void SetActive(bool isActive)
	{
		tabContentTransform.gameObject.SetActive (isActive);
	}

	public string GetName()
	{
		return tabContentTransform.name;
	}

	public void ToggleHideShow()
	{
		isHidden = !isHidden;

		if (!tabContentTransform.gameObject.activeSelf)
			SetActive(true);

		foreach (Transform tabContentTransformChild in tabContentTransform) 
		{
			HideShowAnimationForChild(tabContentTransformChild);

			OffOnButtonsInteractibleForChild(tabContentTransformChild);
		}

		CanvasGroup canvasGroup =
			tabContentTransform.GetComponent<CanvasGroup>();

		if (canvasGroup) 
		{
			canvasGroup.interactable = !isHidden;
			canvasGroup.blocksRaycasts = !isHidden;
		}
	}

	private void HideShowAnimationForChild(Transform tabContentTransformChild)
	{
		Animator[] animators =
			tabContentTransformChild.GetComponentsInChildren<Animator>();

		foreach (Animator animator in animators) 
		{
			if (animator) 
			{
				animator.SetBool("Hide", isHidden);
				animator.SetBool("Show", !isHidden);
			}
		}
	}
		
	private void OffOnButtonsInteractibleForChild(
		Transform tabContentTransformChild)
	{
		Button[] buttons =
			tabContentTransform.GetComponentsInChildren<Button>();

		foreach (Button button in buttons) 
		{
			if (button) 
			{
				button.interactable = !isHidden;
			}
		}
	}
}