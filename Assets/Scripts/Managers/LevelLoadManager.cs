using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour
{
	public static LevelLoadManager levelLoading;

	void Awake()
	{
		levelLoading = this;
	}

	void Start()
	{
		UIManager.uiManager.setFaderState (false);
	}

	public void loadLevel(string levelName)
	{
		UIManager.uiManager.setFaderState (true);
		StartCoroutine (loadSceneTimed(3f,levelName));
	}

	IEnumerator loadSceneTimed(float time, string levelName)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (levelName);
	}

}

