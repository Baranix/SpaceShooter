using UnityEngine;
using System.Collections;

public class EvasiveManuevers : MonoBehaviour
{
	public float dodge;
	public float smoothing;
	public float tilt;
	public Vector2 startWait;
	public Vector2 manueverTime;
	public Vector2 manueverWait;
	public Boundary boundary;

	private float currentSpeed;
	private float targetManuever;
	private Rigidbody rb;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		StartCoroutine( Evade() );
	}

	IEnumerator Evade()
	{
		yield return new WaitForSeconds( Random.Range( startWait.x, startWait.y ) );

		while(true)
		{
			targetManuever = Random.Range(1, dodge) * -Mathf.Sign( transform.position.x );
			yield return new WaitForSeconds( Random.Range( manueverTime.x, manueverTime.y ) );
			targetManuever = 0;
			yield return new WaitForSeconds( Random.Range( manueverWait.x, manueverWait.y ) );
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		currentSpeed = rb.velocity.z;
		float newManuever = Mathf.MoveTowards( rb.velocity.x, targetManuever, Time.deltaTime * smoothing );
		rb.velocity = new Vector3(newManuever, 0.0f, currentSpeed);
		rb.position = new Vector3
		(
			Mathf.Clamp( rb.position.x, boundary.xMin, boundary.xMax ),
			0.0f,
			Mathf.Clamp( rb.position.z, boundary.zMin, boundary.zMax )
		);
		rb.rotation = Quaternion.Euler( 0.0f, 0.0f, rb.velocity.x * -tilt );
	}
}
