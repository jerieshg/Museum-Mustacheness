using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	public static UIManager uiManager;

<<<<<<< HEAD
	public Canvas UICanvas;

	[Header("Player UI Elements")]
=======
	[Header ("Player UI Elements")]
>>>>>>> origin/master
	public Text scoreText;
	public Text markerText;
	public Animator playerControlAnim;
	public Animator playerInfoAnim;
	public GameObject playerUICon;

<<<<<<< HEAD
	[Header("General UI Elements")]
	public Animator faderAnim;
	public Animator pauseMenuAnim;
	public Animator counterAnim;
	public GameObject pauseUICon;
	public GameObject counterUICon;
=======
	[Header ("General UI Elements")]
	public Animator faderAnimator;
>>>>>>> origin/master

	private GameObject currentPlayerObj;
	private GameObject currentSceneCam;

	void Awake ()
	{
		uiManager = this;
	}

	void Update ()
	{
		UpdatePlayerInfo ();
	}

<<<<<<< HEAD
	public void setPauseMenuState(bool state)
	{
		pauseMenuAnim.SetBool ("On",state);
	}

	public void setUIState(bool state)
	{
		playerUICon.SetActive (state);
		pauseUICon.SetActive (state);
		counterUICon.SetActive (state);
	}

	public void setCounterState(bool state)
	{
		counterAnim.SetBool ("Trigger",state);
	}

	//Sets the fader on or off depending on state
	public void setFaderState(bool state)
=======
	public void setFaderState (bool state)
>>>>>>> origin/master
	{
		faderAnim.SetBool ("FaderOn", state);
	}

	//Sets the playerControlUI on or off depending on state
	public void setPlayerControlsState(bool state)
	{
		playerControlAnim.SetBool ("On",state);
	}

	//Sets the playerInfoUI on or off depending on state
	public void setPlayerInfoState(bool state)
	{
		playerInfoAnim.SetBool ("On",state);
	}

	//Returns the currentScene camera gameobject
	public GameObject getSceneCamera()
	{
		if(currentSceneCam != null)
		{
			return this.currentSceneCam;
		}
		return null;
	}

	//Finds scene camera gameobject and sets a reference to it
	public void findSceneCamera()
	{
		this.currentSceneCam = Camera.main.gameObject;
		setCameraToCanvas ();
	}

	//Sets current camera to carry the canvas
	void setCameraToCanvas()
	{
		UICanvas.worldCamera = this.currentSceneCam.GetComponent<Camera>();
		UICanvas.renderMode = RenderMode.ScreenSpaceCamera;
	}

	//Updates player info if player is on scene
	void UpdatePlayerInfo ()
	{
<<<<<<< HEAD
		if (currentPlayerObj != null)
		{
			//Debug.Log ("<color=green><b>PlayerPrefab Detected</b></color>");
			scoreText.text = "Score: " + currentPlayerObj.GetComponent<PlayerController> ().score;
			markerText.text = "X" + currentPlayerObj.GetComponent<PlayerController> ().markers;
		} 
		else
		{
			//Debug.Log ("<color=red><b>PlayerPrefab Not Found.</b></color>");
=======
		if (currentPlayerObj != null) {
			Debug.Log ("<color=green><b>PlayerPrefab Detected</b></color>");
			scoreText.text = "Score: " + currentPlayerObj.GetComponent<PlayerController> ().score;
			markerText.text = "X" + currentPlayerObj.GetComponent<PlayerController> ().getPlayerStats ().markers;
		} else {
			Debug.Log ("<color=red><b>PlayerPrefab Not Found.</b></color>");
>>>>>>> origin/master
		}
	}

	//Sets the current player obj as reference
<<<<<<< HEAD
	public void getUICurrentPlayerObj()
=======
	public void setUICurrentPlayerObj (GameObject prefab)
>>>>>>> origin/master
	{
		currentPlayerObj = GameManager.gameManager.currentPlayerObj;
	}

}
