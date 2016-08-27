using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	public static UIManager uiManager;

	public Canvas UICanvas;

	[Header ("Player UI Elements")]
	public Text scoreText;
	public Text markerText;
	public Animator playerControlAnim;
	public Animator playerInfoAnim;
	public GameObject playerUICon;

	[Header("General UI Elements")]
	public Animator faderAnim;
	public Animator pauseMenuAnim;
	public Animator counterAnim;
	public Animator alarmIndicatorAnim;
	public GameObject pauseUICon;
	public GameObject counterUICon;
	public GameObject loadingCon;
	public GameObject alarmIndicatorCon;
	public Text AlarmIndcText;
	public Image AlarmIndcBar;

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

	public void setAlarmIndcProperties(string alarmText, float fillBarAmount)
	{
		AlarmIndcText.text = alarmText;
		AlarmIndcBar.fillAmount = fillBarAmount;
	}

	//Sets if alarm indicator is on or off
	public void setAlarmIndicatorActive(bool isActive)
	{
		alarmIndicatorAnim.SetBool ("On",isActive);
	}

	//Enables or disables loadingImage depending on bool
	public void setLoadingActive(bool isActive)
	{
		loadingCon.SetActive (isActive);
	}

	//Sets if pausemenu is on or off
	public void setPauseMenuState(bool state)
	{
		pauseMenuAnim.SetBool ("On",state);
	}

	//Disables or enables UI depending on bool
	public void setUIState(bool state)
	{
		playerUICon.SetActive (state);
		pauseUICon.SetActive (state);
		counterUICon.SetActive (state);
		alarmIndicatorCon.SetActive (state);
	}

	//Sets if counter is on or off
	public void setCounterState(bool state)
	{
		counterAnim.SetBool ("Trigger",state);
	}

	//Sets the fader on or off depending on state
	public void setFaderState (bool state)
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
		if (currentPlayerObj != null)
		{
			PlayerController pp = currentPlayerObj.GetComponent<PlayerController> ();
			scoreText.text = "Score: " + pp.getScore();
			markerText.text = "X" + pp.getMarkers();
		} 
	}

	//Sets the current player obj as reference
	public void getUICurrentPlayerObj ()
	{
		currentPlayerObj = GameManager.gameManager.currentPlayerObj;
	}

}
