using UnityEngine;
using System.Collections;

public class PersistentManager : MonoBehaviour
{

	private static PersistentManager _instace;

	public static PersistentManager Instance
	{
		get 
		{
			return _instace;
		}
	}

	void Awake()
	{
		if (_instace != null && _instace != this) 
		{
			Destroy (this.gameObject);
		}
		else
		{
			_instace = this;
			DontDestroyOnLoad (this);
		}
	}


}

