using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class DataController
{

	public static List<Game> savedGames = new List<> ();
	private static string dataPath = Path.Combine (Application.dataPath, "mr.mustache");

	public static void Save ()
	{
		savedGames.Add (Game.current);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (dataPath);
		bf.Serialize (file, DataController.savedGames);
		file.Close ();
	}

	public static void Load ()
	{
		if (File.Exists (dataPath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (dataPath, FileMode.Open);
			DataController.savedGames = (List<Game>)bf.Deserialize (file);
			file.Close ();
		}
	}
}
