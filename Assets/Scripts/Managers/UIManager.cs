using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager uiManager;

	[Header("Player UI Elements")]
	public Text scoreText;
	public Text markerText;

	public GameObject currentPlayerObj;

	void Awake()
	{
		uiManager = this;
	}

	void Update()
	{
		//TODO: Fix player UI for updating info
		//UpdatePlayerInfo ();
	}

	void UpdatePlayerInfo()
	{
		if (currentPlayerObj != null)
		{
			Debug.Log ("<color=green><b>PlayerPrefab Detected</b></color>");
			scoreText.text = "Score: " + currentPlayerObj.GetComponent<PlayerStats> ().score;
			markerText.text = "X" + currentPlayerObj.GetComponent<PlayerStats> ().markers;
		} 
		else
		{
			Debug.Log ("<color=red><b>PlayerPrefab Not Found.</b></color>");
		}
	}

	public void setUICurrentPlayerObj(GameObject prefab)
	{
		currentPlayerObj = prefab;
	}

}
