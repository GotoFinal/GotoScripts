using UnityEditor;
using UnityEngine;

namespace GotoFinal.PlayModeBenchmark.Editor
{
	public class PlayModeTest
	{
		[InitializeOnLoadMethod]
		private static void Initialize()
		{
			void OnPlayModeStateChanged(PlayModeStateChange state)
			{
				if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode)
					SessionState.SetFloat("state_exit_time", (float)EditorApplication.timeSinceStartup);
				else
					Debug.Log(state + " in: " + ((float)EditorApplication.timeSinceStartup - SessionState.GetFloat("state_exit_time", 0f)));
			}

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}
	}
}