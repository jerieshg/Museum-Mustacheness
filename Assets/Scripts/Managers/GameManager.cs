using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager = null;

	public GameObject playerPrefab;
	public GameObject currentPlayerObj;
    public difficulty gameDifficulty = difficulty.Normal;

	private bool playerAlive = true;
	private bool gamePaused = false;
	private int difMultiplier;

    void Awake()
    {
		gameManager = this;
    }

	public void setGamePaused(bool isPaused)
	{
		this.gamePaused = isPaused;
		UIManager.uiManager.setPauseMenuState (isPaused);
		UIManager.uiManager.setPlayerInfoState (!isPaused);
		UIManager.uiManager.setPlayerControlsState (!isPaused);

		if (gamePaused)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void setCurrentPlayerObj(GameObject playerObj)
	{
		currentPlayerObj = playerObj;
	}

	public bool getPlayerAliveState()
	{
		return this.playerAlive;
	}

	public difficulty getGameDifficulty()
	{
		return this.gameDifficulty;
	}

	public int getDifMultiplier()
	{
		if (difMultiplier == null || difMultiplier == 0) 
		{
			return UtilityMoustache.ScoreManagment.getDifficultyMultiplier (gameDifficulty);
		} 
		else 
		{
			return this.difMultiplier;
		}
	}

	//Changes values depending on difficulty
	public void setDifficulty(difficulty newDifficulty)
	{
		gameDifficulty = newDifficulty;
		difMultiplier = UtilityMoustache.ScoreManagment.getDifficultyMultiplier (gameDifficulty);

		switch (gameDifficulty)
		{
		case difficulty.Normal:
			AlarmManager.alarmManager.loseTargetDelay = 5f;
			break;
		case difficulty.Hard:
			AlarmManager.alarmManager.loseTargetDelay = 7f;
			break;
		case difficulty.MrMoustache:
			AlarmManager.alarmManager.loseTargetDelay = 10f;
			break;
		default:
			AlarmManager.alarmManager.loseTargetDelay = 5f;
			break;
		}
	}

}

public enum difficulty
{
    Normal,
    Hard,
    MrMoustache
}
