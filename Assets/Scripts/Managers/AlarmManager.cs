using UnityEngine;
using System.Collections;

public class AlarmManager : MonoBehaviour
{
	//This script controls and syncs all the alarms in the level

	public static AlarmManager alarmManager;

	public bool AlarmOn = false;					//Sets the level alarms depending on value
	public bool targetOnSight = false;				//Sets if the target is on sight
	public float loseTargetDelay = 5f;				//Delay after target is lost by camera, this value will change depending on difficulty

	private GameObject cameraContainerObject;		//Contains all the cameras of the level
	private bool triggeredAlarm = false;			//Sets if the alarm has being triggered by player
	private float currentAlarmTime;
	private float maxAlarmTime;


	void Awake()
	{
		alarmManager = this;
	}

	void Start()
	{
		cameraContainerObject = LevelManager.levelManager.CameraContainer;
	}

	void Update()
	{
		levelAlarmLogic ();
		updateAlarmIndicator ();
	}

	void updateAlarmIndicator()
	{
		if (AlarmOn) 
		{
			this.currentAlarmTime -= Time.deltaTime;
			float currentTimeRounded = Mathf.Round (currentAlarmTime);
			UIManager.uiManager.setAlarmIndcProperties ( currentTimeRounded + "s", currentAlarmTime / maxAlarmTime);
			UIManager.uiManager.setAlarmIndicatorActive (true);
		}
		else
		{
			UIManager.uiManager.setAlarmIndicatorActive (false);
		}
	}

	public void setAlarmManagerProperties()
	{
		this.currentAlarmTime = loseTargetDelay;
		this.maxAlarmTime = loseTargetDelay;
	}

	//Controls the alarms depending if target is on vision or lost
	void levelAlarmLogic()
	{
		this.targetOnSight = checkIfCameraHasTarget ();

		//If target is on camera vision then sets up the alarm continously
		if (targetOnSight)
		{
			this.AlarmOn = true;
			this.triggeredAlarm = true;
			currentAlarmTime = this.loseTargetDelay;
			//Cancels the deactivate alarm delay
			StopAllCoroutines ();
		}
		else
		{
			//If target is lost then the deactivate alarm delay starts
			if(triggeredAlarm)
			{
				this.triggeredAlarm = false;
				StartCoroutine (deactivateAlarmDelay ());
			}
		}
	}

	//Checks all the cameras, if one of the cameras have a target then trigger the alarms/refresh alarms
	bool checkIfCameraHasTarget()
	{
		if(cameraContainerObject.transform.childCount > 0 && cameraContainerObject.transform.childCount != 0)
		{
			for(int a = 0; a < cameraContainerObject.transform.childCount; a++)
			{
				if (cameraContainerObject.transform.GetChild (a).GetComponent<CameraAI> ().targetOnSight) 
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	//Deactivates the alarms with a delay
	IEnumerator deactivateAlarmDelay()
	{
		yield return new WaitForSeconds (loseTargetDelay);
		Debug.Log ("Stopping Alarms");
		this.AlarmOn = false;
	}

}

