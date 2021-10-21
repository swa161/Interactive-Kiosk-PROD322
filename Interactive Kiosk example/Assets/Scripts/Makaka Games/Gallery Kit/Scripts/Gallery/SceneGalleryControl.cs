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
using UnityEngine.Video;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;

[HelpURL("https://makaka.org/unity-assets")]
public class SceneGalleryControl : MonoBehaviour 
{
	[SerializeField]
	private GameObject mainCamera;

	[SerializeField]
	private VideoPlayerXControl videoPlayerXControl;

	public ImageGalleryControl imageGalleryOfTriples;
	public ImageGalleryControl imageGalleryFullScreen;

	public TextsControl textsControls;

	[HideInInspector] 
	public bool isTestMode = false;

	[HideInInspector] 
	public string currentProjectName = "Almaty";

	[HideInInspector] 
	public  string currentTabName = "Eurasia";

	private void Start() 
	{
		if (isTestMode)
		{
			ResourcePaths.TabContents.currentTabName = currentTabName;
			ResourcePaths.TabContents.currentProjectName = currentProjectName;
		}

		LoadTexts();

		LoadResources();
	}

	private void LoadResources()
	{
		VideoClip[] videoClips =
			Resources.LoadAll<VideoClip>(
				ResourcePaths.TabContents.GetCurrentProjectVideoClipsPath());

		Sprite[] videoCovers =
			Resources.LoadAll<Sprite>(
				ResourcePaths.TabContents.GetCurrentProjectVideoCoversPath());

		if (videoClips.Length != videoCovers.Length)
		{
			Debug.LogError("Amount of VideoClips " +
				"& VideoCovers  are not match in Resources!");
		}
		else
		{
			Sprite[] fullScreenImages = Resources.LoadAll<Sprite>(
					ResourcePaths.TabContents.
						GetCurrentProjectImagesFullScreenPath());

			Sprite[] videoCoversAndFullscreenImages =
				new Sprite[videoCovers.Length + fullScreenImages.Length];

			if (videoCoversAndFullscreenImages.Length > 0)
			{
				Array.Copy(videoCovers, videoCoversAndFullscreenImages,
					videoCovers.Length);

				Array.Copy(
					fullScreenImages,
					0,
					videoCoversAndFullscreenImages,
					videoCovers.Length,
					fullScreenImages.Length);

				imageGalleryOfTriples.Init(
					videoCoversAndFullscreenImages, null, null);

				imageGalleryFullScreen.Init(
					videoCoversAndFullscreenImages,
					ChangeScreenOfImageGalleryFullscreen,
					ChangeScreenOfImageGalleryFullscreen);

				for (int i = 0; i < imageGalleryOfTriples.Buttons.Count; i++)
				{
					//It needs for lambda behaviour
					int tempIndexForLambda = i;

					if (i < videoCovers.Length)
					{
						imageGalleryOfTriples.AddButtonAction(
							i,
							() =>
							{
								OpenImageGalleryFullScreenAt(
									tempIndexForLambda);

								videoPlayerXControl.ShowAndPlay(
									videoClips[tempIndexForLambda]);

								mainCamera.SetActive(false);

								imageGalleryFullScreen.SetBackgroundEnabled(
									false);
							});
					}
					else
					{
						imageGalleryOfTriples.AddButtonAction(
							i,
							() =>
							{
								OpenImageGalleryFullScreenAt(
									tempIndexForLambda);

								videoPlayerXControl.Close();

								mainCamera.SetActive(true);

								imageGalleryFullScreen.SetBackgroundEnabled(
									true);
							});

						imageGalleryOfTriples.HideImageVideoOverlay(i);
					}
				}

				for (int i = 0; i < imageGalleryFullScreen.Buttons.Count; i++)
				{
					if (i < videoCovers.Length)
					{
						imageGalleryFullScreen.AddButtonAction(
							i,
							() =>
							{
								videoPlayerXControl.Close();

								mainCamera.SetActive(true);

								imageGalleryFullScreen.SetActive(false);

							});

						imageGalleryFullScreen.HideImage(i);
					}
					else
					{
						imageGalleryFullScreen.AddButtonAction(
							i,
							() =>
							{
								imageGalleryFullScreen.SetActive(false);
							});
					}
				}

				void ChangeScreenOfImageGalleryFullscreen(int i)
				{
					if (i < videoCovers.Length)
					{
						videoPlayerXControl.ShowAndPlay(videoClips[i]);

						mainCamera.SetActive(false);

						imageGalleryFullScreen.SetBackgroundEnabled(false);
					}
					else
					{
						videoPlayerXControl.Close();

						mainCamera.SetActive(true);

						imageGalleryFullScreen.SetBackgroundEnabled(true);
					}
				}
			}
            else
            {
				Debug.LogError("No Media Loaded!");
            }
		}
	}

	private void LoadTexts()
	{
		textsControls.InitByPath(
			ResourcePaths.TabContents.GetCurrentProjectTextsPath());
	}

	public void OpenImageGalleryFullScreenAt(int galleryElementIndex)
	{
		imageGalleryFullScreen.SetActive(true);
		imageGalleryFullScreen.GoToGalleryElementBy(
			galleryElementIndex, false);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneGalleryControl))]
public class SceneGalleryControl_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DrawDefaultInspector();

		SceneGalleryControl sceneGalleryControl = (SceneGalleryControl)target;

		EditorGUILayout.PropertyField(
			serializedObject.FindProperty("isTestMode"));

		if(sceneGalleryControl.isTestMode)
		{
			EditorGUILayout.PropertyField(
				serializedObject.FindProperty("currentTabName"));

			EditorGUILayout.PropertyField(
				serializedObject.FindProperty("currentProjectName"));
		}

		EditorGUILayout.HelpBox(
			"Test mode is used to check the loading of resources " +
            "for target content", MessageType.Info);

		// Apply changes to the serializedProperty -
        // always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties();
	}
}
#endif
