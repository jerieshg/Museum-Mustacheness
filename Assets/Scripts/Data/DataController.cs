using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class DataController  {

	public static Game savedGame;
	private static string dataPath = Path.Combine (Application.dataPath, "mr.mustache");

	public static void Save() {
		
		DataController.savedGame = new Game ();//TESTING
		DataController.savedGame.savedGameName = "Hai maik";
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (dataPath);
		bf.Serialize(file, DataController.savedGame);
		file.Close();
	}

	public static void Load() {
		if(File.Exists(dataPath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(dataPath, FileMode.Open);
			DataController.savedGame = (Game) bf.Deserialize(file);
			file.Close();
		}
	}
}
