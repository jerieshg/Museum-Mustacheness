using UnityEngine;
using System.Collections;

namespace Tile2D
{
	[ExecuteInEditMode]
	public class SnapGO : MonoBehaviour 
	{
		void Update()
		{
			transform.position = Tile2DUtility.TileUtility.roundPos (transform.position);
		}
	}

}


