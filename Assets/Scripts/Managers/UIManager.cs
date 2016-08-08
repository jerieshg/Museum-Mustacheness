using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	public static UIManager uiManager;

	[Header ("Player UI Elements")]
	public Text scoreText;
	public Text markerText;

	[Header ("General UI Elements")]
	public Animator faderAnimator;

	public GameObject currentPlayerObj;

	void Awake ()
	{
		uiManager = this;
	}

	void Update ()
	{
		UpdatePlayerInfo ();
	}

	public void setFaderState (bool state)
	{
		faderAnimator.SetBool ("FaderOn", state);
	}

	//Updates player info if player is on scene
	void UpdatePlayerInfo ()
	{
		if (currentPlayerObj != null) {
			Debug.Log ("<color=green><b>PlayerPrefab Detected</b></color>");
			scoreText.text = "Score: " + currentPlayerObj.GetComponent<PlayerController> ().score;
			markerText.text = "X" + currentPlayerObj.GetComponent<PlayerController> ().getPlayerStats ().markers;
		} else {
			Debug.Log ("<color=red><b>PlayerPrefab Not Found.</b></color>");
		}
	}

	//Sets the current player obj as reference
	public void setUICurrentPlayerObj (GameObject prefab)
	{
		currentPlayerObj = prefab;
	}

}
