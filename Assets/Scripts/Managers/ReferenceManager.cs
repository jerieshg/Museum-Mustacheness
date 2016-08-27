using UnityEngine;
using System.Collections;

public class ReferenceManager : MonoBehaviour
{

	public static ReferenceManager referenceManager;

	[Header("Projectiles")]
	public GameObject bullet;
	public GameObject marker;
	[Header("Materials")]
	public Material whiteOutline;
	public Material redOutline;
	public Material normalSprite;

	void Awake()
	{
		referenceManager = this;
	}

}

