using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tile2DUtility
{

	public class TileUtility 
	{
        public static float getFloatBool(bool boolean)
        {
            if (boolean)
            {
                return 1;
            }

            return 0;
        }

		public static List<Vector3> createRoomPositions(Vector2 roomSize,GameObject roomObj)
		{
			List<Vector3> positionArray = new List<Vector3> ();

			for(int x = 0; x < roomSize.x; x++)
			{
				for(int y = 0; y < roomSize.y; y++)
				{
					Vector3 newSlot = new Vector3 (roomObj.transform.position.x + x * 1f, roomObj.transform.position.y + y * 1f);
					Vector3 localSlotPos = roomObj.transform.InverseTransformPoint (newSlot);

					positionArray.Add (newSlot);
				}
			}

			return positionArray;
		}

		public static Vector3 roundPos(Vector3 posToCalc)
		{
			Vector3 calc = posToCalc;
			calc.x = Mathf.Round (calc.x);
			calc.y = Mathf.Round (calc.y);
			calc.z = 0;

			return calc;
		}

		public static Vector2 roundRoomSizeFloats(Vector2 room)
		{
			Vector2 calc = room;
			calc.x = Mathf.Round (calc.x);
			calc.y = Mathf.Round (calc.y);

			if(calc.x <= 0)
			{
				calc.x = 1;
			}

			if(calc.y <= 0)
			{
				calc.y = 1;
			}

			return calc;
		}

		public static void displayConsoleMessage(string msg)
		{
			Debug.Log ("<b><color=green>"+ msg +"</color></b>");	
		}

		public static void displayConsoleErrorMessage(string errormsg)
		{
			Debug.Log ("<b><color=red>"+ errormsg +"</color></b>");	
		}

		public static void drawBoxLabel(string titleName)
		{
			GUILayout.BeginHorizontal ("box");
			GUILayout.FlexibleSpace ();
			GUILayout.Label (titleName);
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
		}

		public static Texture2D textureFromSprite(Sprite sprite)
		{
			if (sprite.rect.width != sprite.texture.width)
			{
				Texture2D newText = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
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

	}

}

public enum ActionType
{
	DRAW,
	ERASE,
	MOVE
}


