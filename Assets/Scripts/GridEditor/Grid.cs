using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public float width = 16f;
    public float height = 16f;

    public float gridSizeX = 200f;
    public float gridSizeY = 200f;

    public Color gridColor = Color.white;

    public Transform selectedPrefab;
    public Transform selectedLayer;

    public TileSet tileSet;

    public List<GameObject> gridLayers;
    public bool drawGrid = true;

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        Gizmos.color = this.gridColor;

        if (drawGrid)
        {
            for (float y = pos.y - gridSizeY ; y < pos.y + gridSizeY; y += this.height)
            {
                Gizmos.DrawLine(new Vector3(-gridSizeY, Mathf.Floor(y / this.height) * this.height, 0f), new Vector3(gridSizeY, Mathf.Floor(y / this.height) * this.height, 0f));
            }

            for (float x = pos.x - gridSizeX; x < pos.x + gridSizeX; x += this.width)
            {
                Gizmos.DrawLine(new Vector3(Mathf.Floor(x / this.width) * this.width,-gridSizeX, 0f), new Vector3(Mathf.Floor(x / this.width) * this.width,gridSizeX, 0f));
            }
        }
    }
}
