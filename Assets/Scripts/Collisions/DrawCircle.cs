using UnityEngine;
using System.Collections;

public class DrawCircle : MonoBehaviour {

	//This script draws a circle collider on the object that stores information to be used later
	//This circle collision only captures one collision at a time

	[HideInInspector] public RaycastHit2D hitInfo;		//Stored information about collisions
	public bool drawLineCast = true;					//Sets if circle gizmo can be drawn
	public float circleRadius = 1f;						//Sets the circle radius
	public Color circleColor;							//Sets the color of the circle gizmo
	public LayerMask circleCollisions;					//Sets the collisions for the circle

	void Update()
	{
		hitInfo = Physics2D.CircleCast (transform.position, circleRadius, new Vector2 (0, 0), 0, circleCollisions);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = circleColor;

		if(drawLineCast)
		{
			Gizmos.DrawWireSphere (transform.position, circleRadius);
		}
	}

}
