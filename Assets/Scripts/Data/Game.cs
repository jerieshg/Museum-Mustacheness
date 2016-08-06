using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Game
{
	public Game current;
	public string savedGameName;
	public Character player;

	public Game ()
	{
		player = new Character ();
	}

}
