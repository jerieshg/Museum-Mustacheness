using UnityEngine;
using System.Collections;

public class PlayerStats : Stats {

	public int markers;
	public int score;

	public PlayerStats()
	{
		
	}

	public PlayerStats(float hitpoints, int markers, int score){
		this.hitpoints = hitpoints;
		this.markers = markers;
		this.score = score;
	}
}
