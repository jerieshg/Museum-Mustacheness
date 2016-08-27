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
		UIManager.uiManager.setLoadingActive (false);
	}

	public void loadLevel(string levelName)
	{
		SceneLoadingManager.sceneLoader.loadLevel (levelName);
	}

}

