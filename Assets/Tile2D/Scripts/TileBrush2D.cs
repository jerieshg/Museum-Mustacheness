using UnityEngine;
using System.Collections;

namespace Tile2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileBrush2D : MonoBehaviour
    {
        private Color brushColor = Color.magenta;

        void OnDrawGizmos()
        {
            drawCube();
        }

        void drawCube()
        {
            Gizmos.color = brushColor;
            Gizmos.DrawWireCube(transform.position,new Vector3(1,1));
        }

        public void setBrushSprite(Sprite newSprite)
        {
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
}


