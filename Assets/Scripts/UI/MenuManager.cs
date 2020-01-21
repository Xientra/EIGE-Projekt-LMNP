using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void LoadScene(int sceneIndex) {
		SceneManager.LoadScene(sceneIndex);
	}

	public void EndGame() {

#if UNITY_STANDALONE
		//If we are running in a standalone build of the game
		Application.Quit();
#endif

#if UNITY_EDITOR
		//If we are running in the editor
		UnityEditor.EditorApplication.isPlaying = false;
#endif

	}
}