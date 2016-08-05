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

	void getSTARTENDPos()
	{
		if(startEndCon.transform.childCount > 0 && startEndCon.transform.childCount != 0)
		{
			startPos = startEndCon.transform.FindChild ("STARTPosObj").gameObject;
			endPos = startEndCon.transform.FindChild ("ENDPosObj").gameObject;
		}
	}

	public IEnumerator startLevel()
	{
		yield return new WaitForSeconds (5f);

		GameObject instPlayer = Instantiate (GameManager.gameManager.playerPrefab, startPos.transform.position, startPos.transform.rotation) as GameObject;

		GameManager.gameManager.setCurrentPlayerObj (instPlayer);
		UIManager.uiManager.setUICurrentPlayerObj (instPlayer);
	}

}

