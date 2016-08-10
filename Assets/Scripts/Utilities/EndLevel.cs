using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player")
		{
		}
	}

}

