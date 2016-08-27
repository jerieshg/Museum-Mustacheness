using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D),typeof(SpriteRenderer),typeof(PixelPerfectSprite))]
public class ArtObj : MonoBehaviour
{
	public Sprite moustacheSprite;
	public ArtValue artV = ArtValue.low;

	private int artValue;
	private int difficultyMultiplier;

	void Start()
	{
		this.artValue = UtilityMoustache.ScoreManagment.getArtValue (artV) * UtilityMoustache.ScoreManagment.getDifficultyMultiplier(GameManager.gameManager.getGameDifficulty());
	}

	public int getArtScore()
	{
		return this.artValue;	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player")
		{
			GetComponent<CircleCollider2D> ().enabled = false;
			GetComponent<SpriteRenderer> ().sprite = moustacheSprite;
			GetComponent<SpriteRenderer> ().material = ReferenceManager.referenceManager.whiteOutline;
			PlayerManager.playerManager.addScore (artValue);
		}
	}

}



