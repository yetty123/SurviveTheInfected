using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Script for running the helicopter and scripting narration (FSM using PUBLISH-SUBSCRIBE MODEL)
/// </summary>

/*public class Helicopter : MonoBehaviour {
	
	private bool called = false;
	private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}

	void OnDispatchHelicopter () {
		Debug.Log ("Helicopter called");
		rigidBody.velocity = new Vector3 (0,0,50f);
		called = true;
	}
} */

public class Helicopter : MonoBehaviour {

	private bool called = false; // when helicopter is called
	private Rigidbody rigidBody;
	private Transform targetLocation, playerLocation;
	private GameObject targetObject, playerObject;
	private float speed = 30f; // speed of the helicopter

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		playerObject = GameObject.FindGameObjectWithTag ("Player");
		playerLocation = playerObject.transform;
	}

	void Update() {

		// movement function for the helicopter on getting called, if it reaches the player, then win scene is loaded
		if (called) {
			if (transform.position != targetLocation.position) {
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, targetLocation.position, step);
			} 
			else if (dist (transform.position.x, transform.position.z, playerLocation.position.x, playerLocation.position.z) < 5) {
				WhenWin ();
			}
		}
	}

	// when the player has won
	void WhenWin() {
		Application.LoadLevel ("Win Screen");
	}

	// distance formula between two points
	double dist(double x, double z, double x2, double z2) {

		return Math.Sqrt ((x - x2) * (x - x2) + (z - z2) * (z - z2));
	}

	// when the helicopter is called towards the flare
	void OnDispatchHelicopter() {
		print ("heli called");
		targetObject = GameObject.FindGameObjectWithTag ("LandingZone");
		targetLocation = targetObject.transform;
		called = true;
	}
}