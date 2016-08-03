using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour
{
    public float cameraWaitTime = 3f;               //Sets the delay time to turn camera
    private float cameraWaitCD = 0f;                //Tracks the delay time to turn camera
    public bool startRight;                         //Sets if camera should be facing right at the start, if not then it starts left
	public DrawCircle[] cameraVisionColliders;		//Stores the camera vision colliders
	public bool targetOnSight = false;				//Sets if target is on sight

    private Animator cameraAnimator;
    private bool canTurnCamera = true;

	void Awake ()
	{
        //Getting camera animator reference
        cameraAnimator = GetComponent<Animator> ();
    }

    void Start()
    {
        cameraAnimator.SetBool("StartRight", startRight);
		disableCols ();
    }

	void Update ()
	{
        trackTurnTime();
		checkIfCollisionsHaveTarget ();

        if (canTurnCamera)
        {
            if (cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("Left_Idle"))
            {
                if (cameraWaitCD >= cameraWaitTime)
                {
                    cameraAnimator.SetBool("RotateLeft", false);
                    cameraAnimator.SetBool("RotateRight", true);
                    cameraWaitCD = 0f;
                }
            }
            if (cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("Right_Idle"))
            {
                if (cameraWaitCD >= cameraWaitTime)
                {
                    cameraAnimator.SetBool("RotateLeft", true);
                    cameraAnimator.SetBool("RotateRight", false);
                    cameraWaitCD = 0f;
                }
            }
        }
	}

	void checkIfCollisionsHaveTarget()
	{
		if (cameraVisionColliders [0].hitInfo) 
		{
			canTurnCamera = false;
			targetOnSight = true;
			cameraAnimator.SetBool("RotateLeft", true);
			cameraAnimator.SetBool("RotateRight", false);
		}
		else if(cameraVisionColliders [1].hitInfo)
		{
			canTurnCamera = false;
			targetOnSight = true;
			cameraAnimator.SetBool("RotateLeft", false);
			cameraAnimator.SetBool("RotateRight", true);
		}
		else 
		{
			targetOnSight = false;
			canTurnCamera = true;
		}
	}

    //Tracks the delay time to turn camera to opposite direction
    void trackTurnTime()
    {
        cameraWaitCD += Time.deltaTime;
        //Debug.Log(cameraWaitCD);

        if (cameraWaitCD >= cameraWaitTime)
        {
            cameraWaitCD = cameraWaitTime;
        }
    }

	void disableCols()
	{
		cameraVisionColliders [0].gameObject.SetActive (false);
		cameraVisionColliders [1].gameObject.SetActive (false);
	}

	void enableRightCol()
	{
		cameraVisionColliders [0].gameObject.SetActive (false);
		cameraVisionColliders [1].gameObject.SetActive (true);
	}

	void enableLeftCol()
	{
		cameraVisionColliders [0].gameObject.SetActive (true);
		cameraVisionColliders [1].gameObject.SetActive (false);
	}
    
}