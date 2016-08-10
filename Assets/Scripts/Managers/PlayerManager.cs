using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager playerManager;

	private bool canControl = false;
	private PlayerController currentPlayerControls;

	void Awake()
	{
		playerManager = this;
	}

	//Sets if player can be controlled
	public void setCanControlPlayer(bool control)
	{
		this.canControl = control;
	}

	public bool getCanControlPlayer()
	{
		return canControl;
	}

	//Gets reference of controls from currentPlayerObj
	public void setPlayerControls()
	{
		this.currentPlayerControls = GameManager.gameManager.currentPlayerObj.GetComponent<PlayerController>();
	}

	//sets the game paused depending on bool
	public void pauseGame(bool isPaused)
	{
		GameManager.gameManager.setGamePaused (isPaused);
	}

	//Sets the playerCamera to follow player
	public void setPlayerCamera()
	{
		Camera currentCamera = UIManager.uiManager.getSceneCamera ().GetComponent<Camera> ();
		currentCamera.transform.position = new Vector3 (currentPlayerControls.transform.position.x,currentPlayerControls.transform.position.y,-10f);
		UnityStandardAssets._2D.Camera2DFollow playerCam = currentCamera.GetComponent<UnityStandardAssets._2D.Camera2DFollow> ();
		playerCam.target = GameManager.gameManager.currentPlayerObj.transform;
		playerCam.enabled = true;
	}

	public void movePlayerRight()
	{
		if(currentPlayerControls != null && this.canControl)
		{
			currentPlayerControls.getPlayerMovementController ().movingRight = true;
		}
	}

	public void cancelMovePlayerRight()
	{
		if(currentPlayerControls != null && this.canControl)
		{
			currentPlayerControls.getPlayerMovementController ().movingRight = false;
		}
	}

	public void movePlayerLeft()
	{
		if(currentPlayerControls != null && this.canControl)
		{
			currentPlayerControls.getPlayerMovementController ().movingLeft = true;
		}
	}

	public void cancelMovePlayerLeft()
	{
		if(currentPlayerControls != null && this.canControl)
		{
			currentPlayerControls.getPlayerMovementController ().movingLeft = false;
		}
	}

	public void jumpPlayer()
	{
		if(currentPlayerControls != null && this.canControl)
		{
			currentPlayerControls.getPlayerMovementController ().canJump = true;
		}
	}

	public void throwMarker()
	{
		if(currentPlayerControls != null && this.canControl)
		{
			currentPlayerControls.isThrowing ();
		}
	}

}
