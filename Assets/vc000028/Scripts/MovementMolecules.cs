using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class MovementMolecules : MonoBehaviour
{

	Rigidbody rigidnod;
	public float varx = 2;
	public float vary = 2;
	public float varz = 2;

	public float speed = 50;

	void Start ()
	{
		rigidnod = transform.GetComponent <Rigidbody > ();
		Vector3 temp = new Vector3 (Random.Range (-3f, 3f), Random.Range (-2f, 2f), Random.Range (-2f, 2f));
		rigidnod.velocity = temp;
	}

	void FixedUpdate ()
	{

		if (rigidnod.velocity.x > 0) {
			
			varx = speed / 20;
			//varx = Random.Range (-20, 20);

		} else {
			varx = -speed / 20;
			//varx = Random.Range (-20, 20);
		}
		if (rigidnod.velocity.y > 0) {
			
			vary = speed / 20;
			//vary = Random.Range (-20, 20);

		} else {
			vary = -speed / 20;
			//vary = Random.Range (-20, 20);
		}
		if (rigidnod.velocity.z > 0) {
			
			varz = speed / 20;
			//varz = Random.Range (-20, 20);

		} else {
			varz = -speed / 20;	
			//varz = Random.Range (-20, 20);
		}


		if ((rigidnod.velocity.x == 0) || (rigidnod.velocity.y == 0) || (rigidnod.velocity.z == 0)) {
			
			rigidnod.velocity = new Vector3 (Random.Range (-varx, varx), Random.Range (-vary, vary), Random.Range (-varz, varz));

		} else if ((rigidnod.velocity.x >= varx) || (rigidnod.velocity.y >= vary) || (rigidnod.velocity.z >= varz)) {
			rigidnod.velocity = new Vector3 (varx, vary, varz);

		} else if ((rigidnod.velocity.x <= varx) || (rigidnod.velocity.y <= vary) || (rigidnod.velocity.z <= varz)) {
			rigidnod.velocity = new Vector3 (varx, vary, varz);

		}
	}
}

	



