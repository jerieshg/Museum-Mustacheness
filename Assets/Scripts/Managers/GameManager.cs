using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

	public GameObject playerPrefab;
	public GameObject currentPlayerObj;
    public difficulty gameDifficulty = difficulty.Normal;

    void Awake()
    {
        gameManager = this;
    }

	void Start()
	{
		//TODO: Fix level loading
		//StartCoroutine (LevelManager.levelManager.startLevel());
	}

	public void setCurrentPlayerObj(GameObject playerObj)
	{
		currentPlayerObj = playerObj;
	}

	//Changes values depending on difficulty
	public void setDifficulty(difficulty newDifficulty)
	{
		gameDifficulty = newDifficulty;

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
