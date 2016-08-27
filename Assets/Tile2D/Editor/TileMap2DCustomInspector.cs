using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Tile2D
{
	[CustomEditor(typeof(Tile2D.TileMap2D))]
	public class TileMap2DCustomInspector : Editor {

		Tile2D.TileMap2D currentMapRef;

		private string roomName = "newRoom";
		private Vector2 roomSize = new Vector2(5f,5f);
        //private Color gridColor = Color.white;

		void OnEnable()
		{
			currentMapRef = (Tile2D.TileMap2D)this.target;
            createLayerContainer();
        }

		public override void OnInspectorGUI()
		{
			Tile2DUtility.TileUtility.drawBoxLabel ("TileMap2D");
            GUILayout.Space(5f);
            drawTileMap2DControls();
            GUILayout.Space (5f);
			drawRoomGeneratorControls ();
			GUILayout.Space (5f);
			drawRoomList ();

			if(GUI.changed)
			{
				EditorUtility.SetDirty (target);
			}
		}

        void drawTileMap2DControls()
        {
            GUILayout.BeginVertical("box");
            Tile2DUtility.TileUtility.drawBoxLabel("TileMap2D Controls");

            if (GUILayout.Button("Update Room List"))
            {
                updateRoomList();
            }

            GUILayout.EndVertical();
        }

        void updateRoomList()
        {
            this.currentMapRef.rooms.Clear();

            if (this.currentMapRef.transform.childCount > 0)
            {
                for (int a = 0; a < this.currentMapRef.transform.childCount; a++)
                {
                    if (this.currentMapRef.transform.GetChild(a).GetComponent<Tile2D.TileRoom2D>())
                    {
                        this.currentMapRef.rooms.Add(this.currentMapRef.transform.GetChild(a).gameObject);
                    }
                }
            }
        }

		void drawRoomList()
		{
			GUILayout.BeginVertical ("box");

			Tile2DUtility.TileUtility.drawBoxLabel ("Room List");

			if(this.currentMapRef.rooms.Count > 0)
			{
				for (int a = 0; a < this.currentMapRef.rooms.Count; a++) 
				{
					Tile2D.TileRoom2D currentRoom = this.currentMapRef.rooms [a].GetComponent<Tile2D.TileRoom2D> ();

					currentRoom.roomName = EditorGUILayout.TextField ("Room Name", currentRoom.roomName);
					currentRoom.setRoomName (currentRoom.roomName);

					if(GUILayout.Button("Edit"))
					{
						InitEditorRoom ();
						Selection.activeGameObject = currentRoom.gameObject;
					}

					GUILayout.BeginHorizontal ();
					currentRoom.lockedRoom = GUILayout.Toggle (currentRoom.lockedRoom,"Locked","button");
					currentRoom.drawGrid = GUILayout.Toggle (currentRoom.drawGrid,"Draw Grid","button");
					GUILayout.EndHorizontal ();

					if(GUILayout.Button("Remove"))
					{
						removeRoom (currentRoom.gameObject);
					}
				}
			}

			GUILayout.EndVertical ();
		}

		void drawRoomGeneratorControls()
		{
			GUILayout.BeginVertical ("box");

			Tile2DUtility.TileUtility.drawBoxLabel ("Room Generator");

			this.roomName = EditorGUILayout.TextField ("Room Name", this.roomName);
			this.roomSize = EditorGUILayout.Vector2Field ("Room Size", Tile2DUtility.TileUtility.roundRoomSizeFloats (this.roomSize));

            this.currentMapRef.activateExtraSettings = GUILayout.Toggle(this.currentMapRef.activateExtraSettings, "Add Default Room Settings");

            GUILayout.Space(5f);

            drawDefaultTiles();

            GUILayout.Space(5f);

            if (GUILayout.Button("Generate Room"))
            {
                createRoom();
            }

            GUILayout.EndVertical();
        }

		void removeRoom(GameObject roomToRemove)
		{
			if (!roomToRemove.GetComponent<Tile2D.TileRoom2D> ().lockedRoom) 
			{
				this.currentMapRef.rooms.Remove (roomToRemove);
				DestroyImmediate (roomToRemove.gameObject);
			}
			else 
			{
				Tile2DUtility.TileUtility.displayConsoleErrorMessage ("Error: Cannot remove this room because its locked.");
			}
		}

		void createRoom()
		{
			GameObject newRoom = new GameObject ();
			newRoom.AddComponent<Tile2D.TileRoom2D> ();
			newRoom.GetComponent<Tile2D.TileRoom2D> ().setRoomName (this.roomName);
			newRoom.GetComponent<Tile2D.TileRoom2D> ().roomSize = this.roomSize;
            newRoom.GetComponent<Tile2D.TileRoom2D>().roomMinXYBounds = new Vector3(newRoom.transform.position.x - 0.5f, newRoom.transform.position.y - 0.5f);
            newRoom.GetComponent<Tile2D.TileRoom2D>().roomMaxXYBounds = new Vector3(newRoom.transform.position.x + this.roomSize.x - 0.5f, newRoom.transform.position.y + this.roomSize.y - 0.5f);
            newRoom.GetComponent<Tile2D.TileRoom2D> ().roomSlots = Tile2DUtility.TileUtility.createRoomPositions (this.roomSize, newRoom);
			newRoom.transform.parent = this.currentMapRef.transform;
			newRoom.transform.localPosition = Vector3.zero;

            addDefaultLayers(newRoom);

            this.currentMapRef.rooms.Add (newRoom);
		}

        void addDefaultLayers(GameObject newRoom)
        {
            if (this.currentMapRef.activateExtraSettings && this.currentMapRef.defaultLayers.Count > 0)
            {
                for (int a = 0; a < this.currentMapRef.defaultLayers.Count; a++)
                {
                    GameObject newLayer = new GameObject();
                    newLayer.AddComponent<Tile2D.TileLayer2D>();
                    newLayer.GetComponent<Tile2D.TileLayer2D>().layerName = this.currentMapRef.defaultLayers[a].GetComponent<Tile2D.TileLayer2D>().layerName;
                    newLayer.GetComponent<Tile2D.TileLayer2D>().visible = this.currentMapRef.defaultLayers[a].GetComponent<Tile2D.TileLayer2D>().visible;
                    newLayer.GetComponent<Tile2D.TileLayer2D>().locked = this.currentMapRef.defaultLayers[a].GetComponent<Tile2D.TileLayer2D>().locked;
                    newLayer.gameObject.name = "Layer_" + this.currentMapRef.defaultLayers[a].GetComponent<Tile2D.TileLayer2D>().layerName;
                    newLayer.transform.parent = newRoom.transform;
                    newLayer.transform.localPosition = Vector3.zero;

                    newRoom.GetComponent<Tile2D.TileRoom2D>().roomLayers.Add(newLayer.GetComponent<Tile2D.TileLayer2D>());
                }
            }
        }

        void drawDefaultTiles()
        {
            if (this.currentMapRef.activateExtraSettings)
            {
                drawRoomLayers();
            }
        }

        void drawRoomLayers()
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

            Tile2DUtility.TileUtility.drawBoxLabel("Room Layers");

            if (this.currentMapRef.defaultLayers != null && this.currentMapRef.defaultLayers.Count > 0)
            {
                Tile2DUtility.TileUtility.drawBoxLabel("Total Layers: " + this.currentMapRef.defaultLayers.Count);

                for (int a = 0; a < this.currentMapRef.defaultLayers.Count; a++)
                {
                    GameObject layer = this.currentMapRef.defaultLayers[a];
                    Tile2D.TileLayer2D layerInfo = layer.GetComponent<Tile2D.TileLayer2D>();

                    GUILayout.BeginVertical("box");

                    layerInfo.layerName = EditorGUILayout.TextField("Layer Name", layerInfo.layerName);
                    layerInfo.setLayerName(layerInfo.layerName);

                    GUILayout.BeginHorizontal();

                    layerInfo.locked = GUILayout.Toggle(layerInfo.locked, "Locked", "button");
                    layerInfo.visible = GUILayout.Toggle(layerInfo.visible, "Visible", "button");

                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Remove"))
                    {
                        removeLayer(layer);
                    }

                    GUILayout.EndVertical();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("There are no layers to display.", MessageType.Error);
            }

            if (GUILayout.Button("Clear Layers"))
            {
                clearLayers();
            }

            if (GUILayout.Button("Add Layer"))
            {
                addLayer();
            }

            GUILayout.EndVertical();
        }

        void removeLayer(GameObject layerToRemove)
        {
            if (!layerToRemove.GetComponent<Tile2D.TileLayer2D>().locked)
            {
                this.currentMapRef.defaultLayers.Remove(layerToRemove);
                DestroyImmediate(layerToRemove.gameObject);
            }
            else
            {
                Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: Cannot remove layer because its locked.");
            }
        }

        void addLayer()
        {
            GameObject newLayer = new GameObject();
            newLayer.AddComponent<Tile2D.TileLayer2D>();
            newLayer.transform.parent = this.currentMapRef.tileMapLayerCon.transform;
            newLayer.transform.position = Vector3.zero;
            this.currentMapRef.defaultLayers.Add(newLayer);
        }

        void clearLayers()
        {
            this.currentMapRef.defaultLayers.Clear();

            if (this.currentMapRef.tileMapLayerCon.transform.childCount > 0)
            {
                for (int a = 0; a < this.currentMapRef.tileMapLayerCon.transform.childCount; a++)
                {
                    DestroyImmediate(this.currentMapRef.tileMapLayerCon.transform.GetChild(a).gameObject);
                }

                if (this.currentMapRef.tileMapLayerCon.transform.childCount > 0)
                {
                    clearLayers();
                }
            }
        }

        void createLayerContainer()
        {
            if (this.currentMapRef.tileMapLayerCon == null)
            {
                GameObject layerCon = new GameObject();
                layerCon.name = "TileMap2D_Layers";
                layerCon.transform.parent = this.currentMapRef.transform;
                layerCon.transform.position = Vector3.zero;
                this.currentMapRef.tileMapLayerCon = layerCon;
            }
        }

        [MenuItem("Tile2D/TilePicker2D")]
		public static void InitEditorRoom()
		{
			Tile2D.TilePicker2DWin window = (Tile2D.TilePicker2DWin)EditorWindow.GetWindow (typeof(Tile2D.TilePicker2DWin),false,"TilePicker2D");
			window.Show ();
		}

        [MenuItem("Assets/Tile2D/Create/TileSet2D")]
        static void CreateTileSet()
        {
            Tile2D.TileSet2D asset = ScriptableObject.CreateInstance<Tile2D.TileSet2D>();
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                //path = path.Replace(Path.GetFileName(path), "");
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            else
            {
                path += "/";
            }

            string AssetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet2D.asset");
            AssetDatabase.CreateAsset(asset, AssetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            asset.hideFlags = HideFlags.None;
        }

    }
}

