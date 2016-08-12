using UnityEngine;
using System.Collections;

public class ReferenceManager : MonoBehaviour
{

	public static ReferenceManager referenceManager;

	[Header("Projectiles")]
	public GameObject bullet;
	public GameObject marker;

	void Awake()
	{
		referenceManager = this;
	}

}

