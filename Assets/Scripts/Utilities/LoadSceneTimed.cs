using UnityEngine;
using System.Collections;

public class LoadSceneTimed : MonoBehaviour
{
	public string sceneName;

	void Start()
	{
		SceneLoadingManager.sceneLoader.loadLevel (sceneName);
	}

}

