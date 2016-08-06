using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public static LevelManager levelManager;

	public GameObject CameraContainer;
	public GameObject startEndCon;	

	[HideInInspector] public GameObject startPos;
	[HideInInspector] public GameObject endPos;

	void Awake()
	{
		levelManager = this;
		getSTARTENDPos ();
	}

	//Gets the start pos and end pos to be used later
	void getSTARTENDPos()
	{
		if(startEndCon.transform.childCount > 0 && startEndCon.transform.childCount != 0)
		{
			startPos = startEndCon.transform.FindChild ("STARTPosObj").gameObject;
			endPos = startEndCon.transform.FindChild ("ENDPosObj").gameObject;
		}
	}

	//Setting up the level
	public IEnumerator startLevel()
	{
		UIManager.uiManager.setFaderState (true);

		yield return new WaitForSeconds (2f);

		GameObject instPlayer = Instantiate (GameManager.gameManager.playerPrefab, startPos.transform.position, startPos.transform.rotation) as GameObject;

		GameManager.gameManager.setCurrentPlayerObj (instPlayer.gameObject);
		UIManager.uiManager.setUICurrentPlayerObj (instPlayer.gameObject);
		PlayerManager.playerManager.setPlayerCamera (instPlayer.gameObject);
		PlayerManager.playerManager.setCurrentPlayerObj (instPlayer.gameObject);
		UIManager.uiManager.setFaderState (false);
	}

}

