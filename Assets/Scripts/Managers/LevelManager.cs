using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public static LevelManager levelManager;

	public GameObject CameraContainer;

	void Awake()
	{
		levelManager = this;
	}

}

