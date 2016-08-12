using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

	public static DataManager dataManager;

	void Awake()
	{
		dataManager = this;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void save ()
	{
		DataController.Save ();
	}

	public void load ()
	{
		DataController.Load ();
	}
}
