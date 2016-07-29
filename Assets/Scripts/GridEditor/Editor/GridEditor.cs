using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    Grid grid;

    private float gridSize = 50f;
    private int tileIndex = 0;

    private bool canDraw = false;
    private bool canRemove = false;

    private Texture2D[] tileTextures;

    void OnEnable()
    {
        grid = (Grid)target;
    }

    [MenuItem("Assets/Create/TileSet")]
    static void CreateTileSet()
    {
        TileSet asset = ScriptableObject.CreateInstance<TileSet>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path), "");
        }
        else
        {
            path += "/";
        }

        string AssetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset");
        AssetDatabase.CreateAsset(asset, AssetPathAndName);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.None;

    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Dimension");
        grid.width = EditorGUILayout.FloatField(grid.width);
        GUILayout.Label("X");
        grid.height = EditorGUILayout.FloatField(grid.height);
        GUILayout.EndHorizontal();

        grid.gridColor = EditorGUILayout.ColorField("Grid Color", grid.gridColor);
        grid.drawGrid = EditorGUILayout.Toggle("Draw Grid",grid.drawGrid);
        gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);
        EditorGUILayout.HelpBox("Grid is drawned from origin.",MessageType.Info);
        grid.gridSizeX = gridSize;
        grid.gridSizeY = gridSize;

        GUILayout.EndVertical();

        GUILayout.Space(10f);

        GUILayout.BeginVertical("box");

        grid.tileSet = (TileSet)EditorGUILayout.ObjectField("Tile Set", grid.tileSet, typeof(TileSet), false);

        GUILayout.BeginHorizontal();
        drawGridControls();
        GUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("Left click to draw and right click to remove.",MessageType.Info);

        drawLayerControls();
        drawTileChooser();

        GUILayout.EndVertical();

        GUILayout.EndVertical();
    }

    void drawLayerControls()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginVertical("box");
        if (grid.selectedPrefab != null)
        {
            GUILayout.Label("Selected Prefab: " + grid.selectedPrefab.name);
        }
        else
        {
            GUILayout.Label("Selected Prefab: None");
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        if (grid.selectedLayer != null)
        {
            GUILayout.Label("Selected Layer: " + grid.selectedLayer.name);
        }
        else
        {
            GUILayout.Label("Selected Layer: None");
        }
        GUILayout.EndVertical();

        if (grid.gridLayers.Count != 0)
        {
            for (int a = 0; a < grid.gridLayers.Count; a++)
            {
                GUILayout.BeginVertical("box");

                grid.gridLayers[a].name = EditorGUILayout.TextField("Layer Name:", grid.gridLayers[a].name);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Select Layer"))
                {
                    selectLayer(grid.gridLayers[a]);
                }

                if (GUILayout.Button("Remove Layer"))
                {
                    removeLayer(grid.gridLayers[a]);
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("There are no layers to display.",MessageType.Error);
        }

        GUILayout.EndVertical();
    }

    void removeLayer(GameObject layer)
    {
        grid.gridLayers.Remove(layer);
        DestroyImmediate(layer);
    }

    void selectLayer(GameObject layer)
    {
        grid.selectedLayer = layer.transform;
    }

    void drawGridControls()
    {
        //canDraw = GUILayout.Toggle(canDraw,"Draw Tile","Button");
        if (GUILayout.Button("Add Layer"))
        {
            GameObject newLayer = new GameObject();

            newLayer.transform.SetParent(grid.transform);
            newLayer.name = "Layer " + grid.gridLayers.Count;

            grid.gridLayers.Add(newLayer);
        }
    }

    void drawTileChooser()
    {
        GUILayout.BeginVertical("box");

        if (grid.tileSet != null)
        {
            if (grid.tileSet.Prefabs.Length != 0 && grid.tileSet.Prefabs[0] != null)
            {
                tileTextures = new Texture2D[grid.tileSet.Prefabs.Length];

                for (int a = 0; a < grid.tileSet.Prefabs.Length; a++)
                {
                    tileTextures[a] = textureFromSprite(grid.tileSet.Prefabs[a].GetComponent<SpriteRenderer>().sprite);
                }

                tileIndex = GUILayout.SelectionGrid(tileIndex, tileTextures, 10);
                grid.selectedPrefab = grid.tileSet.Prefabs[tileIndex];

                float newWidth = grid.selectedPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
                float newHeight = grid.selectedPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

                grid.width = newWidth;
                grid.height = newHeight;
            }
            else
            {
                EditorGUILayout.HelpBox("Tileset has no tile prefabs.", MessageType.Error);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No tileset has been loaded.", MessageType.Error);
        }

        GUILayout.EndVertical();
    }

    Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    void OnSceneGUI()
    {
        int controllId = GUIUtility.GetControlID(FocusType.Passive);
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(Event.current.mousePosition.x, -Event.current.mousePosition.y + Camera.current.pixelHeight,0));
        Vector3 mousePos = ray.origin;
        Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2f, Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2f, 0f);

        if (canDraw)
        {

        }

        switch (Event.current.GetTypeForControl(controllId))
        {
            case EventType.MouseDown:

                GUIUtility.hotControl = controllId;
                Event.current.Use();
                //Debug.Log("Mouse down");

                if (Event.current.button == 0)
                {
                    GameObject go;
                    Transform prefab = grid.selectedPrefab;

                    if (prefab != null && grid.selectedLayer != null)
                    {
                        Undo.IncrementCurrentGroup();

                        if (grid.selectedLayer.transform.childCount > 0 && getTransformFromPosition(aligned) != null)
                        {
                            Debug.Log("<color=red>Can't place tile, there is one already in that position.</color>");
                            Debug.Log("TEST");
                        }
                        else
                        {
                            go = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
                            go.transform.position = aligned;
                            go.transform.parent = grid.selectedLayer.transform;
                            Undo.RegisterCreatedObjectUndo(go, "Creater" + go.name);
                        }
                    }
                    else
                    {
                        Debug.Log("<color=red>Please select a layer before drawing a tile.</color>");
                    }
                }
                else if (Event.current.button == 1)
                {
                    if (grid.selectedLayer != null)
                    {
                        if (grid.selectedLayer.transform.childCount > 0 && getTransformFromPosition(aligned) != null)
                        {
                            Transform transform = getTransformFromPosition(aligned);
                            DestroyImmediate(transform.gameObject);
                        }
                        else
                        {
                            Debug.Log("<color=green>There is no tile on that position to remove.</color>");
                        }
                    }
                    else
                    {
                        Debug.Log("<color=red>Please select a layer before removing a tile.</color>");
                    }
                }

                break;
            case EventType.MouseUp:
                //Debug.Log("Mouse Up");
                GUIUtility.hotControl = 0;
                Event.current.Use();
                break;
            case EventType.MouseDrag:
                //Debug.Log("Mouse Drag");
                GUIUtility.hotControl = 0;
                Event.current.Use();
                break;
        }
    }

    Transform getTransformFromPosition(Vector3 v3)
    {
        for (int a = 0; a < grid.selectedLayer.transform.childCount; a++)
        {
            Transform trans = grid.selectedLayer.transform.GetChild(a);

            if (trans.position == v3)
            {
                return trans;
            }
        }
        return null;
    }

}
