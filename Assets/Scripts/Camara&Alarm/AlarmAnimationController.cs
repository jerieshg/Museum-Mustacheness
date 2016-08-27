using UnityEngine;
using System.Collections;

public class AlarmAnimationController : MonoBehaviour {

    private Animator alarmAnimator;

    void Awake()
    {
        alarmAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        setAlarmAnimation();
		changeMaterialIfAlarmTriggered ();
    }

    //Sets the alarm animation to on or off depending if global alarm is on
    void setAlarmAnimation()
    {
		alarmAnimator.SetBool("On", AlarmManager.alarmManager.AlarmOn);
    }

	void changeMaterialIfAlarmTriggered()
	{
		if (AlarmManager.alarmManager.AlarmOn)
		{
			GetComponent<SpriteRenderer> ().material = ReferenceManager.referenceManager.redOutline;
		} 
		else
		{
			GetComponent<SpriteRenderer> ().material = ReferenceManager.referenceManager.normalSprite;
		}
	}

}
