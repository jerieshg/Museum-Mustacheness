using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Tile2D
{
	[CustomEditor(typeof(Tile2D.TileRoom2D))]
	public class TileRoom2DCustomInspector : Editor {

		Tile2D.TileRoom2D currentTileRoomRef;

        void OnEnable()
		{
			this.currentTileRoomRef = (Tile2D.TileRoom2D)this.target;
		}

		public override void OnInspectorGUI()
		{
			drawRoomProperties ();
			GUILayout.Space (5f);
            drawRoomLayers();


            if (GUI.changed)
			{
				EditorUtility.SetDirty (this.target);
			}
		}

		void drawRoomProperties()
		{
			GUILayout.BeginVertical ("box");

			Tile2DUtility.TileUtility.drawBoxLabel ("TileRoom2D Properties");

			GUILayout.Label ("Room Name: " + this.currentTileRoomRef.roomName);
			GUILayout.Label ("Room Size: " + this.currentTileRoomRef.roomSize);
            GUILayout.Label("Room Min Bounds: " + this.currentTileRoomRef.roomMinXYBounds);
            GUILayout.Label("Room Max Bounds: " + this.currentTileRoomRef.roomMaxXYBounds);
            GUILayout.Label("Room Center Pos: " + this.currentTileRoomRef.roomCenterPos);
            
			this.currentTileRoomRef.drawGrid = GUILayout.Toggle (currentTileRoomRef.drawGrid,"Draw Grid","button");

            if (GUILayout.Button("Open TilePicker2D"))
            {
                Tile2D.TilePicker2DWin window = (Tile2D.TilePicker2DWin)EditorWindow.GetWindow(typeof(Tile2D.TilePicker2DWin), false, "TilePicker2D");
                window.Show();
            }

			GUILayout.EndVertical ();
		}

        void drawRoomLayers()
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

            Tile2DUtility.TileUtility.drawBoxLabel("Room Layers");

            if (this.currentTileRoomRef != null)
            {
                if (this.currentTileRoomRef.roomLayers.Count > 0)
                {
                    Tile2DUtility.TileUtility.drawBoxLabel("Total Layers: " + this.currentTileRoomRef.roomLayers.Count);

                    for (int a = 0; a < this.currentTileRoomRef.roomLayers.Count; a++)
                    {
                        GameObject currentLayer = this.currentTileRoomRef.roomLayers[a].gameObject;
                        Tile2D.TileLayer2D layer = this.currentTileRoomRef.roomLayers[a];

                        GUILayout.BeginVertical("box");

                        layer.layerName = EditorGUILayout.TextField("Layer Name", layer.layerName);
                        layer.setLayerName(layer.layerName);

                        if (GUILayout.Button("Select"))
                        {
                            selectLayer(currentLayer);
                        }

                        GUILayout.BeginHorizontal();

                        layer.locked = GUILayout.Toggle(layer.locked, "Locked", "button");
                        layer.visible = GUILayout.Toggle(layer.visible, "Visible", "button");
                        currentLayer.SetActive(layer.visible);

                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("Clear"))
                        {
                            clearLayer(currentLayer);
                        }

                        if (GUILayout.Button("Remove"))
                        {
                            removeLayer(currentLayer);
                        }

                        GUILayout.EndVertical();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("There are no layers to display.", MessageType.Error);
                }

                if (GUILayout.Button("Add Layer"))
                {
                    addLayer();
                }
            }

            GUILayout.EndVertical();
        }

        void selectLayer(GameObject currentLayer)
        {
            this.currentTileRoomRef.selectedLayer = currentLayer.GetComponent<Tile2D.TileLayer2D>();
        }

        void clearLayer(GameObject layerToClear)
        {
            if (layerToClear.transform.childCount > 0)
            {
                for (int a = 0; a < layerToClear.transform.childCount; a++)
                {
                    DestroyImmediate(layerToClear.transform.GetChild(a).gameObject);
                }

                if (layerToClear.transform.childCount > 0)
                {
                    clearLayer(layerToClear);
                }
            }
            else
            {
                Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: Layer doesn't contain tiles to clear");
            }
        }

        void removeLayer(GameObject layerToRemove)
        {
            if (!layerToRemove.GetComponent<Tile2D.TileLayer2D>().locked)
            {
                this.currentTileRoomRef.roomLayers.Remove(layerToRemove.GetComponent<Tile2D.TileLayer2D>());
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
            newLayer.gameObject.name = "Layer_" + newLayer.GetComponent<Tile2D.TileLayer2D>().layerName;
            newLayer.transform.parent = this.currentTileRoomRef.transform;
            newLayer.transform.localPosition = Vector3.zero;

            currentTileRoomRef.roomLayers.Add(newLayer.GetComponent<Tile2D.TileLayer2D>());
        }

    }
}


