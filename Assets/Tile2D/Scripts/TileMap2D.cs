using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tile2D
{
	[RequireComponent(typeof(Tile2D.SnapGO))]
	public class TileMap2D : MonoBehaviour
	{
        //TileMap2D Variables
		public List<GameObject> rooms = new List<GameObject>();

        //Room default user settings Variables
        public bool activateExtraSettings = false;
        public List<GameObject> defaultLayers = new List<GameObject>();
        public GameObject tileMapLayerCon;
	}
}


