using UnityEngine;
using System.Collections;

namespace UtilityMoustache
{

	public class ScoreManagment
	{

		public static int getArtValue(ArtValue artV)
		{
			switch(artV)
			{
			case ArtValue.low:
				return 1000;
			case ArtValue.medium:
				return 2000;
			case ArtValue.high:
				return 3000;
			case ArtValue.extreme:
				return 4000;
			default:
				return 1000;
			}
		}

		public static int getDifficultyMultiplier(difficulty dif)
		{
			switch (dif)
			{
			case difficulty.Normal:
				return 1;
			case difficulty.Hard:
				return 2;
			case difficulty.MrMoustache:
				return 3;
			default:
				return 1;
			}
		}

	}

}

public enum ArtValue
{
	low,
	medium,
	high,
	extreme
}





