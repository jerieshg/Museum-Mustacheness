using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static LevelManager levelManager;

	public GameObject CameraContainer;
	public GameObject AIContainer;
	public GameObject startEndCon;
	public GameObject artCon;

	[HideInInspector] public GameObject startPos;
	[HideInInspector] public GameObject endPos;

	private bool levelEnded = false;
	[SerializeField] private int levelMaxScore;

	void Awake()
	{
		levelManager = this;
		getSTARTENDPos ();
	}

	void Start()
	{
		StartCoroutine (setupLevel());
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

	//Enables alarms and mobs on the level
	void enablesAIFromLevel()
	{
		AIContainer.SetActive (true);
		CameraContainer.SetActive (true);
	}

	int getMaximunLevelScore()
	{
		if (artCon.transform.childCount > 0 && artCon.transform.childCount != 0)
		{
			int artScoreSum = 0;
			for (int a = 0; a < artCon.transform.childCount; a++)
			{
				artScoreSum += artCon.transform.GetChild (a).GetComponent<ArtObj> ().getArtScore ();
			}
			return artScoreSum;
		}
		else
		{
			return 0;
		}
	}

	//Setting up the level
	public IEnumerator setupLevel()
	{
		Debug.Log("<b>----- LevelManager -----</b>");
		UIManager.uiManager.setFaderState (true);

		yield return new WaitForSeconds (2f);

		Debug.Log ("(1) -- Setting Difficulty and GameManager currentPlayerObj -- ");
		GameObject instPlayer = Instantiate (GameManager.gameManager.playerPrefab, startPos.transform.position, startPos.transform.rotation) as GameObject;
		GameManager.gameManager.setCurrentPlayerObj (instPlayer.gameObject);
		GameManager.gameManager.setGamePaused (true);
		GameManager.gameManager.setDifficulty (GameManager.gameManager.getGameDifficulty());


		Debug.Log ("(2) -- Setting SceneCamera and PlayerUI -- ");
		UIManager.uiManager.setUIState (true);
		UIManager.uiManager.findSceneCamera ();
		UIManager.uiManager.getUICurrentPlayerObj ();

		Debug.Log ("(3) -- Setting cameraFollow Properties -- ");
		PlayerManager.playerManager.setPlayerControls ();
		PlayerManager.playerManager.setPlayerCamera ();

		Debug.Log ("(4) -- Setting level mobs, alarms and properties --");
		AlarmManager.alarmManager.setAlarmManagerProperties ();
		enablesAIFromLevel ();
		levelMaxScore = getMaximunLevelScore ();

		Debug.Log ("(5) -- Enabling start counter --");
		UIManager.uiManager.setCounterState (true);
		UIManager.uiManager.setLoadingActive (false);
		UIManager.uiManager.setFaderState (false);

		Debug.Log("<b>----- Setup Complete -----</b>");
	}

	public void startLevel()
	{
		UIManager.uiManager.setPlayerControlsState (true);
		UIManager.uiManager.setPlayerInfoState (true);
		UIManager.uiManager.setCounterState (false);
		PlayerManager.playerManager.setCanControlPlayer (true);
		GameManager.gameManager.setGamePaused (false);
	}

}

