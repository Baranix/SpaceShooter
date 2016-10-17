using UnityEngine;
using System.Collections;

public class BossWeaponController : MonoBehaviour {

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float delay;

	private AudioSource audioSource;
	private GameController gameController;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		GameObject gameControllerObject = GameObject.FindWithTag("GameController"); // Find the Game Controller object
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>(); // Get the component of the object
		}
		if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script.");
		}

		InvokeRepeating( "Fire", delay, fireRate );
	}
	
	void Fire()
	{
		if( !gameController.GetRestartWaveStatus() )
		{
			Instantiate( shot, shotSpawn.position, shotSpawn.rotation );
			audioSource.Play();
		}

		if( gameController.GetGameOverStatus() )
		{
			CancelInvoke ("Fire");
		}
	}
}
