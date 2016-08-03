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
    }

    //Sets the alarm animation to on or off depending if global alarm is on
    void setAlarmAnimation()
    {
		alarmAnimator.SetBool("On", AlarmManager.alarmManager.AlarmOn);
    }

}
