using UnityEngine;
using System.Collections;

/// <summary>
/// Script for setting zoom FOV of the player
/// </summary>

public class Eyes : MonoBehaviour {

	private Camera eyes;
	private float defaultFOV;

	void Start () {
		eyes = GetComponent<Camera> ();
		defaultFOV = eyes.fieldOfView;
	}
		
	void Update () {
		if (Input.GetButton ("Zoom")) {
			eyes.fieldOfView = defaultFOV / 1.5f;
		} else {
			eyes.fieldOfView = defaultFOV;
		}
	}
}