using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.IO;

public class Projectile : MonoBehaviour
{
	public float projectileSpeed = 5f;
	public GameObject projectileExplotion;

	[HideInInspector] public Vector3 direction;

	private DrawCircle circleCol;
	private float lifetime = 5f;

	void Awake ()
	{
//		GetComponent<SpriteRenderer> ().flipX = (direction.x > 0);
		circleCol = GetComponent<DrawCircle> ();
		Destroy (this.gameObject, lifetime);
	}

	void Update ()
	{
		travelDirection ();
		checkCollisions ();
	}

	//Checks the collision type, depending on which collision it will interact differently
	void checkCollisions ()
	{
		if(circleCol.hitInfo )
		{
			if (circleCol.hitInfo.collider.CompareTag ("Enemy") || circleCol.hitInfo.collider.CompareTag ("Player")) {
				circleCol.hitInfo.collider.SendMessage ("takeDamage", 1,SendMessageOptions.DontRequireReceiver);
			}
			Instantiate (projectileExplotion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}

	private void travelDirection ()
	{
		transform.Translate (direction * Time.fixedDeltaTime * projectileSpeed);
	}
}
