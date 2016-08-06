using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager playerManager;
	public GameObject playerCamera;

	private PlayerController currentPlayerControls;

	void Awake()
	{
		playerManager = this;
	}

	public void setCurrentPlayerObj(GameObject playerObj)
	{
		this.currentPlayerControls = playerObj.GetComponent<PlayerController>();
	}

	public void setPlayerCamera(GameObject playerObj)
	{
		UnityStandardAssets._2D.Camera2DFollow playerCam = playerCamera.GetComponent<UnityStandardAssets._2D.Camera2DFollow> ();
		playerCam.target = playerObj.transform;
		playerCam.enabled = true;
	}

	public void movePlayerRight()
	{
		if(currentPlayerControls != null)
		{
			currentPlayerControls.getPlayerMovementController ().movingRight = true;
		}
	}

	public void cancelMovePlayerRight()
	{
		if(currentPlayerControls != null)
		{
			currentPlayerControls.getPlayerMovementController ().movingRight = false;
		}
	}

	public void movePlayerLeft()
	{
		if(currentPlayerControls != null)
		{
			currentPlayerControls.getPlayerMovementController ().movingLeft = true;
		}
	}

	public void cancelMovePlayerLeft()
	{
		if(currentPlayerControls != null)
		{
			currentPlayerControls.getPlayerMovementController ().movingLeft = false;
		}
	}

	public void jumpPlayer()
	{
		if(currentPlayerControls != null)
		{
			currentPlayerControls.getPlayerMovementController ().canJump = true;
		}
	}

	public void throwMarker()
	{
		if(currentPlayerControls != null)
		{
			currentPlayerControls.isThrowing ();
		}
	}

}
