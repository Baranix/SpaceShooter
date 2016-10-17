using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{

	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public GameObject missile;
	public Transform shotSpawn; 
	public float fireRate;

	private float nextFire;
	private Rigidbody rb;
	private AudioSource audio;
	private GameController gameController;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();

		GameObject gameControllerObject = GameObject.FindWithTag("GameController"); // Find the Game Controller object
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>(); // Get the component of the object
		}
		if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script.");
		}
	}

	void Update()
	{
		if (Input.GetButton("Jump") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play();
		}

		if (Input.GetButton("Fire1") && Time.time > (nextFire + 0.5f) && gameController.missiles > 0)
		{
			nextFire = Time.time + fireRate;
			Instantiate(missile, shotSpawn.position, shotSpawn.rotation);
			gameController.missiles--;
			gameController.missilesText.text = gameController.missiles.ToString();
			audio.Play();
		}
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;

		rb.position = new Vector3 
		(
			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
