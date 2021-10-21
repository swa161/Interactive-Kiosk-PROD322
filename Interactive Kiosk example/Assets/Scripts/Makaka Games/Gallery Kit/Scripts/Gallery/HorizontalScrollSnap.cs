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

using UnityEngine.EventSystems;

using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	[HelpURL("https://makaka.org/unity-assets")]
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Horizontal Scroll Snap")]
	public class HorizontalScrollSnap :
		MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		private Transform _screensContainer;
		
		private int _screens = 1;
		private int _startingScreen = 1;

		private bool _startDrag = true;
		private Vector3 _startPosition = new Vector3();
		private int _currentScreenOnBeginDrag;
		
		private bool _fastSwipeTimer = false;
		//to determine if a fast swipe was performed
		private bool fastSwipe = false; 
		private int _fastSwipeCounter = 0;
		private int _fastSwipeTarget = 30;
		
		private List<Vector3> _positions;
		private ScrollRect _scroll_rect;
		private Vector3 _lerp_target;

		private int _containerSize;
		private int _contentSpacing;

		private bool isLerp = false;

		public bool IsLerp
		{
			get
			{
				return isLerp;
			}
			set
			{
				isLerp = value;
			}
		}

		public Action<int> OnNextScreenSetting;
		public Action<int> OnPreviousScreenSetting;

		[Tooltip("The gameobject that contains " +
            "toggles which suggest pagination. (optional)")]
		private GameObject Pagination;

		[Tooltip("Button to go to the previous page.")]
		[SerializeField]
		private Button prevButton;
		private GameObject prevButtonChild;

		[Tooltip("Button to go to the next page.")]
		[SerializeField]
		private Button nextButton;
		private GameObject nextButtonChild;

		[Tooltip("Speed for NextButton & PrevButton")]
		public float navigationSpeed = 7f;
		
		private bool UseFastSwipe = true;
		public int FastSwipeThreshold = 100;

		public void Init(
			Action<int> OnNextScreenSetting,
			Action<int> OnPreviousScreenSetting)
		{
			_scroll_rect = gameObject.GetComponent<ScrollRect>();
			_screensContainer = _scroll_rect.content;

			_contentSpacing = (int)_screensContainer
				.GetComponent<HorizontalLayoutGroup>().spacing;

			DistributePages();
			
			_screens = _screensContainer.childCount;
			
			_positions = new List<Vector3>();
			
			if (_screens > 0)
			{
				for (int i = 0; i < _screens; ++i)
				{
					_scroll_rect.horizontalNormalizedPosition =
						(float)i / (float)(_screens - 1);

					_positions.Add(_screensContainer.localPosition);
				}
			}

			_scroll_rect.horizontalNormalizedPosition =
				(float)(_startingScreen - 1) / (float)(_screens - 1);
			
			_containerSize = (int)_screensContainer.gameObject
				.GetComponent<RectTransform>().rect.width;
			
			ChangeBulletsInfo(CurrentScreen());

			InitPreviousNextButtons();

			ShowPreviousNextButtons(0);

			this.OnNextScreenSetting += ShowPreviousNextButtons;
			this.OnPreviousScreenSetting += ShowPreviousNextButtons;

			this.OnNextScreenSetting += OnNextScreenSetting;
			this.OnPreviousScreenSetting += OnPreviousScreenSetting;
		}

		private void Update()
		{
			if (IsLerp)
			{
				_screensContainer.localPosition =
					Vector3.Lerp(
						_screensContainer.localPosition,
						_lerp_target,
						navigationSpeed * Time.deltaTime);

				if (Vector3.Distance(
					_screensContainer.localPosition, _lerp_target) < 0.005f)
				{
					IsLerp = false;
				}

				// Change the info bullets at the bottom of the screen.
				// Just for visual effect
				if (Vector3.Distance(
					_screensContainer.localPosition, _lerp_target) < 10f)
				{
					ChangeBulletsInfo(CurrentScreen());
				}
			}

			if (_fastSwipeTimer)
			{
				_fastSwipeCounter++;
			}
		}

		public void CompleteTransition()
		{
			if (_screensContainer)
			{	
				//Makes sense if IsLerp == true
				_screensContainer.localPosition = _lerp_target; 
			}
		}

		/// <summary>
		/// For transition to target Gallery Element
		/// </summary>
		public void GoToGalleryElementBy(
			int galleryElementIndex, bool isAnimated)
		{
			if (_positions != null && galleryElementIndex < _positions.Count) 
			{
				if (isAnimated) 
				{
					if (CurrentScreen() < _screens - 1)
					{
						IsLerp = true;

						_lerp_target = _positions[galleryElementIndex];
							
						ChangeBulletsInfo(CurrentScreen() + 1);
					}
				}
				else
				{
					_screensContainer.localPosition =
						_positions[galleryElementIndex];
				}

				ShowPreviousNextButtons(galleryElementIndex);
			} 
			else 
			{
				Debug.Log(string.Format(
					"GoToGalleryElementBy: Index {0} Out Of Range!", 
					galleryElementIndex));
			}
		}

		private void InitPreviousNextButtons()
        {
			if (nextButton.gameObject && prevButton.gameObject)
            {
                nextButton.onClick.AddListener(NextScreen);
                prevButton.onClick.AddListener(PreviousScreen);

                SetButtonDisabledColorAlpha(nextButton, 0f);
				SetButtonDisabledColorAlpha(prevButton, 0f);

				if (nextButton.transform.childCount > 0)
                {
                    nextButtonChild =
                        nextButton.transform.GetChild(0).gameObject;
                }

                if (prevButton.transform.childCount > 0)
                {
                    prevButtonChild =
                        prevButton.transform.GetChild(0).gameObject;
                }
            }
        }

        private void SetButtonDisabledColorAlpha(Button button, float alpha)
        {
            ColorBlock colorBlock = button.colors;

            colorBlock.disabledColor = new Color(
                colorBlock.disabledColor.r,
                colorBlock.disabledColor.g,
                colorBlock.disabledColor.b,
				alpha);

			button.colors = colorBlock;
        }

        private void ShowPreviousNextButtons(int index)
        {
			if (index == 0)
			{
                if (_positions.Count == 1)
                {
					ShowPreviousNextButtons(false, false);
				}
                else
                {
					ShowPreviousNextButtons(false, true);
				}
			}
			else if (index == _positions.Count - 1)
			{
				ShowPreviousNextButtons(true, false);
			}
			else
			{
				ShowPreviousNextButtons(true, true);
			}	
		}

		private void ShowPreviousNextButtons(
			bool isPrevButtonActive, bool isNextButtonActive)
        {
			if (nextButton.gameObject && prevButton.gameObject)
			{
				prevButton.interactable = isPrevButtonActive;
				prevButtonChild.SetActive(isPrevButtonActive);

				nextButton.interactable = isNextButtonActive;
				nextButtonChild.SetActive(isNextButtonActive);
			}
		}
		
		//Function for switching screens with buttons
		public void NextScreen()
		{
			int i = CurrentScreen();

			if (i < _screens - 1)
			{
				IsLerp = true;
				_lerp_target = _positions[i + 1];
				
				ChangeBulletsInfo(i + 1);

				OnNextScreenSetting?.Invoke(i + 1);
			}
		}
		
		//Function for switching screens with buttons
		public void PreviousScreen()
		{
			int i = CurrentScreen();

			if (i > 0)
			{
				IsLerp = true;
				_lerp_target = _positions[i - 1];
				
				ChangeBulletsInfo(i - 1);

				OnPreviousScreenSetting?.Invoke(i - 1);
			}
		}
		
		//Because the CurrentScreen function is not so reliable,
        //these are the functions used for swipes
		private void NextScreenCommand()
		{
			if (_currentScreenOnBeginDrag < _screens - 1)
			{
				IsLerp = true;
				_lerp_target = _positions[_currentScreenOnBeginDrag + 1];
				
				ChangeBulletsInfo(_currentScreenOnBeginDrag + 1);

				OnNextScreenSetting?.Invoke(_currentScreenOnBeginDrag + 1);
			}
		}
		
		//Because the CurrentScreen function is not so reliable,
        //these are the functions used for swipes
		private void PrevScreenCommand()
		{
			if (_currentScreenOnBeginDrag > 0)
			{
				IsLerp = true;
				_lerp_target = _positions[_currentScreenOnBeginDrag - 1];
				
				ChangeBulletsInfo(_currentScreenOnBeginDrag - 1);

				OnPreviousScreenSetting?.Invoke(_currentScreenOnBeginDrag - 1);
			}
		}
		
		//find the closest registered point to the releasing point
		private Vector3 FindClosestOnEndDragFrom(Vector3 start)
		{
			Vector3 closest = Vector3.zero;
			int closestIndex = 0;
			float distance = Mathf.Infinity;

            for (int i = 0; i < _positions.Count; i++)
			{
                if (Vector3.Distance(start, _positions[i]) < distance)
				{
					distance = Vector3.Distance(start, _positions[i]);
					closest = _positions[i];
					closestIndex = i;
				}
			}

            if (closestIndex > _currentScreenOnBeginDrag)
            {
				OnNextScreenSetting?.Invoke(closestIndex);
			}
            else if (closestIndex < _currentScreenOnBeginDrag)
            {
				OnPreviousScreenSetting?.Invoke(closestIndex);
			}
			
			return closest;
		}
		
		//returns the current screen that the is seeing
		public int CurrentScreen()
		{
			float absPoz = Math.Abs(
				_screensContainer.gameObject
					.GetComponent<RectTransform>().offsetMin.magnitude);
			
			absPoz = Mathf.Clamp(absPoz, 1, _containerSize - 1);
			
			float calc = (absPoz / _containerSize) * _screens;

			return Mathf.RoundToInt(calc);
		}
		
		//changes the bullets on the bottom of the page - pagination
		private void ChangeBulletsInfo(int currentScreen)
		{
			if (Pagination)
			{
				for (int i = 0; i < Pagination.transform.childCount; i++)
				{
					Pagination.transform.GetChild(i).
						GetComponent<Toggle>().isOn = currentScreen == i;
				}
			}
		}

		//used for changing between screen resolutions
		//not relevant for the first instance on scene
		private void DistributePages()
		{
			int _offset = 0;

			int _step = (int)_screensContainer.gameObject
				.GetComponent<RectTransform>().rect.width + _contentSpacing;

			int currentXPosition = 0;

			for (int i = 0; i < _screensContainer.transform.childCount; i++)
			{
				RectTransform child =
					_screensContainer.transform.GetChild(i).gameObject
						.GetComponent<RectTransform>();

				currentXPosition = _offset + i * _step;

				child.anchoredPosition = new Vector2(currentXPosition, 0f);
				child.sizeDelta = new Vector2(
					gameObject.GetComponent<RectTransform>().sizeDelta.x,
					gameObject.GetComponent<RectTransform>().sizeDelta.y);
			}

			int _dimension = currentXPosition + _offset * -1;

			_screensContainer.GetComponent<RectTransform>().offsetMax =
				new Vector2(_dimension, 0f);
		}

		#region Interfaces
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (_screensContainer) {
				
				_startPosition = _screensContainer.localPosition;
				_fastSwipeCounter = 0;
				_fastSwipeTimer = true;
				_currentScreenOnBeginDrag = CurrentScreen();
			}
		}
		
		public void OnEndDrag(PointerEventData eventData)
		{
			_startDrag = true;

			if (_scroll_rect && _scroll_rect.horizontal)
			{
				if (UseFastSwipe)
				{
					fastSwipe = false;

					_fastSwipeTimer = false;

					if (_fastSwipeCounter <= _fastSwipeTarget)
					{
						bool isFastSwipeDetected =
							Math.Abs(
								_startPosition.x
								- _screensContainer.localPosition.x)
							> FastSwipeThreshold;

						if (isFastSwipeDetected)
						{
							fastSwipe = true;
						}
					}

					if (fastSwipe)
					{
						if (_startPosition.x - _screensContainer.localPosition.x
							> 0)
						{
							NextScreenCommand();
						}
						else
						{
							PrevScreenCommand();
						}
					}
					else
					{
						IsLerp = true;

						_lerp_target = FindClosestOnEndDragFrom(
							_screensContainer.localPosition);
					}
				}
				else
				{
					IsLerp = true;

					_lerp_target = FindClosestOnEndDragFrom(
						_screensContainer.localPosition);
				}
			}
		}
		
		public void OnDrag(PointerEventData eventData)
		{
			IsLerp = false;

			if (_startDrag)
			{
				OnBeginDrag(eventData);

				_startDrag = false;
			}
		}
		#endregion
	}
}