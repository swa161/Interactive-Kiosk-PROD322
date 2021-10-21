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
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

using System;
using System.Collections.Generic;

[HelpURL("https://makaka.org/unity-assets")]
public class ImageGalleryControl : MonoBehaviour 
{
	public Transform content;

	[SerializeField]
	private HorizontalScrollSnap horizontalScrollSnap; 
	public Sprite noImagePlaceHolder;

	[SerializeField]
	private Image imageBackground;

	private CanvasGroup canvasGroup;

	public List<Button> Buttons { get => buttons; }
	private List<Button> buttons;

	private List<Image> images;

	private List<Image> imagesVideoOverlay;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void Init(Sprite[] sprites,
		Action<int> OnNextScreenSetting,
		Action<int> OnPreviousScreenSetting)
	{
		if (sprites.Length == 0) 
		{
			Debug.Log("Images are Missing in Resources!!!");
		}
		else
		{
			Transform template = content.GetChild(0);

			if (template)
			{
				int duplicatingCount =
					Mathf.CeilToInt((float)sprites.Length
						/ template.childCount) - 1;

				DuplicateTemplate (template, duplicatingCount);

				InitImages(sprites);

				horizontalScrollSnap.Init(
					OnNextScreenSetting,
					OnPreviousScreenSetting);

				InitButtons();
			}
			else
			{
				Debug.Log("Template is Missing!!!");
			}
		}
	}

	private void InitImages(Sprite[] sprites)
	{
		images = new List<Image>();

		imagesVideoOverlay = new List<Image>();

		int i = 0;
		
		foreach (Transform currentTemplate in content) 
		{
			foreach (Transform buttonImage in currentTemplate) 
			{
				images.Add(buttonImage.GetComponent<Image>());

				if (buttonImage.childCount > 0)
				{
					imagesVideoOverlay.Add(
						buttonImage.GetChild(0).GetComponent<Image>());
				}

				if (i < sprites.Length)
				{
					images[i].sprite = sprites[i];
				}
				else
				{
					//put "no image" place holder
					images[i].sprite = noImagePlaceHolder;
					images[i].raycastTarget = false;

					if (buttonImage.childCount > 0)
                    {
						imagesVideoOverlay[i].sprite =
							noImagePlaceHolder;
					}
				}

				i++;
			}
		}
	}

	private void DuplicateTemplate(Transform template, int count)
	{
		for (int i = 0; i < count; i++) 
		{
			Transform newGalleryElement = Instantiate(template);

			newGalleryElement.SetParent(content);
			newGalleryElement.localScale = Vector3.one;
		}
	}

	/// <summary>
	/// To Manage Event Listeners for Gallery Elements (ImageButton)
    /// independently
	/// </summary>
	private void InitButtons()
	{
		buttons = new List<Button>();

		foreach (Transform triple in content) 
		{
			foreach (Transform buttonImage in triple) 
			{
				buttons.Add(buttonImage.GetComponent<Button>());
			}
		}
	}
		
	/// <summary>
	/// For transition to target Gallery Element
	/// </summary>
	public void GoToGalleryElementBy(int galleryElementIndex, bool isAnimated)
	{
		horizontalScrollSnap.GoToGalleryElementBy(
			galleryElementIndex, isAnimated);
	}

	public void SetActive(bool isActive)
	{
		canvasGroup.alpha = isActive ? 1 : 0;
		canvasGroup.interactable = isActive;
		canvasGroup.blocksRaycasts = isActive;

		//Set On Top || On Bottom
		if (isActive)
		{
			transform.SetAsLastSibling();
		}
		else 
		{
			transform.SetAsFirstSibling();
			horizontalScrollSnap.CompleteTransition();
		}
	}

	public void SetBackgroundEnabled(bool isEnabled)
	{
        if (imageBackground)
        {
			imageBackground.enabled = isEnabled;
		}
	}

	public void HideImage(int index)
	{
		images[index].color = new Color(1f, 1f, 1f, 0f);
	}

	public void HideImageVideoOverlay(int index)
	{
		imagesVideoOverlay[index].color = new Color(1f, 1f, 1f, 0f);
	}

	public void AddButtonAction(int index, UnityAction call)
    {
		buttons[index].onClick.AddListener(call);
	}
}