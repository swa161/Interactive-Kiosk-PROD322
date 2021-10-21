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

using UnityEngine.Video;

public static class ResourcePaths
{
	public static class Video
	{
		public static VideoClip current;
	}

	public static class TabContents
	{
		private static string tabContentsRoot = "TabContents";

		/// <summary>
		/// Must be string.Empty for remembering Active Tab.
		/// Active Tab is set to the Editor by activating target tab content.
		/// </summary>
		public static string currentTabName = string.Empty;

		public static string currentProjectName = string.Empty;

		private static string relativeProjectImagesFullScreen = "Images/FullScreen";

		private static string relativeProjectVideoClips = "Videos/Clips";

		private static string relativeProjectVideoCovers= "Videos/Covers";

		private static string relativeProjectText = "Text";

		private static string GetCurrentProjectPath()
		{
			return tabContentsRoot + "/" + currentTabName + "/"+ currentProjectName;
		}

		public static string GetCurrentProjectImagesFullScreenPath()
		{
			return GetCurrentProjectPath() + "/" + relativeProjectImagesFullScreen;
		}

		public static string GetCurrentProjectTextsPath()
		{
			return GetCurrentProjectPath() + "/" + relativeProjectText;
		}

		public static string GetCurrentProjectVideoClipsPath()
		{
			return GetCurrentProjectPath() + "/" + relativeProjectVideoClips;
		}

		public static string GetCurrentProjectVideoCoversPath()
		{
			return GetCurrentProjectPath() + "/" + relativeProjectVideoCovers;
		}
	}
		
}
