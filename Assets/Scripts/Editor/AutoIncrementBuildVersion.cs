using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor
{
	public class AutoIncrementBuildVersion : MonoBehaviour
	{
		[PostProcessBuild(0)]
		public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
		{
			var currentVersion = PlayerSettings.bundleVersion;
			Debug.Log($"Building version : {currentVersion}");

			try
			{
				var major = Convert.ToInt32(currentVersion.Split('.')[0]);
				var minor = Convert.ToInt32(currentVersion.Split('.')[1]);
				var build = Convert.ToInt32(currentVersion.Split('.')[2]);
				var local = Convert.ToInt32(currentVersion.Split('.')[3]) + 1;

				PlayerSettings.bundleVersion = major + "." + minor + "." + build + "." + local;

				if (buildTarget == BuildTarget.iOS)
				{
					PlayerSettings.iOS.buildNumber = "" + build + "";
					Debug.Log("Finished with bundle version code:" + PlayerSettings.iOS.buildNumber +
					          "and version" + PlayerSettings.bundleVersion);

				}
				else if (buildTarget == BuildTarget.Android)
				{
					PlayerSettings.Android.bundleVersionCode = build;
					Debug.Log("Finished with bundle version code:" +
					          PlayerSettings.Android.bundleVersionCode + " and version" +
					          PlayerSettings.bundleVersion);
				}

			}
			catch (Exception e)
			{
				Debug.LogError(e);
				Debug.LogError(
					"AutoIncrementBuildVersion script failed. Make sure your current bundle version is in the format X.X.X.X (e.g. 1.0.0.0) and not X.X (1.0) or X (1).");
			}
			finally
			{
				Debug.Log($"Changed build version to : {PlayerSettings.bundleVersion}");
			}
		}
	}
}