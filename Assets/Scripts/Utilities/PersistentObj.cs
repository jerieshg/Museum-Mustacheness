using UnityEngine;
using System.Collections;

public class PersistentObj : MonoBehaviour
{

	void Awake()
	{
		DontDestroyOnLoad (gameObject);
	}

}

