using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tile2D
{
	[RequireComponent(typeof(Tile2D.SnapGO))]
	public class TileRoom2D : MonoBehaviour {

        //Room Property Variables
		private int tileWidth = 1;
		private int tileHeight = 1;

		public string roomName = "Room";
		public Color tileGridColor = Color.grey;
		public Vector2 roomSize = new Vector2(5,5);
        public Vector3 roomMinXYBounds = Vector3.zero;
        public Vector3 roomMaxXYBounds = Vector3.zero; 
        public Vector2 roomCenterPos = new Vector2(5,5);

		public bool drawGrid = true;
		public bool lockedRoom = false;

		public List<Tile2D.TileLayer2D> roomLayers = new List<Tile2D.TileLayer2D>();
		public List<Vector3> roomSlots = new List<Vector3>();

        private Color boundColor = Color.white;
        private Color ocuppiedCube = Color.red;

        //TileRoom2DEditor Variables
        public Tile2D.TileLayer2D selectedLayer;

        void Start()
        {
            calcRoomBounds();
        }

		void OnDrawGizmos()
		{
			drawGridRoom ();
            drawRedCubeOnObj(this.selectedLayer);
            drawRoomBounds();
        }

		void drawGridRoom()
		{
            Gizmos.color = tileGridColor;

            if (drawGrid)
			{
				for(int x = 0; x < roomSize.x; x++)
				{
					for(int y = 0; y < roomSize.y; y++)
					{
						Gizmos.DrawWireCube (new Vector3 (transform.position.x + x * tileWidth, transform.position.y + y * tileHeight), new Vector3 (tileWidth, tileHeight, 0));
					}
				}
			}
        }

        public void drawRedCubeOnObj(Tile2D.TileLayer2D layer)
        {
            Gizmos.color = ocuppiedCube;

            if (layer != null && this.selectedLayer != null)
            {
                if (layer.transform.childCount > 0 && this.drawGrid)
                {
                    for (int a = 0; a < layer.transform.childCount; a++)
                    {
                        Transform tile = layer.transform.GetChild(a).gameObject.transform;
                        Gizmos.DrawWireCube(new Vector3(tile.position.x,tile.position.y), new Vector3(tileWidth, tileHeight, 0));
                    }
                }
            }
        }

        void drawRoomBounds()
        {
            Gizmos.color = boundColor;

            calcRoomBounds();
            Gizmos.DrawWireCube(this.roomCenterPos, new Vector3(roomSize.x, roomSize.y,0));
        }

        void calcRoomBounds()
        {
            this.roomCenterPos = new Vector3(transform.position.x + (roomSize.x / 2) - 0.5f, transform.position.y + (roomSize.y / 2) - 0.5f, 0);
            this.roomMinXYBounds = new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f);
            this.roomMaxXYBounds = new Vector3(transform.position.x + this.roomSize.x - 0.5f, transform.position.y + this.roomSize.y - 0.5f);
        }

		public void setRoomName(string newName)
		{
			this.roomName = newName;
			gameObject.name = "Room2D_" + newName;
		}

	}
}


