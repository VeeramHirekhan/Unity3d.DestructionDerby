using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour {


	public void QuitBtnClick()
	{
        Application.Quit();
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#endif
    }
	public void RestartBtnClickLevel1()
    {
		Scene currentScene = SceneManager.GetActiveScene ();
		string sceneName = currentScene.name;
        if (sceneName == "Level1")
            SceneManager.LoadScene("Level1");
        else if (sceneName == "Level2")
            SceneManager.LoadScene("Level1");
        else if (sceneName == "EndGame")
            SceneManager.LoadScene("Level1");
    }
	public void NextBtnClick()
    {
		Scene currentScene = SceneManager.GetActiveScene ();
		string sceneName = currentScene.name;
		if (sceneName == "Level1")
        	SceneManager.LoadScene("Level2");
        else if (sceneName == "Level2")
            SceneManager.LoadScene("EndGame");
    }
}
