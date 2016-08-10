using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour {

	public static SceneLoadingManager sceneLoader;

	void Awake()
	{
		sceneLoader = this;
	}

	public void reloadCurrentLevel()
	{
		string currentLvl = SceneManager.GetActiveScene ().name;
		GameManager.gameManager.setGamePaused (false);
		clearUIFromCamera ();
		StartCoroutine (loadSceneTimed(3f,currentLvl));
	}

	void clearUIFromCamera()
	{
		UIManager.uiManager.setFaderState (true);
		UIManager.uiManager.setPauseMenuState (false);
		UIManager.uiManager.setPlayerControlsState (false);
		UIManager.uiManager.setPlayerInfoState (false);
	}

	void disableUI()
	{
		UIManager.uiManager.setUIState (false);
	}

	public void loadLevel(string levelName)
	{
		clearUIFromCamera ();
		GameManager.gameManager.setGamePaused (false);
		StartCoroutine (loadSceneTimed(3f,levelName));
	}

	IEnumerator loadSceneTimed(float time, string levelName)
	{
		yield return new WaitForSeconds (time);
		disableUI ();
		SceneManager.LoadScene (levelName);
	}

}
