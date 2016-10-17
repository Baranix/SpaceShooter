using UnityEngine;
using System.Collections;

public class BossMover : MonoBehaviour
{
	public float speed;
	public float startWait;
	public float slowDown;

	private Rigidbody rb;
	private GameController gameController;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		GameObject gameControllerObject = GameObject.FindWithTag("GameController"); // Find the Game Controller object
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>(); // Get the component of the object
		}
		if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script.");
		}

		StartCoroutine ( BossMove () );
	}

	IEnumerator BossMove()
	{
		yield return new WaitForSeconds( startWait );

		for( ; speed <= 0; speed+=0.25f )
		{
			rb.velocity = transform.forward * speed;
			yield return new WaitForSeconds( slowDown );
		}
	}

	void Update()
	{
		if( gameController.GetGameOverStatus() )
		{
			rb.velocity = transform.forward * -3;
		}
	}

}