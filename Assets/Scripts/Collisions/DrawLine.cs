using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
	//This script draws a linecast from the object that stores information to be used later
	//This linecast captures only one collision at a time 

	[HideInInspector] public RaycastHit2D hitInfo;			//Stored information about collisions
	public bool drawLineCast = true;						//Sets if circle gizmo can be drawn
	public float XLineDirection = 0;						//Sets the line direction on X
	public float YLineDirection = 0;						//Sets the line direction on Y
	public Color lineColor;									//Sets the line gizmo color
	public LayerMask lineCollisions;						//Sets the collisions for the line
	public GameObject parentObject;							//If line has a parent object then it will change its transform localX and localY to match its parent

    private float parentLocalX = 1;
    private float parentLocalY = 1;

    void Update()
    {
        if (parentObject != null)
        {
            parentLocalX = parentObject.transform.localScale.x;
            parentLocalY = parentObject.transform.localScale.y;
        }

        hitInfo = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + XLineDirection * parentLocalX, transform.position.y + YLineDirection * parentLocalY), lineCollisions);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        if (drawLineCast)
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + XLineDirection * parentLocalX, transform.position.y + YLineDirection * parentLocalY, 0));
        }
    }
}

