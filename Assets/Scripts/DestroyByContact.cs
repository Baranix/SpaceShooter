using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public int health;

	private GameController gameController;
	private bool bossExplode;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag("GameController"); // Find the Game Controller object
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>(); // Get the component of the object
		}
		if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script.");
		}

		bossExplode = false;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Boss"))
		{
			return;
		}

		if (CompareTag("Boss"))
		{
			health--;
			if (other.CompareTag("Bomb"))
			{
				health-=4;
			}
			
			if (health <=0)
			{
				bossExplode = true;
				gameController.SetWinStatus(true);
			}
		}

		if (explosion != null && ( bossExplode || !CompareTag("Boss") ) )
		{
			Instantiate(explosion, transform.position, transform.rotation);
		}

		if (other.CompareTag("Player"))
		{
			gameController.lives--;
			gameController.livesText.text = gameController.lives.ToString();
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);

			if (gameController.lives == 0)
			{
				gameController.GameOver();
			}
			else
			{
				gameController.RestartWave();
			}
		}

		Destroy(other.gameObject);
		if(bossExplode || !CompareTag("Boss"))
		{
			Destroy(gameObject);
			gameController.AddScore(scoreValue);
		}
		
	}
}
