using UnityEngine;
using System.Collections;

namespace Tile2D
{
	public class TileLayer2D : MonoBehaviour
	{
		public string layerName = "newLayer";
		public bool locked = false;
		public bool visible = true;

        public void setLayerName(string newName)
        {
            gameObject.name = "Layer_" + newName;
        }
	}
}


