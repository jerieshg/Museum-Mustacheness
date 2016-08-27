using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Tile2D
{

    public class TilePicker2DWin : EditorWindow
    {
        public static Tile2D.TilePicker2DWin currentRoomEditor;

        //Selected Room Variables
        private Tile2D.TileRoom2DCustomInspector selectedRoomInspect;
        private Tile2D.TileRoom2D selectedRoom;
        private Tile2D.TileRoom2D lastSelectedRoom;
        private GameObject currentTileBrush;

        //TilePicker2D Variables
        private Tile2D.TileSet2D selectedTileSet;
        private Tile2D.TileSet2D currentTileSet;

        private GameObject selectedObj;
        private GameObject selectedPrefab;
        private Vector2 windowScroll;
        private Vector3 lastObjPosition;
        private Vector3 startDragPosition;
        private Vector3 endDragPosition;

        private bool flipTile = false;
        private bool replaceTile = false;
        private bool enabled = false;
        private bool isDragging = false;
        private ActionType actionType = ActionType.DRAW;
        private Texture2D[] tileTextures;
        private int selectedIndex;

        //SceneUI Variables
        private Vector3 currentMousePosition;
        private Vector3 currentLocalMousePosition;

        void OnEnable()
        {
            if (currentRoomEditor == null)
            {
                currentRoomEditor = this;
            }
            else
            {
                currentRoomEditor.Close();
            }

            this.tileTextures = null;

            SceneView.onSceneGUIDelegate += SceneGUI;
        }

        void Update()
        {
            getSelectedRoom();
        }

        void SceneGUI(SceneView sceneView)
        {
            SceneEvents();
            drawSceneUI();
        }

        void drawSceneUI()
        {
            Handles.BeginGUI();
            GUILayout.BeginVertical("box", GUILayout.Width(100f));
            Tile2DUtility.TileUtility.drawBoxLabel("Mouse Position: " + this.currentMousePosition);
            Tile2DUtility.TileUtility.drawBoxLabel("Local Mouse Position: " + this.currentLocalMousePosition);

            if (this.selectedRoom != null)
            {
                Tile2DUtility.TileUtility.drawBoxLabel("Selected Room: " + this.selectedRoom.roomName);
            }
            else
            {
                Tile2DUtility.TileUtility.drawBoxLabel("Selected Room: None");
            }

            if (this.selectedRoom != null && this.selectedRoom.selectedLayer != null)
            {
                Tile2DUtility.TileUtility.drawBoxLabel("Selected Layer: " + this.selectedRoom.selectedLayer.layerName);
            }
            else
            {
                Tile2DUtility.TileUtility.drawBoxLabel("Selected Layer: None");
            }

            GUILayout.EndVertical();

            Handles.EndGUI();
        }

        void SceneEvents()
        {
            if (this.selectedRoom != null && this.enabled)
            {
                Tools.current = Tool.View;
                
                instBrush(this.enabled);

                Event e = Event.current;
                int controlID = GUIUtility.GetControlID(FocusType.Passive);
                EventType type = e.GetTypeForControl(controlID);

                Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight, 0));
                Vector3 mousePos = ray.origin;

                this.currentMousePosition = Tile2DUtility.TileUtility.roundPos(mousePos);
                this.currentTileBrush.transform.position = this.currentMousePosition;
                updateTileBrush();

                if (this.selectedRoom != null)
                {
                    this.currentLocalMousePosition = this.selectedRoom.transform.InverseTransformPoint(this.currentMousePosition);
                }

                switch (type)
                {
                    case EventType.MouseUp:

                        this.isDragging = false;
                        this.selectedObj = null;

                        if (GUIUtility.hotControl == controlID)
                        {
                            GUIUtility.hotControl = 0;
                            e.Use();
                        }
                        break;
                    case EventType.MouseDown:

                        if (e.button == 0)
                        {
                            this.isDragging = true;
                            GUIUtility.hotControl = controlID;
                            e.Use();
                            doCurrentAction();
                        }
                        break;
                    case EventType.MouseDrag:
                        dragginLogic();
                        break;

                    case EventType.KeyDown:

                        if (e.keyCode == KeyCode.Q)
                        {
                            setActionType(ActionType.DRAW);
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.W)
                        {
                            setActionType(ActionType.MOVE);
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.E)
                        {
                            setActionType(ActionType.ERASE);
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.X)
                        {
                            this.flipTile = !this.flipTile;
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.C)
                        {
                            this.replaceTile = !this.replaceTile;
                            e.Use();
                        }

                        break;
                }
            }
            else
            {
                instBrush(this.enabled);
            }
        }

        void instBrush(bool activate)
        {
            if (activate)
            {
                if (this.currentTileBrush == null)
                {
                    this.currentTileBrush = new GameObject();
                    this.currentTileBrush.name = "TileBrush2D";
                    this.currentTileBrush.AddComponent<Tile2D.TileBrush2D>();
                    this.currentTileBrush.transform.SetAsFirstSibling();
                }
            }
            else
            {
                if (this.currentTileBrush != null)
                {
                    DestroyImmediate(this.currentTileBrush.gameObject);
                }
            }
        }

        void updateTileBrush()
        {
            if (this.currentTileBrush != null)
            {
                if (this.enabled && this.actionType == ActionType.DRAW && this.selectedTileSet != null && this.selectedPrefab != null)
                {
                    this.currentTileBrush.GetComponent<Tile2D.TileBrush2D>().setBrushSprite(this.selectedPrefab.GetComponent<SpriteRenderer>().sprite);

                    if (this.flipTile)
                    {
                        this.currentTileBrush.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        this.currentTileBrush.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
                else
                {
                    this.currentTileBrush.GetComponent<Tile2D.TileBrush2D>().setBrushSprite(null);
                }
            }
        }

        void dragginLogic()
        {
            if (this.actionType == ActionType.MOVE && this.isDragging && this.selectedObj != null)
            {
                Vector3 calcPos = this.selectedRoom.transform.InverseTransformPoint(this.currentMousePosition);
                if (checkIfClikedInRoom(this.currentMousePosition))
                {
                    this.selectedObj.transform.localPosition = calcPos;
                }
                else
                {
                    this.isDragging = false;
                    this.selectedObj.transform.position = this.lastObjPosition;
                }
            }
            else if (this.actionType == ActionType.DRAW && this.isDragging)
            {
                doCurrentAction();
            }
            else if (this.actionType == ActionType.ERASE && this.isDragging)
            {
                doCurrentAction();
            }
        }

        void doCurrentAction()
        {
            switch (this.actionType)
            {
                case ActionType.DRAW:
                    drawTile();
                    break;
                case ActionType.ERASE:
                    eraseTile();
                    break;
                case ActionType.MOVE:
                    moveTile();
                    break;
            }
        }

        void moveTile()
        {
            Transform currentTile;

            if (this.selectedRoom.selectedLayer != null)
            {
                if (checkIfClikedInRoom(this.currentMousePosition) && getTransformFromPos(this.currentMousePosition) != null)
                {
                    currentTile = getTransformFromPos(this.currentMousePosition);
                    this.selectedObj = currentTile.gameObject;
                    this.lastObjPosition = this.selectedObj.transform.position;
                }
                else
                {
                    Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: You must click inside room bounds or there is no object to move in that position.");
                }
            }
            else
            {
                Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: There must be a selected layer to move tile.");
            }
        }

        void eraseTile()
        {
            if (this.selectedRoom.selectedLayer != null)
            {
                if (checkIfClikedInRoom(this.currentMousePosition) && getTransformFromPos(this.currentMousePosition) != null)
                {
                    Transform erasedObj = getTransformFromPos(this.currentMousePosition);
                    DestroyImmediate(erasedObj.gameObject);
                }
            }
            else
            {
                Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: There must be a selected layer to erase tile to.");
            }
        }

        void drawTile()
        {
            GameObject go;

            if (this.selectedRoom.selectedLayer != null && this.selectedPrefab != null)
            {
                if (checkIfClikedInRoom(this.currentMousePosition) && getTransformFromPos(this.currentMousePosition) == null)
                {
                    go = (GameObject)PrefabUtility.InstantiatePrefab(this.selectedPrefab.gameObject);
                    go.transform.position = this.currentMousePosition;
                    go.transform.parent = this.selectedRoom.selectedLayer.transform;

                    flipObjTile(this.flipTile, go);
                }
                else if (checkIfClikedInRoom(this.currentMousePosition) && getTransformFromPos(this.currentMousePosition) != null && this.replaceTile)
                {
                    if (this.replaceTile)
                    {
                        Transform erasedObj = getTransformFromPos(this.currentMousePosition);
                        DestroyImmediate(erasedObj.gameObject);
                    }

                    go = (GameObject)PrefabUtility.InstantiatePrefab(this.selectedPrefab.gameObject);
                    go.transform.position = this.currentMousePosition;
                    go.transform.parent = this.selectedRoom.selectedLayer.transform;

                    flipObjTile(this.flipTile, go);
                }
                else
                {
                    Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: You Must click inside room bounds.");
                }
            }
            else
            {
                Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: There must be a selected layer and prefab to perform draw action.");
            }
        }

        void flipObjTile(bool isFlipped, GameObject tile)
        {
            if (isFlipped)
            {
                tile.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        Transform getTransformFromPos(Vector3 pos)
        {
            Transform currentRoom = this.selectedRoom.gameObject.transform;

            if (this.selectedRoom.selectedLayer.transform.childCount > 0)
            {
                for (int a = 0; a < this.selectedRoom.selectedLayer.transform.childCount; a++)
                {
                    if (currentRoom.TransformVector(this.selectedRoom.selectedLayer.transform.GetChild(a).transform.position) == pos)
                    {
                        return this.selectedRoom.selectedLayer.transform.GetChild(a).transform;
                    }
                }
            }
            return null;
        }

        bool checkIfClikedInRoom(Vector3 pos)
        {
            Transform currentRoom = this.selectedRoom.gameObject.transform;

            for (int a = 0; a < this.selectedRoom.roomSlots.Count; a++)
            {
                if (currentRoom.TransformPoint(this.selectedRoom.roomSlots[a]) == pos)
                {
                    return true;
                }
            }
            return false;
        }

        void OnGUI()
        {
            drawWindowLayout();
        }

        void drawWindowLayout()
        {
            this.windowScroll = GUILayout.BeginScrollView(this.windowScroll);
            drawTilePickerControls();
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            drawTileChooser();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }

        void drawTilePickerControls()
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

            Tile2DUtility.TileUtility.drawBoxLabel("TilePicker2D Controls");

            if (selectedRoom == null)
            {
                EditorGUILayout.HelpBox("There is no room selected.", MessageType.Error);
                this.enabled = false;
            }
            else
            {
                this.enabled = GUILayout.Toggle(this.enabled, "Enable", "button");
            }

            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            this.replaceTile = GUILayout.Toggle(this.replaceTile, "Replace (C)");
            this.flipTile = GUILayout.Toggle(this.flipTile, "Flip (X)");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Draw (Q)"))
            {
                setActionType(ActionType.DRAW);
            }
            if (GUILayout.Button("Move (W)"))
            {
                setActionType(ActionType.MOVE);
            }
            if (GUILayout.Button("Erase (E)"))
            {
                setActionType(ActionType.ERASE);
            }
            
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        void setActionType(ActionType at)
        {
            this.actionType = at;
        }

        void drawTileChooser()
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            Tile2DUtility.TileUtility.drawBoxLabel("Tile Chooser");

            this.selectedTileSet = (Tile2D.TileSet2D)EditorGUILayout.ObjectField("Tile Set", this.selectedTileSet, typeof(Tile2D.TileSet2D), false);

            if (GUILayout.Button("Load TileSet2D"))
            {
                loadTileTextures();
            }

            drawTiles();

            GUILayout.EndVertical();
        }

        void drawTiles()
        {
            checkIfTileSetChanged();

            if (this.currentTileSet != null && this.tileTextures != null)
            {
                if (this.currentTileSet.tilesetPrefabs.Length > 0 && this.currentTileSet.tilesetPrefabs != null && this.currentTileSet.tilesetPrefabs.Length != 0)
                {
                    this.selectedIndex = GUILayout.SelectionGrid(this.selectedIndex, this.tileTextures, 5);
                    this.selectedPrefab = this.currentTileSet.tilesetPrefabs[this.selectedIndex];
                }
            }
            else
            {
                this.tileTextures = null;
                this.selectedPrefab = null;
            }
        }

        void checkIfTileSetChanged()
        {
            if (this.selectedTileSet != null)
            {
                if (this.selectedTileSet != this.currentTileSet)
                {
                    this.currentTileSet = null;
                    this.selectedIndex = 0;
                }
            }
        }

        void loadTileTextures()
        {
            if (this.selectedTileSet != null && this.selectedTileSet.tilesetPrefabs.Length > 0)
            {
                this.currentTileSet = this.selectedTileSet;
                this.tileTextures = new Texture2D[this.selectedTileSet.tilesetPrefabs.Length];

                for (int a = 0; a < this.selectedTileSet.tilesetPrefabs.Length; a++)
                {
                    this.tileTextures[a] = Tile2DUtility.TileUtility.textureFromSprite(this.selectedTileSet.tilesetPrefabs[a].GetComponent<SpriteRenderer>().sprite);
                    Tile2D.TextureScale.Point(this.tileTextures[a], this.tileTextures[a].width * 2, this.tileTextures[a].height * 2);
                }
            }
            else
            {
                Tile2DUtility.TileUtility.displayConsoleErrorMessage("Error: Tileset2D doesn't have any prefabs to display.");
            }
        }

        void getSelectedRoom()
        {
            if (Selection.activeGameObject)
            {
                if (Selection.activeGameObject.gameObject.GetComponent<Tile2D.TileRoom2D>())
                {
                    this.selectedRoom = Selection.activeGameObject.gameObject.GetComponent<Tile2D.TileRoom2D>();
                    this.selectedRoom.tileGridColor = Color.green;
                }
                else
                {
                    this.selectedRoom = null;
                }
            }
            else
            {
                this.selectedRoom = null;
            }

            if (this.lastSelectedRoom != null)
            {
                if (this.selectedRoom == null)
                {
                    this.lastSelectedRoom.selectedLayer = null;
                    this.lastSelectedRoom.tileGridColor = Color.grey;
                    this.lastSelectedRoom = null;
                }
                else if (this.selectedRoom != null && this.selectedRoom.name != this.lastSelectedRoom.name)
                {
                    this.lastSelectedRoom.selectedLayer = null;
                    this.lastSelectedRoom.tileGridColor = Color.grey;
                    this.lastSelectedRoom = null;
                }
            }

            this.lastSelectedRoom = this.selectedRoom;
            currentRoomEditor.Repaint();
        }

        void destroyAllReferences()
        {
            if (this.currentTileBrush != null)
            {
                DestroyImmediate(this.currentTileBrush.gameObject);
            }

            if (this.selectedRoom != null || this.lastSelectedRoom != null)
            {
                this.selectedRoom.tileGridColor = Color.grey;
                this.selectedRoom.selectedLayer = null;
                this.selectedRoom = null;

                this.lastSelectedRoom.tileGridColor = Color.grey;
                this.lastSelectedRoom.selectedLayer = null;
                this.lastSelectedRoom = null;
            }
        }

        void OnDestroy()
        {
            destroyAllReferences();
            SceneView.onSceneGUIDelegate -= SceneGUI;
        }

    }
}


