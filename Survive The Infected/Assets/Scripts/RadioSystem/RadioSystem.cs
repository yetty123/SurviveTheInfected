using UnityEngine;
using System.Collections;

/// <summary>
/// Script for radio system between player and helicopter and scripting narration (FSM using PUBLISH-SUBSCRIBE MODEL)
/// </summary>

public class RadioSystem : MonoBehaviour {

	public AudioClip initialHeliCall;
	public AudioClip initialCallReply;

	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	// when initial call to helicopter is made
	void OnMakeInitialHeliCall () {
		print (name + " OnMakeInitialHeliCall");
		audioSource.clip = initialHeliCall;
		audioSource.Play ();
		Invoke ("InitialReply", initialHeliCall.length + 1f);
	}

	// reply from the helicopter when it is dispatched
	void InitialReply () {
		audioSource.clip = initialCallReply;
		audioSource.Play ();
		BroadcastMessage ("OnDispatchHelicopter");
	}

}