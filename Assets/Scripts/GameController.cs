using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject player;
	public GameObject[] hazards;
	public GameObject[] enemies;
	public GameObject boss;
	public AudioClip bossMusic;
	public Vector3 spawnValues;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public int waveCount;
	public int lives;
	public int missiles;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText livesText;
	public GUIText missilesText;
	public GUIText creditText;

	private bool gameOver;
	private bool win;
	private bool restart;
	private bool pause;
	private bool restartWave;
	private bool musicFadeOut;
	private int score;
	private int waveScore;
	private int hazardCount;
	private int enemyCount;

	// Use this for initialization
	void Start ()
	{
		gameOver = false;
		win = false;
		restart = false;
		pause = false;
		restartWave = false;
		musicFadeOut = false;
		UpdateScore();
		restartText.text = "";
		gameOverText.text = "";
		creditText.text = "";
		livesText.text = lives.ToString();
		missilesText.text = missiles.ToString();
		score = 0;
		waveScore = 0;
		hazardCount = 5;
		enemyCount = 0;
		StartCoroutine ( SpawnWaves() );
	}

	void Update()
	{
		if( restart )
		{
			if( Input.GetKeyDown(KeyCode.R) )
			{
				Application.LoadLevel( Application.loadedLevel );
			}
		}

		if( Input.GetKeyDown(KeyCode.Escape) )
		{
			if( !pause )
			{
				gameOverText.text = "Press Enter to Quit.\nPress Esc again to go back.";
				creditText.text = "Game made by: Nikki C. Ebora\nMusic from: Robson Cozendey - www.cozendey.com";
				pause = true;
				Time.timeScale = 0;
			}
			else
			{
				pause = false;
				gameOverText.text = "";
				creditText.text = "";
				Time.timeScale = 1;
			}
		}
		
		if ( pause && Input.GetKeyDown(KeyCode.Return) )
		{
			Application.Quit();
		}
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds( startWait );

		int wave = 1;

		while(true)
		{
			Debug.Log("Wave: " + wave);
			for( int i = 0; !restartWave && i < waveCount && wave < waveCount; i++ )
			{
				if( i < hazardCount )
				{
					GameObject hazard = hazards[ Random.Range( 0, hazards.Length ) ];
					Vector3 spawnPosition = new Vector3(Random.Range( -spawnValues.x, spawnValues.x ), spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					yield return new WaitForSeconds( spawnWait );
				}
				if( i < enemyCount )
				{
					GameObject enemy = enemies[ Random.Range( 0, enemies.Length ) ];
					Vector3 spawnPosition2 = new Vector3(Random.Range( -spawnValues.x, spawnValues.x ), spawnValues.y, spawnValues.z);
					Quaternion spawnRotation2 = Quaternion.identity;
					Instantiate (enemy, spawnPosition2, spawnRotation2);
					yield return new WaitForSeconds( spawnWait );
				}
			} // end spawn wave
			yield return new WaitForSeconds( waveWait );
			
			if (restartWave)
			{
				AddScore( waveScore * -1 );
				Instantiate (player, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
				restartWave = false;
			}
			else
			{
				UpdateScore();
				hazardCount--;
				enemyCount++;

				if( wave == waveCount )
				{
					AudioSource audio = GetComponent<AudioSource>();

					if( !musicFadeOut )
					{
						musicFadeOut = true;
						float t = 0.5f;
						while( t > 0.0f )
						{
							t -= Time.deltaTime;
							audio.volume = t;
							yield return new WaitForSeconds( 0.1f );
						}
					}

					audio.clip = bossMusic;
					audio.volume = 0.5f;
					audio.Play();
					yield return new WaitForSeconds( 2.0f );

					Vector3 spawnPosition = new Vector3(0.0f, spawnValues.y, spawnValues.z + 4.0f);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (boss, spawnPosition, spawnRotation);
				} // end spawn boss

				if ( wave <= waveCount )
				{
					wave++;
				}
			}

			if( gameOver )
			{
				restartText.text = "Press 'R' to Restart.";
				restart = true;
				break;
			}
		}
	}

	public void AddScore(int newScoreValue)
	{
		waveScore += newScoreValue;
		scoreText.text = "Score: " + (score + waveScore);
	}

	void UpdateScore()
	{
		score += waveScore;
		waveScore = 0;
		scoreText.text = "Score: " + score;
	}

	public void GameOver()
	{
		gameOverText.text = "Game Over";
		creditText.text = "Game made by: Nikki C. Ebora\nMusic from: Robson Cozendey - www.cozendey.com";
		gameOver = true;
	}

	public bool GetGameOverStatus()
	{
		return gameOver;
	}

	public void SetWinStatus(bool status)
	{
		win = status;
		gameOverText.text = "You win!";
		creditText.text = "Game made by: Nikki C. Ebora\nMusic from: Robson Cozendey - www.cozendey.com";
		gameOver = status;
		restart = status;
	}

	public void RestartWave()
	{
		Debug.Log("Restart Wave called.");
		restartWave = true;
	}

	public bool GetRestartWaveStatus()
	{
		return restartWave;
	}

}
