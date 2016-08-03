using UnityEngine;
using System.Collections;

public class CameraVision : MonoBehaviour
{
    public float LoseTargetDelay = 5f;                 //Delay in which the alarm is set to off
    public bool hasTarget = false;

	/*void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
        {
            GameManager.gameManager.AlarmOn = true;
            StopCoroutine(alarmOffDelay());
        }
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Setting Alarm Off");
            StartCoroutine(alarmOffDelay());
        }
    }

    IEnumerator alarmOffDelay()
    {
        yield return new WaitForSeconds(LoseTargetDelay);

        GameManager.gameManager.AlarmOn = false;
        Debug.Log("Alarm set off");
    }
*/

}
