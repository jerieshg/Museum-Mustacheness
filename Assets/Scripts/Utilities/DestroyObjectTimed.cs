using UnityEngine;
using System.Collections;

public class DestroyObjectTimed : MonoBehaviour
{
	public float destroyDelay = 5f;

	void Start()
	{
		Destroy (gameObject, this.destroyDelay);
	}

}

