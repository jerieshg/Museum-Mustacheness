using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.IO;

public class Projectile : MonoBehaviour
{

	public float projectileCooldown = 2f;
	public float projectileSpeed = 5f;
	public float projectileRadiusImpact = 0.2f;
	public LayerMask hitMask;
	public LayerMask collision;

	[HideInInspector] public Vector3 direction;

	private float lifetime = 5f;

	void Awake ()
	{
		GetComponent<SpriteRenderer> ().flipX = (direction.x > 0);
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
		RaycastHit2D hitMob = Physics2D.CircleCast (transform.position, projectileRadiusImpact, new Vector2 (0.5f, 0.5f), 0.1f, hitMask);
		RaycastHit2D hitCol = Physics2D.CircleCast (transform.position, projectileRadiusImpact, new Vector2 (0.5f, 0.5f), 0.5f, collision);

		if (hitMob) {
			if (hitMob.transform.CompareTag ("Player") || hitMob.transform.CompareTag ("Mob")) {
				//Dosomething
				Destroy (gameObject);
			}
		}

		if (hitCol) {
			
			Destroy (gameObject);
		}
	}

	private void travelDirection ()
	{
		transform.Translate (direction * Time.fixedDeltaTime * projectileSpeed);
	}
}
